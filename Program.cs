using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

ExcelPackage.LicenseContext = LicenseContext.NonCommercial; 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(origin => true)
        .AllowCredentials());
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHub<SignalrHub>("/hub");

// Background task to send random data points for different stocks
var dataTask = Task.Run(async () =>
{
    var hubContext = app.Services.GetRequiredService<IHubContext<SignalrHub>>();
    var random = new Random();
    var stocks = new List<string> { "AAPL", "GOOGL", "MSFT", "AMZN", "TSLA" };

    while (true)
    {
        foreach (var stock in stocks)
        {
            await hubContext.Clients.All.SendAsync("dataPointReceived", stock, random.NextDouble() * 1000);
        }
        await Task.Delay(1000); // Send data points every second
    }
});

app.Run();

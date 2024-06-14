using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class SignalrHub : Hub
{
    public async Task SendDataPoint(string stockName, double value)
    {
        await Clients.All.SendAsync("dataPointReceived", stockName, value);
    }


}

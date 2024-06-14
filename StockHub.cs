using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignalrProject
{
    public class StockHub : Hub
    {
        private readonly List<Stock> _stocks = new List<Stock>();

        public StockHub()
        {
            // Initialize stocks data
            _stocks.Add(new Stock { Name = "MSFT", Price = 200.0m, Volume = 1000 });
            _stocks.Add(new Stock { Name = "AAPL", Price = 150.0m, Volume = 500 });
            _stocks.Add(new Stock { Name = "GOOG", Price = 3000.0m, Volume = 2000 });
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.Caller.SendAsync("UpdateStocks", _stocks);

            // Start a task to update stocks every 5 seconds
            _ = UpdateStocksPeriodically();
        }

        private async Task UpdateStocksPeriodically()
        {
            while (true)
            {
                await UpdateStocks();
                await Task.Delay(5000);
            }
        }

        public async Task UpdateStocks()
        {
            var random = new Random();
            foreach (var stock in _stocks)
            {
                stock.Price += (decimal)(random.NextDouble() * 10 - 5);
                stock.Volume += random.Next(100);
            }

            // Broadcast updated data to all connected clients
            await Clients.All.SendAsync("UpdateStocks", _stocks);
        }
    }

    public class Stock
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Volume { get; set; }
    }
}

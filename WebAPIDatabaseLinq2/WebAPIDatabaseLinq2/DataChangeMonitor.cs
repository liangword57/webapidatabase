using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPIDatabaseLinq2
{
    public class DataChangeMonitor : BackgroundService
    {
        private readonly IHubContext<DataHub> _hubContext;

        public DataChangeMonitor(IHubContext<DataHub> hubContext)
        {
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _hubContext.Clients.All.SendAsync("StartListening");
        }
    }
}

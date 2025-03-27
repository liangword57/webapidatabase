using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPIDatabaseLinq2
{
    public class DataChangeMonitor(IHubContext<DataHub> hubContext) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await hubContext.Clients.All.SendAsync("StartListening");
        }
    }
}

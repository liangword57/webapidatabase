using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reactive.Linq;

namespace WebAPIDatabaseLinq2.Controllers
{
    [Route("api/Data")]
    [ApiController]
    public class DataController(DataService dataService) : ControllerBase
    {
        [HttpGet("stream")]
        public async Task GetStream()
        {
            var datastream = dataService.GetDataStream();
            var transformedstream = datastream.Where(x => x % 2 == 0).Select(x => $"Transfromed:{x}").Delay(TimeSpan.FromSeconds(1));
            await transformedstream.ToList();
        }

        [HttpGet("external/{id}")]
        public async Task GetExternalData(int id)
        {
            var data = dataService.GetExternalDataAsync(id);
            var observable = Observable.Return($"External Data:{data}");
            await observable.FirstAsync();
        }
    }
}

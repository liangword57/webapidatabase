using System.Reactive.Linq;

namespace WebAPIDatabaseLinq2
{
    public class DataService
    {
        public IObservable<int> GetDataStream()
        {
            return Observable.Generate(
                0,
                i => i < 10,
                i => i + 1,
                i => i * 2,
                i => TimeSpan.FromMilliseconds(500)
                );
        }

        public async Task<int> GetExternalDataAsync(int id)
        {
            await Task.Delay(1000);
            return id * 10;
        }
    }
}

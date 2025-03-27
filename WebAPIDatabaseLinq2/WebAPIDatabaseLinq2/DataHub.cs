using LinqToDB;
using Microsoft.AspNetCore.SignalR;
using Npgsql;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace WebAPIDatabaseLinq2
{
    public class DataHub : Hub
    {
        private readonly string _connectionString;
        
        public DataHub(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //public async Task SubscribeToDataChanges()
        //{
        //    using (var connection = new Npgsql.NpgsqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();
        //        connection.Notification += OnDataChanged;

        //        using (var cmd = new NpgsqlCommand("LISTEN data_change", connection))
        //        {
        //            await cmd.ExecuteNonQueryAsync();
        //            while (true)
        //            {
        //                await connection.WaitAsync();
        //            }
        //        }
        //    }
        //}

        //private async void OnDataChanged(object sender, Npgsql.NpgsqlNotificationEventArgs e)
        //{
        //    if (e.Channel == "data_change")
        //    {
        //        var updateData = await GetUpdatedDataFromDatabase();
        //        await Clients.All.SendAsync("ReceiveDataUpdate", updateData);
        //    }
        //}

        //private async Task<object> GetUpdatedDataFromDatabase()
        //{
        //    using (var db = new AppDataConnection(_connectionString))
        //    {
        //        var query = from employee in db.GetTable<Employee>()
        //                select employee;
        //        var employees = await query.ToListAsync();
        //        return employees;
        //    }
        //}

        public async Task StartListening()
        {
            await SubscribeToDataChanges();
        }

        //结合Rx.net实现
        private async Task SubscribeToDataChanges()
        {
            var notificationObservable = Observable.FromAsync(async () =>
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var cmd = new NpgsqlCommand("LISTEN data_change", connection))
                    {
                        await cmd.ExecuteNonQueryAsync();
                        return connection;
                    }
                }
            })
            .SelectMany(connection => Observable.FromEventPattern<NpgsqlNotificationEventArgs>(
                 handler => connection.Notification += new Npgsql.NotificationEventHandler(handler),
                 handler => connection.Notification += new Npgsql.NotificationEventHandler(handler)
            ))
            .Where(e => e.EventArgs.Channel == "data_change")
            .SelectMany(async _ => await GetUpdatedDataFromDatabase())
            .SelectMany(data => Observable.FromAsync(async () => await Clients.All.SendAsync("ReceiveDataUpdate", data)));

            notificationObservable.Subscribe();
        }

        private async Task<object> GetUpdatedDataFromDatabase()
        {
            using (var db = new AppDataConnection(_connectionString))
            {
                var query = from employee in db.GetTable<Employee>()
                            select employee;
                var employees = await query.ToListAsync();
                return employees;
            }
        }
    }
}

using LinqToDB;
using Microsoft.AspNetCore.SignalR;
using Npgsql;
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

        public async Task SubscribeToDataChanges()
        {
            using (var connection = new Npgsql.NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                connection.Notification += OnDataChanged;

                using (var cmd = new NpgsqlCommand("LISTEN data_change", connection))
                {
                    await cmd.ExecuteNonQueryAsync();
                    while (true)
                    {
                        await connection.WaitAsync();
                    }
                }
            }
        }

        private async void OnDataChanged(object sender, Npgsql.NpgsqlNotificationEventArgs e)
        {
            if (e.Channel == "data_change")
            {
                var updateData = await GetUpdatedDataFromDatabase();
                await Clients.All.SendAsync("ReceiveDataUpdate", updateData);
            }
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

        public async Task StartListening()
        {
            await SubscribeToDataChanges();
        }
    }
}

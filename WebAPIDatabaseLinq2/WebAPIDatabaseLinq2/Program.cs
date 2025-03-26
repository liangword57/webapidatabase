using LinqToDB;
using LinqToDB.AspNet;
using WebAPIDatabaseLinq2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var conn = builder.Configuration.GetConnectionString("DefaultConnection");

//ע��AppDataConnectionΪ����
builder.Services.AddLinqToDBContext<AppDataConnection>((option, config) => config.UsePostgreSQL(conn));

//���SignalR����
builder.Services.AddSignalR();

//��Ӻ�̨����
builder.Services.AddHostedService<DataChangeMonitor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//SignalR
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<DataHub>("/dataHub");
});

app.Run();

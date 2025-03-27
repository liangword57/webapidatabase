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

//注册AppDataConnection为服务
builder.Services.AddLinqToDBContext<AppDataConnection>((option, config) => config.UsePostgreSQL(conn));

//添加SignalR服务
builder.Services.AddSignalR();

//添加后台服务
builder.Services.AddHostedService<DataChangeMonitor>();

//添加数据服务
builder.Services.AddScoped<DataService>();

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
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<DataHub>("/dataHub");
});

app.Run();

using Play.Catalog.Service.Settings;
using Play.Catalog.Service.Repos;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IMongoDatabase>(options =>
{
    var mongoSettings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDBSettings>();
    var serviceSettings = builder.Configuration.GetSection("ServiceSettings").Get<ServiceSettings>();
    var client = new MongoClient(mongoSettings.ConnectionString);
    return client.GetDatabase(serviceSettings.ServiceName);
});
builder.Services.AddSingleton<IItemsRepository, ItemsRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();

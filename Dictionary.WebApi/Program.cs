using DbUp;
using Dictionary.WebApi.Interfaces;
using Dictionary.WebApi.Repositories;
using Dictionary.WebApi.Services;
using ItemStore.WebApi.csproj.Middlewares;
using Npgsql;
using System.Data;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IItemRepository, ItemRepository>();
builder.Services.AddTransient<IItemService, ItemService>();
builder.Services.AddHostedService<CleanupHostedService>();

var connectionString = builder.Configuration["MySecrets:PostgreConnection"] ?? throw new ArgumentNullException("Connection string was not found."); ;
builder.Services.AddTransient<IDbConnection>(sp => new NpgsqlConnection(connectionString));
// DbUp
EnsureDatabase.For.PostgresqlDatabase(connectionString);
var upgrader = DeployChanges.To
        .PostgresqlDatabase(connectionString)
        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
        .LogToNowhere()
        .Build();

var result = upgrader.PerformUpgrade();

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.Run();

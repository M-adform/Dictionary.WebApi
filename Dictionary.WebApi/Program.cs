using Application;
using Dictionary.WebApi;
using Dictionary.WebApi.Authentication;
using Infrastructure;
using ItemStore.WebApi.csproj.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.WebApiDependencyInjection();
builder.Services.ApplicationDependencyInjection();
builder.Services.InfrastructureDependencyInjection(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ApiKeyAuthMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.Run();
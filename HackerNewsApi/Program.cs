using HackerNewsApi.Contract;
using HackerNewsApi.Interface;
using HackerNewsApi.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpClient();

builder.Services.Configure<ApiSource>(builder.Configuration.GetSection("ApiSource"));

builder.Services.AddScoped<IGenericApiService, GenericApiService>(
        serviceProvider => new GenericApiService(httpClientFactory: serviceProvider.GetRequiredService<IHttpClientFactory>())
    );

builder.Services.AddScoped<IStoryService, StoryService>();

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

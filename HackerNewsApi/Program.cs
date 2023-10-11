using HackerNewsApi.Contract;
using HackerNewsApi.Interface;
using HackerNewsApi.Service;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Added Authorization to swagger
builder.Services.AddSwaggerGen(option => {
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "HackerNews API Demo", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please enter a valid token"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpClient();

builder.Services.Configure<ApiSource>(builder.Configuration.GetSection("ApiSource"));

builder.Services.AddScoped<IGenericApiService, GenericApiService>(
        serviceProvider => new GenericApiService(httpClientFactory: serviceProvider.GetRequiredService<IHttpClientFactory>())
    );

builder.Services.AddScoped<IStoryService, StoryService>();

builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration,"AzureAd");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //Any development related settings
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

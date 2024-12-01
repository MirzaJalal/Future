using Microsoft.EntityFrameworkCore;
using Bangla.Services.RewardAPI.Data;
using Bangla.Services.RewardAPI.Services;
using Bangla.Services.RewardAPI.Messaging;
using Bangla.Services.EmailAPI.Extension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDatabaseConnection"));
});

var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDatabaseConnection"));
builder.Services.AddSingleton(new RewardsService(optionBuilder.Options));

builder.Services.AddSingleton<IAzureServiceBusReceiver, AzureServiceBusReceiver>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Reward API");
    c.RoutePrefix = string.Empty;
});


app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAzureServiceBusReceiver();
app.MapControllers();

app.Run();

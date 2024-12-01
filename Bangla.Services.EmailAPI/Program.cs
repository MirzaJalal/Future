using Bangla.Services.EmailAPI.Data;
using Bangla.Services.EmailAPI.Extension;
using Bangla.Services.EmailAPI.Messaging;
using Bangla.Services.EmailAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDatabaseConnection"));
});

var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDatabaseConnection"));
builder.Services.AddSingleton(new EmailService(optionBuilder.Options));

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
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Email API");
    c.RoutePrefix = string.Empty;
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

ApplyMigration();
// added in the pipeline
app.UseAzureServiceBusReceiver();
app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        ApplicationDbContext _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}

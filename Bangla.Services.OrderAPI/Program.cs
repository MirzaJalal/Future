using AutoMapper;
using Builder.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Builder.Services.ShoppingCartAPI.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Bangla.Services.OrderAPI;
using Builder.Services.OrderAPI.Utility;
using Bangla.MessageBus;
using Bangla.Services.OrderAPI.Extensions;
using Bangla.Services.OrderAPI.Data;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDatabaseConnection"));
});

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper); // available throughout the lifecycle
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // to use in the DI
builder.Services.AddScoped<IProductService, Builder.Services.ShoppingCartAPI.Service.ProductService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TokenPropagationHandler>();
builder.Services.AddScoped<IMessageBus, MessageBus>();
builder.Services.AddHttpClient("Product", u =>
                u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"]))
    .AddHttpMessageHandler<TokenPropagationHandler>();

builder.Services.AddHttpClient("Coupon", u =>
                u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CouponAPI"]))
    .AddHttpMessageHandler<TokenPropagationHandler>(); ;


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    // Enable the JWT bearer token input in Swagger UI
    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,

                    //JwtBearerDefaults.AuthenticationScheme is same as "Bearer"
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            new string[] {}
        }
    });
});

// ### STEP-1,2: Adding Authentication services ###
builder.AddAppAuthentication();
// ### STEP-3: Adding Authorization services ###
builder.Services.AddAuthorization();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

ApplyMigration();

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

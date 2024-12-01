using AutoMapper;
using Bangla.Services.ShoppingCartAPI;
using Bangla.Services.ShoppingCartAPI.Data;
using Bangla.Services.ShoppingCartAPI.Extensions;
using Builder.Services.ShoppingCartAPI.Service.IService;
using Builder.Services.ShoppingCartAPI.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Builder.Services.ShoppingCartAPI.Utility;
using Bangla.MessageBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDatabaseConnection"));
});

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper); // available throughout the lifecycle
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // to use in the DI
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TokenPropagationHandler>();
builder.Services.AddScoped<ICouponService, CouponService>();
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
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shopping Cart API");
    c.RoutePrefix = string.Empty;
});


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

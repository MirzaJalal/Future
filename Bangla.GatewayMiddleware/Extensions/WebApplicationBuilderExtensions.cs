using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Bangla.GatewayMiddleware.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder builder)
        {
            // ### STEP-1: Adding Authentication settings in variable ###
            var secret = builder.Configuration["ApiSettings:Secret"];
            var issuer = builder.Configuration["ApiSettings:Issuer"]; // issuer from settings
            var audience = builder.Configuration["ApiSettings:Audience"]; // audience from settings

            // ### STEP-2: Adding Authentication services ###
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)), // Secret key
                    ValidIssuer = issuer,   // The issuer from your settings
                    ValidAudience = audience, // The audience from your settings
                };
            });

            return builder;
        }
    }
}

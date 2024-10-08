using Bangla.Services.EmailAPI.Data;
using Bangla.Services.EmailAPI.Models;
using Bangla.Services.EmailAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Bangla.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<ApplicationDbContext> _dbOptions;

        public EmailService(DbContextOptions<ApplicationDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task EmailCartAndLog(ShoppingCartDto shoppingCartDto)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("<br/> Cart Email Requested ");
            message.AppendLine("<br/> Total " + shoppingCartDto.CartHeader.CartTotal);
            message.AppendLine("<br/>");
            message.AppendLine("<br/> Coupon Code: " + shoppingCartDto.CartHeader.CouponCode != null ? 
                shoppingCartDto.CartHeader.CouponCode : "not applied");
            message.AppendLine("<ul>");
            foreach(var item in shoppingCartDto.CartDetails)
            {
                message.AppendLine("<li>");
                message.AppendLine(item.Product.Name + " X " + item.Count);
                message.AppendLine("</li>");
            }
            message.AppendLine("</ul>");

            await LogAndEmail(message.ToString(), shoppingCartDto.CartHeader.Email);
        }

        public async Task RegistrationUserEmailAndLog(string email)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("User registration Successful. <br/> Email: " + email);

            await LogAndEmail(message.ToString(), email);
        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLogger = new () 
                { 
                    Email = email,
                    SentEmailTime = DateTime.Now,
                    Message = message,
                };

                await using var _db = new ApplicationDbContext(_dbOptions);
                await _db.EmailLoggers.AddAsync(emailLogger);
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}

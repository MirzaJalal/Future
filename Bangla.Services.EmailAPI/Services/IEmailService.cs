using Bangla.Services.EmailAPI.Models.Dto;

namespace Bangla.Services.EmailAPI.Services
{
    public interface IEmailService
    {
        Task EmailCartAndLog(ShoppingCartDto shoppingCartDto);
    }
}

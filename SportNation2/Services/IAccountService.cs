using Microsoft.AspNetCore.Mvc;

namespace SportNation2.Services
{
    public interface IAccountService
    {
        Task LoginAsync(string email, string password, bool rememberme);
        Task LogoutAsync();
        Task RegisterAsync(string email, string password, DateTime birthDate, string genre);
        
    }
}

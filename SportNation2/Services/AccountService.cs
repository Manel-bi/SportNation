using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SportNation2.Data;
using SportNation2.Data.Models;
using SportNation2.Infrastructure;
using SportNation2.Services;
using System.Security.Claims;
using static SportNation2.Infrastructure.Enumerations;

namespace SportNation2.Services
{
    public class AccountService : IAccountService
    {
        private readonly IHttpContextAccessor accessor;
        private readonly AppDbContext dbContext;



        public AccountService(IHttpContextAccessor accessor, AppDbContext dbContext)
        {
            this.accessor = accessor;
            this.dbContext = dbContext;
        }



        public async Task LoginAsync(string email, string password, bool rememberme)
        {
            //Création de cookie avec claim
            // - Trouver le User dans la bdd
            var user = dbContext.Users
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.Email == email);



            if (user == null)
            {
                throw new Exception("Not found");
            }
            if (!await Helpers.IsPasswordCorrect(password, user.HashedPassword))
            {
                throw new Exception("Incorrect credientials");
            }



            // - Créer une liste de Claim
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Gender, user.Genre.ToString()),
                new Claim(ClaimTypes.DateOfBirth, user.BirthDate.ToString("O")),
                new Claim(ClaimTypes.Email, user.Email)
            };
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
            }



            // - Créer un objet ClaimsIdentity
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);



            // - Créer un objet ClaimsPrincipal
            var principal = new ClaimsPrincipal(identity);



            // - Utiliser HttpContext pour le SIgnIn du principal
            await accessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties()
                {
                    IsPersistent = rememberme
                });
        }



        public async Task LogoutAsync()
        {
            if (accessor.HttpContext!.User.Identity!.IsAuthenticated)
            {
                await accessor.HttpContext.SignOutAsync();
            }
        }

        public async Task RegisterAsync(string email, string password, DateTime birthday, string genre)
        {
            UserGenre genretab = UserGenre.Male;

            if (genre == "Female")
                genretab = UserGenre.Female;

            var newUser = new User
            {
                Email = email,
                HashedPassword = await Helpers.HashPasswordAsync(password),
                BirthDate = birthday,
                Genre = genretab
            };

            dbContext.Users.Add(newUser);
            await dbContext.SaveChangesAsync();
        }



    }
}
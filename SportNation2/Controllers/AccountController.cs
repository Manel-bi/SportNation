using SportNation2.Models;
using Microsoft.AspNetCore.Mvc;
using SportNation2.Data.Models;
using SportNation2.Services;
using Microsoft.AspNetCore.Authorization;

namespace SportNation2.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;



        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }



        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]

        public async Task<IActionResult> Login(LoginFormViewModel model)
        //public IActionResult Login(string email, string password, bool rememberme)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                await accountService.LoginAsync(model.Email, model.Password, model.RememberMe);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View();
            }
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await accountService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }



        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                await accountService.RegisterAsync(model.Email, model.Password, model.BirthDate, model.Genre);



                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View();
            }


        }
    }
}

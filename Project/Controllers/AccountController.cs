using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Models;

namespace Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View();
            var existUser = await _userManager.FindByNameAsync(model.Username);
            if (existUser is null)
            {
                ModelState.AddModelError("", "invalid login");
                return View();
            }
            var signResult = await _signInManager.PasswordSignInAsync(existUser, model.Password, model.RememberMe, false);
            if (!signResult.Succeeded) 
            {
                ModelState.AddModelError("", "invalid password");
                return View();
            }

            return RedirectToAction("Index","home");

        }

        public IActionResult AccessDenied()
        {
            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index","home");
        }
        
    }
}

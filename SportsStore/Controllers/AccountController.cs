using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsStore.ViewModels;

namespace SportsStore.Controllers;


public class AccountController : Controller
{
    private UserManager<IdentityUser> _userManager;
    private SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public ViewResult Login(string returnUrl)
    {
        return View(new LoginModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        if (ModelState.IsValid)
        {
            IdentityUser user = await _userManager.FindByNameAsync(loginModel.Name);
            if (user != null)
            {
                await _signInManager.SignOutAsync();
                var signSuccess = await _signInManager.PasswordSignInAsync(loginModel.Name, loginModel.Password, false, false);
                if (signSuccess.Succeeded)
                {
                    return Redirect(loginModel?.ReturnUrl ?? "/Admin");
                }
            }
            ModelState.AddModelError("", "Invalid name or password");
        }
        return View(loginModel);
    }

    [Authorize]
    public async Task<RedirectResult> Logout(string returnUrl = "/")
    {
        await _signInManager.SignOutAsync();
        return Redirect(returnUrl);
    }

}
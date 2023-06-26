using Bookshop_Project.Controllers;
using Bookshop_Project.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

public class UserController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public UserController(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View("~/Areas/Identity/Pages/Account/Login.cshtml");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginInputModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,
                new ClaimsPrincipal(identity));
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        else
        {
            ModelState.AddModelError("", "Invalid UserName or Password");
            return View("~/Areas/Identity/Pages/Account/Login.cshtml", model);
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View("~/Areas/Identity/Pages/Account/Register.cshtml");
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterInputModel model)
    {
        if (ModelState.IsValid)
        {
            var passwordHasher = new PasswordHasher<User>();
            var user = new User { UserName = model.Email, Email = model.Email};
            user.PasswordHash = passwordHasher.HashPassword(user, model.Password);
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }

        return View("~/Areas/Identity/Pages/Account/Register.cshtml", model);
    }
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}

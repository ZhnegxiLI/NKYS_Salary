using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NKYS.Account.Model;

namespace NKYS.Account.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<User> _signManager;
        private UserManager<User> _userManager;
        public AccountController(UserManager<User> userManager, SignInManager<User> signManager)
        {
            _userManager = userManager;
            _signManager = signManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            if (User.Identity.IsAuthenticated != true)
            {
                var model = new LoginViewModel { ReturnUrl = returnUrl };
                return View(model);
            }
            else
            {
                if (returnUrl==null|| returnUrl =="")
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(returnUrl);
                }
            }
      
        }

        [HttpGet]
        public IActionResult Forbidden()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user==null)
                {
                    ModelState.AddModelError(string.Empty, "该账号不存在，请重新输入");
                    return View(model);
                }
                else if (user.Validity == false)
                {
                    ModelState.AddModelError(string.Empty, "账号未被激活，请联系管理员激活账号");
                    return View(model);
                }

                var result = await _signManager.PasswordSignInAsync(model.Username,
                   model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return Redirect("/home");
                    }
             
                }
                else
                {

                    ModelState.AddModelError(string.Empty, "账号密码错误");
                    return View(model);
                }
            }

            if (string.IsNullOrEmpty(model.ReturnUrl))
            {
                model.ReturnUrl = "";
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }

        [HttpGet]
        public ViewResult Signup()
        {
            return View();
        }

        [HttpGet]
        public ViewResult RegistreSucess()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Signup(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Username };
                user.Validity = false;

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Default user role : Employee
                    await _userManager.AddToRoleAsync(user, "Employee");
                    // Login
                    return RedirectToAction("RegistreSucess", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
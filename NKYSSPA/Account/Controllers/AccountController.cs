using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NKYSSPA.Account.Model;
using NKYSSPA.Models;

namespace NKYSSPA.Account.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private SignInManager<User> _signManager;
        private UserManager<User> _userManager;
        private readonly Context _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signManager, Context context)
        {
            _userManager = userManager;
            _signManager = signManager;
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        //[HttpGet]
        //public IActionResult Login(string returnUrl = "")
        //{
        //    if (User.Identity.IsAuthenticated != true)
        //    {
        //        var model = new LoginViewModel { ReturnUrl = returnUrl };
        //        return View(model);
        //    }
        //    else
        //    {
        //        if (returnUrl==null|| returnUrl =="")
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            return Redirect(returnUrl);
        //        }
        //    }

        //}

        //[HttpGet]
        //public IActionResult Forbidden()
        //{
        //    return View();
        //}
        [Route("[action]")]
        [HttpPost]
        public async Task<object> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user==null)
                {
                    throw new ApiException("该账号不存在，请重新输入", 400);
                 
                }
                else if (user.Validity == false)
                {

                    throw new ApiException("账号未被激活，请联系管理员激活账号", 400);
                }

                var result = await _signManager.PasswordSignInAsync(model.Username,
                   model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    // Get token(cookies for the identification) here and resend to front-end
                    HttpContext.Response.Headers.TryGetValue("Set-Cookie",out var token);
                    var roles = (from r in _context.Roles
                                 join ur in _context.UserRoles on r.Id equals ur.RoleId
                                 where ur.UserId == user.Id
                                 select r).ToList();
                    return new {
                        user.Id,
                        token = token
                    };
                }
                else
                {
                    throw new ApiException("账号密码错误", 400);
                }
            }

            throw new ApiProblemDetailsException(ModelState); ;
        }

        [Route("[action]")]
        [Authorize]
        [HttpGet]
        public async Task<dynamic> GetUserInfoAsync()
        {
            var username = HttpContext.User.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);
            var Roles = await _userManager.GetRolesAsync(user);
            return new { 
                user.Id,
                user.UserName,
                user.Validity,
                Roles
            };
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

        [Route("[action]")]
        [Authorize]
        [HttpPost]
        public async Task Logout()
        {
            await _signManager.SignOutAsync();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Chest.Models;
using Chest.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly EmailService _emailService;

        public AccountController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //Глава 12: Работа со строками Regex. ASP.NET Core Глава 20 :Регистрация и создание пользователей в Identity
        //Глава 18: Асинхронное программирование
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm]string email, [FromForm]string password)
        {
            string regexEmailPattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                       @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
            if (Regex.IsMatch(email, regexEmailPattern, RegexOptions.IgnoreCase))
            {
                User user = new User { Email = email, UserName = email };
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("GetAllGoods", "Goods");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(String.Empty, error.Description);
                    }
                }
            }
            else
            {
                return new ObjectResult("Incorrect Email");
            }

            return RedirectToAction("GetGoodsByCategory", "Goods");
        }

        //ASP.NET Core Глава 20 : Авторизация пользователей в Identity
        //Глава 18: Асинхронное программирование
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm]string email, [FromForm]string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
            if (result.Succeeded)
            {
                return RedirectToAction("GetAllGoods", "Goods");
            }
            else
            {
                return new ObjectResult("Incorrect email and (or) password");
            }
        }

        //Logoff user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("GetAllGoods", "Goods");
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromForm]string email ,[FromForm]string newPassword)
        {
            User user = await _userManager.FindByEmailAsync(email);
            string message = $"<h2>You has been changed your password</h2><p>Your new password : {newPassword}</p>";
            if (user != null)
            {
                var passwordValidator =
                    HttpContext.RequestServices.GetService(
                        typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                var PasswordHasher =
                    HttpContext.RequestServices.GetService(
                        typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                var result = await passwordValidator.ValidateAsync(_userManager, user, newPassword);
                if (result.Succeeded)
                {
                    user.PasswordHash = PasswordHasher.HashPassword(user, newPassword);
                    await _userManager.UpdateAsync(user);
                    _emailService.SendEmail(email, "New Password", message);

                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(String.Empty, error.Description);
                    }
                }
            }
            else
            {
                return new ObjectResult("User not found");
            }

            return RedirectToAction("GetAllGoods", "Goods");
        }
    }
}
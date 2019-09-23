using System;
using System.Collections;
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
    public class UserController : Controller
    {

        private readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        //Коллекции. Очередь
        [HttpGet("UsersList")]
        public IActionResult Users()
        {
            Queue<User> usersQueue = new Queue<User>();
            foreach (var user in _userManager.Users.ToList())
            {
                usersQueue.Enqueue(user);
            }

            ViewBag.UsersQueue = usersQueue;
            return View();
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            User userToEdit = await _userManager.FindByIdAsync(id);
            HttpContext.Session.Set<User>($"{userToEdit.Email}", userToEdit);
            return View();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm]string email, [FromForm]string password)
        {
            string regexEmailPattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                       @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
            if (Regex.IsMatch(email, regexEmailPattern))
            {
                User user = new User
                {
                    Email = email,
                    UserName = email
                };
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Users");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    
                }
            }
            else
            {
                return Content("User not created");
            }

            return RedirectToAction("GetGoodsByCategory", "Goods");
        }


        [HttpPost("Edit")]
        public async Task<IActionResult> PostEdit([FromForm] string email)
        {
            User userToEdit = null;
            foreach (var key in HttpContext.Session.Keys)
            {
                if (_userManager.Users.Any(user => user.Email == key))
                {
                    userToEdit = HttpContext.Session.Get<User>(key);
                }
            }
            if (userToEdit != null)
            {
                userToEdit.Email = email;
                userToEdit.UserName = email;

                var result = await _userManager.UpdateAsync(userToEdit);
                if (result.Succeeded)
                {
                    return RedirectToAction("Users");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            

            return RedirectToAction("GetAllGoods", "Goods");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction("Users");
        }



    }
}
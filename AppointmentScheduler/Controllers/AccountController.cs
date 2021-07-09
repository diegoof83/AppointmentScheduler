﻿using AppointmentScheduler.Models;
using AppointmentScheduler.Models.ViewModels;
using AppointmentScheduler.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduler.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly SignInManager<ApplicationIdentityUser> _signInManager;

        public AccountController(UserManager<ApplicationIdentityUser> userManager,
            SignInManager<ApplicationIdentityUser> signInManager)
        {            
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logoff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    //Adding the logged user into the session
                    var user = await _userManager.FindByNameAsync(model.Email);
                    HttpContext.Session.SetString(Helper.LoggedUserSession, user.FullName);

                    return RedirectToAction("Index", "Appointment");
                }
                ModelState.AddModelError("", "Invalid email or password.");
            }
            return View(model);
        }

        public async Task<IActionResult> Register()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationIdentityUser
                {
                    //using email as the user name for the Login key
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName                    
                };

                //create a new user
                var result = await _userManager.CreateAsync(user, model.Password);
                                
                if (result.Succeeded)
                {
                    //set the user's role
                    await _userManager.AddToRoleAsync(user, model.RoleName);

                    //sign in the new user in case user is not Admin(The admin could be registering others user only)
                    if (!User.IsInRole(Helper.Admin))
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                    }
                    else
                    {
                        TempData[Helper.TempDataNewCreatedUserName] = user.FullName;
                    }                    
                    return RedirectToAction("Index", "Appointment");
                }
                //show message error in case they happen
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }
    }
}

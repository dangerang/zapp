using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Coddit.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Coddit.Models;
using Coddit.Models.Entities;

namespace Coddit.Controllers
{
    public class AccountController : Controller
    {
        UserManager<IdentityUser> _accountManager;
        SignInManager<IdentityUser> _signInManager;
        IdentityDbContext _context;
        DataManager _dm;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IdentityDbContext identityContext,
            CodditContext dbContext)
        {
            _accountManager = userManager;
            _signInManager = signInManager;
            _context = identityContext;
            _dm = new DataManager(dbContext);

            _context.Database.EnsureCreated();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(LandingController.Index), "Landing");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Profile()
        {
            var viewModel = _dm.GetProfile(User.Identity.Name);
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Edit()
        {
            var viewModel = _dm.GetEditProfile(User.Identity.Name);

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(AccountEditVM viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            _dm.EditAccount(viewModel, User.Identity.Name);

            return RedirectToAction(nameof(AccountController.Profile));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Remove()
        {
            _dm.RemoveAccount(User.Identity.Name);

            return RedirectToAction(nameof(AccountController.Logout));
        }

    }
}
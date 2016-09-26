using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Coddit.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Coddit.Models;
using Coddit.Models.Entities;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Coddit.Controllers
{
    public class LandingController : Controller
    {
        SignInManager<IdentityUser> _signInManager;
        UserManager<IdentityUser> _accountManager;
        IdentityDbContext _context;
        DataManager _dm;

        public LandingController(
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
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(IdeasController.Index), "ideas");
            return View(new LandingIndexVM());
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Project()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Project(IdeaCreateVM viewModel, string newTags)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(LandingController.Project));
            }

            if (!User.Identity.IsAuthenticated)
            {
                //var indexViewModel = _dm.GetIndexVM(User.Identity.Name);
                //indexViewModel.IdeasCreateModel = viewModel;
                ModelState.AddModelError("", "You have to log in to start a project");
                return View(nameof(LandingController.Project));
            }

            _dm.AddIdea(viewModel, newTags?.Split(','), User.Identity.Name);

            return RedirectToAction(nameof(IdeasController.Index));
        }


        [HttpPost]
        public IActionResult Create(IdeaCreateVM viewModel, string newTags)
        {
            if (!ModelState.IsValid)
            {
                //var indexViewModel = _dm.GetIndexVM(User.Identity.Name);
                //indexViewModel.IdeasCreateModel = viewModel;

                return View(nameof(LandingController.Project));
            }

            _dm.AddIdea(viewModel, newTags?.Split(','), User.Identity.Name);

            return RedirectToAction(nameof(IdeasController.Index));
        }

        [HttpPost]
        public async Task<IActionResult> Login(LandingLoginVM viewModel)
        {
            if (!ModelState.IsValid)
                return View("Index", new LandingIndexVM { Login = viewModel });

            var loginResult = await _signInManager.PasswordSignInAsync(
                viewModel.Username, viewModel.Password, false, false);

            if (!loginResult.Succeeded)
            {
                ModelState.AddModelError("Username", "Invalid credentials");
                return View(nameof(IdeasController.Index), new LandingIndexVM { Login = viewModel });
            }

            return RedirectToAction(nameof(IdeasController.Index), "ideas");
        }

        [HttpPost]
        public async Task<IActionResult> Register(LandingRegisterVM viewModel)
        {
            if (viewModel == null)
                viewModel = new LandingRegisterVM();

            if (!ModelState.IsValid)
                return View(nameof(IdeasController.Index), new LandingIndexVM { Register = viewModel });

            var createUserResult = await _accountManager.CreateAsync(new IdentityUser(viewModel.NewUsername), viewModel.NewPassword);

            if (!createUserResult.Succeeded)
            {
                ModelState.AddModelError("Username", createUserResult.Errors.First().Description);
                return View("Index", new LandingIndexVM { Register = viewModel });
            }

            _dm.AddAccount(viewModel);
            ModelState.AddModelError("", "Registration Succeeded!");
            return View("Index", new LandingIndexVM { Register = viewModel });

            //return RedirectToAction(nameof(LandingController.Index));
        }
    }
}


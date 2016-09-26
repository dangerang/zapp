using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Coddit.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Coddit.Models;
using Coddit.Models.Entities;

namespace Coddit.Controllers
{
    [Authorize]
    public class IdeasController : Controller
    {
        DataManager _dm;

        public IdeasController(CodditContext context)
        {
            _dm = new DataManager(context);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var viewModel = _dm.GetIndexVM(User.Identity.Name);

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Create(LandingProjectVM viewModel, string newTags)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var indexViewModel = _dm.GetIndexVM(User.Identity.Name);
        //        indexViewModel.LandingProjectModel = viewModel;

        //        return View(nameof(IdeasController.Index), indexViewModel);
        //    }

        //    _dm.AddIdea(viewModel, newTags?.Split(','), User.Identity.Name);

        //    return RedirectToAction(nameof(IdeasController.Index));
        //}

        [HttpGet]
        public IActionResult Join(int id)
        {
            bool joinSucceeded = _dm.JoinIdea(User.Identity.Name, id);

            return RedirectToAction(nameof(IdeasController.Index));
        }

        [HttpGet]
        public IActionResult Leave(int id)
        {
            _dm.LeaveIdea(User.Identity.Name, id);

            return RedirectToAction(nameof(IdeasController.Index));
        }

        [HttpGet]
        public IActionResult Remove(int id)
        {
            _dm.RemoveIdea(id);
            return RedirectToAction(nameof(IdeasController.Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var viewModel = _dm.GetEditIdea(id);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(int id, IdeaEditVM viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            _dm.EditIdea(id, viewModel);
            return RedirectToAction(nameof(IdeasController.Index));
        }

        [HttpPost]
        public IActionResult Upvote(int id)
        {
            bool result = _dm.Upvote(id, User.Identity.Name);

            if (!result)
                _dm.RemoveUpvote(id, User.Identity.Name);

            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public IActionResult Comment(int id)
        {
            var viewModel = _dm.GetCommentList(id, User.Identity.Name);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Comment(AddCommentVM viewModel, int id)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            _dm.AddComment(id, User.Identity.Name, viewModel.Text);

            return RedirectToAction(nameof(IdeasController.Comment));
        }

        [HttpPost]
        public IActionResult RemoveComment(int id)
        {
            bool result = _dm.RemoveComment(id);

            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public IActionResult OrderByRating()
        {
            var viewModel = _dm.GetIndexVM(User.Identity.Name, true);

            return View(nameof(IdeasController.Index), viewModel);
        }
    }
}
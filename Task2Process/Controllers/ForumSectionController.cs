using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task2Process.Data;
using Task2Process.Models;
using Task2Process.Services;
using Task2Process.ViewModels;

namespace Task2Process.Controllers
{
	public class ForumSectionController : Controller
	{
		private readonly UserManager<User> _userManager;
		private IForumSectionService ForumSectionService { get; }
		private IAdministrationService AdministrationService { get; }
		public ForumSectionController(UserManager<User> userManager, IForumSectionService forumSectionService, IAdministrationService administrationService)
		{
			ForumSectionService = forumSectionService;
			_userManager = userManager;
			AdministrationService = administrationService;
		}
		// GET: ForumSectionController
		public ActionResult Index()
		{
			return View(ForumSectionService.GetIndexViewModel(_userManager.GetUserId(User)));
		}

		// GET: ForumSectionController/Details/5
		public ActionResult Details(int id)
		{
			var viewModel = ForumSectionService.GetViewModel(id);
			if (viewModel == null)
			{
				return RedirectToAction(nameof(Index));
			}
			return View(viewModel);
		}

		// GET: ForumSectionController/Create
		[Authorize(Constants.AdminPolicy)]
		public ActionResult Create()
		{
			return View(ForumSectionService.GetCreateViewModel());
		}

		// POST: ForumSectionController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Constants.AdminPolicy)]
		public ActionResult Create(ForumSectionCreateViewModel viewModel)
		{
			try
			{
				ForumSectionService.Create(viewModel);
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: ForumSectionController/Edit/5
		[Authorize]
		public ActionResult Edit(int id)
		{
			var currentUserId = _userManager.GetUserId(User);
			var viewModel = ForumSectionService.GetEditViewModel(id);
			if (AdministrationService.IsAdminOrMod(currentUserId, viewModel.Id))
			{
				return View(viewModel);
			}
			else
			{
				return Forbid();
			}
		}

		// POST: ForumSectionController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Constants.AdminPolicy)]
		public ActionResult Edit(ForumSectionEditViewModel viewModel)
		{
			try
			{
				var currentUserId = _userManager.GetUserId(User);

				if (AdministrationService.IsAdminOrMod(currentUserId, viewModel.Id))
				{
					ForumSectionService.Edit(viewModel);
					return RedirectToAction(nameof(Index));
				}
				else
				{
					return Forbid();
				}
			}
			catch
			{
				return View();
			}
		}

		// GET: ForumSectionController/Delete/5
		[Authorize(Constants.AdminPolicy)]
		public ActionResult Delete(int id)
		{
			var viewModel = ForumSectionService.GetViewModel(id);
			var currentUserId = _userManager.GetUserId(User);
			if (AdministrationService.IsAdminOrMod(currentUserId, viewModel.Id))
			{
				if (viewModel == null)
				{
					return RedirectToAction(nameof(Index));
				}
				return View(viewModel);
			}
			else
			{
				return Forbid();
			}
		}

		// POST: ForumSectionController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Constants.AdminPolicy)]
		public ActionResult Delete(ForumSectionViewModel viewModel)
		{
			try
			{
				var currentUserId = _userManager.GetUserId(User);
				if (AdministrationService.IsAdminOrMod(currentUserId, viewModel.Id))
				{
					ForumSectionService.Delete(viewModel);
					return RedirectToAction(nameof(Index));
				}
				else
				{
					return Forbid();
				}
			}
			catch (Exception e)
			{
				return View(viewModel);
			}
		}
	}
}

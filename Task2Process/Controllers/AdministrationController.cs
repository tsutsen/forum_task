using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Task2Process.Models;
using Task2Process.Services;
using Task2Process.ViewModels;

namespace Task2Process.Controllers
{
	public class AdministrationController : Controller
	{
		private IAdministrationService AdministrationService { get; }
		public AdministrationController(IAdministrationService administrationService)
		{
			AdministrationService = administrationService;
		}

		// GET: AdministrationController
		public ActionResult Index(int sectionId)
		{
			//var users = _userManager.GetUsersForClaimAsync(new Claim(Constants.ModeratorClaimName, "")).Result;
			return View(AdministrationService.GetIndexViewModel(sectionId));
		}

		public ActionResult Add(int sectionId, string userId, bool isModerator)
		{
			return View(AdministrationService.GetViewModel(sectionId, userId, isModerator));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Add(AdministrationViewModel viewModel)
		{
			try
			{
				AdministrationService.AddModerator(viewModel);
				return RedirectToAction("Index", "Administration", new { sectionId = viewModel.ForumSectionId });
			}
			catch
			{
				return View();
			}
		}

		public ActionResult Remove(int sectionId, string userId, bool isModerator)
		{
			return View(AdministrationService.GetViewModel(sectionId, userId, isModerator));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Remove(AdministrationViewModel viewModel)
		{
			try
			{
				AdministrationService.RemoveModerator(viewModel);
				return RedirectToAction("Index", "Administration", new { sectionId = viewModel.ForumSectionId });
			}
			catch
			{
				return View();
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Update(ModeratedSectionsIndexViewModel viewModel)
		{
			try
			{
				//_userManager.RemoveClaimAsync(user, new Claim(Constants.ModeratorClaimName, ""));
				//_userManager.AddClaimAsync(moderator, new Claim(Constants.ModeratorClaimName, "")).Result;
				AdministrationService.Update(viewModel);
				return RedirectToAction("Index", "ForumSection", new { sectionId = viewModel.CurrentSectionId });
			}
			catch
			{
				return View();
			}
		}
	}
}

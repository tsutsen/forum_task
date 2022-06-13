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
	public class TopicController : Controller
	{
		private readonly UserManager<User> _userManager;
		private ITopicService TopicService { get; }
		private IAdministrationService AdministrationService { get; }
		public TopicController(UserManager<User> userManager, ITopicService topicService, IAdministrationService administrationService)
		{
			TopicService = topicService;
			_userManager = userManager;
			AdministrationService = administrationService;
		}
		// GET: TopicController
		public ActionResult Index(int sectionId)
		{
			return View(TopicService.GetIndexViewModel(sectionId, _userManager.GetUserId(User)));
		}

		// GET: TopicController/Details/5
		public ActionResult Details(int id)
		{
			var viewModel = TopicService.GetViewModel(id);
			if (viewModel == null)
			{
				return RedirectToAction(nameof(Index));
			}
			return View(viewModel);
		}

		// GET: TopicController/Create
		[Authorize]
		public ActionResult Create(int sectionId)
		{
			var currentUserId = _userManager.GetUserId(User);
			var viewModel = TopicService.GetCreateViewModel(sectionId, currentUserId);
			return View(viewModel);
		}

		// POST: TopicController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public ActionResult Create(TopicCreateViewModel viewModel)
		{
			try
			{
				TopicService.Create(viewModel);
				return RedirectToAction("Index","Topic", new { sectionId = viewModel.SectionId });
			}
			catch (Exception e)
			{
				return View(viewModel);
			}
		}

		// GET: TopicController/Edit/5
		public ActionResult Edit(int id)
		{
			var currentUserId = _userManager.GetUserId(User);
			var viewModel = TopicService.GetEditViewModel(id);

			if (AdministrationService.IsAdminOrModOrAuthor(currentUserId, viewModel.SectionId, viewModel.AuthorId))
			{
				return View(viewModel);
			}
			else
			{
				return Forbid();
			}
		}

		// POST: TopicController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(TopicEditViewModel viewModel)
		{

			try
			{
				if (AdministrationService.IsAdminOrModOrAuthor(_userManager.GetUserId(User), viewModel.SectionId, viewModel.AuthorId))

				{
					TopicService.Edit(viewModel);
					return RedirectToAction("Index", "Topic", new { sectionId = viewModel.SectionId });
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

		// GET: TopicController/Delete/5
		public ActionResult Delete(int id)
		{
			var viewModel = TopicService.GetViewModel(id);
			var currentUserId = _userManager.GetUserId(User);

			if (AdministrationService.IsAdminOrModOrAuthor(currentUserId, viewModel.SectionId, viewModel.AuthorId))
			{
				return View(viewModel);
			}
			else
			{
				return Forbid();
			}
		}

		// POST: TopicController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(TopicViewModel viewModel)
		{
			try
			{
				if (AdministrationService.IsAdminOrModOrAuthor(_userManager.GetUserId(User), viewModel.SectionId, viewModel.AuthorId))

				{
					TopicService.Delete(viewModel);
					return RedirectToAction("Index", "Topic", new { sectionId = viewModel.SectionId });
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

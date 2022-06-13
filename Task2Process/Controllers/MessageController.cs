using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Task2Process.Data;
using Task2Process.Models;
using Task2Process.Services;
using Task2Process.ViewModels;

namespace Task2Process.Controllers
{
	public class MessageController : Controller
	{
		private readonly UserManager<User> _userManager;
		private IMessageService MessageService { get; }
		IAdministrationService AdministrationService { get; }
		private IWebHostEnvironment AppEnvironment { get; }

		private static readonly string[] AllowedExtensions = { "jpeg", "jpg", "png" };
		public MessageController(IWebHostEnvironment appEnvironment, UserManager<User> userManager, IMessageService messageService, IAdministrationService administrationService)
		{
			_userManager = userManager;
			AppEnvironment = appEnvironment;
			MessageService = messageService;
			AdministrationService = administrationService;
		}
		// GET: MessageController
		public ActionResult Index(int topicId)
		{
			//var isAuthorized = AdministrationService.IsAdminOrModOrAuthor(,);
			return View(MessageService.GetIndexViewModel(topicId, _userManager.GetUserId(User)));
		}

		// GET: MessageController/Details/5
		public ActionResult Details(int id)
		{
			var viewModel = MessageService.GetViewModel(id);
			if (viewModel == null)
			{
				return RedirectToAction("Index", "Message", new { topicId = viewModel.TopicId });
			}
			return View(viewModel);
		}

		// GET: MessageController/Create
		[Authorize]
		public ActionResult Create(int topicId)
		{
			var currentUserId = _userManager.GetUserId(User);
			return View(MessageService.GetCreateViewModel(topicId, currentUserId));
		}

		// POST: MessageController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public ActionResult Create(MessageCreateViewModel viewModel, List<IFormFile> files, int topicId)
		{
			try
			{
				MessageService.Create(viewModel, files);
				return RedirectToAction("Index", "Message", new { topicId = topicId });
			}
			catch (Exception e)
			{
				return View();
			}
		}

		[Authorize]
		// GET: MessageController/Edit/5
		public ActionResult Edit(int id)
		{
			var viewModel = MessageService.GetEditViewModel(id);

			if (AdministrationService.IsAdminOrModOrAuthor(_userManager.GetUserId(User), viewModel.SectionId, viewModel.AuthorId))
			{
				return View(viewModel);
			}
			else
			{
				return Forbid();
			}
		}

		// POST: MessageController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public ActionResult Edit(MessageEditViewModel viewModel, List<IFormFile> files)
		{
			try
			{
				if (AdministrationService.IsAdminOrModOrAuthor(_userManager.GetUserId(User), viewModel.SectionId, viewModel.AuthorId))
				{
					MessageService.Edit(viewModel, files);
					return RedirectToAction("Index", "Message", new { topicId = viewModel.TopicId });
				}
				else
				{
					return Forbid();
				}

			}
			catch
			{
				return View(viewModel);
			}
		}

		// GET: MessageController/Delete/5
		[Authorize]
		public ActionResult Delete(int id)
		{
			var viewModel = MessageService.GetViewModel(id);

			if (AdministrationService.IsAdminOrModOrAuthor(_userManager.GetUserId(User), viewModel.SectionId, viewModel.AuthorId))
			{
				if (viewModel == null)
				{
					return RedirectToAction("Index", "ForumSection");
				}
				return View(viewModel);
			}
			else
			{
				return Forbid();
			}
		}

		// POST: MessageController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public ActionResult Delete(MessageViewModel viewModel)
		{
			try
			{
				if (AdministrationService.IsAdminOrModOrAuthor(_userManager.GetUserId(User), viewModel.SectionId, viewModel.AuthorId))
				{
					MessageService.Delete(viewModel);
					return RedirectToAction("Index", "Message", new { topicId = viewModel.TopicId });
				}
				else
				{
					return Forbid();
				}
			}
			catch
			{
				return View(viewModel);
			}
		}
	}
}

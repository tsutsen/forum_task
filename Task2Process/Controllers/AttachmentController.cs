using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
	public class AttachmentController : Controller
	{
		private IWebHostEnvironment AppEnvironment { get; }
		private IAttachmentService AttachmentService { get; }

		public AttachmentController(IWebHostEnvironment appEnvironment, IAttachmentService attachmentService)
		{
			AttachmentService = attachmentService;
			AppEnvironment = appEnvironment;
		}


		// GET: AttachmentController/Details/5
		public ActionResult Details(int id)
		{
			var viewModel = AttachmentService.GetViewModel(id); 
			if (viewModel == null)
			{
				return RedirectToAction("Details", "Message", new { id = viewModel.MessageId });
			}
			return View(viewModel);
		}

		// GET: AttachmentController/Delete/5
		public ActionResult Delete(int id)
		{
			var model = AttachmentService.GetViewModel(id);
			if (model == null)
			{
				return RedirectToAction("Index", "ForumSection");
			}
			return View(model);
		}

		// POST: AttachmentController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(AttachmentViewModel viewModel)
		{
			try
			{
				AttachmentService.Delete(viewModel);
				return RedirectToAction("Edit", "Message", new { id = viewModel.MessageId });
			}
			catch
			{
				return View(viewModel);
			}
		}
	}
}

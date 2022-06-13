using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Task2Process.Data;
using Task2Process.Models;
using Task2Process.ViewModels;

namespace Task2Process.Services
{
	public interface IAttachmentService
	{
		AttachmentViewModel GetViewModel(int id);
		void Delete(AttachmentViewModel model);
	}

	public class AttachmentService : IAttachmentService
	{
		private ApplicationDbContext ApplicationDbContext { get; }
		private IWebHostEnvironment AppEnvironment { get; }


		public AttachmentService(ApplicationDbContext applicationDbContext, IWebHostEnvironment appEnvironment)
		{
			ApplicationDbContext = applicationDbContext;
			AppEnvironment = appEnvironment;
		}

		public void Delete(AttachmentViewModel model)
		{
			var attachment = ApplicationDbContext.Attachments.Include(x => x.Message).FirstOrDefault(x => x.Id == model.Id);
			ApplicationDbContext.Attachments.Remove(attachment);
			ApplicationDbContext.SaveChanges();
		}


		public AttachmentViewModel GetViewModel(int id)
		{
			var model = ApplicationDbContext.Attachments.Include(x => x.Message).ThenInclude(x => x.Author).FirstOrDefault(x => x.Id == id);
			return new AttachmentViewModel
			{
				Id = id,
				UserId = model.Message.Author.Id,
				UserName = model.Message.Author.UserName,
				MessageId = model.Message.Id,
				FileName = model.FileName
			};
		}
	}
}

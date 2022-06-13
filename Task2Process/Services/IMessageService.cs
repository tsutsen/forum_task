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
using Task2Process.DtoModels;
using Task2Process.Models;
using Task2Process.ViewModels;

namespace Task2Process.Services
{
	public interface IMessageService
	{
		MessageViewModel GetViewModel(int id);
		MessageIndexViewModel GetIndexViewModel(int topicId, string currentUserId);
		MessageCreateViewModel GetCreateViewModel(int topicId, string currentUserId);
		MessageEditViewModel GetEditViewModel(int id);
		void Create(MessageCreateViewModel model, List<IFormFile> files);
		void Edit(MessageEditViewModel model, List<IFormFile> files);
		void Delete(MessageViewModel model);
		Task<List<MessageDto>> GetMessages(int topicId);
		Task AddMessage(MessageCreateEditDto model, int topicId);
		Task EditMessage(MessageCreateEditDto model, int id);
		Task DeleteMessage(int id);
	}

	public class MessageService : IMessageService
	{
		private ApplicationDbContext ApplicationDbContext { get; }
		IAdministrationService AdministrationService { get; }
		private IMapper Mapper { get; }
		private IWebHostEnvironment AppEnvironment { get; }

		private static readonly string[] AllowedExtensions = { "jpeg", "jpg", "png" };

		public MessageService(ApplicationDbContext applicationDbContext, IMapper mapper, IWebHostEnvironment appEnvironment, IAdministrationService administrationService)
		{
			ApplicationDbContext = applicationDbContext;
			Mapper = mapper;
			AdministrationService = administrationService;
			AppEnvironment = appEnvironment;
		}
		public MessageCreateViewModel GetCreateViewModel(int topicId, string currentUserId)
		{
			return new MessageCreateViewModel
			{
				AuthorId = currentUserId,
				TopicId = topicId
			};
		}
		public void Create(MessageCreateViewModel model, List<IFormFile> files)
		{
			var message = new Message
			{
				Text = model.Text,
				Created = DateTime.Now,
				Topic = ApplicationDbContext.Topics.FirstOrDefault(x => x.Id == model.TopicId),
			};

			if (files != null && files.Any())
			{

				message.Attachments = new List<Attachment>();
				foreach (var file in files)
				{
					var extension = Path.GetExtension(file.FileName)?.Replace(".", "");
					if (AllowedExtensions.Contains(extension))
					{
						var fileId = Guid.NewGuid();
						var path = $"Attachments/{fileId}_{file.FileName}";
						using (var fileStream = new FileStream(Path.Combine(AppEnvironment.WebRootPath, path),
							FileMode.Create))
						{
							file.CopyTo(fileStream);
						}
						var attachment = new Attachment
						{
							FileName = path,
						};
						message.Attachments.Add(attachment);
						ApplicationDbContext.Attachments.Add(attachment);
						
					}
				}
			}

			if (model.AuthorId != null)
			{
				message.Author = ApplicationDbContext.Users.FirstOrDefault(x => x.Id == model.AuthorId);
			}

			ApplicationDbContext.Messages.Add(message);
			ApplicationDbContext.SaveChanges();
		}
		public MessageEditViewModel GetEditViewModel(int id)
		{
			var message = ApplicationDbContext.Messages.Include(x => x.Topic).Include(x => x.Attachments).Include(x => x.Author).FirstOrDefault(x => x.Id == id);
			var sectionId = ApplicationDbContext.ForumSections.FirstOrDefault(x => x.Id == ApplicationDbContext.Topics.Include(x => x.ForumSection).FirstOrDefault(x => x.Id == message.Topic.Id).ForumSection.Id).Id;
			return new MessageEditViewModel
			{
				Id = message.Id,
				Text = message.Text,
				TopicId = message.Topic.Id,
				AuthorId = message.Author.Id,
				Attachments = message.Attachments,
				SectionId = sectionId
			};
		}
		public void Edit(MessageEditViewModel model, List<IFormFile> files)
		{
			var message = ApplicationDbContext.Messages.Include(x => x.Topic).Include(x => x.Attachments).Include(x => x.Author).FirstOrDefault(x => x.Id == model.Id);
			message.Text = model.Text;
			message.Modified = DateTime.Now;
			if (files != null && files.Any())
			{
				foreach (var file in files)
				{
					var extension = Path.GetExtension(file.FileName)?.Replace(".", "");
					if (AllowedExtensions.Contains(extension))

					{
						var fileId = Guid.NewGuid();
						var path = $"Attachments/{fileId}_{file.FileName}";
						using (var fileStream = new FileStream(Path.Combine(AppEnvironment.WebRootPath, path),
							FileMode.Create))
						{
							file.CopyTo(fileStream);
						}
						var attachment = new Attachment
						{
							FileName = path,
						};
						message.Attachments.Add(attachment);
						ApplicationDbContext.Attachments.Add(attachment);
					}
				}
			}
			ApplicationDbContext.SaveChanges();
		}
		public MessageViewModel GetViewModel(int id)
		{
			var message = ApplicationDbContext.Messages.Include(x => x.Author).Include(x => x.Topic).Include(x => x.Attachments).FirstOrDefault(x => x.Id == id);
			var sectionId = ApplicationDbContext.ForumSections.FirstOrDefault(x => x.Id == ApplicationDbContext.Topics.Include(x => x.ForumSection).FirstOrDefault(x => x.Id == message.Topic.Id).ForumSection.Id).Id;
			var viewModel = new MessageViewModel
			{
				Id = message.Id,
				Created = message.Created,
				Modified = message.Modified,
				TopicId = message.Topic.Id,
				Attachments = message.Attachments,
				SectionId = sectionId
			};
			if (message.Author != null)
			{
				viewModel.AuthorId = message.Author.Id;
				viewModel.AuthorUserName = message.Author.UserName;
			}
			return viewModel;
		}
		public void Delete(MessageViewModel model)
		{
			var message = ApplicationDbContext.Messages.FirstOrDefault(x => x.Id == model.Id);
			ApplicationDbContext.Messages.Remove(message);
			ApplicationDbContext.SaveChanges();
		}
		public MessageIndexViewModel GetIndexViewModel(int topicId, string currentUserId)
		{
			var messages = ApplicationDbContext.Messages.Include(x => x.Author).Include(x => x.Topic).Where(x => x.Topic.Id == topicId).ToList();
			var topic = ApplicationDbContext.Topics.Include(x => x.ForumSection).FirstOrDefault(x => x.Id == topicId);
			var viewModel = new MessageIndexViewModel
			{
				Messages = Mapper.Map<List<MessageShortViewModel>>(messages),
				TopicId = topicId,
				TopicName = topic.Name,
				SectionId = topic.ForumSection.Id,
			};

			foreach (var message in viewModel.Messages)
			{
				message.IsAuthorized = AdministrationService.IsAdminOrModOrAuthor(currentUserId, viewModel.SectionId, message.AuthorId);
			}

			return viewModel;
		}

		public async Task<List<MessageDto>> GetMessages(int topicId)
		{
			var messages = await ApplicationDbContext.Messages
				.Include(x => x.Topic)
				.Where(x => x.Topic.Id == topicId)
				.ToListAsync();

			if (!messages.Any())
			{
				await ApplicationDbContext.Messages.AddAsync(new Message
				{
					Text = "Test Topic"
				});
			}

			return Mapper.Map<List<MessageDto>>(messages);
		}

		public async Task AddMessage(MessageCreateEditDto model, int topicId)
		{
			var adminId = ApplicationDbContext.UserClaims.FirstOrDefault(x => x.ClaimType == "Admin").UserId;
			var newMessage = new Message()
			{
				Topic = ApplicationDbContext.Topics.FirstOrDefault(x => x.Id == topicId),
				Text = model.Text,
				Created = DateTime.Now,
				Author = ApplicationDbContext.Users.FirstOrDefault(x => x.Id == adminId)
			};
			await ApplicationDbContext.Messages.AddAsync(newMessage);
			await ApplicationDbContext.SaveChangesAsync();
		}

		public async Task EditMessage(MessageCreateEditDto model, int id)
		{
			var message = ApplicationDbContext.Messages.FirstOrDefault(x => x.Id == id);
			if (message == null)
			{
				throw new KeyNotFoundException($"Message with id = {id} not found");
			}
			message.Text = model.Text;
			message.Modified = DateTime.Now;

			await ApplicationDbContext.SaveChangesAsync();
		}

		public async Task DeleteMessage(int id)
		{
			var message = ApplicationDbContext.Messages.FirstOrDefault(x => x.Id == id);
			if (message == null)
			{
				throw new KeyNotFoundException($"Message with id = {id} not found");
			}
			ApplicationDbContext.Messages.Remove(message);
			await ApplicationDbContext.SaveChangesAsync();
		}
	}
}

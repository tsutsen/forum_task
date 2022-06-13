using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Task2Process.Data;
using Task2Process.DtoModels;
using Task2Process.Models;
using Task2Process.ViewModels;

namespace Task2Process.Services
{
	public interface ITopicService
	{
		TopicViewModel GetViewModel(int id);
		TopicIndexViewModel GetIndexViewModel(int sectionId, string currentUserId);
		TopicCreateViewModel GetCreateViewModel(int sectionId, string currentUserId);
		TopicEditViewModel GetEditViewModel(int id);
		void Create(TopicCreateViewModel model);
		void Edit(TopicEditViewModel model);
		void Delete(TopicViewModel model);
		Task<List<TopicDto>> GetTopics(int sectionId);
		Task AddTopic(TopicCreateEditDto model, int sectionId);
		Task DeleteTopic(int topicId);
		Task EditTopic(TopicCreateEditDto model, int id);
	}

	public class TopicService : ITopicService
	{
		private ApplicationDbContext ApplicationDbContext { get; }
		IAdministrationService AdministrationService { get; }
		private IMapper Mapper { get; }
		private IWebHostEnvironment AppEnvironment { get; }

		public TopicService(ApplicationDbContext applicationDbContext, IMapper mapper, IWebHostEnvironment appEnvironment, IAdministrationService administrationService)
		{
			AdministrationService = administrationService;
			ApplicationDbContext = applicationDbContext;
			Mapper = mapper;
			AppEnvironment = appEnvironment;
		}
		public TopicCreateViewModel GetCreateViewModel(int sectionId, string currentUserId)
		{
			return new TopicCreateViewModel
			{
				AuthorId = currentUserId,
				SectionId = sectionId
			};
		}
		public void Create(TopicCreateViewModel model)
		{
			var topic = new Topic
			{
				Name = model.Name,
				Description = model.Description,
				Created = model.Created,
				ForumSection = ApplicationDbContext.ForumSections.FirstOrDefault(x => x.Id == model.SectionId),
			};

			if (model.AuthorId != null)
			{
				topic.Author = ApplicationDbContext.Users.FirstOrDefault(x => x.Id == model.AuthorId);
			}

			ApplicationDbContext.Topics.Add(topic);
			ApplicationDbContext.SaveChanges();
		}
		public TopicEditViewModel GetEditViewModel(int id)
		{
			var topic = ApplicationDbContext.Topics.Include(x => x.ForumSection).Include(x => x.Author).FirstOrDefault(x => x.Id == id);
			return new TopicEditViewModel
			{
				Id = topic.Id,
				Name = topic.Name,
				SectionId = topic.ForumSection.Id,
				Description = topic.Description,
				AuthorId = topic.Author.Id
			};
		}
		public void Edit(TopicEditViewModel model)
		{
			var topic = ApplicationDbContext.Topics.FirstOrDefault(x => x.Id == model.Id);
			topic.Name = model.Name;
			topic.Description = model.Description;
			topic.Modified = DateTime.Now;
			ApplicationDbContext.SaveChanges();
		}
		public TopicViewModel GetViewModel(int id)
		{
			var topic = ApplicationDbContext.Topics.Include(x => x.Author).Include(x => x.ForumSection).FirstOrDefault(x => x.Id == id);
			var viewModel = new TopicViewModel
			{
				Id = topic.Id,
				Name = topic.Name,
				Description = topic.Description,
				Created = topic.Created,
				Modified = topic.Modified,
				SectionId = topic.ForumSection.Id
			};
			if (topic.Author != null)
			{
				viewModel.AuthorId = topic.Author.Id;
				viewModel.AuthorName = topic.Author.UserName;
			}
			return viewModel;
		}
		public void Delete(TopicViewModel model)
		{
			var topic = ApplicationDbContext.Topics.FirstOrDefault(x => x.Id == model.Id);
			ApplicationDbContext.Topics.Remove(topic);
			ApplicationDbContext.SaveChanges();
		}
		public TopicIndexViewModel GetIndexViewModel(int sectionId, string currentUserId)
		{
			var topics = ApplicationDbContext.Topics.Include(x => x.ForumSection).Include(x => x.Author).Where(x => x.ForumSection.Id == sectionId).ToList();
			var viewModel = new TopicIndexViewModel
			{
				Topics = Mapper.Map<List<TopicShortViewModel>>(topics),
				SectionId = sectionId,
				SectionName = ApplicationDbContext.ForumSections.FirstOrDefault(x => x.Id == sectionId).Name
			};

			foreach (var topic in viewModel.Topics)
			{
				topic.IsAuthorized = AdministrationService.IsAdminOrModOrAuthor(currentUserId, viewModel.SectionId, topic.AuthorId);
			}
			return viewModel;
		}

		public async Task<List<TopicDto>> GetTopics(int sectionId)
		{
			var topics = await ApplicationDbContext.Topics
				.Include(x => x.ForumSection)
				.Where(x => x.ForumSection.Id == sectionId)
				.ToListAsync();

			if (!topics.Any())
			{
				await ApplicationDbContext.Topics.AddAsync(new Topic
				{
					Name = "Test Topic",
					Description = "Test Description"
				});
			}

			return Mapper.Map<List<TopicDto>>(topics);
		}

		public async Task AddTopic(TopicCreateEditDto model, int sectionId)
		{
			var adminId = ApplicationDbContext.UserClaims.FirstOrDefault(x => x.ClaimType == "Admin").UserId;

			var newTopic = new Topic()
			{
				ForumSection = ApplicationDbContext.ForumSections.FirstOrDefault(x => x.Id == sectionId),
				Name = model.Name,
				Description = model.Description,
				Author = ApplicationDbContext.Users.FirstOrDefault(x => x.Id == adminId)
			};
			await ApplicationDbContext.Topics.AddAsync(newTopic);
			await ApplicationDbContext.SaveChangesAsync();
		}
		public async Task EditTopic(TopicCreateEditDto model, int id)
		{
			var topic = ApplicationDbContext.Topics.FirstOrDefault(x => x.Id == id);
			if (topic == null)
			{
				throw new KeyNotFoundException($"Topic with id = {id} not found");
			}
			topic.Name = model.Name;
			topic.Description = model.Description;
			topic.Modified = DateTime.Now;

			await ApplicationDbContext.SaveChangesAsync();
		}

		public async Task DeleteTopic(int topicId)
		{
			var topic = ApplicationDbContext.Topics.FirstOrDefault(x => x.Id == topicId);
			if (topic == null)
			{
				throw new KeyNotFoundException($"Topic with id = {topicId} not found");
			}
			ApplicationDbContext.Topics.Remove(topic);
			await ApplicationDbContext.SaveChangesAsync();
		}
	}
}

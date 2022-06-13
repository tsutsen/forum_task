using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task2Process.Data;
using Task2Process.DtoModels;
using Task2Process.Models;
using Task2Process.ViewModels;

namespace Task2Process.Services
{
	public interface IForumSectionService
	{
		ForumSectionViewModel GetViewModel(int id);
		ForumSectionIndexViewModel GetIndexViewModel(string currentUserId);
		ForumSectionCreateViewModel GetCreateViewModel();
		ForumSectionEditViewModel GetEditViewModel(int id);
		void Create(ForumSectionCreateViewModel model);
		void Edit(ForumSectionEditViewModel model);
		void Delete(ForumSectionViewModel model);
		public Task<List<ForumSectionDto>> GetForumSections();
		public Task AddForumSection(ForumSectionCreateEditDto model);
		public Task EditForumSection(ForumSectionCreateEditDto model, int id);
		public Task DeleteForumSection(int id);
	}

	public class ForumSectionService : IForumSectionService
	{
		private ApplicationDbContext ApplicationDbContext { get; }
		IAdministrationService AdministrationService { get; }
		private IMapper Mapper { get; }
		private IWebHostEnvironment AppEnvironment { get; }
		public ForumSectionService(ApplicationDbContext applicationDbContext, IMapper mapper, IWebHostEnvironment appEnvironment, IAdministrationService administrationService)
		{
			AdministrationService = administrationService;
			ApplicationDbContext = applicationDbContext;
			Mapper = mapper;
			AppEnvironment = appEnvironment;
		}
		public async Task<List<ForumSectionDto>> GetForumSections()
		{
			var sections = await ApplicationDbContext.ForumSections.ToListAsync();

			if (!sections.Any())
			{
				await ApplicationDbContext.ForumSections.AddAsync(new ForumSection
				{
					Name = "Test Section",
					Description = "Test Description"
				});
			}	

			return Mapper.Map<List<ForumSectionDto>>(sections);
		}
		public async Task AddForumSection(ForumSectionCreateEditDto model)
		{
			var newSection = new ForumSection()
			{
				Name = model.Name,
				Description = model.Description
			};
			await ApplicationDbContext.ForumSections.AddAsync(newSection);
			await ApplicationDbContext.SaveChangesAsync();
		}
		public async Task EditForumSection(ForumSectionCreateEditDto model, int id)
		{
			var section = ApplicationDbContext.ForumSections.FirstOrDefault(x => x.Id == id);
			if (section == null)
			{
				throw new KeyNotFoundException($"Section with id = {id} not found");
			}
			section.Name = model.Name;
			section.Description = model.Description;
			await ApplicationDbContext.SaveChangesAsync();
		}

		public async Task DeleteForumSection(int id)
		{
			var section = ApplicationDbContext.ForumSections.FirstOrDefault(x => x.Id == id);
			if (section == null)
			{
				throw new KeyNotFoundException($"Section with id = {id} not found");
			}
			ApplicationDbContext.ForumSections.Remove(section);
			await ApplicationDbContext.SaveChangesAsync(); 
		}

		public ForumSectionCreateViewModel GetCreateViewModel()
		{
			return new ForumSectionCreateViewModel { };
		}
		public void Create(ForumSectionCreateViewModel model)
		{
			var forumSection = new ForumSection
			{
				Name = model.Name,
				Description = model.Description
			};
			ApplicationDbContext.ForumSections.Add(forumSection);
			ApplicationDbContext.SaveChanges();
		}
		public ForumSectionEditViewModel GetEditViewModel(int id)
		{
			var section = ApplicationDbContext.ForumSections.FirstOrDefault(x => x.Id == id);
			return new ForumSectionEditViewModel {
				Id = section.Id,
				Name = section.Name,
				Description = section.Description
			} ;
		}
		public void Edit(ForumSectionEditViewModel model)
		{
			var section = ApplicationDbContext.ForumSections.FirstOrDefault(x => x.Id == model.Id);
			section.Name = model.Name;
			section.Description = model.Description;
			ApplicationDbContext.SaveChanges();
		}
		public ForumSectionViewModel GetViewModel(int id)
		{
			var section = ApplicationDbContext.ForumSections.FirstOrDefault(x => x.Id == id);
			return new ForumSectionViewModel {
				Id = section.Id,
				Name = section.Name, 
				Description = section.Description,
			};
		}
		public void Delete(ForumSectionViewModel model)
		{
			var section = ApplicationDbContext.ForumSections.Include(x => x.Moderators).Include(x => x.Topics).ThenInclude(x => x.Messages).FirstOrDefault(x => x.Id == model.Id);
			var dependantTopics = ApplicationDbContext.Topics.Include(x => x.ForumSection).Where(x => x.ForumSection.Id == section.Id).ToList(); 
			ApplicationDbContext.ForumSections.Remove(section);
			foreach (var topic in dependantTopics)
			{
				ApplicationDbContext.Topics.Remove(topic);
			}
			ApplicationDbContext.SaveChanges();
		}
		public ForumSectionIndexViewModel GetIndexViewModel(string currentUserId)
		{
			var forumSections = ApplicationDbContext.ForumSections.ToList();
			var viewModel = new ForumSectionIndexViewModel
			{
				ForumSections = Mapper.Map<List<ForumSectionShortViewModel>>(forumSections)
			};

			foreach (var section in viewModel.ForumSections)
			{
				section.IsAuthorized = AdministrationService.IsAdminOrMod(currentUserId, section.Id);
			}

			return viewModel;
		}
	}
}

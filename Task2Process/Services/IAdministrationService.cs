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
using Task2Process.Models;
using Task2Process.ViewModels;

namespace Task2Process.Services
{
	public interface IAdministrationService
	{
		ModeratedSectionsIndexViewModel GetIndexViewModel(int sectionId);
		TopicCreateViewModel GetCreateViewModel(int sectionId, string currentUserId);
		AdministrationViewModel GetViewModel(int sectionId, string userId, bool isModerator);
		public bool IsAdminOrModOrAuthor(string currentUserId, int forumSectionId, string authorId);
		public bool IsAdminOrMod(string currentUserId, int forumSectionId);
		void AddModerator(AdministrationViewModel model);
		void RemoveModerator(AdministrationViewModel model);
		void Create(TopicCreateViewModel model);
		void Delete(TopicViewModel model);
		void Update(ModeratedSectionsIndexViewModel model);
	}

	public class AdministrationService : IAdministrationService
	{
		private readonly UserManager<User> _userManager;
		private ApplicationDbContext ApplicationDbContext { get; }
		private IMapper Mapper { get; }
		private IWebHostEnvironment AppEnvironment { get; }

		public AdministrationService(ApplicationDbContext applicationDbContext, IMapper mapper, IWebHostEnvironment appEnvironment, UserManager<User> userManager)
		{
			_userManager = userManager;
			ApplicationDbContext = applicationDbContext;
			Mapper = mapper;
			AppEnvironment = appEnvironment;
		}
		public bool IsAdminOrModOrAuthor(string currentUserId, int forumSectionId, string authorId)
		{
			var isAdmin = ApplicationDbContext.UserClaims.Any(x => x.UserId == currentUserId && x.ClaimType == Constants.AdminClaimName);
			var isModerator = ApplicationDbContext.ModeratedSections.Include(x => x.User).Include(x => x.ForumSection).Any(x => x.User.Id == currentUserId && x.ForumSection.Id == forumSectionId);
			var isAuthor = currentUserId == authorId;
			return (isAdmin || isModerator || isAuthor);
		}
		public bool IsAdminOrMod(string currentUserId, int forumSectionId)
		{
			var isAdmin = ApplicationDbContext.UserClaims.Any(x => x.UserId == currentUserId && x.ClaimType == Constants.AdminClaimName);
			var isModerator = ApplicationDbContext.ModeratedSections.Include(x => x.User).Include(x => x.ForumSection).Any(x => x.User.Id == currentUserId && x.ForumSection.Id == forumSectionId);
			return (isAdmin || isModerator);
		}
		public bool IsAdmin(string currentUserId)
		{
			var isAdmin = ApplicationDbContext.UserClaims.Any(x => x.UserId == currentUserId && x.ClaimType == Constants.AdminClaimName);
			return (isAdmin);
		}
		public AdministrationViewModel GetViewModel(int sectionId, string userId, bool isModerator)
		{
			return new AdministrationViewModel
			{
				ForumSectionId = sectionId,
				ForumSectionName = ApplicationDbContext.ForumSections.FirstOrDefault(x => x.Id == sectionId).Name,
				UserId = userId,
				UserName = ApplicationDbContext.Users.FirstOrDefault(x => x.Id == userId).UserName,
				IsModerator = isModerator
			};
		}
		public void AddModerator(AdministrationViewModel model)
		{
			var moderator = new ModeratedSections
			{
				User = ApplicationDbContext.Users.FirstOrDefault(x => x.Id == model.UserId),
				ForumSection = ApplicationDbContext.ForumSections.FirstOrDefault(x => x.Id == model.ForumSectionId)
			};
			ApplicationDbContext.ModeratedSections.Add(moderator);
			ApplicationDbContext.SaveChanges();
		}
		public void RemoveModerator(AdministrationViewModel model)
		{
			var moderator = ApplicationDbContext.ModeratedSections.Include(x => x.User).Include(x => x.ForumSection).FirstOrDefault(x => x.User.Id == model.UserId && x.ForumSection.Id == model.ForumSectionId);
			ApplicationDbContext.ModeratedSections.Remove(moderator);
			ApplicationDbContext.SaveChanges();
		}
		public ModeratedSectionsIndexViewModel GetIndexViewModel(int sectionId)
		{
			var users = _userManager.Users.ToList();
			var sectionModerators = ApplicationDbContext.ModeratedSections.Include(x => x.User).Include(x => x.ForumSection).Where(x => x.ForumSection.Id == sectionId).Select(x => x.User).ToList();

			List<ModeratedSectionsShortViewModel> usersViewModels = new List<ModeratedSectionsShortViewModel>();

			var moderatedSectionsIndexViewModel = new ModeratedSectionsIndexViewModel
			{
				Users = Mapper.Map<List<ModeratedSectionsShortViewModel>>(users),
				CurrentSectionId = sectionId,
				SectionName = ApplicationDbContext.ForumSections.FirstOrDefault(x => x.Id == sectionId).Name
			};

			foreach (var shortViewModel in moderatedSectionsIndexViewModel.Users)
			{
				shortViewModel.SectionId = sectionId;
				//shortViewModel.IsModerator = false;
				//var userModeratedSections = ApplicationDbContext.Users.Include(x => x.ModeratedSections).ThenInclude(x => x.ForumSection).FirstOrDefault(x => x.Id == shortViewModel.UserId).ModeratedSections.Select(x => x.ForumSection.Id).ToList();

				var isModerator = ApplicationDbContext.ModeratedSections.Include(x => x.User).Include(x => x.ForumSection).Any(x => x.User.Id == shortViewModel.UserId && x.ForumSection.Id == sectionId);
				shortViewModel.IsModerator = isModerator;
			}
			return moderatedSectionsIndexViewModel;
		}


		public TopicCreateViewModel GetCreateViewModel(int sectionId, string currentUserId)
		{
			throw new NotImplementedException();
		}
		public void Create(TopicCreateViewModel model)
		{
			//might implement assigning the moderator claim to common users 
			throw new NotImplementedException();
		}
		public void Delete(TopicViewModel model)
		{
			//might implement removal of the moderator claim 
			throw new NotImplementedException();
		}


		public void Update(ModeratedSectionsIndexViewModel model)
		{
			var moderators = ApplicationDbContext.ModeratedSections.Include(x => x.User).Where(x => x.ForumSection.Id == model.CurrentSectionId).ToList();
			foreach (var moderator in moderators)
			{		
				var claimResult = _userManager.RemoveClaimAsync(moderator.User, new Claim(Constants.ModeratorClaimName, "")).Result;
			}
			ApplicationDbContext.ModeratedSections.RemoveRange(moderators);
			ApplicationDbContext.SaveChanges();	

			if (model.SelectedUsers != null)
			{
				foreach (var userId in model.SelectedUsers)
				{
					var newModerator = new ModeratedSections
					{
						User = ApplicationDbContext.Users.FirstOrDefault(x => x.Id == userId),
						ForumSection = ApplicationDbContext.ForumSections.FirstOrDefault(x => x.Id == model.CurrentSectionId)
					};
					ApplicationDbContext.ModeratedSections.Add(newModerator);
					var claimResult = _userManager.AddClaimAsync(newModerator.User, new Claim(Constants.ModeratorClaimName, "")).Result;
				}
				ApplicationDbContext.SaveChanges();
			}
		}
	}
}

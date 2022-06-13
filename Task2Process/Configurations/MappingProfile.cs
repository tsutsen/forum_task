using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Task2Process.DtoModels;
using Task2Process.Models;
using Task2Process.ViewModels;

namespace NewsWebExample.Configuration
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<ForumSection, ForumSectionDto>();
			CreateMap<Topic, TopicDto>();
			CreateMap<Message, MessageDto>();

			CreateMap<Attachment, MessageViewModel>();
			CreateMap<ForumSection, ForumSectionShortViewModel>();
			CreateMap<ForumSectionCreateViewModel, ForumSection>();
			CreateMap<ForumSection, ForumSectionEditViewModel>();

			CreateMap<Topic, TopicShortViewModel>()
				.ForMember(x => x.AuthorId, opt => opt.MapFrom(t => t.Author.Id));
			CreateMap<Topic, TopicIndexViewModel>()
				.ForMember(x => x.SectionId, opt => opt.MapFrom(t => t.ForumSection.Id))
				.ForMember(x => x.SectionName, opt => opt.MapFrom(t => t.ForumSection.Name));
			CreateMap<Topic, TopicCreateViewModel>()
				.ForMember(x => x.SectionId, opt => opt.MapFrom(t => t.ForumSection.Id));
			CreateMap<Topic, TopicEditViewModel>()
				.ForMember(x => x.SectionId, opt => opt.MapFrom(t => t.ForumSection.Id));
			CreateMap<Topic, TopicViewModel>()
				.ForMember(x => x.SectionId, opt => opt.MapFrom(t => t.ForumSection.Id))
				.ForMember(x => x.AuthorId, opt => opt.MapFrom(t => t.Author.Id));

			CreateMap<Message, MessageShortViewModel>()
				.ForMember(x => x.AuthorId, opt => opt.MapFrom(t => t.Author.Id))
				.ForMember(x => x.AuthorUserName, opt => opt.MapFrom(t => t.Author.UserName))
				.ForMember(x => x.AttachmentIds, opt => opt.MapFrom(t => t.Attachments.Select(tt => tt.Id)));
			CreateMap<Message, MessageIndexViewModel>()
				.ForMember(x => x.TopicName, opt => opt.MapFrom(t => t.Topic.Name))
				.ForMember(x => x.TopicId, opt => opt.MapFrom(t => t.Topic.Id));
			CreateMap<Message, MessageCreateViewModel>()
				.ForMember(x => x.TopicId, opt => opt.MapFrom(t => t.Topic.Id))
				.ForMember(x => x.AuthorId, opt => opt.MapFrom(t => t.Author.Id))
				.ForMember(x => x.Attachments, opt => opt.MapFrom(t => t.Attachments))
				.ForMember(x => x.AttachmentIds, opt => opt.MapFrom(t => t.Attachments.Select(tt => tt.Id)));
			CreateMap<Message, MessageEditViewModel>();
			CreateMap<Message, MessageViewModel>()
				.ForMember(x => x.TopicId, opt => opt.MapFrom(t => t.Topic.Id))
				.ForMember(x => x.AuthorId, opt => opt.MapFrom(t => t.Author.Id))
				.ForMember(x => x.Attachments, opt => opt.MapFrom(t => t.Attachments))
				.ForMember(x => x.AttachmentIds, opt => opt.MapFrom(t => t.Attachments.Select(tt => tt.Id)));

			CreateMap<User, ModeratedSectionsShortViewModel>()
				.ForMember(x => x.UserId, opt => opt.MapFrom(t => t.Id))
				.ForMember(x => x.UserName, opt => opt.MapFrom(t => t.UserName));

			CreateMap<ModeratedSections, ModeratedSectionsShortViewModel>()
				.ForMember(x => x.SectionId, opt => opt.MapFrom(t => t.ForumSection.Id));
		}
	}
}

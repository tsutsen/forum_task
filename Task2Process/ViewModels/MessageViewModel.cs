using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Task2Process.Models;

namespace Task2Process.ViewModels
{
	public class MessageViewModel
	{
		public int Id { get; set; }
		public string Text { get; set; }
		public DateTime Created { get; set; }
		public DateTime? Modified { get; set; }
		public string AuthorId { get; set; }
		public string AuthorUserName { get; set; }
		public int TopicId { get; set; }
		public int SectionId { get; set; }
		public ICollection<Attachment> Attachments { get; set; }
		public List<int> AttachmentIds { get; set; }
	}
}

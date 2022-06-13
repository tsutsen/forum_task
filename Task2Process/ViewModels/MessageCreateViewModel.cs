using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task2Process.Models;

namespace Task2Process.ViewModels
{
	public class MessageCreateViewModel
	{
		public string Text { get; set; }
		public int TopicId { get; set; }
		public string AuthorId { get; set; }
		
		public ICollection<Attachment> Attachments { get; set; }
		public List<int> AttachmentIds { get; set; }
	}
	public class MessageEditViewModel : MessageCreateViewModel
	{
		public int Id { get; set; }
		public int SectionId { get; set; }
	}
}

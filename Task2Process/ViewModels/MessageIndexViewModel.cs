using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task2Process.ViewModels
{
	public class MessageShortViewModel
	{
		public int Id { get; set; }
		public string Text { get; set; }
		public DateTime Created { get; set; }
		public DateTime? Modified { get; set; }
		public List<int> AttachmentIds { get; set; }
		public string AuthorId { get; set; }
		public string AuthorUserName { get; set; }
		public bool IsAuthorized { get; set; }
	}
	public class MessageIndexViewModel
	{
		public List<MessageShortViewModel> Messages { get; set; }
		public int SectionId { get; set; }
		public int TopicId { get; set; }
		public string TopicName { get; set; }
	}
}

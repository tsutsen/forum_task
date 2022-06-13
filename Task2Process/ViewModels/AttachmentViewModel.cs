using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task2Process.ViewModels
{
	public class AttachmentViewModel
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public string UserName { get; set; }
		public int MessageId { get; set; }
		public string FileName { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Task2Process.Models;

namespace Task2Process.ViewModels
{
	public class TopicViewModel
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public string Description { get; set; }
		[Required]
		public DateTime Created { get; set; }
		public DateTime? Modified { get; set; }
		public string AuthorId { get; set; }
		public string AuthorName { get; set; }
		public int SectionId { get; set; }
		//public ICollection<Message> Messages { get; set; }
		//public ForumSection ForumSection { get; set; }
		//public User Author { get; set; }
	}
}

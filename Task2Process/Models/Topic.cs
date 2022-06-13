using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Task2Process.Models
{
	public class Topic
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public string Description { get; set; }
		[Required]
		public DateTime Created { get; set; }
		public DateTime? Modified { get; set; }
		public ICollection<Message> Messages { get; set; }
		public ForumSection ForumSection { get; set; }
		public User Author { get; set; }
	}
}

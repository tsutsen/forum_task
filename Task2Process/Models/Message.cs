using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Task2Process.Models
{
	public class Message
	{
		[Key]
		public int Id { get; set; }
		[Required, MaxLength(240)]
		public string Text { get; set; }
		[Required]
		public DateTime Created { get; set; }
		public DateTime? Modified { get; set; }
		[Required]
		public User Author { get; set; }
		public ICollection<Attachment> Attachments { get; set; }
		public Topic Topic { get; set; }
	}
}

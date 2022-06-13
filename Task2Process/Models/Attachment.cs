using System;
using System.ComponentModel.DataAnnotations;

namespace Task2Process.Models
{
	public class Attachment
	{
		[Required]
		public int Id { get; set; }
		[Required]
		public string FileName { get; set; }
		public Message Message { get; set; }
	}
}

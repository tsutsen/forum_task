using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Task2Process.Models
{
	public class ModeratedSections
	{
		[Key]
		public int Id { get; set; }
		public User User { get; set; }
		public ForumSection ForumSection { get; set; }
	}
}

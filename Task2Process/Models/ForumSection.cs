using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Task2Process.Models
{
	public class ForumSection
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Description { get; set; }
		public ICollection<Topic> Topics { get; set; }
		public ICollection<ModeratedSections> Moderators { get; set; }
	}
}

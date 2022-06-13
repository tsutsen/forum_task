using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Task2Process.ViewModels
{
	public class ForumSectionCreateViewModel
	{
		[Required(ErrorMessage = "Forum section should have a name")]
		public string Name { get; set; }
		[Required(ErrorMessage = "Forum section should have a description")]
		public string Description { get; set; }
	}
	public class ForumSectionEditViewModel : ForumSectionCreateViewModel
	{
		[Required]
		public int Id { get; set; }
	}
}

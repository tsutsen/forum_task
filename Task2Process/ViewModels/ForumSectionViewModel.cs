using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Task2Process.Models;

namespace Task2Process.ViewModels
{
	public class ForumSectionViewModel
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Description { get; set; }
		//public ICollection<Topic> Topics { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Task2Process.Models;

namespace Task2Process.ViewModels
{
	public class ForumSectionShortViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsAuthorized { get; set; }
	}
	public class ForumSectionIndexViewModel
	{
		public List<ForumSectionShortViewModel> ForumSections { get; set; }
	}
}

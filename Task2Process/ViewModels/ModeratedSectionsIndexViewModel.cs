using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task2Process.ViewModels
{
	public class ModeratedSectionsShortViewModel
	{
		public string UserId { get; set; }
		public string UserName { get; set; }
		public int SectionId { get; set; }
		public bool IsModerator { get; set; }
	}
	public class ModeratedSectionsIndexViewModel
	{
		public List<ModeratedSectionsShortViewModel> Users { get; set; }
		public List<String> SelectedUsers { get; set; }
		public int CurrentSectionId { get; set; }
		public string SectionName { get; set; }
	}
}

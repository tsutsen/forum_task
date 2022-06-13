using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task2Process.ViewModels
{
	public class AdministrationViewModel
	{
		public string UserId { get; set; }
		public string UserName { get; set; }
		public int ForumSectionId { get; set; }
		public string ForumSectionName{ get; set; }
		public bool IsModerator { get; set; }
	}
}

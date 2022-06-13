using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task2Process.ViewModels
{
	public class TopicShortViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string AuthorId { get; set; }
		public string Description { get; set; }
		public bool IsAuthorized { get; set; }
	}
	public class TopicIndexViewModel
	{
		public List<TopicShortViewModel> Topics { get; set; }
		public int SectionId { get; set; }
		public string SectionName { get; set; }
	}
}

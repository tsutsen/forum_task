using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task2Process.ViewModels
{
	public class TopicCreateViewModel
	{
		
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime Created { get; set; }
		public int SectionId { get; set; }
		public string AuthorId { get; set; }
	}

	public class TopicEditViewModel : TopicCreateViewModel
	{
		public int Id { get; set; }
	}
}

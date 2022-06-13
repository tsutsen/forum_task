using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Task2Process.DtoModels
{
	public class ForumSectionCreateEditDto
	{
		[JsonProperty("name")]
		[Required]
		public string Name { get; set; }
		[JsonProperty("description")]
		[Required]
		public string Description { get; set; }
	}
}

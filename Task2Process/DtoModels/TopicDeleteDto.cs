using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Task2Process.DtoModels
{
	public class TopicDeleteDto
	{
		[JsonProperty("id")]
		[Required]
		public int Id { get; set; }
	}
}

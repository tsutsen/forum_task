using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task2Process.DtoModels
{
	public class MessageDto
	{
		[JsonProperty("id")]
		public int Id { get; set; }
		[JsonProperty("text")]
		public string Text { get; set; }
		[JsonProperty("created")]
		public DateTime Created { get; set; }
		[JsonProperty("modified")]
		public DateTime Modified { get; set; }
	}
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task2Process.DtoModels
{
	public class MessageDeleteDto
	{
		[JsonProperty("id")]
		public int Id { get; set; }
	}
}

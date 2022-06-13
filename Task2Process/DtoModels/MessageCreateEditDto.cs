using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Task2Process.Models;

namespace Task2Process.DtoModels
{
	public class MessageCreateEditDto
	{
		[JsonProperty("text")]
		[MinLength(1)]
		public string Text { get; set; }
	}
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Task2Process.Models
{
	public class User : IdentityUser
	{
		//public DateTime? Birthday { get; set; }
		public ICollection<Message> Messages { get; set; }
		public ICollection<ModeratedSections> ModeratedSections { get; set; }
	}
}

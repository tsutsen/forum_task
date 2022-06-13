using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Task2Process.Models;

namespace Task2Process.Data
{
	public class ApplicationDbContext : IdentityDbContext<User>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{

		}

		//public DbSet<Message> Users { get; set; }
		public DbSet<ModeratedSections> ModeratedSections { get; set; }
		public DbSet<Message> Messages { get; set; }
		public DbSet<Attachment> Attachments { get; set; }
		public DbSet<Topic> Topics { get; set; }
		public DbSet<ForumSection> ForumSections { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<User>().HasMany(x => x.Messages).WithOne(x => x.Author).IsRequired();
			modelBuilder.Entity<User>().HasMany(x => x.ModeratedSections).WithOne(x => x.User);

			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Message>().HasKey(x => x.Id);
			modelBuilder.Entity<Message>().Property(x => x.Text).HasMaxLength(240).IsRequired();
			modelBuilder.Entity<Message>().Property(x => x.Created).IsRequired();
			modelBuilder.Entity<Message>().HasMany(x => x.Attachments).WithOne(x => x.Message).IsRequired(); //really required?

			modelBuilder.Entity<Attachment>().HasKey(x => x.Id);

			modelBuilder.Entity<Topic>().HasKey(x => x.Id);
			modelBuilder.Entity<Topic>().Property(x => x.Name).HasMaxLength(100).IsRequired();
			modelBuilder.Entity<Topic>().Property(x => x.Created).IsRequired();
			modelBuilder.Entity<Topic>().HasMany(x => x.Messages).WithOne(x => x.Topic).IsRequired();

			modelBuilder.Entity<ForumSection>().HasKey(x => x.Id);
			modelBuilder.Entity<ForumSection>().HasMany(x => x.Moderators).WithOne(x => x.ForumSection);
		}
	}
}

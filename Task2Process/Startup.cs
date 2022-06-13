using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewsWebExample.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Task2Process.Data;
using Task2Process.Models;
using Task2Process.Services;

namespace Task2Process
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var mappingConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new MappingProfile());
			});
			var mapper = mappingConfig.CreateMapper();
			services.AddSingleton(mapper);
			services.AddScoped<IForumSectionService, ForumSectionService>();
			services.AddScoped<ITopicService, TopicService>();
			services.AddScoped<IMessageService, MessageService>();
			services.AddScoped<IAdministrationService, AdministrationService>();
			services.AddScoped<IAttachmentService, AttachmentService>();

			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));
			services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
				.AddEntityFrameworkStores<ApplicationDbContext>();
			services.AddControllersWithViews().AddRazorRuntimeCompilation();
			services.AddRazorPages();

			services.AddAuthorization(options =>
	options.AddPolicy(Constants.AdminPolicy, policyBuilder => policyBuilder.RequireClaim("Admin")));
			services.AddAuthorization(options =>
				options.AddPolicy(Constants.AdminAndModeratorPolicy, policyBuilder => policyBuilder
					.RequireClaim("Admin")
					.RequireClaim("Moderator")));
			services.AddControllers();

			services.AddSwaggerGen(c =>
			{
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
			});

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<User> userManager)
		{
			var email = "admin@example.com";
			var pass = "1233333aB!";

			var admin = userManager.FindByNameAsync(email).Result;
			if (admin == null)
			{
				var result = userManager.CreateAsync(new User
				{
					Email = email,
					UserName = email
				}, pass).Result;
				if (result.Succeeded)
				{
					admin = userManager.FindByNameAsync(email).Result;
					var claimResult = userManager.AddClaimAsync(admin, new Claim(Constants.AdminClaimName, "")).Result;
				}
			}

			var moderator = userManager.FindByNameAsync("moderator1@admin.com").Result;
			if (moderator == null)
			{
				var result = userManager.CreateAsync(new User()
				{
					Email = "moderator1@admin.com",
					UserName = "moderator1@admin.com"
				}, pass).Result;
				if (result.Succeeded)
				{
					moderator = userManager.FindByNameAsync("moderator1@admin.com").Result;
					var claimResult2 = userManager.AddClaimAsync(moderator, new Claim(Constants.ModeratorClaimName, "")).Result;
				}
			}

			var moderator2 = userManager.FindByNameAsync("moderator2@admin.com").Result;
			if (moderator2 == null)
			{
				var result = userManager.CreateAsync(new User()
				{
					Email = "moderator2@admin.com",
					UserName = "moderator2@admin.com"
				}, pass).Result;
				if (result.Succeeded)
				{
					moderator2 = userManager.FindByNameAsync("moderator2@admin.com").Result;
					var claimResult3 = userManager.AddClaimAsync(moderator2, new Claim(Constants.ModeratorClaimName, "")).Result;
				}
			}

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseSwagger(c =>
			{
				c.SerializeAsV2 = true;
			});

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
			// specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
			});

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=ForumSection}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
				endpoints.MapControllers();
			});
		}
	}
}

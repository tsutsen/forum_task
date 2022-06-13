using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task2Process.DtoModels;
using Task2Process.Services;

namespace Task2Process.Controllers
{
	public class ForumSectionApiController : Controller
	{
		private IForumSectionService ForumSectionService { get; set; }

		public ForumSectionApiController(IForumSectionService forumSectionService)
		{
			ForumSectionService = forumSectionService;
		}

		/// <summary>
		/// Get all existing forum sections
		/// </summary>
		/// <returns></returns>
		[Route("api/sections")]
		[HttpGet]
		public async Task<List<ForumSectionDto>> Get()
		{
			var sections = await ForumSectionService.GetForumSections();
			return sections;
		}

		/// <summary>
		/// Create new forum section
		/// </summary>
		/// <returns></returns>
		// POST: ForumSectionApiController/Create
		[Route("api/sections")]
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] ForumSectionCreateEditDto model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Request is not valid");
			}
			try
			{
				await ForumSectionService.AddForumSection(model);
				return Ok();
			}
			catch
			{
				return StatusCode(500);
			}
		}

		/// <summary>
		/// Edit forum section with a given id
		/// </summary>
		/// <returns></returns>
		[Route("api/sections/{id}")]
		[HttpPut]
		public async Task<IActionResult> Put([FromBody] ForumSectionCreateEditDto model, int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Request is not valid");
			}
			try
			{
				await ForumSectionService.EditForumSection(model, id);
				return Ok();
			}
			catch (KeyNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch
			{
				return StatusCode(500);
			}
		}
		/// <summary>
		/// Delete forum section with a given id
		/// </summary>
		/// <returns></returns>
		[Route("api/sections/{id}")]
		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Request is not valid");
			}

			try
			{
				await ForumSectionService.DeleteForumSection(id);
				return Ok();
			}
			catch (KeyNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch
			{
				return StatusCode(500);
			}
		}
	}
}

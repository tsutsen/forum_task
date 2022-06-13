using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Task2Process.DtoModels;
using Task2Process.Services;

namespace Task2Process.Controllers
{
	public class TopicApiController : Controller
	{
		private ITopicService TopicService { get; set; }

		/// <summary>
		/// Get all topics in a forum section with a given id
		/// </summary>
		/// <returns></returns>
		public TopicApiController(ITopicService topicService)
		{
			TopicService = topicService;
		}

		[Route("api/sections/{sectionId}")]
		[HttpGet]
		public async Task<List<TopicDto>> Get(int sectionId)
		{
			var topics = await TopicService.GetTopics(sectionId);
			return topics;
		}
		/// <summary>
		/// Create new topic in a forum section with a given id
		/// </summary>
		/// <returns></returns>
		[Route("api/sections/{sectionId}/topics")]
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] TopicCreateEditDto model, int sectionId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Request is not valid");
			}
			try
			{
				await TopicService.AddTopic(model, sectionId);
				return Ok();
			}
			catch
			{
				return StatusCode(500);
			}
		}
		/// <summary>
		/// Edit topic with a given id
		/// </summary>
		/// <returns></returns>
		[Route("api/topics/{topicId}")]
		[HttpPut]
		public async Task<IActionResult> Put([FromBody] TopicCreateEditDto model, int topicId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Request is not valid");
			}
			try
			{
				await TopicService.EditTopic(model, topicId);
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
		/// Delete topic with a given id
		/// </summary>
		/// <returns></returns>
		[Route("api/topics/{topicId}")]
		[HttpDelete]
		public async Task<IActionResult> Delete(int topicId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Request is not valid");
			}
			try
			{
				await TopicService.DeleteTopic(topicId);
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
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
	public class MessageApiController : Controller
	{
		private IMessageService MessageService { get; set; }

		public MessageApiController(IMessageService messageService)
		{
			MessageService = messageService;
		}
		/// <summary>
		/// Get all messages in a topic with a given id
		/// </summary>
		/// <returns></returns>
		[Route("api/topics/{topicId}/messages")]
		[HttpGet]
		public async Task<List<MessageDto>> Get(int topicId)
		{
			var messages = await MessageService.GetMessages(topicId);
			return messages;
		}
		/// <summary>
		/// Create new message in a topic with a given id
		/// </summary>
		/// <returns></returns>
		[Route("api/topics/{topicId}/messages")]
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] MessageCreateEditDto model, int topicId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Request is not valid");
			}
			try
			{
				await MessageService.AddMessage(model, topicId);
				return Ok();
			}
			catch
			{
				return BadRequest();
			}
		}
		/// <summary>
		/// Edit message with a given id
		/// </summary>
		/// <returns></returns>
		[Route("api/messages/{messageId}")]
		[HttpPut]
		public async Task<IActionResult> Put([FromBody] MessageCreateEditDto model, int messageId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Request is not valid");
			}
			try
			{
				await MessageService.EditMessage(model, messageId);
				return Ok();
			}
			catch (KeyNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch
			{
				return BadRequest();
			}
		}
		/// <summary>
		/// Delete message with a given id
		/// </summary>
		/// <returns></returns>
		[Route("api/messages/{messageId}")]
		[HttpDelete]
		public async Task<IActionResult> Delete(int messageId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Request is not valid");
			}
			try
			{
				await MessageService.DeleteMessage(messageId);
				return Ok();
			}
			catch (KeyNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}
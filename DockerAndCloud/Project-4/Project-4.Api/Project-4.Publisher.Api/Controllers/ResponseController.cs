using CoreLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Project_4.Publisher.Api.Repository;
using Project_4.Publisher.Api.Services;

namespace Project_4.Publisher.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ResponseController(IMessageRepository<RabbitmqResponse> messageRepository) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> Get()
    {
        var messages = messageRepository.Drain();
        if (messages.Count == 0)
        {
            messages = new (){ new RabbitmqResponse{Payload = "Nothing stored", CorrelationId = "0"} };
        }
        return Ok(messages);
    }
}
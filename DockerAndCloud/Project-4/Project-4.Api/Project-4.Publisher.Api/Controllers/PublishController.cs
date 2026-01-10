using Microsoft.AspNetCore.Mvc;
using Project_4.Publisher.Api.Models;
using Project_4.Publisher.Api.Services;

namespace Project_4.Publisher.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PublishController : ControllerBase
{
    private readonly IRabbitmqPublishService _rabbitmqPublishService;
    public PublishController(IRabbitmqPublishService rabbitmqPublishService)
    {
        _rabbitmqPublishService = rabbitmqPublishService;
    }

    [HttpPost("direct")]
    public async Task<IActionResult> PostDirect(RequestModel request)
    {
        await _rabbitmqPublishService.PublishAsync(request);
        return Ok();
    }
    
    [HttpPost("topic")]
    public async Task<IActionResult> PostTopic(RequestModel request)
    {
        await _rabbitmqPublishService.PublishAsync(request);
        return Ok();
    }

    [HttpPost("fanout")]
    public async Task<IActionResult> PostFanout(RequestModel request)
    {
        await _rabbitmqPublishService.PublishAsync(request);
        return Ok();
    }

}
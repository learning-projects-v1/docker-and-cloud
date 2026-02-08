using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace MasstransitWebApi;

[ApiController]
[Route("[controller]")]
public class MyController: ControllerBase
{
    private readonly IBus _bus;
    public MyController(IBus bus)
    {
        _bus = bus;
    }
    [HttpGet("check")]
    public async Task<IActionResult> Check([FromServices] IRequestClient<CheckInventory> client)
    {
        var response = await client.GetResponse<InventoryStatus>(new CheckInventory(){CoffeeType = "DarkRoast"});
        return Ok(new
        {
            Status = response.Message.InStock ? "InStock" : "OutOfStock",
            Price = response.Message.Price,
        });
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("TEST OK");
    }
    
}
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

    [HttpGet("order")]
    public async Task<IActionResult> GetOrder()
    {
        await _bus.Publish<IOrderSubmitted>(new
        {
            OrderId = Guid.NewGuid(),
            CoffeeType = "Capucchino",
            Timestamp = DateTime.UtcNow
        });
        return Ok("Order Submitted");
    }
    [HttpGet("check")]
    public async Task<IActionResult> Check([FromServices] IRequestClient<CheckInventory> client, string coffeType)
    {
        Console.WriteLine("At check");
        Response response = await client.GetResponse<InventoryStatus, OrderNotFound>(new CheckInventory(){CoffeeType = coffeType});
        
        //v1
        // return Ok(new
        // {
        //     Status = response.Message.InStock ? "InStock" : "OutOfStock",
        //     Price = response.Message.Price,
        // });
        
        ///v2
        // if (response.Is(out Response<InventoryStatus>?res1))
        // {
        //     return Ok("In Stock");
        // }
        // else if(response.Is(out Response<OrderNotFound>?res2))
        // {
        //     Console.WriteLine(res2.Message.CoffeeType);
        //     return Ok("Order Not Found");
        // }
        switch (response)
        {
            case (_, OrderNotFound a) responseA:
                Console.WriteLine(responseA.Message);
                return Ok("Order Not Found");
            case (_, InventoryStatus b) responseB:
                Console.WriteLine(responseB.Message);
                return Ok("In Stock");
            
            default:
                return Ok("BAD PATH");
        }
        return Ok("Bad Path");
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("TEST OK");
    }
    
}
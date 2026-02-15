using Microsoft.AspNetCore.Mvc;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly AppDbContext _db;

    public HealthController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult Get() => Ok(new { status = "ok"});

    [HttpGet("db")]
    public async Task<IActionResult> TestDb()
    {
        var canConnect = await _db.Database.CanConnectAsync();
        return Ok(new { database = canConnect ? "connected" : "failed" });
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

[ApiController]
[Route("api/[controller]")]
public class DebugController : ControllerBase
{
    private readonly IConfiguration _config;

    public DebugController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet("sql-debug")]
    public IActionResult SqlDebug()
    {
        try
        {
            string connString = _config.GetConnectionString("DefaultConnection");
            using var conn = new SqlConnection(connString);
            conn.Open();
            return Ok("✅ SQL Connection OK!");
        }
        catch (Exception ex)
        {
            string connString = _config.GetConnectionString("DefaultConnection");
            return BadRequest($"❌ SQL Connection Failed!\nConn: {connString}\nError: {ex.Message}");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IConfiguration _config;

    public TestController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet("testsql")]
    public IActionResult TestSql()
    {
        try
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            conn.Open();
            return Ok("✅ SQL Connection OK!");
        }
        catch (Exception ex)
        {
            return BadRequest($"❌ SQL Connection Failed: {ex.Message}");
        }
    }
}

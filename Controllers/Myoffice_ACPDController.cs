using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using System.Text.Json;
using test.Models;

namespace test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Myoffice_ACPDController : ControllerBase
    {
        private readonly IConfiguration _config;

        public Myoffice_ACPDController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Myoffice_ACPD model)
        {
            var connStr = _config.GetConnectionString("DefaultConnection");
            using var conn = new SqlConnection(connStr);
            using var cmd = new SqlCommand("usp_Create_ACPD", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@JsonData", JsonSerializer.Serialize(model));

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return Ok(new
            {
                success = true,
                message = "Create success",
                data = model
            });
        }

        [HttpGet("read")]
        public async Task<IActionResult> Read()
        {
            var connStr = _config.GetConnectionString("DefaultConnection");
            using var conn = new SqlConnection(connStr);
            using var cmd = new SqlCommand("usp_Read_ACPD", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            var result = new StringBuilder();
            while (reader.Read())
            {
                result.Append(reader.GetString(0));
            }

            return Content(result.ToString(), "application/json");
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] Myoffice_ACPD model)
        {
            var connStr = _config.GetConnectionString("DefaultConnection");
            using var conn = new SqlConnection(connStr);
            using var cmd = new SqlCommand("usp_Update_ACPD", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@JsonData", JsonSerializer.Serialize(model));

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return Ok(new
            {
                success = true,
                message = "Update success",
                data = model
            });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var json = JsonSerializer.Serialize(new { ID = id });
            var connStr = _config.GetConnectionString("DefaultConnection");
            using var conn = new SqlConnection(connStr);
            using var cmd = new SqlCommand("usp_Delete_ACPD", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@JsonData", json);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return Ok(new
            {
                success = true,
                message = $"Delete success (ID = {id})"
            });
        }
    }
}

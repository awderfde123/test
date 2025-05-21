using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Text.Json;
using test.Models;

namespace test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Myoffice_ACPDController : ControllerBase
    {
        private readonly DbCommand _dbCommand;

        public Myoffice_ACPDController(DbCommand dbCommand)
        {
            _dbCommand = dbCommand;
        }

        [HttpGet("read")]
        public async Task<IActionResult> Read()
        {
            using var cmd = _dbCommand.CreateStoredProcedureCommand("usp_Read_ACPD");
            await cmd.Connection.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            var result = new StringBuilder();

            while (await reader.ReadAsync())
            {
                result.Append(reader.GetString(0));
            }

            return Content(result.ToString(), "application/json");
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Myoffice_ACPD model)
        {
            using var cmd = _dbCommand.CreateStoredProcedureCommand("usp_Create_ACPD");
            cmd.Parameters.AddWithValue("@JsonData", JsonSerializer.Serialize(model));

            await cmd.Connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return Ok(new { success = true, message = "Create success", data = model });
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] Myoffice_ACPD model)
        {
            using var cmd = _dbCommand.CreateStoredProcedureCommand("usp_Update_ACPD");
            cmd.Parameters.AddWithValue("@JsonData", JsonSerializer.Serialize(model));

            await cmd.Connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return Ok(new { success = true, message = "Update success", data = model });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            using var cmd = _dbCommand.CreateStoredProcedureCommand("usp_Delete_ACPD");
            var json = JsonSerializer.Serialize(new { ID = id });
            cmd.Parameters.AddWithValue("@JsonData", json);

            await cmd.Connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return Ok(new { success = true, message = $"Delete success (ID = {id})" });
        }
    }
}

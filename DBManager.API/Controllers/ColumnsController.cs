using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DBManager.Models;
using DBManager.Services;

namespace DBManager.API.Controllers
{
    [Route("api/db/{dbName}/tables/{tableName}/[controller]")]
    [ApiController]
    public class ColumnsController : ControllerBase
    {
        private IDBManagerService _service;

        public ColumnsController(IDBManagerService service)
        {
            _service = service;
        }

        [HttpGet("{columnName}")]
        public IActionResult GetColumn(string dbName, string tableName, string columnName)
        {
            try
            {
                var column = _service.GetColumn(dbName, tableName, columnName);

                if(column == null) return NotFound();

                return Ok(column);
            }
            catch(ArgumentException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult CreateColumn(string dbName, string tableName, [FromBody] Column column)
        {
            if(!_service.TableExists(dbName, tableName)) return NotFound();

            _service.CreateColumn(dbName, tableName, column);

            return Ok();
        }
    }
}

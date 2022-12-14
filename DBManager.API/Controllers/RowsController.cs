using DBManager.Models;
using DBManager.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DBManager.API.Controllers
{
    [Route("api/db/{dbName}/tables/{tableName}/[controller]")]
    [ApiController]
    public class RowsController : ControllerBase
    {
        private IDBManagerService _service;

        public RowsController(IDBManagerService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get(string dbName, string tableName) 
        {
            var rows = _service.GetRows(dbName, tableName);

            if(rows == null)
            {
                return NotFound();
            }

            return Ok(rows);
        }

        [HttpGet("{temp}")]
        public IActionResult GetRowsByTemp(string dbName, string tableName, string temp) 
        {
            var rows = _service.FindRowsByTemplate(dbName, tableName, temp);

            if(rows == null)
            { 
                return NotFound(); 
            }

            return Ok(rows);
        }

        [HttpPost]
        public IActionResult Post(string dbName, string tableName, [FromBody] Row row) 
        {
            if (!_service.TableExists(dbName, tableName)) return NotFound();

            _service.AddRow(dbName, tableName, row);

            return Ok();
        }

        [HttpPut]
        public IActionResult Put(string dbName, string tableName, [FromBody] Row row) 
        {
            if(!_service.RowExists(dbName, tableName, row.Id)) return NotFound();

            _service.UpdateRow(dbName, tableName, row);

            return Ok();
        }

        [HttpDelete("{rowId}")]
        public IActionResult Delete(string dbName, string tableName, int rowId)
        {
            if(!_service.RowExists(dbName, tableName, rowId)) return NotFound();

            _service.DeleteRow(dbName, tableName, rowId);

            return Ok();
        }
    }
}

using DBManager.Models;
using DBManager.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DBManager.API.Controllers
{
    [Route("api/db/{dbName}/[controller]")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        private IDBManagerService _service;

        public TablesController(IDBManagerService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get(string dbName)
        {
            var tables = _service.GetTables(dbName);

            if(!tables.Any()) 
            {
                return NotFound();
            }

            return Ok(tables);
        }

        [HttpGet("{tableName}")]
        public IActionResult GetTable(string dbName, string tableName)
        {
            try
            {
                var table = _service.GetTable(dbName, tableName);

                if(table is null) return NotFound();

                return Ok(table);
            }
            catch(ArgumentException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Post(string dbName, [FromBody] Table table)
        {
            if(!_service.DatabaseExists(dbName))
            {
                return NotFound();
            }

            try
            {
                _service.CreateTable(dbName, table);
            }
            catch(ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpDelete("{tableName}")]
        public IActionResult Delete(string dbName, string tableName) 
        {
            if(!_service.TableExists(dbName, tableName)) return NotFound();

            _service.DeleteTable(dbName, tableName);

            return Ok();
        }
    }
}

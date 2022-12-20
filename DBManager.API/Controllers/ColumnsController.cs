using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DBManager.Models;
using DBManager.Services;
using DBManager.API.Hateoas.LinkGetters;
using DBManager.API.Hateoas;

namespace DBManager.API.Controllers
{
    [Route("api/db/{dbName}/tables/{tableName}/[controller]")]
    [ApiController]
    public class ColumnsController : ControllerBase
    {
        private IDBManagerService _service;
        private readonly ColumnLinkGetter _columnLinkGetter;

        public ColumnsController(IDBManagerService service, ColumnLinkGetter columnLinkGetter)
        {
            _service = service;
            _columnLinkGetter = columnLinkGetter;
        }

        [HttpGet("{columnName}")]
        public IActionResult GetColumn(string dbName, string tableName, string columnName)
        {
            try
            {
                var column = _service.GetColumn(dbName, tableName, columnName);

                if(column == null) return NotFound();

                var res = new LinkWrapper<Column>(column, _columnLinkGetter.GetLinks(dbName, tableName, column.Name));

                return Ok(res);
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

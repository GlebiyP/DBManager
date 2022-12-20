using DBManager.API.Hateoas;
using DBManager.API.Hateoas.LinkGetters;
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
        private readonly RowLinkGetter _rowLinkGetter;

        public RowsController(IDBManagerService service, RowLinkGetter rowLinkGetter)
        {
            _service = service;
            _rowLinkGetter = rowLinkGetter;
        }

        [HttpGet]
        public IActionResult Get(string dbName, string tableName) 
        {
            var rows = _service.GetRows(dbName, tableName);

            if(rows == null)
            {
                return NotFound();
            }

            var res = rows.Select(row => new LinkWrapper<Row>(row, _rowLinkGetter.GetLinks(dbName, tableName, row.Id)));

            return Ok(res);
        }

        [HttpGet("{temp}")]
        public IActionResult GetRowsByTemp(string dbName, string tableName, string temp) 
        {
            var rows = _service.FindRowsByTemplate(dbName, tableName, temp);

            if(rows == null)
            { 
                return NotFound(); 
            }

            var res = rows.Select(row => new LinkWrapper<Row>(row, _rowLinkGetter.GetLinks(dbName, tableName, row.Id)));

            return Ok(res);
        }

        [HttpPost]
        public IActionResult Post(string dbName, string tableName, [FromBody] Row row) 
        {
            if (!_service.TableExists(dbName, tableName)) return NotFound();

            try
            {
                _service.AddRow(dbName, tableName, row);
            }
            catch(Exception ex)
            {
                return BadRequest($"Validation error: {ex.Message}");
            }

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

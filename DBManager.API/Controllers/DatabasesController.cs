using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DBManager.Models;
using DBManager.Services;

namespace DBManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabasesController : ControllerBase
    {
        private IDBManagerService _service;

        public DatabasesController(IDBManagerService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var databases = _service.GetDatabaseNames();

            if(!databases.Any()) return NotFound();

            return Ok(databases);
        }

        [HttpPost("{dbName}")]
        public IActionResult Post(string dbName)
        {
            _service.CreateDatabase(dbName);

            return Ok();
        }

        [HttpDelete("{dbName}")]
        public IActionResult Delete(string dbName)
        {
            if(!_service.DatabaseExists(dbName)) return NotFound();

            _service.DeleteDatabase(dbName);

            return Ok();
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DBManager.Models;
using DBManager.Services;
using DBManager.API.Hateoas.LinkGetters;
using DBManager.API.Hateoas;

namespace DBManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabasesController : ControllerBase
    {
        private readonly IDBManagerService _service;
        private readonly DbLinkGetter _dbLinkGetter;

        public DatabasesController(IDBManagerService service, DbLinkGetter dbLinkGetter)
        {
            _service = service;
            _dbLinkGetter = dbLinkGetter;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var databases = _service.GetDatabaseNames();

            if(!databases.Any()) return NotFound();

            var res = databases.Select(db => new LinkWrapper<string>(db, _dbLinkGetter.GetLinks(db)));

            return Ok(res);
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

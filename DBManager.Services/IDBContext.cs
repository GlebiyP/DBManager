using DBManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Services
{
    public interface IDBContext
    {
        public List<Database> Databases { get; set; }

        public Database LoadDatabase(string dbName);

        public void SaveDatabases();

        public void SaveDatabase(string dbName);

        public bool isDatabaseLoaded(string dbName);

        public bool DatabaseExists(string dbName);

        public IEnumerable<string> GetDBNames();

        public void DeleteDatabase(string dbName);
    }
}

using DBManager.Models;
using DBManager.Services;
using System.Xml;
using System.Xml.Linq;

namespace DBManager.LocalDB
{
    public class LocalDBContext : IDBContext
    {
        private readonly string _dbPathsFile = "./databases/paths.txt";

        private int _dbsCount = 0;

        private Dictionary<string, string> _databases = new Dictionary<string, string>();

        public LocalDBContext()
        {
            ReadDBPaths();
        }

        public List<Database> Databases { get; set; } = new List<Database>();

        private void ReadDBPaths()
        {
            if (!File.Exists(_dbPathsFile))
            {
                File.CreateText(_dbPathsFile);
                return;
            }

            using (StreamReader file = new StreamReader(_dbPathsFile))
            {
                string ln = string.Empty;
                while ((ln = file.ReadLine()) is not null)
                {
                    var dBName = ln.Substring(0, ln.IndexOf(" "));
                    var dBPath = ln.Substring(ln.IndexOf(" ") + 1);
                    _databases[dBName] = dBPath;
                    ++_dbsCount;
                }
            }
        }


        private void SavePaths()
        {
            if(!File.Exists(_dbPathsFile))
            {
                File.CreateText(_dbPathsFile);
                return;
            }

            using(StreamWriter file = new StreamWriter(_dbPathsFile))
            {
                foreach(var item in _databases)
                {
                    file.WriteLine($"{item.Key} {item.Value}");
                }
            }
        }

        public Database LoadDatabase(string dbName)
        {
            if (!_databases.ContainsKey(dbName)) throw new ArgumentException(dbName);

            using (TextReader file = new StreamReader(_databases[dbName]))
            {
                XElement dbEl = XElement.Load(file);

                var database = new Database();
                database.Name = (string)dbEl.Element("Name");

                database.Tables = new List<Table>();
                var tables = from table in dbEl.Descendants("Table")
                             select new Table()
                             {
                                 Name = (string)table.Element("Name"),
                                 Columns = (from column in table.Descendants("Column")
                                            select new Column()
                                            {
                                                Name = (string)column.Element("Name"),
                                                Type = (string)column.Element("Type")
                                            }).ToList(),
                                 Rows = (from row in table.Descendants("Row")
                                         select new Row()
                                         {
                                             Id = (int)row.Element("Id"),
                                             Values = (from val in row.Descendants("Item")
                                                       select (string)val).ToList()
                                         }).ToList()
                             };
                database.Tables = tables.ToList();
                Databases.Add(database);

                return database;                             
            }
        }

        public void SaveDatabases()
        {
            foreach(var db in Databases)
            {
                SaveDatabase(db.Name);
            }

            SavePaths();
        }

        public void SaveDatabase(string dbName)
        {
            var database = Databases.Where(db => db.Name == dbName).FirstOrDefault();
            if (database is null) return;

            var path = _databases.ContainsKey(database.Name) ? _databases[dbName] : String.Empty;

            if (!path.Any())
            {
                path = $"./databases/{_dbsCount}.xml";
                _databases.Add(database.Name, path);
            }

            var dbEl = new XElement("Database", new XElement("Name", database.Name));

            foreach (var table in database.Tables) 
            {
                var tableEl = new XElement("Table", new XElement("Name", table.Name));

                foreach (var column in table.Columns)
                {
                    tableEl.Add(new XElement("Column", new XElement("Name", column.Name), new XElement("Type", column.Type)));
                }

                foreach (var row in table.Rows) 
                {
                    var rowEl = new XElement("Row", new XElement("Id", row.Id));

                    foreach (var item in row.Values)
                    {
                        rowEl.Add(new XElement("Item", item));
                    }

                    tableEl.Add(rowEl);
                }

                dbEl.Add(tableEl);
            }

            dbEl.Save(path);

            SavePaths();
        }

        public bool isDatabaseLoaded(string dbName)
        {
            return Databases.Where(db => db.Name == dbName).Any();
        }

        public bool DatabaseExists(string dbName)
        {
            return _databases.ContainsKey(dbName);
        }

        public IEnumerable<string> GetDBNames()
        {
            return _databases.Keys;
        }

        public void DeleteDatabase(string dbName)
        {
            if (!_databases.ContainsKey(dbName)) return;

            _databases.Remove(dbName);
        }
    }
}
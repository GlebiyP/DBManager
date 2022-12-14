using DBManager;
using DBManager.Models;
using DBManager.Services.Validation;

namespace DBManager.Services
{
    public class DBManagerService : IDBManagerService
    {
        private IDBContext _dBContext;

        public DBManagerService(IDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public IEnumerable<string> GetDatabaseNames()
        {
            return _dBContext.GetDBNames();
        }

        public Database GetDatabase(string name)
        {
            var database = _dBContext.Databases.Where(database => database.Name == name).FirstOrDefault();

            if(database == null)
                database = _dBContext.LoadDatabase(name);

            return database;
        }

        public void CreateDatabase(string name) 
        {
            var database = new Database() { Name = name };

            _dBContext.Databases.Add(database);

            _dBContext.SaveDatabase(name);
        }

        public void DeleteDatabase(string name)
        {
            _dBContext.DeleteDatabase(name);
            _dBContext.SaveDatabases();
        }

        public bool DatabaseExists(string name)
        {
            return _dBContext.DatabaseExists(name);
        }

        public IEnumerable<string> GetTableNames(string dbName)
        {
            var database = GetDatabase(dbName);

            var tables = database?.Tables.Select(table => table.Name);

            return tables ?? new List<string>();
        }

        public IEnumerable<Table> GetTables(string dbName)
        {
            var database = GetDatabase(dbName);

            return database?.Tables;
        }

        public Table GetTable(string dbName, string tableName)
        {
            var database = GetDatabase(dbName);

            return database?.Tables.Where(t => t.Name == tableName).FirstOrDefault();
        }

        public void CreateTable(string dbName, Table table)
        {
            if (!Validator.ValidateTable(table)) throw new ApplicationException();

            var database = GetDatabase(dbName);

            if (database is null) return;

            database.Tables.Add(table);
            _dBContext.SaveDatabase(dbName);
        }

        public void DeleteTable(string dbName, string tableName) 
        {
            var database = GetDatabase(dbName);

            database.Tables.Remove(database?.Tables.Where(t => t.Name == tableName).FirstOrDefault());

            _dBContext.SaveDatabase(dbName);
        }

        public bool TableExists(string dbName, string tableName)
        {
            if(!DatabaseExists(dbName)) return false;

            return GetTable(dbName, tableName) is not null;
        }

        public Column GetColumn(string dbName, string tableName, string columnName)
        {
            var table = GetTable(dbName, tableName);

            return table?.Columns.Where(c => c.Name == columnName).FirstOrDefault();
        }

        public void CreateColumn(string dbName, string tableName, Column column)
        {
            if(ColumnExists(dbName, tableName, column.Name)) return;

            var table = GetTable(dbName, tableName);
            table.Columns.Add(column);
            table.Rows.ForEach(r => r.Values.Add(string.Empty));

            _dBContext.SaveDatabase(dbName);
        }

        public bool ColumnExists(string dbName, string tableName, string columnName)
        {
            if(!TableExists(dbName, tableName)) return false;

            return GetColumn(dbName, tableName, columnName) is not null;
        }

        public IEnumerable<Row> GetRows(string dbName, string tableName)
        {
            var table = GetTable(dbName, tableName);

            return table.Rows;
        }

        public bool RowExists(string dbName, string tableName, int rowId)
        {
            if (!TableExists(dbName, tableName)) return false;

            var table = GetTable(dbName, tableName);

            return table.Rows.Where(r => r.Id == rowId).Any();
        }

        public void AddRow(string dbName, string tableName, Row row)
        {
            var table = GetTable(dbName, tableName);

            if (!Validator.ValidateRow(table, row)) throw new ApplicationException();

            if (table is null) return;

            table.Rows.Add(row);
            _dBContext.SaveDatabase(dbName);
        }

        public void DeleteRow(string dbName, string tableName, int rowId)
        {
            var table = GetTable(dbName, tableName);

            if(table is null || !table.Rows.Where(r => r.Id == rowId).Any()) return;

            table.Rows.Remove(table.Rows.Where(r => r.Id == rowId).FirstOrDefault());

            _dBContext.SaveDatabase(dbName);
        }

        public void UpdateRow(string dbName, string tableName, Row row)
        {
            if (!RowExists(dbName, tableName, row.Id)) return;
            
            var table = GetTable(dbName, tableName);

            if (!Validator.ValidateRow(table, row)) throw new ApplicationException();

            var oldRow = table.Rows.Where(r => r.Id == row.Id).FirstOrDefault();

            row.Values.ForEach(v => oldRow.Values.Add(v));
        }

        public IEnumerable<Row> FindRowsByTemplate(string dbName, string tableName, string template)
        {
            var table = GetTable(dbName, tableName);

            return table.Rows.Where(r => r.Values.Contains(template));
        }
    }
}
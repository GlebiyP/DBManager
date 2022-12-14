using DBManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Services
{
    public interface IDBManagerService
    {
        public IEnumerable<string> GetDatabaseNames();

        public Database GetDatabase(string databaseName);

        public void CreateDatabase(string databaseName);

        public void DeleteDatabase(string databaseName);

        public bool DatabaseExists(string databaseName);

        public IEnumerable<string> GetTableNames(string databaseName);

        public IEnumerable<Table> GetTables(string databaseName);

        public Table GetTable(string databaseName, string tableName);

        public void CreateTable(string databaseName, Table table);

        public void DeleteTable(string databaseName, string tableName);

        public bool TableExists(string databaseName, string tableName);

        public IEnumerable<Row> GetRows(string databaseName, string tableName);

        public void AddRow(string databaseName, string tableName, Row row);

        public void UpdateRow(string databaseName, string tableName, Row row);

        public void DeleteRow(string databaseName, string tableName, int rowId);

        public IEnumerable<Row> FindRowsByTemplate(string databaseName, string tableName, string template);

        public bool RowExists(string databaseName, string tableName, int rowId);

        public Column GetColumn(string databaseName, string tableName, string columnName);

        public void CreateColumn(string databaseName, string tableName, Column column);

        public bool ColumnExists(string databaseName, string tableName, string columnName);

    }
}

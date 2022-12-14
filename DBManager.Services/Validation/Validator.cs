using DBManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DBManager.Services.Validation
{
    public class Validator
    {
        public static bool ValidateRow(Table table, Row row)
        {
            if(table.Columns.Count != row.Values.Count) return false;

            bool isValid = true;

            for (int i = 0; i < table.Columns.Count; i++)  
            {
                var validate = ValidateColumn(table.Columns.ElementAt(i).Type);

                if(!(isValid &= validate(row.Values[i]))) return false;
            }

            return true;
        }

        public static bool ValidateTable(Table table)
        {
            foreach(Row row in table.Rows)
            {
                if(!Validator.ValidateRow(table, row)) return false;
            }

            return true;
        }

        public static Predicate<string> ValidateColumn(string type)
        {
            var moneyRegex = @"^[0-9]{1,13}(\.[0-9]{1,2})*|(10000000000000)(\.00)*$";

            return type.ToUpper() switch
            {
                "STRING" => (string a) => true,
                "CHAR" => (string a) => Char.TryParse(a, out _),
                "REAL" => (string a) => Double.TryParse(a, out _),
                "INTEGER" => (string a) => Int32.TryParse(a, out _),
                "$" => (string a) => Regex.Match(a, moneyRegex).Value.Equals(a),
                "INVL" => (string a) => ValidateMoneyInterval(a),
                _ => (string a) => false
            };
        }

        public static bool ValidateMoneyInterval(string interval)
        {
            try
            {
                var values = interval.Split('-').Select(v => v.Trim()).ToList();
                var validator = ValidateColumn("$");

                if (!validator(values[0]) || !validator(values[1])) return false;

                var firstPart = values[0].Split('.');
                var secondPart = values[1].Split('.');
                var firstInt = BigInteger.Parse(firstPart[0]);
                var secondInt = BigInteger.Parse(secondPart[0]);
                var firstDec = Decimal.Parse(firstPart.ElementAtOrDefault(1) ?? "0");
                var secondDec = Decimal.Parse(secondPart.ElementAtOrDefault(1) ?? "0");

                return firstInt < secondInt || firstInt == secondInt && firstDec < secondDec;
            }
            catch
            {
                return false;
            }
        }
    }
}

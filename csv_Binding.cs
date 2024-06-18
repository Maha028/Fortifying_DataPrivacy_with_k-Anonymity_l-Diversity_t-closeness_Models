using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataArmor
{
    internal class csv_Binding
    {
        static public List<string> num_col = new List<string>();
        static public List<string> cat_col = new List<string>();
        static List<string> col_name = new List<string>();
        public static DataTable csv_opening(string csvFilePath)
        {
            var dataTable = new DataTable();
            using (StreamReader reader = new StreamReader(csvFilePath))
            {
                string[] headers = reader.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dataTable.Columns.Add(header, typeof(string)); 
                    col_name.Add(header);
                }
                while (!reader.EndOfStream)
                {
                    string[] data = reader.ReadLine().Split(',');
                    dataTable.Rows.Add(data);
                }
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    if (IsNumericColumn(dataTable, i))
                    {
                        num_col.Add(dataTable.Columns[i].ColumnName);
                        ConvertColumnToNumeric(dataTable, i);
                    }
                    else
                    {
                        cat_col.Add(dataTable.Columns[i].ColumnName);
                    }
                }

            }
            var dataTabl = new DataTable();
            using (StreamReader reader = new StreamReader(csvFilePath))
            {
                string[] headers = reader.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    if (cat_col.Contains(header))
                    {
                        dataTabl.Columns.Add(header, typeof(string)); // Default type is string
                    }
                    else
                    {
                        dataTabl.Columns.Add(header, typeof(double));
                    }
                }

                while (!reader.EndOfStream)
                {
                    string[] data = reader.ReadLine().Split(',');
                    dataTabl.Rows.Add(data);
                }
            }
            return dataTabl;
        }
        static bool IsNumericColumn(DataTable dataTable, int columnIndex)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                if (!IsNumeric(row[columnIndex].ToString()))
                {
                    return false;
                }
            }
            return true;
        }
        static bool IsNumeric(string value)
        {
            double result;
            return double.TryParse(value, out result);
        }

        static void ConvertColumnToNumeric(DataTable dataTable, int columnIndex)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                double value;
                if (double.TryParse(row[columnIndex].ToString(), out value))
                {
                    row[columnIndex] = value;
                }
            }
        }
    }
}

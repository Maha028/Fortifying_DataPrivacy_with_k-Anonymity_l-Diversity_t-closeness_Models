using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataArmor
{
    internal class mergeTable
    {

        public static DataTable MergeTables(List<DataTable> tables)
        {
            DataTable mergedTable = tables[0].Clone();
            foreach (var table in tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    mergedTable.ImportRow(row);
                }
            }
            return mergedTable;
        }
        public static DataTable MergeTables(List<DataTable> tables,List <string> num_QID)
        {
            DataTable mergedTable = tables[0].Clone();
            foreach (var i in num_QID)
            {
                mergedTable.Columns[i].DataType = typeof(string);
            }
            foreach (var table in tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    mergedTable.ImportRow(row);
                }
            }
            return mergedTable;
        }
    }
}

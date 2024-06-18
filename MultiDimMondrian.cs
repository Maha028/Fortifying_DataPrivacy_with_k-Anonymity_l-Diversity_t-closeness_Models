using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
namespace DataArmor
{
   
    public class MultiDimMondrian
    {
        public decimal K;
        public List<string> QIDS = new List<string>();
        public List<string> num_QID;
        public List<string> cat_QID;
        public DataTable data;
        public DataTable anonymizedDataTable;
        public MultiDimMondrian(DataTable dt, decimal k, List<string> cat_qid, List<string> num_qid)
        {
            data= dt;
            this.K = k;
            this.num_QID = num_qid.ToList();
            this.cat_QID = cat_qid.ToList();
            this.QIDS.AddRange(cat_qid);
            this.QIDS.AddRange(num_qid);

            this.QIDS = this.SortColumnsByDiversity(this.data, QIDS);
            
            this.SortDataTable();

            List<DataTable> anonymizedData = Mondrian(data);
            //counting of quasi qombination
            Dictionary<string, decimal> c = counting(data, QIDS.ToArray());

            this.anonymizedDataTable = mergeTable.MergeTables(anonymizedData,num_qid);

            cat_QID = this.SortColumnsByDiversity(this.data, cat_QID);
            
            this.anonymizedDataTable = Swapping(data, QIDS.ToArray(), cat_QID[0], c);
            
            anonymizedData = Mondrian(anonymizedDataTable);
            
            anonymizedData = AnonymizePartitions(anonymizedData, QIDS.ToArray(), num_QID.ToArray());
            this.num_QID.Clear();
            
            anonymizedData = AnonymizePartitions(anonymizedData, QIDS.ToArray(), num_QID.ToArray());
            
            this.anonymizedDataTable = mergeTable.MergeTables(anonymizedData, num_qid);
        }
        public List<string> SortColumnsByDiversity(DataTable dataTable, List<string> Q_ID)
        {
            List<string> columnNames = Q_ID
                .OrderByDescending(columnName => dataTable.AsEnumerable()
                    .Select(row => row[columnName])
                    .Distinct()
                    .Count())
                .ToList();
            return columnNames;

        }

        public void SortDataTable()
        {
            string sortExpression = string.Join(", ", this.QIDS);

            this.data.DefaultView.Sort = sortExpression;
            this.data = this.data.DefaultView.ToTable();
        }
        public List<DataTable> Mondrian(DataTable data)
        {
            List<DataTable> partitions = new List<DataTable>();
            data = data.DefaultView.ToTable();

            if (data.Rows.Count <= 2 * this.K - 1)
            {
                partitions.Add(data);
                return partitions;
            }

            int si = data.Rows.Count;
            int mid = si / 2;

            DataTable lhs = data.Clone();
            DataTable rhs = data.Clone();
            for (int i = 0; i < mid; i++)
            {
                lhs.ImportRow(data.Rows[i]);
            }

            for (int i = mid; i < si; i++)
            {
                rhs.ImportRow(data.Rows[i]);
            }

            partitions.AddRange(Mondrian(lhs));
            partitions.AddRange(Mondrian(rhs));
            return partitions;
        }

        public List<DataTable> AnonymizePartitions(List<DataTable> resultPartitions, string[] qid, string[] numQid)
        {
            int g = 0;
            foreach (string qi in qid)
            {
                //generlization & Perturbation counter
                Dictionary<string, decimal> c = counting(this.data, this.QIDS.ToArray());
                for (int i = 0; i < resultPartitions.Count; i++)
                {
                    DataTable partition = resultPartitions[i];

                    if (CountQiCombinations(partition, qid) == 1)
                    {
                        continue;
                    }
                    else
                    {
                        if (numQid.Contains(qi))
                        {
                            if (g % 2 == 0)
                            {
                                partition = GeneralizeNumerical(partition, data, qi, qid, c);

                            }
                            else
                            {
                                partition = Perturbation(partition, qi, c);

                            }
                        }
                        else
                        {
                            partition = Suppression(partition, qi, c);
                        }
                        resultPartitions[i] = partition;
                    }
                }
                this.data = mergeTable.MergeTables(resultPartitions);
                this.SortDataTable();
                resultPartitions = Mondrian(this.data);
                if (numQid.Contains(qi))
                { ++g; }

            }
            return resultPartitions;
        }
        private int CountQiCombinations(DataTable partition, string[] quasiIdentifiers)
        {
            HashSet<string> qiCombinations = new HashSet<string>();

            foreach (DataRow row in partition.Rows)
            {
                List<string> qiValues = new List<string>();
                foreach (string qi in quasiIdentifiers)
                {
                    qiValues.Add(row[qi].ToString());
                }
                string qiCombination = string.Join(",", qiValues);
                qiCombinations.Add(qiCombination);
            }

            return qiCombinations.Count;
        }

        public DataTable GeneralizeNumerical(DataTable partition, DataTable data, string numQid, string[] QID, Dictionary<string, decimal> c)
        {
            DataTable dt2 = partition.Clone();
            dt2.Columns[numQid].DataType = typeof(string);
            foreach (DataRow dr in partition.Rows)
            {
                dt2.ImportRow(dr);
            }
            dt2.AcceptChanges();

            double min = data.AsEnumerable().Min(row => Convert.ToDouble(row[numQid]));
            double max = data.AsEnumerable().Max(row => Convert.ToDouble(row[numQid]));
            double mid = (min + max) / 2;
            // Modify the values
            foreach (DataRow row in dt2.Rows)
            {
                string rowQidValues = string.Join(",", QID.Select(column => row[column]));
                // Check if the quasi-identifier values are identical in at least k rows in the original data
                if (c[rowQidValues] >= this.K)
                {
                    continue;
                }
                else
                {
                    double value = Convert.ToDouble(row[numQid]);
                    if (value >= mid)
                    {
                        row[numQid] = $">={mid}";
                    }
                    else
                    {
                        row[numQid] = $"<{mid}";
                    }
                }
            }
            return dt2;
        }

        public DataTable Perturbation(DataTable partition, string numQid, Dictionary<string, decimal> c)
        {
            DataTable dt2 = partition.Clone();
            dt2.Columns[numQid].DataType = typeof(string);
            foreach (DataRow dr in partition.Rows)
            {
                dt2.ImportRow(dr);
            }
            dt2.AcceptChanges();
            Random random = new Random();
            double min = partition.AsEnumerable().Min(row => Convert.ToDouble(row[numQid]));
            double max = partition.AsEnumerable().Max(row => Convert.ToDouble(row[numQid]));
            double mid = (min + max) / 2;
           
            double perturbation = random.Next(5, 10);
            // adding the random number
            double pert = mid + perturbation;
            foreach (DataRow row in dt2.Rows)
            {
                string rowQidValues = string.Join(",", this.QIDS.Select(column => row[column]));
                // Check if the quasi-identifier values are identical in at least k rows in the original data
                if (c[rowQidValues] >= this.K)
                {
                    continue;
                }
                else
                {
                  
                        row[numQid] =pert;
                    
                }
            }
            return dt2;
        }
        public DataTable Suppression(DataTable partition, string attributeName, Dictionary<string, decimal> c)
        {
            string replacement = "******";
            List<string> m = new List<string>();
        
            foreach (DataRow row in partition.Rows)
            {
                string rowQidValues = string.Join(",", QIDS.Select(column => row[column]));
                if (c.ContainsKey(rowQidValues))
                {
                    if (c[rowQidValues] >= this.K)
                    {
                        continue;
                    }
                    else
                    {
                        row[attributeName] = replacement;
                    }
                }
                else
                {
                    row[attributeName] = replacement;
                }
            }
            return partition;
        }
        public Dictionary<string, decimal> counting(DataTable dt, string[] attributeColumns)
        {
            Dictionary<string, decimal> counti = new Dictionary<string, decimal>();
            var groupedRows = dt.AsEnumerable().GroupBy(row => string.Join(",", attributeColumns.Select(column => row[column])));
            foreach (var row in groupedRows)
            {
                counti.Add(row.Key, row.Count());
            }
            return counti;
        }
        public DataTable Swapping(DataTable partition, string[] QIDS, string qi, Dictionary<string, decimal> c)
        {
            // Counting similar quasi combinations for QidValuesWithoutqi
            Dictionary<string, decimal> countingofsimilarqidswithoutqi = counting(this.data, QIDS.Where(q => q != qi).ToArray());
            DataTable rowsatisfyswappingcondition = this.data.Clone();
            List<string> qiToswap = new List<string>();
            List<int> indexOfRowToSwap = new List<int>();
            List<string> QIDWithOutQI = QIDS.Where(item => item != qi).ToList();

            
            foreach (DataRow row in partition.Rows)
            {
                string rowQidValues = string.Join(",", QIDS.Select(column => row[column]));
                Dictionary<string, decimal> st = counting(rowsatisfyswappingcondition, QIDWithOutQI.ToArray());
                // Check if the counting of a similar quasi combination is k-1
                if (c.ContainsKey(rowQidValues) && c[rowQidValues] == this.K - 1)
                {
                    string[] qidWithoutCountry = QIDS.Where(q => q != qi).ToArray();
                    string rowQidValuesWithoutCountry = string.Join(",", qidWithoutCountry.Select(column => row[column]));

                    // Check if the counting of a similar quasi combination (except for the qi attribute) is k
                    if (countingofsimilarqidswithoutqi.ContainsKey(rowQidValuesWithoutCountry))
                    {

                        if (Decimal.ToInt32(countingofsimilarqidswithoutqi[rowQidValuesWithoutCountry]) >= Decimal.ToInt32(this.K))
                        {
                            // Find rows in the dataset where the qi value matches the qi value in the k-1 quasi combination
                            DataRow rowsWithNotMatchingCountry = this.data.AsEnumerable()
                            .Where(r => (QIDWithOutQI.All(col => r[col].ToString() == row[col].ToString())) && (r[qi].ToString() != row[qi].ToString())).First();

                            if (!st.ContainsKey(rowQidValuesWithoutCountry))
                            {
                                qiToswap.Add(row[qi].ToString());
                                rowsatisfyswappingcondition.ImportRow(rowsWithNotMatchingCountry);
                                indexOfRowToSwap.Add(this.data.Rows.IndexOf(rowsWithNotMatchingCountry));
                            }

                        }
                    }
                }
            }
            int i = 0;
            foreach (DataRow row in rowsatisfyswappingcondition.Rows)
            {

                var rowsWithMatchingCountry = this.data.AsEnumerable()
                    .Where(r => (r[qi].ToString() == qiToswap[i] && (row[QIDS[0]].ToString() != r[QIDS[0]].ToString() || row[QIDS[2]].ToString() != r[QIDS[2]].ToString())));
                // If such rows exist, perform the swap
                if (rowsWithMatchingCountry.Any())
                {
                    DataRow rowToSwap = rowsWithMatchingCountry.First();
                    var temp = row[qi];
                    this.data.Rows[indexOfRowToSwap[i]][qi] = rowToSwap[qi];
                    this.data.Rows[this.data.Rows.IndexOf(rowToSwap)][qi] = temp;
               
                    i++;
                }
            }
            return partition;

        }
    }
}
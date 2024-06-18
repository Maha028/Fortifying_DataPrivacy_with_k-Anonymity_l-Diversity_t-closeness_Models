using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataArmor
{
    internal class correlationClassification
    {
        public DataTable data;
        public DataTable cramerV_dt = new DataTable();
        public DataTable pearson_dt = new DataTable();
        List<string> col_name = new List<string>();
        public List<string> num_col = new List<string>();
        public List<string> cat_col = new List<string>();
        public List<string> num_QID = new List<string>();
        public List<string> QID = new List<string>();
        public List<string> EID = new List<string>();
        public List<string> SA = new List<string>();
        public List<List<string>> possible_comb = new List<List<string>>();
        public double threshold = 0;
        public string first_col = "       ";
        public correlationClassification(string csvPath)
        {
            this.data = this.csv_opening(csvPath);
            FindUniqueColumnName(data);
            cramerV_Datatable();
            // Pearson_datatable();
        }
        void cramerV_Datatable()
        {
            threshold = 0;
            double corrSum = 0;
            double corrCount = 0;
            cramerV_dt.Columns.Add(first_col);
            foreach (var i in col_name)
            {
                cramerV_dt.Columns.Add(new DataColumn(i, typeof(double)));
                cramerV_dt.Rows.Add();

            }
            var h = 0;
            foreach (var i in col_name)
            {
                cramerV_dt.Rows[h][0] = i;
                h++;
            }
            int s = 0;
            for (int i = 0; i < col_name.Count(); i++)
            {
                for (int j = 0; j < col_name.Count(); j++)
                {
                    if (i == j)
                    {
                        cramerV_dt.Rows[j][i + 1] = 1;
                    }
                    else
                    {
                        double cramerV = CalculateCramersV(this.data, col_name[i], col_name[j]);
                        cramerV_dt.Rows[j][i + 1] = Math.Round(cramerV, 2);
                        corrSum += cramerV;
                        corrCount += 1;
                        s++;
                        if (s >= col_name.Count())
                        {
                            break;
                        }
                    }
                }
            }
            threshold = corrSum / corrCount;
            for (var i = 0; i < cramerV_dt.Columns.Count; i++)
            {
                for (var j = 0; j < cramerV_dt.Rows.Count; j++)
                {
                    var cellValue = cramerV_dt.Rows[j][i].ToString();
                    double doubleValue;
                    if (double.TryParse(cellValue, out doubleValue))
                    {
                        if (doubleValue == 1)
                        {
                            continue;
                        }
                        else if (doubleValue < threshold)
                        {
                            if (QID.Contains(cramerV_dt.Columns[i].ColumnName))
                            {
                                continue;
                            }
                            else
                            {
                                QID.Add(cramerV_dt.Columns[i].ColumnName);
                            }

                        }
                        else
                        {
                            if (!(SA.Contains(cramerV_dt.Columns[i].ColumnName)))
                            {
                                if (!(QID.Contains(cramerV_dt.Columns[i].ColumnName)))
                                {
                                    SA.Add(cramerV_dt.Columns[i].ColumnName);
                                }
                            }

                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
        /*
        void Pearson_datatable()
        {
            double corrSum = 0;
            double corrCount = 0;
            threshold = 0;
            pearson_dt.Columns.Add(first_col);
            foreach (var i in num_col)
            {
                pearson_dt.Columns.Add(new DataColumn(i, typeof(double)));
                pearson_dt.Rows.Add();
            }
            var a = 0;
            foreach (var i in num_col)
            {
                pearson_dt.Rows[a][0] = i;
                a++;
            }


            int s = 0;
            // Pearson correlation for numeric values
            for (int i = 0; i < num_col.Count(); i++)
            {
                for (int j = 0; j < num_col.Count(); j++)
                {
                    if (i == j)
                    {
                        pearson_dt.Rows[j][i + 1] = 1;
                    }
                    else
                    {
                        double pearson = CalculatePearsonCorrelation(this.data, num_col[i], num_col[j]);

                        if (pearson < 0)
                        {
                            pearson *= -1;

                        }
                        pearson_dt.Rows[j][i + 1] = Math.Round(pearson, 2);
                        corrSum += pearson;
                        corrCount += 1;
                        s++;
                        if (s >= cat_col.Count())
                        {
                            break;
                        }

                    }
                }
            }
            threshold = corrSum / corrCount;
            for (var i = 0; i < pearson_dt.Columns.Count; i++)
            {
                for (var j = 0; j < pearson_dt.Rows.Count; j++)
                {
                    var cellValue = pearson_dt.Rows[j][i].ToString();

                    // Convert the string cell value to double
                    double doubleValue;
                    if (double.TryParse(cellValue, out doubleValue))
                    {
                        if (doubleValue == 1)
                        {
                            continue;
                        }
                        else if (doubleValue <= threshold)
                        {
                            if (num_QID.Contains(cramerV_dt.Columns[i].ColumnName) || cat_QID.Contains(cramerV_dt.Columns[i].ColumnName))
                            {
                                continue;
                            }
                            else
                            {
                                if (num_col.Contains(cramerV_dt.Columns[i].ColumnName))
                                {
                                    num_QID.Add(cramerV_dt.Columns[i].ColumnName);
                                }
                                else if (cat_col.Contains(cramerV_dt.Columns[i].ColumnName))
                                {
                                    cat_QID.Add(cramerV_dt.Columns[i].ColumnName);
                                }

                            }
                        }
                        else
                        {
                            SA.Add(cramerV_dt.Columns[i].ColumnName);

                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }

        }*/
        double CalculateCramersV(DataTable dataTable, string column1, string column2)
        {
            // Create contingency table
            Dictionary<string, Dictionary<string, int>> confusionMatrix = CreateConfusionMatrix(dataTable, column1, column2);
            double chi2 = CalculateChiSquare(confusionMatrix);
            int n = dataTable.Rows.Count;

            int r = confusionMatrix.Keys.Count();
            int k = confusionMatrix.SelectMany(row => row.Value.Keys).Distinct().Count();

            double phi2 = chi2 / n;
            double phi2corr = Math.Max(0, phi2 - ((k - 1) * (r - 1)) / (n - 1));
            double rcorr = r - Math.Pow(r - 1, 2) / (n - 1);
            double kcorr = k - Math.Pow(k - 1, 2) / (n - 1);
            double v;
            try
            {
                if (Math.Min(kcorr - 1, rcorr - 1) == 0)
                {
                    throw new Exception("Unable to calculate Cramer's V using bias correction. Consider not using bias correction");
                }
                else
                {
                    v = Math.Sqrt(phi2corr / Math.Min(kcorr - 1, rcorr - 1));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                v = 1;
            }
            return v;
        }

        Dictionary<string, Dictionary<string, int>> CreateConfusionMatrix(DataTable dataTable, string column1, string column2)
        {
            var confusionMatrix = new Dictionary<string, Dictionary<string, int>>();

            foreach (DataRow row in dataTable.Rows)
            {
                string val1 = row[column1].ToString();
                string val2 = row[column2].ToString();

                if (!confusionMatrix.ContainsKey(val1))
                {
                    confusionMatrix[val1] = new Dictionary<string, int>();
                }

                if (!confusionMatrix[val1].ContainsKey(val2))
                {
                    confusionMatrix[val1][val2] = 0;
                }
                confusionMatrix[val1][val2]++;
            }

            return confusionMatrix;
        }


        double CalculateChiSquare(Dictionary<string, Dictionary<string, int>> confusionMatrix)
        {
            double chi2 = 0;
            int rowCount = confusionMatrix.Keys.Count;
            int columnCount = confusionMatrix.SelectMany(row => row.Value.Keys).Distinct().Count();

            var rowSums = confusionMatrix.ToDictionary(r => r.Key, r => r.Value.Sum(kv => kv.Value));

            var columnSums = confusionMatrix.Values
    .SelectMany(r => r.Keys)
    .Distinct()
    .ToDictionary(
        c => c,
        c => confusionMatrix.Values.Sum(r => r.ContainsKey(c) ? r[c] : 0)
    );

            var totalSum = confusionMatrix.Values.Sum(r => r.Values.Sum());

            foreach (var rowLabel in confusionMatrix.Keys)
            {
                foreach (var columnLabel in confusionMatrix[rowLabel].Keys)
                {
                    double observed = confusionMatrix[rowLabel][columnLabel];
                    //double rowSum = rowSums[rowLabel];
                    double expected = (rowSums[rowLabel] * columnSums[columnLabel]) / totalSum;
                    if (expected != 0) //expected value is not zero
                    {
                        chi2 += Math.Pow(observed - expected, 2) / expected;
                    }
                }
            }

            return chi2;
        }
        public DataTable csv_opening(string csvFilePath)
        {
            var dataTable = new DataTable();
            using (StreamReader reader = new StreamReader(csvFilePath))
            {
                string[] headers = reader.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dataTable.Columns.Add(header, typeof(string));
                    this.col_name.Add(header);
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
                        this.num_col.Add(dataTable.Columns[i].ColumnName);
                        ConvertColumnToNumeric(dataTable, i);
                    }
                    else
                    {
                        this.cat_col.Add(dataTable.Columns[i].ColumnName);
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
        bool IsNumericColumn(DataTable dataTable, int columnIndex)
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
        bool IsNumeric(string value)
        {
            double result;
            return double.TryParse(value, out result);
        }

        void ConvertColumnToNumeric(DataTable dataTable, int columnIndex)
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
        void FindUniqueColumnName(DataTable table)
        {
            foreach (DataColumn column in table.Columns)
            {
                var uniqueValues = table.AsEnumerable()
                                        .Select(row => row[column.ColumnName])
                                        .Distinct()
                                        .Count();

                var totalValues = table.Rows.Count;

                if (uniqueValues == totalValues)
                {
                    EID.Add(column.ColumnName);
                    if (cat_col.Contains(column.ColumnName))
                    {
                        cat_col.Remove(column.ColumnName);
                    }
                    else if (num_col.Contains(column.ColumnName))
                    {
                        num_col.Remove(column.ColumnName);
                    }

                }
            }

        }

        double CalculatePearsonCorrelation(DataTable dataTable, string column1, string column2)
        {
            List<double> x = dataTable.AsEnumerable().Select(row => row.Field<double>(column1)).ToList();
            List<double> y = dataTable.AsEnumerable().Select(row => row.Field<double>(column2)).ToList();
            if (x.Count != y.Count)
            {
                throw new ArgumentException("Lists must have the same length");
            }
            int n = x.Count;
            double sumX = x.Sum();
            double sumY = y.Sum();
            double sumXSquared = x.Sum(xi => xi * xi);
            double sumYSquared = y.Sum(yi => yi * yi);
            double sumXY = x.Zip(y, (xi, yi) => xi * yi).Sum();
            double numerator = n * sumXY - sumX * sumY;
            double denominator = Math.Sqrt((n * sumXSquared - Math.Pow(sumX, 2)) * (n * sumYSquared - Math.Pow(sumY, 2)));

            if (denominator == 0)
            {
                throw new DivideByZeroException("Denominator is zero");
            }
            return numerator / denominator;
        }


    }
}

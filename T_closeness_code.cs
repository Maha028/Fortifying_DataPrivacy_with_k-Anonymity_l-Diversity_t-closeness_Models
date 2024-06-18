using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DataArmor
{
    internal class T_closeness_code
    {
        public DataTable dataTable = new DataTable();
        public List<string> SA = new List<string>();
        double T;
        public List<string> QID = new List<string>();


        public T_closeness_code(DataTable data, double t, List<string> sa, List<string> qid)
        {
            dataTable = data;
            T = t;
            SA = sa;
            QID = qid;

            List<DataTable> PartitionNeedSwapping = new List<DataTable>();
            List<DataTable> ResultPartition = new List<DataTable>();
            List<DataTable> PartitionsAfterSwapping = new List<DataTable>();
            foreach (string s in SA)
            {
                List<DataTable> partitions = PartitionData(dataTable, QID);
                foreach (DataTable partition in partitions)
                {
                    List<string> relativeFrequencies = RelativeFrequencies(partition, s);
                    double emd = EMD(OverallDistribution(dataTable, relativeFrequencies, s), relativeFrequencies);

                    if (emd <= T)
                    {
                        ResultPartition.Add(partition);
                    }

                    else
                    {
                        PartitionNeedSwapping.Add(partition);
                    }
                }
                PartitionsAfterSwapping = Swapping(PartitionNeedSwapping, s);
                DataTable mergedTable = new DataTable();
                if (PartitionsAfterSwapping.Count > 1)
                {
                    mergedTable = mergeTable.MergeTables(PartitionsAfterSwapping);
                    ResultPartition.Add(mergedTable);
                }
                else if (PartitionsAfterSwapping.Count == 1)
                {
                    ResultPartition.Add(PartitionsAfterSwapping[0]);
                }

                dataTable = mergeTable.MergeTables(ResultPartition);
            }
        }

        public static List<DataTable> PartitionData(DataTable dataTable, List<string> QID)
        {
            List<DataTable> partitions = new List<DataTable>();

            Dictionary<string, DataTable> partitionTables = new Dictionary<string, DataTable>();

            foreach (DataRow row in dataTable.Rows)
            {
                string key = GenerateKey(row, QID);

                if (!partitionTables.ContainsKey(key))
                {
                    DataTable partitionTable = dataTable.Clone();
                    partitionTables.Add(key, partitionTable);
                    partitions.Add(partitionTable);
                }

                partitionTables[key].ImportRow(row);
            }

            return partitions;
        }

        // Helper method to generate a key based on QID values for a row
        public static string GenerateKey(DataRow row, List<string> QID)
        {
            List<string> keyValues = new List<string>();
            foreach (string column in QID)
            {
                string value = row[column].ToString();
                if (value != "******")
                {
                    keyValues.Add(value);
                }
            }
            return string.Join("\t", keyValues);
        }

        public static double[] OverallDistribution(DataTable dataTable, List<string> sensitiveValues, string sa)
        {
            int totalCount = 0;
            List<string> SensitiveValuesOnALL = new List<string>();

            foreach (DataRow row in dataTable.Rows)
            {
                totalCount++;

                object fieldValueObject = row[sa];
                string value = fieldValueObject.ToString();
                if (sensitiveValues.Contains(value))
                {
                    SensitiveValuesOnALL.Add(value);
                }
            }
            var groupedData = SensitiveValuesOnALL
                .GroupBy(x => x)
                .OrderBy(g => sensitiveValues.IndexOf(g.Key)); // Sort the groups based on the order in sensitiveValues
            int uniqueValueCount = groupedData.Count();
            double[] histogramRatios = new double[uniqueValueCount];

            int i = 0;
            foreach (var group in groupedData)
            {
                int count = group.Count();
                histogramRatios[i] = count / (double)totalCount;
                histogramRatios[i] = Math.Round(histogramRatios[i], 1);
                i++;
            }

            return histogramRatios;
        }


        public static List<string> RelativeFrequencies(DataTable partitions, string sa)
        {
            List<string> sensitiveValues = new List<string>();
            foreach (DataRow row in partitions.Rows)
            {
                object fieldValueObject = row[sa];
                string value = fieldValueObject.ToString(); ;
                sensitiveValues.Add(value);
            }
            return sensitiveValues;
        }

        public static double EMD(double[] overallDistribution, List<string> relativeFrequencies)
        {
            double[] overallHistogram = overallDistribution;

            double[] relativeHistogram = Histogram(relativeFrequencies);

            if (overallHistogram.Length != relativeHistogram.Length)
            {
                throw new ArgumentException("Distributions must have the same length.");
            }

            // Calculate the Earth Mover's Distance
            double emd = 0.0;
            for (int i = 0; i < overallHistogram.Length; i++)
            {
                emd += Math.Abs(overallHistogram[i] - relativeHistogram[i]);
            }

            return emd;
        }

        public static double[] Histogram(List<string> data)
        {
            var groupedData = data.GroupBy(x => x);
            int uniqueValueCount = groupedData.Count();
            double totalCount = data.Count;
            double[] histogramRatios = new double[uniqueValueCount];

            int i = 0;
            foreach (var group in groupedData)
            {
                int count = group.Count();
                histogramRatios[i] = count / totalCount;
                histogramRatios[i] = Math.Round(histogramRatios[i], 1);
                i++;
            }
            return histogramRatios;
        }
        public static List<DataTable> Swapping(List<DataTable> Partitions, string sa)
        {
            for (int i = 0; i < Partitions.Count - 1; i += 2)
            {
                DataTable currentPartition = Partitions[i];
                DataTable nextPartition = Partitions[i + 1];
                if (nextPartition.Rows.Count >= 2)
                {
                    DataRow currentRow = currentPartition.Rows[0];
                    string value1 = currentRow.Field<string>(sa);
                    DataRow nextRow = nextPartition.Rows[1];
                    string value2 = nextRow.Field<string>(sa);

                    /*
                     object[] currentRowValues = currentRow.ItemArray;
                     object[] nextRowValues = nextRow.ItemArray;*/
                    MessageBox.Show($"{value1} Swapped with {value2} to achieve t-closeness");
                    currentRow[sa] = value2;
                    nextRow[sa] = value1;
                }
            }
            return Partitions;
        }
    }
}
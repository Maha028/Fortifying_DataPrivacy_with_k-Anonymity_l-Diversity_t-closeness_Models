using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataArmor
{
    internal class L_diversity_code
    {
        public decimal L;
        public List<string> QID = new List<string>();
        public List<string> SA;
        public DataTable data;
        public List <DataTable>  anonymizedDataTable;
        public List<DataTable> not_satisfy_l=new List<DataTable>();
        public List<DataTable> satisfy_l= new List<DataTable>();
        public L_diversity_code(DataTable data, List<string> QID,decimal l, List<string>SA) {
        
            this .data = data;
            this .QID = QID;
            this .SA = SA;
            this.L = l;
            Dictionary<string, string[]> expa_val = new Dictionary<string, string[] > ();
            string[] e = { "", "" };
            this.anonymizedDataTable = ClusterData(data,QID);
            foreach (string s in this.SA) {
                foreach (var p in this.anonymizedDataTable) {
                    if( LDiversityCheck(p, s))
                    {
                        satisfy_l.Add(p);
                    }
                    else
                    {
                        not_satisfy_l .Add(p);
                        foreach(DataRow dr in p.Rows)
                        {
                            if (!(p.AsEnumerable().Count(row => row[s].ToString() == dr[s].ToString()) == 1))
                            {
                                if (!expa_val.ContainsKey(dr[s].ToString()))
                                {
                                    expa_val.Add(dr[s].ToString(), e);
                                }
                            }
                        }
                    }

                } 
            }
            string[] keysCopy = expa_val.Keys.ToArray();
            foreach (string s in keysCopy)
            {
                string userInput = GetUserInput(s);
                string[] expanding_values = userInput.Split(',');
                expa_val[s] = expanding_values;
            }
            Expanding(expa_val);
            satisfy_l.AddRange(not_satisfy_l);
            this.data = mergeTable.MergeTables(satisfy_l);
        }
        public void Expanding(Dictionary<string, string[]> expa_val)
        {
            foreach (string s in SA)
            {
                foreach (DataTable dt in not_satisfy_l)
                {
                    int i = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        string val = dr[s].ToString();
                        if (expa_val.ContainsKey(val))
                        {
                            if (i >= expa_val[val].Length)
                            {
                                i = 0;
                            }
                            dr[s] = expa_val[val][i];
                            i++;
                            
                        }
                    }
                }
            }
        }
        private string GetUserInput(string s)
        {
            string prompt = $"Enter Expanding values for {s} seperated by ,";
            string expand="";
            bool t = true;
     
            do
            {
                if (!t)
                {
                    prompt = $"Enter Proper Expanding Values for {s} (seperated by ,)";
                }
                expand = Microsoft.VisualBasic.Interaction.InputBox(prompt, "Expanding");
                t=false;

            } while (expand == ""|| expand.Split(',').Count()==1);
            return expand;
        }
        public Boolean LDiversityCheck(DataTable partition, string sensitiveAttribute)
        {
            // Count distinct values of the sensitive attribute
            int distinctValues = partition.AsEnumerable()
                                          .Select(row => row[sensitiveAttribute])
                                          .Distinct()
                                          .Count();
            if (distinctValues >= this.L)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // clustering
        public List<DataTable> ClusterData(DataTable data, List<string> QID)
        {
            Dictionary<string, DataTable> clusters = new Dictionary<string, DataTable>();
            foreach (DataRow row in data.Rows)
            {
                List<string> m = new List<string>();
                foreach (string s in QID)
                {
                    if (row[s].ToString() == "******")
                    {
                        continue;
                    }
                    else
                    {
                        m.Add(s);
                    }
                }
                string key = GenerateKey(row, m);
                string[] l = key.Split(' ');
                foreach (string k in clusters.Keys)
                {
                    int c = 0;
                    foreach (string s in l)
                    {
                        if (k.Contains(s))
                        {
                            c++;
                        }

                    }
                    if (c == l.Length)
                    {
                        key = k;
                        break;
                    }
                }
                if (!clusters.ContainsKey(key))
                {
                    DataTable cluster = data.Clone();
                    cluster.TableName = key;
                    clusters.Add(key, cluster);
                }
                clusters[key].ImportRow(row);
            }
            return new List<DataTable>(clusters.Values);
        }
        private string GenerateKey(DataRow row, List<string> QID)
        {

            string key = string.Join(" ", QID.ConvertAll(column => row[column].ToString()));

            return key;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DataArmor
{
    public partial class t_closeness : Form
    {
        public DataTable dataTable = new DataTable();
        public DataTable M=new DataTable();
        public List<string> SA = new List<string>();
        double T;
        public List<string> QID = new List<string>();

        public t_closeness(DataTable dt, List<string> qid, List<string>sa)
        {
            InitializeComponent();
            dataTable=dt;
            SA=sa;
            QID=qid;
        }

        private void t_closeness_Load(object sender, EventArgs e)
        {

        }
        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
             dataGridView10.Columns[e.ColumnIndex].Width = 170;     }
        private void ok2_btn_Click(object sender, EventArgs e)
        {
            if (double.TryParse(T_val.Text,out T))
            {
                if (T > 0 & T < 1)
                {
                    T_closeness_code d = new T_closeness_code(dataTable, T, SA, QID);
                    M = d.dataTable;
                    dataGridView10.Visible = true;
                    dataGridView10.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView10.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView10.RowHeadersDefaultCellStyle.Font = new Font("Times New Roman", 15, FontStyle.Bold);
                    dataGridView10.RowHeadersDefaultCellStyle.ForeColor = Color.White;
                    dataGridView10.RowHeadersDefaultCellStyle.BackColor = Color.Black;
                    dataGridView10.Font = new Font("Times New Roman", 12, FontStyle.Bold);
                    dataGridView10.DefaultCellStyle.ForeColor = Color.Black;
                    dataGridView10.CellFormatting += DataGridView1_CellFormatting;
                    dataGridView10.BorderStyle = BorderStyle.FixedSingle;
                    dataGridView10.DataSource = M;
                    dataGridView10.Visible = true;
                }
                else
                {
                    MessageBox.Show("Please Enter Valid T value \n\"T must be in the range of 0 to 1.\"", "Error");
                }
            }
            else
            {
                MessageBox.Show("Please Enter Valid T value \n\"T must be number in the range of 0 to 1.\"", "Error");
            }
            
            
            
        }

        private void save_btn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.Title = "Save Anonymized CSV File";
            saveFileDialog1.ShowDialog();
            string s = saveFileDialog1.FileName;
            if (s != "")
            {
                K_AnonymityPage.to_csv(M, s);
            }
        }

        private void dataGridView10_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (ClassificationPage.EID.Count == 0)
            {
                MessageBox.Show("Must Add Explicit IDs! (Go back to classification page)");
            }
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }
    }
}

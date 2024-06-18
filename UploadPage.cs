using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
namespace DataArmor
{
    public partial class UploadPage : Form
    {
        static public string csvPath = "";
        static public string[] columnNames;
        static public DataTable data=new DataTable();
        public UploadPage()
        {
            InitializeComponent();
            if (data.Rows.Count != 0)
            {
                Next.Visible = true;    
                dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.RowHeadersDefaultCellStyle.Font = new Font("Times New Roman", 15, FontStyle.Bold);
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.Black;
                dataGridView1.Font = new Font("Times New Roman", 12, FontStyle.Bold);
                dataGridView1.CellFormatting += DataGridView1_CellFormatting;
                dataGridView1.BorderStyle = BorderStyle.FixedSingle;
                dataGridView1.Visible = true;
                dataGridView1.DataSource = data;
            }
        }
        public void loadform(object Form)
        {
            if (this.mainloop2.Controls.Count > 0)
                this.mainloop2.Controls.RemoveAt(0);
            Form f = Form as Form;
            f.FormBorderStyle = FormBorderStyle.None;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            this.mainloop2.Controls.Clear();
            this.mainloop2.Controls.Add(f);
            this.mainloop2.Tag = f;
            f.Show();
        }
        
        public void Column_name(ref string[] col_name)
        {
            string filePath = csvPath;
            using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
            {
                string headerLine = reader.ReadLine();
                if (headerLine != null)
                {
                    col_name = headerLine.Split(',');
                }
            }
        }
        private void upload1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Title = "Open CSV File",
                Filter = "csv files (*.csv)|*.csv",
                CheckFileExists = true,
                CheckPathExists = true
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
               // MessageBox.Show(openFileDialog.FileName);
                csvPath = openFileDialog.FileName;
            }
            if (csvPath != "")
            {
                ClassificationPage.EID.Clear();
                ClassificationPage.QID.Clear();
                ClassificationPage.num_QID.Clear();
                ClassificationPage.cat_QID.Clear();
                ClassificationPage.SA.Clear();
                Column_name(ref columnNames);
                Next.Visible = true;
                dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; 
                dataGridView1.RowHeadersDefaultCellStyle.Font = new Font("Times New Roman", 15, FontStyle.Bold);
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.Black;
                dataGridView1.Font = new Font("Times New Roman", 12, FontStyle.Bold);
                dataGridView1.CellFormatting += DataGridView1_CellFormatting;
                dataGridView1.BorderStyle = BorderStyle.FixedSingle;
                pictureBox5.Visible = false;
                dataGridView1.Visible = true;
                data = csv_Binding.csv_opening(csvPath);
                dataGridView1.DataSource = data;
            }
            
        }

        private void Next_Click(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(155, 152, 190);
            button1.ForeColor = Color.FloralWhite;
            button2.BackColor = Color.FloralWhite;
            button2.ForeColor = Color.FromArgb(155, 152, 190);
            loadform(new ClassificationPage());
        }
        private void button1_Click(object sender, EventArgs e)
        {
            loadform(new UploadPage());
        }
        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dataGridView1.Columns[e.ColumnIndex].Width = 170;
        }
        private void button2_Click(object sender, EventArgs e)
        {
           
            if (csvPath.Length > 0)
            {

                if (ClassificationPage.num_QID != null)
                {
                    loadform(new ClassificationPage());
                }
                else
                {
                    loadform(new ClassificationPage());
                }
            }

            else
            {
                MessageBox.Show("You must upload a file");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (csvPath == "")
            {
                MessageBox.Show("You must upload a file");
            }
            else if ((ClassificationPage.num_QID == null|| ClassificationPage.num_QID.Count == 0)&& (ClassificationPage.cat_QID ==null || ClassificationPage.cat_QID.Count==0))
            {
                MessageBox.Show(text:"You Must Visit classification page");
            }
            else
            {
                loadform(new K_AnonymityPage( ));
            }
        }

        private void DataArmor_Click(object sender, EventArgs e)
        {
            loadform(new DataArmor());
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (csvPath == "")
            {
                MessageBox.Show("You must upload a file");
            }
            else if (ClassificationPage.EID == null || ClassificationPage.EID.Count == 0)
            {
                MessageBox.Show("You Must Visit classification page");
            }
            else
            {

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (csvPath == "")
            {
                MessageBox.Show("You must upload a file");
            }
            else if (ClassificationPage.EID == null || ClassificationPage.EID.Count == 0)
            {
                MessageBox.Show("You Must Visit classification page");
            }
            else
            {
                loadform(new de_identification1());
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (csvPath == "")
            {
                MessageBox.Show("You must upload a file");
            }
            else if ((ClassificationPage.num_QID == null || ClassificationPage.num_QID.Count == 0) && (ClassificationPage.cat_QID == null || ClassificationPage.cat_QID.Count == 0))
            {
                MessageBox.Show(text: "You Must Visit classification page");
            }
            else
            {
                loadform(new de_identification1());
            }
        }
    }
}

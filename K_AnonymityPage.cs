using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DataArmor
{

    public partial class K_AnonymityPage : Form
    {

        DataTable data=new DataTable();
        public List<string> num_QID = new List<string>();
        public List<string> cat_QID = new List<string>();
        public List<string> SA = new List<string>();
        static public DataTable AnonyTable = new DataTable();
        decimal K;

        public K_AnonymityPage( )
        {
            InitializeComponent();
            num_QID = ClassificationPage.num_QID;
            cat_QID = ClassificationPage.cat_QID;
            data = UploadPage.data;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.RowHeadersDefaultCellStyle.Font = new Font("Times New Roman", 15, FontStyle.Bold);
            dataGridView1.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.Black;
            dataGridView1.Font = new Font("Times New Roman", 12, FontStyle.Bold);
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
            dataGridView1.BorderStyle = BorderStyle.FixedSingle;
            SetColumnWidthsBasedOnContent();
        }



        private void AnonymizedFile_Load(object sender, EventArgs e)
        {

        }
        public void Loadform(object Form)
        {
            if (this.panel2.Controls.Count > 0)
                this.panel2.Controls.RemoveAt(0);
            Form f = Form as Form;
            f.FormBorderStyle = FormBorderStyle.None;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(f);
            this.panel2.Tag = f;
            f.Show();
        }
        private void button1_Click_2(object sender, EventArgs e)
        {
            Loadform(new UploadPage());

        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            Loadform(new ClassificationPage());
        }

        public void button4_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.Title = "Save Anonymized CSV File";
            saveFileDialog1.ShowDialog();
            string s=saveFileDialog1.FileName;
            if (s != "")
            {
                to_csv(AnonyTable, s);
            }


        }
    public static void to_csv(DataTable dtDataTable, string strFilePath)
        {
            StreamWriter sw = new StreamWriter(strFilePath, false);
            //headers
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        
    }

        private void ok2_btn_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            if (K_input.Value <=1)
            {
                MessageBox.Show("Please Enter Valid k value");
            }

            else
            {
                K = K_input.Value;
                dataGridView1.Visible = true;
                MultiDimMondrian m1 = new MultiDimMondrian(data, K, cat_QID, num_QID);
                AnonyTable = m1.anonymizedDataTable;
                dataGridView1.DataSource = AnonyTable;
                save_btn.Visible = true;
                L_btn.Visible=true;

            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (ClassificationPage.EID.Count == 0)
            {
                MessageBox.Show("Must Add Explicit IDs! (Go back to classification page)");
            }
            else
            {
                Loadform(new de_identification1());
            }
        }

        private void L_btn_Click(object sender, EventArgs e)
        {
            if ((ClassificationPage.SA == null) || (ClassificationPage.SA.Count == 0))
            {
                MessageBox.Show("Must Add Sensitive Attribute! (go back to Classification page)");
            }
            else
            {
                Loadform(new L_diversity());
            }
            
        }

        private void DataArmor_Click(object sender, EventArgs e)
        {
            Loadform(new DataArmor());
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Set the fixed size for each cell
            //dataGridView1.Rows[e.RowIndex].Height = 50; // Set the height (in pixels)
            dataGridView1.Columns[e.ColumnIndex].Width = 170; // Set the width (in pixels)
        }

        private void SetColumnWidthsBasedOnContent()
        {
            // Iterate through each column to calculate the maximum width of content in that column
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                int maxWidth = 0;

                // Iterate through each cell in the column to find the maximum width
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    DataGridViewCell cell = row.Cells[column.Index];
                    int cellWidth = TextRenderer.MeasureText(cell.Value?.ToString() ?? "", dataGridView1.Font).Width;
                    maxWidth = Math.Max(maxWidth, cellWidth);
                }

                // Set the width of the column to the maximum width found, with some padding
                column.Width = maxWidth + 20; // Add some padding
            }

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void DataArmor_Click_1(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DataArmor
{
    public partial class de_identification1 : Form
    {
        private string[] columnNames;
        public List<string> EID = new List<string>();
        static public DataTable data = new DataTable();
        public de_identification1()
        {

            InitializeComponent();
            columnNames = UploadPage.columnNames;
            EID = ClassificationPage.EID;
            De_identify m = new De_identify(UploadPage.data, EID);
            data = m.data;
            dataGridView1.DataSource = data;
            dataGridView1.Visible = true;   
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

        private void de_identification1_Load(object sender, EventArgs e)
        {

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
                K_AnonymityPage.to_csv(data, s);
            }
        }

        private void K_btn_Click(object sender, EventArgs e)
        {
            if ((ClassificationPage.num_QID == null && ClassificationPage.cat_QID == null) || (ClassificationPage.num_QID.Count == 0 && ClassificationPage.cat_QID.Count == 0))
            {
                MessageBox.Show("Must Add Quasi-Identifiers! (go back to Classification page)");
            }
            else
            {
                loadform(new K_AnonymityPage());
            }
        }
        public void loadform(object Form)
        {
            if (this.panel1.Controls.Count > 0)
                this.panel1.Controls.RemoveAt(0);
            Form f = Form as Form;
            f.FormBorderStyle = FormBorderStyle.None;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            this.panel1.Controls.Clear();
            this.panel1.Controls.Add(f);
            this.panel1.Tag = f;
            f.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            loadform(new UploadPage());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            loadform(new ClassificationPage());
        }

        private void DataArmor_Click(object sender, EventArgs e)
        {
            loadform(new DataArmor());
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
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

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click_1(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click_2(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}

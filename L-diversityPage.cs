using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DataArmor
{
    public partial class L_diversity : Form
    {

        DataTable data = new DataTable();
        public List<string> num_QID = new List<string>();
        public List<string> cat_QID = new List<string>();
        public List<string> SA = new List<string>();
        public List<string> QID = new List<string>();
        decimal L;
        static public DataTable M;
        public L_diversity() { 
            InitializeComponent();
            num_QID = ClassificationPage.num_QID;
            cat_QID = ClassificationPage.cat_QID;
            SA = ClassificationPage.SA;
            data = K_AnonymityPage.AnonyTable;
            this.QID.AddRange(num_QID);
            this.QID.AddRange(cat_QID);


        }

        private void L_diversity_Load(object sender, EventArgs e)
        {

        }

        private void save_btn_Click4(object sender, EventArgs e)
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
        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Set the fixed size for each cell
            dataGridView10.Columns[e.ColumnIndex].Width = 170; // Set the width (in pixels)
        }

        private void ok2_btn_Click(object sender, EventArgs e)
        {
            if (K_input.Value < 2)
            {
                MessageBox.Show("Please Enter Valid L value");
            }

            else
            {
                L = K_input.Value;
                L_diversity_code m = new L_diversity_code(data, QID, L, SA);
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
                M = m.data;
                dataGridView10.DataSource = m.data;
                save_btn.Visible = true;
                to_t_clo.Visible = true;

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DataArmor_Click(object sender, EventArgs e)
        {
            loadform(new DataArmor());
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (ClassificationPage.EID.Count == 0)
            {
                MessageBox.Show("Must Add Explicit IDs! (Go back to classification page)");
            }
            else
            {
                loadform(new de_identification1());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadform(new K_AnonymityPage());
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void to_t_clo_Click(object sender, EventArgs e)
        {

            loadform(new t_closeness(M,QID,SA));
        }

        private void button_tclo_Click(object sender, EventArgs e)
        {
            loadform(new t_closeness(M, QID, SA));
        }

        private void dataGridView10_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void DataArmor_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }
    }
}

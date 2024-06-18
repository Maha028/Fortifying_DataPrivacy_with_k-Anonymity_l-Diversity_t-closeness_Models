using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
namespace DataArmor
{
    public partial class ClassificationPage : Form {
        private string[] columnNames;
        private string csvPath;
        static public List<string> EID=new List<string>();
        static public List<string> num_QID = new List<string>();
        static public List<string> cat_QID = new List<string>();
        static public List<string> QID = new List<string>();
        static public List<string> SA = new List<string>();

        public ClassificationPage()
        {
            InitializeComponent();
            columnNames = UploadPage.columnNames;
            csvPath = UploadPage.csvPath;
            checkedListBox1.Items.AddRange(columnNames.ToArray());
            checkedListBox2.Items.AddRange(columnNames);
            checkedListBox3.Items.AddRange(columnNames);
            checkedListBox4.Items.AddRange(columnNames);
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
        private void Manually_CheckedChanged(object sender, EventArgs e)
        {
            if (Manually.Checked)
            {
                man_box.Visible=true;
                Template_pnl.Visible=false;
            }
        }
        private void correlation_CheckedChanged(object sender, EventArgs e)
        {
            correlationClassification classificate = new correlationClassification(csvPath);
            EID.Clear();
            num_QID.Clear();
            cat_QID.Clear();
            SA.Clear();
            EID = classificate.EID;
            QID = classificate.QID;
            SA = classificate.SA;
            foreach(var i in QID)
            {
                if (classificate.num_col.Contains(i))
                {
                    num_QID.Add(i.ToString());
                    
                }
                if (classificate.cat_col.Contains(i))
                {
                   
                    cat_QID.Add(i.ToString());
                }
            }
            if (CorrelationBased.Checked)
            {
           man_box.Visible = false;
                Template_pnl.Visible = true;
                string ei = " ";
                foreach (var i in EID)
                {
                    ei += " , ";
                    ei += i.ToString();
                }
                if (ei==" ")
                {
                    ei = "  No Exciplict ID";
                }
                label13.Text = ei;
                string cq = " ";
                foreach (var i in QID)
                {
                                cq += "  ";
                                cq += i.ToString();
              
                }
                label16.Text = cq;
                string sa = " ";
                foreach (var i in SA)
                {
                    if (!QID.Contains(i))
                    {
                        if (!cq.Split().Contains(i))
                            sa += "   ";
                        sa += i.ToString();
                    }

                }
                label15.Text = sa;
            }

        }
        private void template_CheckedChanged(object sender, EventArgs e)
        {
            WordList Wordlist_check = new WordList();
            EID.Clear();
            num_QID.Clear();
            QID.Clear();
            SA.Clear();
            foreach(var i in columnNames)
            {
               
                if (Wordlist_check.EID.Any(s => s.Equals(i, StringComparison.OrdinalIgnoreCase)))
                {
                    EID.Add(i);
                }
                else if (Wordlist_check.QID.Any(s => s.Equals(i, StringComparison.OrdinalIgnoreCase)))
                {
                    QID.Add(i);

                }
                
                else if (Wordlist_check.SA.Any(s => s.Equals(i, StringComparison.OrdinalIgnoreCase)))
                {
                    SA.Add(i);
                }
            }
            if (template.Checked)
            {
                man_box.Visible = false;
                Template_pnl.Visible = true;
                string ei = " ";
                foreach (var i in EID)
                {
                    ei += " , ";
                    ei += i.ToString();
                }
                if (ei == " ")
                {
                    ei = "No Exciplict ID";
                }
                label13.Text = ei;
                string cq = " ";
                foreach (var i in QID)
                {
                    cq += "  ";
                    cq += i.ToString();
                }
                label16.Text = cq;
                string sa = " ";
                foreach (var i in SA)
                {
                    if (!QID.Contains(i))
                    {if (!cq.Split().Contains(i))
                        sa += "   ";
                        sa += i.ToString();
                    }
                }
                label15.Text = sa;

            }

        }
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedListBox2.Items.Clear();

            List<string> R_col = new List<string>();

            {
                foreach (var item in checkedListBox1.Items)
                {
                    if (!checkedListBox1.CheckedItems.Contains(item))
                    {
                        R_col.Add(item.ToString());
                    }
                }
                checkedListBox2.Items.AddRange(R_col.ToArray());
            }
        }
        private void ok_btn_Click(object sender, EventArgs e)
        {
            button6.Visible = false;
            Next_c.Visible = false;
            if (Manually.Checked)
            {
                EID.Clear();
                num_QID.Clear();
                cat_QID.Clear();
                SA.Clear();

                foreach (var item in checkedListBox1.Items)
                {
                    if (checkedListBox1.CheckedItems.Contains(item))
                    {
                        EID.Add(item.ToString());
                    }
                }
                foreach (var item in checkedListBox2.Items)
                {
                    if (checkedListBox2.CheckedItems.Contains(item))
                    {
                        num_QID.Add(item.ToString());
                    }
                }
                foreach (var item in checkedListBox3.Items)
                {
                    if (checkedListBox3.CheckedItems.Contains(item))
                    {
                        cat_QID.Add(item.ToString());
                    }
                }

                foreach (var item in checkedListBox4.Items)
                {
                    if (checkedListBox4.CheckedItems.Contains(item))
                    {
                        SA.Add(item.ToString());
                    }
                }

                if (EID.Count != 0)
                {
                    button6.Visible = true;
                }
                if (num_QID.Count != 0 || cat_QID.Count != 0)
                {
                    Next_c.Visible = true;
                }
            }
        }
      
        private void Classification_Load(object sender, EventArgs e)
        {

        }
        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedListBox3.Items.Clear();

            List<string> R_col = new List<string>();

            {
                foreach (var item in checkedListBox1.Items)
                {
                    if (!checkedListBox2.CheckedItems.Contains(item))
                    {
                        if (!checkedListBox1.CheckedItems.Contains(item))
                        {
                            R_col.Add(item.ToString());
                        }
                    }
                }
                checkedListBox3.Items.AddRange(R_col.ToArray());
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            loadform(new UploadPage());
        }

        private void checkedListBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedListBox4.Items.Clear();
            List<string> R_col = new List<string>();
            {
                foreach (var item in checkedListBox1.Items)
                {
                    if (!checkedListBox3.CheckedItems.Contains(item))
                    {
                        if (!checkedListBox2.CheckedItems.Contains(item))
                        {
                            if (!checkedListBox1.CheckedItems.Contains(item))
                            {
                                R_col.Add(item.ToString());

                            }
                        }
                    }
                }
                checkedListBox4.Items.AddRange(R_col.ToArray());
            }
        }

        private void DataArmor_Click(object sender, EventArgs e)
        {
            loadform(new DataArmor());
        }



        private void Next_c_Click(object sender, EventArgs e)
        {
            loadform(new K_AnonymityPage());
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (num_QID.Count==0&&cat_QID.Count==0)
            {
                MessageBox.Show("Must Complete This Step!");
            }
            else
            {
                loadform(new K_AnonymityPage());
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            loadform(new de_identification1());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (EID.Count == 0) {
                MessageBox.Show("Must Complete this Step!");
            }
            else
            {
                loadform(new de_identification1());
            }
        }

    }
}

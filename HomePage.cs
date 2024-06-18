using System;

using System.Linq;

using System.Windows.Forms;

namespace DataArmor
{
    public partial class DataArmor : Form
    {

        System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();

        public DataArmor()
        {
            timer1.Interval = 8200;
            timer1.Tick += new System.EventHandler(timer1_Tick);
            timer1.Start();
            InitializeComponent();
        }
        public void loadform(object Form)
        {
            if (this.mainpanel.Controls.Count > 0)
                this.mainpanel.Controls.RemoveAt(0);
            Form f = Form as Form;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            this.mainpanel.Controls.Clear();
            this.mainpanel.Controls.Add(f);
            this.mainpanel.Tag = f;
            f.Show();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Start();

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //do whatever you want
            loadform(new UploadPage());
            timer1.Stop();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pre_unilab
{
    public partial class ステージ選択画面 : Form
    {
        Form1 f1 = new Form1();

        public ステージ選択画面()
        {
            InitializeComponent();
        }

        private void ステージ選択画面_load(object sender, EventArgs e)
        {
            //Form1 f1 = new Form1();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = comboBox1.SelectedItem.ToString();
            f1.stageName = str;
            f1.Show();
            this.Close();
        }
    }
}

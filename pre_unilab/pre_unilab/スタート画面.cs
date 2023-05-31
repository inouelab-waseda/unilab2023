using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace pre_unilab
{
    public partial class スタート画面 : Form
    {
        public スタート画面()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ステージ選択画面 form = new ステージ選択画面();
            form.Show();
            //this.Close();
        }
    }
}

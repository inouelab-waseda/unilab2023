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

        private void スタート画面_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
        }

        enum chara : int { shizu, ikaP };

        public class Conversion
        {
            public int name = 0;
            public string dialogue = "";
        }

        Conversion making_dia(int i)
        {
            Conversion[] conv = new Conversion[20];

            int S = (int)chara.shizu;
            int I = (int)chara.ikaP;

            for (int j = 0; j < 20; j++)
            {
                conv[j] = new Conversion();
            }

            conv[0].name = I;
            conv[0].dialogue = "メッセージウィンドウ!\n" +
                "ステージをクリアするごとに物語を展開することで\n" +
                "ゲームにストーリ性を持たせることができるっピ！";

            conv[1].name = S;
            conv[1].dialogue = "ストーリーは誰が書くの？\n" +
                "こんな寒いパロディネタでいいの？\n" +
                "諸原はこのレベルの脚本しか作れないよ?";

            conv[2].name = I;
            conv[2].dialogue = "しずちゃ（イカピーを踏む音）\n";

            conv[3].name = S;
            conv[3].dialogue = "ウィンドのデザインは誰がやるの？\n" +
                "こんなクソダサデザインで許してくれるの？\n";

            conv[4].name = I;
            conv[4].dialogue = "しずちゃ\n" +
                "いた（さらに強くイカピーを踏む音）\n";

            conv[5].name = S;
            conv[5].dialogue = "会話システムをゲームに実装するのは誰がやるの？\n" +
                "言い出しっぺの諸原がやればいいの？\n";

            conv[6].name = S;
            conv[6].dialogue = "いったいどうすればいいって\n" +
                "お前に言ってんだよ!!\n";

            conv[7].name = I;
            conv[7].dialogue = "……\n";

            conv[8].name = I;
            conv[8].dialogue = "……\n";

            conv[9].name = I;
            conv[9].dialogue = "わかんないっピ…\n";

            conv[10].name = -1;
            conv[10].dialogue = "デモ終了\n";

            return conv[i];
        }

        int num = 0;
        int max_num = 10;

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Image img_shizu = Image.FromFile("氷.png");
            Image img_ikaP = Image.FromFile("草原.jpg");

            Bitmap bmp1 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g1 = Graphics.FromImage(bmp1);

            Pen pen = new Pen(Color.FromArgb(100, 255, 100), 2);

            int face = 100;
            int name_x = 150;
            int name_y = 40;

            int dia_x = 600;
            int dia_y = 150;

            g1.FillRectangle(Brushes.Black, 0, face, name_x, name_y);
            g1.DrawRectangle(pen, 0, face, name_x, name_y);

            g1.FillRectangle(Brushes.Black, 0, face + name_y, dia_x, dia_y);
            g1.DrawRectangle(pen, 0, face + name_y, dia_x, dia_y);

            Font fnt = new Font("MS UI Gothic", 20);
            int sp = 5;

            Conversion conv = making_dia(num);

            string talker = "";
            if (conv.name == (int)chara.shizu)
            {
                talker = "しずちゃん";
                g1.DrawImage(img_shizu, 0, 0, face, face);
            }
            if (conv.name == (int)chara.ikaP)
            {
                talker = "イカピー";
                g1.DrawImage(img_ikaP, 0, 0, face, face);
            }
            g1.DrawString(talker, fnt, Brushes.White, 0 + sp, face + sp);
            g1.DrawString(conv.dialogue, fnt, Brushes.White, 0 + sp, face + name_y + sp);

            pictureBox1.Image = bmp1;
            g1.Dispose();

            if (num < max_num) num = num + 1;
            else button1.Enabled = true;
        }
    }
}

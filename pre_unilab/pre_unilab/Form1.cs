using System;
using System.Linq;
using System.Windows.Forms;


namespace pre_unilab
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) //tes
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.SelectionMode = SelectionMode.One;

            //ListBox1のイベントハンドラを追加
            listBox2.MouseDown += new MouseEventHandler(ListBox2_MouseDown);
            //ListBox2のイベントハンドラを追加
            listBox1.DragEnter += new DragEventHandler(ListBox1_DragEnter);
            listBox1.DragDrop += new DragEventHandler(ListBox1_DragDrop);

        }

        //ListBox1でマウスボタンが押された時
        private void ListBox2_MouseDown(object sender, MouseEventArgs e)
        {
            //マウスの左ボタンだけが押されている時のみドラッグできるようにする
            if (e.Button == MouseButtons.Left)
            {
                //ドラッグの準備
                ListBox lbx = (ListBox)sender;
                //ドラッグするアイテムのインデックスを取得する
                int itemIndex = lbx.IndexFromPoint(e.X, e.Y);
                if (itemIndex < 0) return;
                //ドラッグするアイテムの内容を取得する
                string itemText = (string)lbx.Items[itemIndex];

                //ドラッグ&ドロップ処理を開始する
                DragDropEffects dde =
                    lbx.DoDragDrop(itemText, DragDropEffects.All);

                ////ドロップ効果がMoveの時はもとのアイテムを削除する
                //if (dde == DragDropEffects.Move)
                //    lbx.Items.RemoveAt(itemIndex);
            }
        }

        //ListBox2内にドラッグされた時
        private void ListBox1_DragEnter(object sender, DragEventArgs e)
        {
            //ドラッグされているデータがstring型か調べ、
            //そうであればドロップ効果をMoveにする
            if (e.Data.GetDataPresent(typeof(string)))
                e.Effect = DragDropEffects.Move;
            else
                //string型でなければ受け入れない
                e.Effect = DragDropEffects.None;
        }

        //ListBox2にドロップされたとき
        private void ListBox1_DragDrop(object sender, DragEventArgs e)
        {
            //ドロップされたデータがstring型か調べる
            if (e.Data.GetDataPresent(typeof(string)))
            {
                ListBox target = (ListBox)sender;
                //ドロップされたデータ(string型)を取得
                string itemText =
                    (string)e.Data.GetData(typeof(string));
                //ドロップされたデータをリストボックスに追加する
                target.Items.Add(itemText);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string[] get_move = this.listBox1.Items.Cast<string>().ToArray();
            var move = new List<int[]>();
            int[,] ints = new int[2, 4] { { 0, 0, 1, -1 }, { -1, 1, 0, 0 } };
            for (int i = 0; i < get_move.Length; i++)
            {
                if (get_move[i] == "up")
                {
                    move.Add(new int[2] { ints[0, 0], ints[1, 0] });
                }
                else if (get_move[i] == "down")
                {
                    move.Add(new int[2] { ints[0, 1], ints[1, 1] });
                }
                else if (get_move[i] == "right")
                {
                    move.Add(new int[2] { ints[0, 2], ints[1, 2] });
                }
                else if (get_move[i] == "left")
                {
                    move.Add(new int[2] { ints[0, 3], ints[1, 3] });
                }
            }
        }

        public bool Colision_detection(int x, int y, int[,] Map, List<int[]> move)
        {
            int max_x = Map.GetLength(0);
            int max_y = Map.GetLength(1);

            int new_x = x + move[0][0];
            int new_y = y + move[0][1];

            if ((new_x + 1) <= 0 || (max_x - new_x) <= 0 || (new_y + 1) <= 0 || (max_y - new_y) <= 0) return false;
            else if (Map[new_x, new_y] == 0) return false;
            else
            {
                //move.RemoveAt(0);
                return true;
            }
        }

        public void SquareMovement(int x, int y, int[,] Map, List<int[]> move)
        {
            while (Colision_detection(x, y, Map, move))
            {
                x += move[0][0];
                y += move[0][1];
                move.RemoveAt(0);
            }
        }
    }

}

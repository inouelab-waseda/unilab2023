using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Text.RegularExpressions;

namespace pre_unilab
{
    public partial class Form1 : Form
    {
        Bitmap bmp1,bmp2;

        private string _stageName;
        public string stageName
        {
            get { return _stageName; }
            set { _stageName = value; }
        }
        

        public Form1()
        {
            InitializeComponent();

            
            //pictureBox2の設定
            pictureBox2.Parent = pictureBox1;
            pictureBox1.Location = new Point(600, 50);
            pictureBox2.Location = new Point(0,0);
            pictureBox1.ClientSize = new Size(350, 350);
            pictureBox2.ClientSize = new Size(350, 350);

            pictureBox2.BackColor = Color.Transparent;


            bmp1 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bmp2 = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            Graphics g = Graphics.FromImage(bmp1);
            Graphics g2 = Graphics.FromImage(bmp2);
            pictureBox1.Image = bmp1;
            pictureBox2.Image = bmp2;

        }

        public class Global //グローバル変数格納
        {
            public static int[,] map = new int[10, 10]; //map情報
            public static int x_start; //スタート位置ｘ
            public static int y_start; //スタート位置ｙ
            public static int x_goal; //ゴール位置ｘ
            public static int y_goal; //ゴール位置ｙ
            public static int x_now; //現在位置ｘ
            public static int y_now; //現在位置 y

            public static int count=0; //試行回数カウント
            public static int miss_count = 0; //ミスカウント

            public static List<int[]> move;
        }
        
        
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Global.map = CreateStage("Map52203"); //ステージ作成
            Global.map = CreateStage(stageName); //ステージ作成


            //ListBox1のイベントハンドラを追加
            listBox1.SelectionMode = SelectionMode.One;
            listBox2.MouseDown += new MouseEventHandler(ListBox_MouseDown);
            //ListBox2のイベントハンドラを追加
            listBox1.DragEnter += new DragEventHandler(ListBox_DragEnter);
            listBox1.DragDrop += new DragEventHandler(ListBox_DragDrop);

            listBox3.SelectionMode = SelectionMode.One;
            //ListBox2のイベントハンドラを追加
            listBox3.DragEnter += new DragEventHandler(ListBox_DragEnter);
            listBox3.DragDrop += new DragEventHandler(ListBox_DragDrop);

            //ListBox4のイベントハンドラを追加
            listBox5.MouseDown += new MouseEventHandler(ListBox_MouseDown);
            listBox4.SelectionMode = SelectionMode.One;
            listBox4.DragEnter += new DragEventHandler(ListBox_DragEnter);
            listBox4.DragDrop += new DragEventHandler(ListBox_DragDrop);

        }


        /****button****/
        private void button1_Click(object sender, EventArgs e)
        {
            Global.move = Movement(); //ユーザーの入力を読み取る
            label6.Visible = false;
            SquareMovement(Global.x_now, Global.y_now, Global.map, Global.move); //キャラ動かす
            label3.Text = Global.count.ToString(); //試行回数の表示

            if (Global.x_goal == Global.x_now && Global.y_goal == Global.y_now)
            {
                label6.Text = "クリア！！";
                label6.Visible = true;

                //button1.Visible = false;
                //button1.Enabled = false;
                //button4.Enabled = true;
                //button5.Enabled = true;
                //button4.Visible = true;
                //button5.Visible = true;


            }
        }

        private void button2_Click(object sender, EventArgs e) //リストボックス内の動き削除
        {
            listBox1.Items.Clear();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            listBox4.Items.Clear();
        }


        //A, Bボタン削除
        //private void button4_Click(object sender, EventArgs e)
        //{
        //    label6.Visible = false;
        //    SquareMovement(Global.x_now, Global.y_now, Global.map, Global.move.Item1); //キャラ動かす
        //    label3.Text = Global.count.ToString(); //試行回数の表示

        //    if(Global.x_goal == Global.x_now && Global.y_goal == Global.y_now)
        //    {
        //        label6.Text = "成功！！";
        //        label6.Visible = true;
        //        button4.Visible = false;
        //        button4.Enabled = false;
        //        button5.Visible = false;
        //        button5.Enabled = false;
        //    }
        //}

        //private void button5_Click(object sender, EventArgs e)
        //{
        //    label6.Visible = false;
        //    var move = Global.move.Item2;
        //    SquareMovement(Global.x_now, Global.y_now, Global.map, move); //キャラ動かす
        //    label3.Text = Global.count.ToString(); //試行回数の表示

        //    if (Global.x_goal == Global.x_now && Global.y_goal == Global.y_now)
        //    {
        //        label6.Text = "クリア！！";
        //        label6.Visible = true;
        //        button4.Visible = false;
        //        button4.Enabled = false;
        //        button5.Visible = false;
        //        button5.Enabled = false;
        //    }
        //}
        private void button6_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        /******button fin******/




        /*******関数******/

        //ListBox1でマウスボタンが押された時
        private void ListBox_MouseDown(object sender, MouseEventArgs e)
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
        private void ListBox_DragEnter(object sender, DragEventArgs e)
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
        private void ListBox_DragDrop(object sender, DragEventArgs e)
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

        void Image_FrameChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        /*
        protected override void OnPaint(PaintEventArgs e)
        {
            //フレームを進める
            ImageAnimator.UpdateFrames(animatedImage);
            //画像の表示
            e.Graphics.DrawImage(animatedImage, 0, 0);

            base.OnPaint(e);
        }
        */
        //ステージ描写
        private int[,] CreateStage(string stage_name) 
        {
            int[,] map = new int[10, 10];

            using (StreamReader sr = new StreamReader($"{stage_name}.csv"))
            {
                int x;
                int y = 0;

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] values = line.Split(',');

                    x = 0;

                    foreach (var value in values)
                    {
                        // enum 使ったほうが分かりやすそう
                        map[y, x] = int.Parse(value);
                        x++;
                    }
                    y++;
                }
            }

            Graphics g1 = Graphics.FromImage(bmp1);
            Graphics g2 = Graphics.FromImage(bmp2);
            Brush Y = new SolidBrush(Color.Yellow);
            Brush B = new SolidBrush(Color.Blue);

            Image img_green = Image.FromFile("草原.jpg");
            Image img_white = Image.FromFile("岩場.jpg");
            Image character_me = Image.FromFile("たぬき.png");
            Image character_enemy = Image.FromFile("ふくろう.png");
            Image img_ice = Image.FromFile("氷.png");
            Image img_jump = Image.FromFile("跳.png");
            Image animatedImage_up = Image.FromFile("動く床_上.gif");
            Image animatedImage_right = Image.FromFile("動く床_右.gif");
            Image animatedImage_down = Image.FromFile("動く床_下.gif");
            Image animatedImage_left = Image.FromFile("動く床_左.gif");

            //MemoryStream stream = new MemoryStream();
            //byte[] bytes = File.ReadAllBytes("右.gif");
            //stream.Write(bytes, 0, bytes.Length);
            //Bitmap animatedImage_right = new Bitmap(stream);

            //アニメ開始
            //ImageAnimator.Animate(animatedImage_right, Image_FrameChanged);
            //DoubleBuffered = true;

            int cell_length = pictureBox1.Width / 10;

            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    switch (map[y, x])
                    {
                        case 0:
                            g1.DrawImage(img_white, x * cell_length, y * cell_length, cell_length, cell_length);
                            break;
                        case 1:
                            g1.DrawImage(img_green, x * cell_length, y * cell_length, cell_length, cell_length);
                            break;
                        case 2:
                            g1.DrawImage(img_ice, x * cell_length, y * cell_length, cell_length, cell_length);
                            break;
                        case 3:
                            g1.DrawImage(img_jump, x * cell_length, y * cell_length, cell_length, cell_length);
                            break;
                        case 4:
                            ImageAnimator.UpdateFrames(animatedImage_up);
                            g1.DrawImage(animatedImage_up, x * cell_length, y * cell_length, cell_length, cell_length);
                            break;
                        case 5:
                            ImageAnimator.UpdateFrames(animatedImage_right);
                            g1.DrawImage(animatedImage_right, x * cell_length, y * cell_length, cell_length, cell_length);
                            break;
                        case 6:
                            ImageAnimator.UpdateFrames(animatedImage_down);
                            g1.DrawImage(animatedImage_down, x * cell_length, y * cell_length, cell_length, cell_length);
                            break;
                        case 7:
                            ImageAnimator.UpdateFrames(animatedImage_left);
                            g1.DrawImage(animatedImage_left, x * cell_length, y * cell_length, cell_length, cell_length);
                            break;
                        //case 8:
                        //    g1.DrawImage(img_tree, x * cell_length, y * cell_length, cell_length, cell_length);
                        //    break;
                        case 100:
                            g1.FillRectangle(B, x * cell_length, y * cell_length, cell_length, cell_length);
                            Global.x_start = x;
                            Global.y_start = y;
                            Global.x_now = x;
                            Global.y_now = y;
                            g2.DrawImage(character_me, x * cell_length, y * cell_length, cell_length, cell_length);
                            break;
                        case 101:
                            g1.FillRectangle(Y, x * cell_length, y * cell_length, cell_length, cell_length);
                            g2.DrawImage(character_enemy, x * cell_length, y * cell_length, cell_length, cell_length);
                            Global.x_goal = x;
                            Global.y_goal = y;
                            break;
                    }
                }
            }

            return map;
        }

        //ユーザーの入力を変換
        public List<int[]> Movement()
        {
            var move_a = new List<int[]>();
            var move_b = new List<int[]>();
            string[] get_move_a = this.listBox1.Items.Cast<string>().ToArray();
            string[] get_move_b = this.listBox3.Items.Cast<string>().ToArray();
            var get_move_a_list = new List<string>();
            var get_move_b_list = new List<string>();

            get_move_b_list.AddRange(get_move_b);


            for (int i = 0; i < get_move_a.Length; i++)
            {
                if (get_move_a[i] == "B")
                {
                    get_move_a_list.AddRange(get_move_b_list);

                }
                else
                {
                    get_move_a_list.Add(get_move_a[i]);
                }
            }

            

            if (get_move_a.Length != 0)
            {
                //string[] get_move_a = this.listBox1.Items.Cast<string>().ToArray();
                for (int i = 0; i < get_move_a_list.Count; i++)
                {
                    if (get_move_a_list[i].StartsWith("for"))
                    {
                        int start = i + 1;
                        int trial = int.Parse(Regex.Replace(get_move_a_list[i], @"[^0-9]", ""));
                        int goal = 0; //後で設定

                        for(int j=0; j<trial; j++)
                        {
                            int k = start;
                            do
                            {
                                if (get_move_a_list[k] == "endfor")
                                {
                                    goal = k;
                                    break;
                                }

                                else if (get_move_a_list[k] == "up")
                                {
                                    move_a.Add(new int[2] { 0, -1 });
                                }
                                else if (get_move_a_list[k] == "down")
                                {
                                    move_a.Add(new int[2] { 0, 1 });
                                }
                                else if (get_move_a_list[k] == "right")
                                {
                                    move_a.Add(new int[2] { 1, 0 });
                                }
                                else if (get_move_a_list[k] == "left")
                                {
                                    move_a.Add(new int[2] { -1, 0 });
                                }

                                k++;
                            } while (true);
                        }
                        i = goal;
                    }
                    else
                    {
                        if (get_move_a_list[i] == "up")
                        {
                            move_a.Add(new int[2] { 0, -1 });
                        }
                        else if (get_move_a_list[i] == "down")
                        {
                            move_a.Add(new int[2] { 0, 1 });
                        }
                        else if (get_move_a_list[i] == "right")
                        {
                            move_a.Add(new int[2] { 1, 0 });
                        }
                        else if (get_move_a_list[i] == "left")
                        {
                            move_a.Add(new int[2] { -1, 0 });
                        }
                    }
                }
            }

            if(get_move_b.Length != 0)
            {
                //string[] get_move_b = this.listBox3.Items.Cast<string>().ToArray();


                for (int i = 0; i < get_move_b_list.Count; i++)
                {
                    if (get_move_b_list[i].StartsWith("for"))
                    {
                        int start = i + 1;
                        int trial = int.Parse(Regex.Replace(get_move_b_list[i], @"[^0-9]", ""));

                        int goal = 0; //後で設定

                        for (int j = 0; j < trial; j++)
                        {
                            int k = start;
                            do
                            {
                                if (get_move_b_list[k] == "endfor")
                                {
                                    goal = k;
                                    break;
                                }

                                else if (get_move_b_list[k] == "up")
                                {
                                    move_b.Add(new int[2] { 0, -1 });
                                }
                                else if (get_move_b_list[k] == "down")
                                {
                                    move_b.Add(new int[2] { 0, 1 });
                                }
                                else if (get_move_b_list[k] == "right")
                                {
                                    move_b.Add(new int[2] { 1, 0 });
                                }
                                else if (get_move_b_list[k] == "left")
                                {
                                    move_b.Add(new int[2] { -1, 0 });
                                }

                                k++;
                            } while (true);
                        }
                        i = goal;
                    }
                    else
                    {
                        if (get_move_b_list[i] == "up")
                        {
                            move_b.Add(new int[2] { 0, -1 });
                        }
                        else if (get_move_b_list[i] == "down")
                        {
                            move_b.Add(new int[2] { 0, 1 });
                        }
                        else if (get_move_b_list[i] == "right")
                        {
                            move_b.Add(new int[2] { 1, 0 });
                        }
                        else if (get_move_b_list[i] == "left")
                        {
                            move_b.Add(new int[2] { -1, 0 });
                        }
                    }
                }

            }
            string[] get_move_main = this.listBox4.Items.Cast<string>().ToArray();
            var move = new List<int[]>();
            for(int i = 0; i < get_move_main.Length; i++)
            {
                if (get_move_main[i] == "A")
                {
                    move.AddRange(move_a);
                }else if(get_move_main[i] == "B")
                {
                    move.AddRange(move_b);
                }
            }
            

            return move;
        }

        //当たり判定
        public bool Colision_detection(int x, int y, int[,] Map, List<int[]> move)
        {
            int max_x = Map.GetLength(0);
            int max_y = Map.GetLength(1);

            int new_x = x + move[0][0];
            int new_y = y + move[0][1];

            if ((new_x + 1) <= 0 || (max_x - new_x) <= 0 || (new_y + 1) <= 0 || (max_y - new_y) <= 0) return false;
            //else if (Map[new_x, new_y] == 0) return false;
            else if(Map[new_y, new_x] == 0) return false;
            else
            {
                //move.RemoveAt(0);
                return true;
            }
        }

        //キャラの座標更新
        public void SquareMovement(int x, int y, int[,] Map, List<int[]> move)
        {
            Graphics g2 = Graphics.FromImage(bmp2);
            int cell_length = pictureBox1.Width / 10;
            if (move.Count == 0)
            {
                return;
            }

            List<int[]> move_copy = new List<int[]>();
            for(int i = 0; i < move.Count; i++)
            {
                move_copy.Add(move[i]);
            }

            bool jump = false;
            bool move_floor = false;
            int waittime = 250; //ミリ秒

            while (true)
            {
                if (!Colision_detection(x, y, Map, move_copy) && !jump)
                {
                    label6.Visible = true;
                    Thread.Sleep(300);
                    //label6.Visible = false;
                    Global.miss_count += 1;
                    label5.Text = Global.miss_count.ToString();
                    break;
                }

                //移動先が木の場合、木の方向には進めない
                if (Map[y + move_copy[0][1], x + move_copy[0][0]] == 8)
                {
                    move_copy.Clear();
                    break;
                    //500ミリ秒=0.5秒待機する
                    Thread.Sleep(waittime);
                    //continue;
                }

                x += move_copy[0][0];
                y += move_copy[0][1];

                Global.x_now = x;
                Global.y_now = y;

                g2.Clear(Color.Transparent);
                Image character_me = Image.FromFile("たぬき.png");
                Image character_enemy = Image.FromFile("ふくろう.png");
                g2.DrawImage(character_me, x * cell_length, y * cell_length, cell_length, cell_length);
                g2.DrawImage(character_enemy, Global.x_goal * cell_length, Global.y_goal * cell_length, cell_length, cell_length);


                //pictureBoxの中身を塗り替える
                InterThreadRefresh(this.pictureBox2.Refresh);

                if (Map[y,x] == 101)
                {
                    break;
                }
                //移動先が氷の上なら同じ方向にもう一回進む
                if (Map[y, x] == 2)
                {
                    //500ミリ秒=0.5秒待機する
                    Thread.Sleep(waittime);
                    continue;
                }

                //移動先がジャンプ台なら同じ方向に二回進む（１個先の障害物は無視）
                if (Map[y, x] == 3 || jump)
                {
                    if (move_floor)
                    {
                        move_floor = false;
                        move_copy.RemoveAt(0);
                    }

                    if (jump) //次の移動で着地
                    {
                        jump = false;
                    }
                    else //ジャンプ台の上（次の移動でジャンプ）
                    {
                        jump = true;
                    }

                    //500ミリ秒=0.5秒待機する
                    Thread.Sleep(waittime);
                    continue;
                }

                //上に移動するマスを踏んだ場合1つ上に進む
                if (Map[y, x] == 4)
                {
                    move_copy[0] = new int[2] { 0, -1 };
                    Thread.Sleep(waittime);
                    continue;
                }

                //右に移動するマスを踏んだ場合1つ右に進む
                if (Map[y, x] == 5)
                {
                    move_copy[0] = new int[2] { 1, 0 };
                    Thread.Sleep(waittime);
                    continue;
                }

                //下に移動するマスを踏んだ場合1つ下に進む
                if (Map[y, x] == 6)
                {
                    move_copy[0] = new int[2] { 0, 1 };
                    Thread.Sleep(waittime);
                    continue;
                }

                //左に移動するマスを踏んだ場合1つ左に進む
                if (Map[y, x] == 7)
                {
                    move_copy[0] = new int[2] { -1, 0 };
                    Thread.Sleep(waittime);
                    continue;
                }

                

                
                move_copy.RemoveAt(0);
                if (move_copy.Count == 0)
                {
                    break;
                }

                //500ミリ秒=0.5秒待機する
                Thread.Sleep(waittime);
            }
            Global.count += 1;
        }


        /*******関数 fin******/




        /******つかわない******/
        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string command = listBox1.SelectedItem.ToString();

                if (command.StartsWith("for"))
                {
                    string str_num = Regex.Replace(command, @"[^0-9]", "");
                    int num = int.Parse(str_num);

                    int id = listBox1.SelectedIndex;
                    listBox1.Items[id] = "for (" + (num % 9 + 1).ToString() + ")";

                    listBox1.Refresh();
                }
            }
        }

        private void listBox3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox3.SelectedItem != null)
            {
                string command = listBox3.SelectedItem.ToString();

                if (command.StartsWith("for"))
                {
                    string str_num = Regex.Replace(command, @"[^0-9]", "");
                    int num = int.Parse(str_num);

                    int id = listBox3.SelectedIndex;
                    listBox3.Items[id] = "for (" + (num % 9 + 1).ToString() + ")";

                    listBox3.Refresh();
                }
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        
    }

}

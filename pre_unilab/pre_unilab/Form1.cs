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

            
            //pictureBox2�̐ݒ�
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

        public class Global //�O���[�o���ϐ��i�[
        {
            public static int[,] map = new int[10, 10]; //map���
            public static int x_start; //�X�^�[�g�ʒu��
            public static int y_start; //�X�^�[�g�ʒu��
            public static int x_goal; //�S�[���ʒu��
            public static int y_goal; //�S�[���ʒu��
            public static int x_now; //���݈ʒu��
            public static int y_now; //���݈ʒu y

            public static int count=0; //���s�񐔃J�E���g
            public static int miss_count = 0; //�~�X�J�E���g

            public static List<int[]> move;
        }
        
        
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Global.map = CreateStage("Map52203"); //�X�e�[�W�쐬
            Global.map = CreateStage(stageName); //�X�e�[�W�쐬


            //ListBox1�̃C�x���g�n���h����ǉ�
            listBox1.SelectionMode = SelectionMode.One;
            listBox2.MouseDown += new MouseEventHandler(ListBox_MouseDown);
            //ListBox2�̃C�x���g�n���h����ǉ�
            listBox1.DragEnter += new DragEventHandler(ListBox_DragEnter);
            listBox1.DragDrop += new DragEventHandler(ListBox_DragDrop);

            listBox3.SelectionMode = SelectionMode.One;
            //ListBox2�̃C�x���g�n���h����ǉ�
            listBox3.DragEnter += new DragEventHandler(ListBox_DragEnter);
            listBox3.DragDrop += new DragEventHandler(ListBox_DragDrop);

            //ListBox4�̃C�x���g�n���h����ǉ�
            listBox5.MouseDown += new MouseEventHandler(ListBox_MouseDown);
            listBox4.SelectionMode = SelectionMode.One;
            listBox4.DragEnter += new DragEventHandler(ListBox_DragEnter);
            listBox4.DragDrop += new DragEventHandler(ListBox_DragDrop);

        }


        /****button****/
        private void button1_Click(object sender, EventArgs e)
        {
            Global.move = Movement(); //���[�U�[�̓��͂�ǂݎ��
            label6.Visible = false;
            SquareMovement(Global.x_now, Global.y_now, Global.map, Global.move); //�L����������
            label3.Text = Global.count.ToString(); //���s�񐔂̕\��

            if (Global.x_goal == Global.x_now && Global.y_goal == Global.y_now)
            {
                label6.Text = "�N���A�I�I";
                label6.Visible = true;

                //button1.Visible = false;
                //button1.Enabled = false;
                //button4.Enabled = true;
                //button5.Enabled = true;
                //button4.Visible = true;
                //button5.Visible = true;


            }
        }

        private void button2_Click(object sender, EventArgs e) //���X�g�{�b�N�X���̓����폜
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


        //A, B�{�^���폜
        //private void button4_Click(object sender, EventArgs e)
        //{
        //    label6.Visible = false;
        //    SquareMovement(Global.x_now, Global.y_now, Global.map, Global.move.Item1); //�L����������
        //    label3.Text = Global.count.ToString(); //���s�񐔂̕\��

        //    if(Global.x_goal == Global.x_now && Global.y_goal == Global.y_now)
        //    {
        //        label6.Text = "�����I�I";
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
        //    SquareMovement(Global.x_now, Global.y_now, Global.map, move); //�L����������
        //    label3.Text = Global.count.ToString(); //���s�񐔂̕\��

        //    if (Global.x_goal == Global.x_now && Global.y_goal == Global.y_now)
        //    {
        //        label6.Text = "�N���A�I�I";
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




        /*******�֐�******/

        //ListBox1�Ń}�E�X�{�^���������ꂽ��
        private void ListBox_MouseDown(object sender, MouseEventArgs e)
        {
            //�}�E�X�̍��{�^��������������Ă��鎞�̂݃h���b�O�ł���悤�ɂ���
            if (e.Button == MouseButtons.Left)
            {
                //�h���b�O�̏���
                ListBox lbx = (ListBox)sender;
                //�h���b�O����A�C�e���̃C���f�b�N�X���擾����
                int itemIndex = lbx.IndexFromPoint(e.X, e.Y);
                if (itemIndex < 0) return;
                //�h���b�O����A�C�e���̓��e���擾����
                string itemText = (string)lbx.Items[itemIndex];

                //�h���b�O&�h���b�v�������J�n����
                DragDropEffects dde =
                    lbx.DoDragDrop(itemText, DragDropEffects.All);

                ////�h���b�v���ʂ�Move�̎��͂��Ƃ̃A�C�e�����폜����
                //if (dde == DragDropEffects.Move)
                //    lbx.Items.RemoveAt(itemIndex);
            }
        }

        //ListBox2���Ƀh���b�O���ꂽ��
        private void ListBox_DragEnter(object sender, DragEventArgs e)
        {
            //�h���b�O����Ă���f�[�^��string�^�����ׁA
            //�����ł���΃h���b�v���ʂ�Move�ɂ���
            if (e.Data.GetDataPresent(typeof(string)))
                e.Effect = DragDropEffects.Move;
            else
                //string�^�łȂ���Ύ󂯓���Ȃ�
                e.Effect = DragDropEffects.None;
        }

        //ListBox2�Ƀh���b�v���ꂽ�Ƃ�
        private void ListBox_DragDrop(object sender, DragEventArgs e)
        {
            //�h���b�v���ꂽ�f�[�^��string�^�����ׂ�
            if (e.Data.GetDataPresent(typeof(string)))
            {
                ListBox target = (ListBox)sender;
                //�h���b�v���ꂽ�f�[�^(string�^)���擾
                string itemText =
                    (string)e.Data.GetData(typeof(string));
                //�h���b�v���ꂽ�f�[�^�����X�g�{�b�N�X�ɒǉ�����
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
            //�t���[����i�߂�
            ImageAnimator.UpdateFrames(animatedImage);
            //�摜�̕\��
            e.Graphics.DrawImage(animatedImage, 0, 0);

            base.OnPaint(e);
        }
        */
        //�X�e�[�W�`��
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
                        // enum �g�����ق���������₷����
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

            Image img_green = Image.FromFile("����.jpg");
            Image img_white = Image.FromFile("���.jpg");
            Image character_me = Image.FromFile("���ʂ�.png");
            Image character_enemy = Image.FromFile("�ӂ��낤.png");
            Image img_ice = Image.FromFile("�X.png");
            Image img_jump = Image.FromFile("��.png");
            Image animatedImage_up = Image.FromFile("������_��.gif");
            Image animatedImage_right = Image.FromFile("������_�E.gif");
            Image animatedImage_down = Image.FromFile("������_��.gif");
            Image animatedImage_left = Image.FromFile("������_��.gif");

            //MemoryStream stream = new MemoryStream();
            //byte[] bytes = File.ReadAllBytes("�E.gif");
            //stream.Write(bytes, 0, bytes.Length);
            //Bitmap animatedImage_right = new Bitmap(stream);

            //�A�j���J�n
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

        //���[�U�[�̓��͂�ϊ�
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
                        int goal = 0; //��Őݒ�

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

                        int goal = 0; //��Őݒ�

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

        //�����蔻��
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

        //�L�����̍��W�X�V
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
            int waittime = 250; //�~���b

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

                //�ړ��悪�؂̏ꍇ�A�؂̕����ɂ͐i�߂Ȃ�
                if (Map[y + move_copy[0][1], x + move_copy[0][0]] == 8)
                {
                    move_copy.Clear();
                    break;
                    //500�~���b=0.5�b�ҋ@����
                    Thread.Sleep(waittime);
                    //continue;
                }

                x += move_copy[0][0];
                y += move_copy[0][1];

                Global.x_now = x;
                Global.y_now = y;

                g2.Clear(Color.Transparent);
                Image character_me = Image.FromFile("���ʂ�.png");
                Image character_enemy = Image.FromFile("�ӂ��낤.png");
                g2.DrawImage(character_me, x * cell_length, y * cell_length, cell_length, cell_length);
                g2.DrawImage(character_enemy, Global.x_goal * cell_length, Global.y_goal * cell_length, cell_length, cell_length);


                //pictureBox�̒��g��h��ւ���
                InterThreadRefresh(this.pictureBox2.Refresh);

                if (Map[y,x] == 101)
                {
                    break;
                }
                //�ړ��悪�X�̏�Ȃ瓯�������ɂ������i��
                if (Map[y, x] == 2)
                {
                    //500�~���b=0.5�b�ҋ@����
                    Thread.Sleep(waittime);
                    continue;
                }

                //�ړ��悪�W�����v��Ȃ瓯�������ɓ��i�ށi�P��̏�Q���͖����j
                if (Map[y, x] == 3 || jump)
                {
                    if (move_floor)
                    {
                        move_floor = false;
                        move_copy.RemoveAt(0);
                    }

                    if (jump) //���̈ړ��Œ��n
                    {
                        jump = false;
                    }
                    else //�W�����v��̏�i���̈ړ��ŃW�����v�j
                    {
                        jump = true;
                    }

                    //500�~���b=0.5�b�ҋ@����
                    Thread.Sleep(waittime);
                    continue;
                }

                //��Ɉړ�����}�X�𓥂񂾏ꍇ1��ɐi��
                if (Map[y, x] == 4)
                {
                    move_copy[0] = new int[2] { 0, -1 };
                    Thread.Sleep(waittime);
                    continue;
                }

                //�E�Ɉړ�����}�X�𓥂񂾏ꍇ1�E�ɐi��
                if (Map[y, x] == 5)
                {
                    move_copy[0] = new int[2] { 1, 0 };
                    Thread.Sleep(waittime);
                    continue;
                }

                //���Ɉړ�����}�X�𓥂񂾏ꍇ1���ɐi��
                if (Map[y, x] == 6)
                {
                    move_copy[0] = new int[2] { 0, 1 };
                    Thread.Sleep(waittime);
                    continue;
                }

                //���Ɉړ�����}�X�𓥂񂾏ꍇ1���ɐi��
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

                //500�~���b=0.5�b�ҋ@����
                Thread.Sleep(waittime);
            }
            Global.count += 1;
        }


        /*******�֐� fin******/




        /******����Ȃ�******/
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

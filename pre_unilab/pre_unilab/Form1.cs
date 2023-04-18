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

            //ListBox1�̃C�x���g�n���h����ǉ�
            listBox2.MouseDown += new MouseEventHandler(ListBox2_MouseDown);
            //ListBox2�̃C�x���g�n���h����ǉ�
            listBox1.DragEnter += new DragEventHandler(ListBox1_DragEnter);
            listBox1.DragDrop += new DragEventHandler(ListBox1_DragDrop);

        }

        //ListBox1�Ń}�E�X�{�^���������ꂽ��
        private void ListBox2_MouseDown(object sender, MouseEventArgs e)
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
        private void ListBox1_DragEnter(object sender, DragEventArgs e)
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
        private void ListBox1_DragDrop(object sender, DragEventArgs e)
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

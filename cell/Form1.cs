using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace cell
{
    public partial class Form1 : Form
    {
        string[] args;
        int[,] tableA;
        int[,] tableB;
        RectangleF[,] rect;
        int count;
        int row, col;
        bool mousedraw;
        bool mousedown;
        //bool autotick;
        float width, height;
        int alivecell;
        int newcell;
        int[,] mcx;
        int[,] hxz;
        int[,] mfc;
        int[,] qbs;
        int[,] ggg;
        int[,] zfc;
        int[,] xfc;
        //int text;
        Timer timer1;
        int tv;

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        public Form1(string[] str)
        {
            InitializeComponent();
            args = str;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            this.FormBorderStyle = FormBorderStyle.None;     //设置窗体为无边框样式
            this.WindowState = FormWindowState.Maximized;    //最大化窗体 
            SetCursorPos(this.Width+1, this.Height+1);
            //this.BackColor = Color.Black;
            //this.label1.Text = "";
            //this.label1.AutoSize = false;
            this.label1.Location= new Point(0, 0);
            this.label1.Width = this.Width;
            this.label1.Height = this.Height;
            this.label1.BackColor = Color.Black;
            this.label1.Visible = true;
            this.label1.Paint += this.Form1_Paint;
            this.label1.MouseDown += this.Form1_MouseDown;
            this.label1.MouseLeave += this.Form1_MouseLeave;
            this.label1.MouseMove += this.Form1_MouseMove;
            this.label1.MouseUp += this.Form1_MouseUp;
            //this.label1.
            getsize();
            alivecell = 0;
            newcell = 0;
            count = 0;
            mousedraw = false;
            mousedown = false;
            initsetdata();
            timer1 = new Timer();
            timer1.Interval = 50;
            tv = 50;
            timer1.Tick += Timer1_Tick;
            timer1.Enabled = true;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            tick();
            this.label1.Refresh();
            if (count*tv > 1000*60*5)
            {
                this.timer1.Enabled = false;
                getrandomtable();
                this.timer1.Enabled = true;
                count = 0;
            }
                
            //this.label1.Refresh();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter)
            {
                Application.Exit();
            }
            else if (e.KeyCode == Keys.N)
            {
                if (timer1.Enabled == false)
                {
                    tick();
                    this.label1.Refresh();
                }
            }
            else if (e.KeyCode == Keys.Space)
            {
                if (timer1.Enabled == false)
                {
                    timer1.Enabled = true;
                    count = 0;
                    mousedraw = false;
                }
                else
                {
                    timer1.Enabled = false;
                    count = 0;
                    mousedraw = true;
                }
            }
            else if (e.KeyCode == Keys.C)
            {
                timer1.Enabled = false;
                cleartable();
                this.label1.Refresh();
            }
            else if (e.KeyCode == Keys.R)
            {
                timer1.Enabled = false;
                getrandomtable();
                this.label1.Refresh();
            }
            else if (e.KeyCode == Keys.Add)
            {
                tv = tv - 20;
                if (tv < 10)
                    tv = 10;
                timer1.Interval = tv;
                count = 0;
            }
            else if (e.KeyCode == Keys.Subtract)
            {
                tv = tv + 20;
                if (tv > 500)
                    tv = 500;
                timer1.Interval = tv;
                count = 0;
            }
            else if (e.KeyCode == Keys.D7)
            {
                cleartable();
                settable(0);
                this.label1.Refresh();
                count = 0;
            }
            else if (e.KeyCode == Keys.D1)
            {
                cleartable();
                settable(1);
                this.label1.Refresh();
                count = 0;
            }
            else if (e.KeyCode == Keys.D2)
            {
                cleartable();
                settable(2);
                this.label1.Refresh();
                count = 0;
            }
            else if (e.KeyCode == Keys.D3)
            {
                cleartable();
                settable(3);
                this.label1.Refresh();
                count = 0;
            }
            else if (e.KeyCode == Keys.D4)
            {
                cleartable();
                settable(4);
                this.label1.Refresh();
                count = 0;
            }
            else if (e.KeyCode == Keys.D5)
            {
                cleartable();
                settable(5);
                this.label1.Refresh();
                count = 0;
            }
            else if (e.KeyCode == Keys.D6)
            {
                cleartable();
                settable(6);
                this.label1.Refresh();
                count = 0;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            SolidBrush sb1 = new SolidBrush(Color.Cyan);
            SolidBrush sb2 = new SolidBrush(Color.Black);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (tableA[i, j] == 1)
                    {
                        g.FillRectangle(sb1, rect[i, j]);
                    }
                    else
                    {
                        g.FillRectangle(sb2, rect[i, j]);
                    }
                }
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (mousedraw == true)
            {
                mousedown = true;
                int di, dj;
                dj = (int)(e.X / width);
                di = (int)(e.Y / height);
                if (e.Button == MouseButtons.Left)
                {
                    tableA[di, dj] = 1;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    tableA[di, dj] = 0;
                }
                this.label1.Refresh();
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mousedraw == true && mousedown == true)
            {
                int di, dj;
                dj = (int)(e.X / width);
                di = (int)(e.Y / height);
                if (di >= row)
                {
                    di = row - 1;
                }
                if (dj >= col)
                {
                    dj = col - 1;
                }
                if (di <= 0)
                    di = 0;
                if (dj <= 0)
                    dj = 0;
                if (e.Button == MouseButtons.Left)
                {
                    tableA[di, dj] = 1;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    tableA[di, dj] = 0;
                }
                this.label1.Refresh();
            }
        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            mousedown = false;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            mousedown = false;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            getsize();
        }

        void getsize()
        {
            col = (int)(this.Width * 1.0 / 5 + 0.5);
            row = (int)(this.Height * 1.0 / 5 + 0.5);
            rect = new RectangleF[row, col];
            width = 1.0f * this.Width / col;
            height = 1.0f * this.Height / row;
            tableA = new int[row, col];
            tableB = new int[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    rect[i, j].X = width * j;
                    rect[i, j].Y = height * i;
                    rect[i, j].Width = width - 1;
                    rect[i, j].Height = height - 1;
                }
            }
            getrandomtable();
        }
        void getrandomtable()
        {
            Random r = new Random();
            //cleartable();
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (r.NextDouble() > 0.8)
                    {
                        tableA[i, j] = 1;
                        alivecell++;
                    }
                    else
                    {
                        tableA[i, j] = 0;
                    }
                    tableB[i, j] = 0;
                }
            }
        }
        void tick()
        {
            newcell = 0;
            alivecell = 0;
            int sum8 = 0;
            int i1, j1, i2, j2;

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    i1 = (i - 1 + row) % row;
                    j1 = (j - 1 + col) % col;
                    i2 = (i + 1) % row;
                    j2 = (j + 1) % col;
                    sum8 = tableA[i1, j1] + tableA[i1, j2] + tableA[i1, j] + tableA[i, j1] + tableA[i, j2] + tableA[i2, j1] + tableA[i2, j] + tableA[i2, j2];
                    if (sum8 == 2)
                    {
                        tableB[i, j] = tableA[i, j];
                        if (tableB[i, j] == 1)
                        {
                            alivecell++;
                        }
                    }
                    else if (sum8 == 3)
                    {
                        tableB[i, j] = 1;
                        newcell++;
                    }
                    else
                    {
                        tableB[i, j] = 0;
                    }
                }
            }
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    tableA[i, j] = tableB[i, j];
                }
            }
            count++;
        }
        void initsetdata()
        {
            mcx = new int[13, 13]{
                { 0,0,1,1,0,0,0,0,0,1,1,0,0},
                { 0,0,0,1,1,0,0,0,1,1,0,0,0},
                { 1,0,0,1,0,1,0,1,0,1,0,0,1},
                { 1,1,1,0,1,1,0,1,1,0,1,1,1},
                { 0,1,0,1,0,1,0,1,0,1,0,1,0},
                { 0,0,1,1,1,0,0,0,1,1,1,0,0},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0},
                { 0,0,1,1,1,0,0,0,1,1,1,0,0},
                { 0,1,0,1,0,1,0,1,0,1,0,1,0},
                { 1,1,1,0,1,1,0,1,1,0,1,1,1},
                { 1,0,0,1,0,1,0,1,0,1,0,0,1},
                { 0,0,0,1,1,0,0,0,1,1,0,0,0},
                { 0,0,1,1,0,0,0,0,0,1,1,0,0},
            };
            hxz = new int[3, 3]{
                { 0,0,1},
                { 1,0,1},
                { 0,1,1}
            };
            mfc = new int[4, 5]{
                { 0,1,1,0,0},
                { 1,1,1,1,0},
                { 1,1,0,1,1},
                { 0,0,1,1,0}
            };
            qbs = new int[7, 22]{
                { 0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0},
                { 0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0},
                { 0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
                { 1,1,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,1},
                { 1,1,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,1,1},
                { 0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0},
                { 0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0}
            };
            ggg = new int[9, 36]{
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0},
                { 0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1},
                { 0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1},
                { 1,1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                { 1,1,0,0,0,0,0,0,0,0,1,0,0,0,1,0,1,1,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0},
                { 0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0},
                { 0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                { 0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
            };
            zfc = new int[4, 6] {
                {0,1,1,1,1,1},
                {1,0,0,0,0,1},
                {0,0,0,0,0,1},
                {1,0,0,0,1,0}
            };
            xfc = new int[4, 5] {
                {0,1,1,1,1 },
                {1,0,0,0,1 },
                {0,0,0,0,1 },
                {1,0,0,1,0 }
            };
        }
        void settable(int a)
        {
            if (a == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        tableA[i + 18, j + 18] = hxz[i, j];
                    }
                }
            }
            else if (a == 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        tableA[i + 20, j + 5] = mfc[i, j];
                    }
                }
            }
            else if (a == 2)
            {
                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < 22; j++)
                    {
                        tableA[i + 2, j + 2] = qbs[i, j];
                    }
                }
            }
            else if (a == 3)
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 36; j++)
                    {
                        tableA[i + 1, j + 1] = ggg[i, j];
                    }
                }
            }
            else if (a == 4)
            {
                for (int i = 0; i < 13; i++)
                {
                    for (int j = 0; j < 13; j++)
                    {
                        tableA[i + 2, j + 2] = mcx[i, j];
                    }
                }
            }
            else if (a == 5)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        tableA[i + 2, j + 2] = xfc[i, j];
                    }
                }
            }
            else if (a == 6)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        tableA[i + 20, j + 2] = zfc[i, j];
                    }
                }
            }
        }
        void cleartable()
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    tableA[i, j] = 0;
                    tableB[i, j] = 0;
                }
            }
            alivecell = 0;
            newcell = 0;
        }
    
    }
}

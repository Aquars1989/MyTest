using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyTest
{
    public partial class MazeBuilder : Form
    {
        private int[,] _PathMap = null;
        private Point[,] _LinkPoint = null;
        private Image _ImageMap = null;

        private int ScaleLevel = 8;
        Pen _DrawPen;
        Random _Rand = new Random();

        public MazeBuilder()
        {
            InitializeComponent();
            buttonCancel.Enabled = false;
            _DrawPen = new Pen(Color.White, ScaleLevel) { Alignment = System.Drawing.Drawing2D.PenAlignment.Center };
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            _ImageMap = null;
            _PathMap = null;
            buttonCancel.Enabled = false;
            buttonBuild.Enabled = true;
            picMaze.Invalidate();
        }

        private void picMaze_Paint(object sender, PaintEventArgs e)
        {
            if (_ImageMap != null)
                e.Graphics.DrawImage(_ImageMap, 0, 0);
        }

        private void buttonBuild_Click(object sender, EventArgs e)
        {
            int mapWidth = picMaze.Width / ScaleLevel;
            int mapHeight = picMaze.Height / ScaleLevel;

            _PathMap = new int[mapWidth, mapHeight];
            _LinkPoint = new Point[mapWidth, mapHeight];
            _ImageMap = new Bitmap(picMaze.Width, picMaze.Height);

            Stack<Point> dfs = new Stack<Point>();
            Point originPoint = new Point(0, 0);

            dfs.Push(originPoint);
            _PathMap[originPoint.X, originPoint.Y] = 1;
            _LinkPoint[originPoint.X, originPoint.Y] = new Point(-1, 0);

            int dr = 0;
            bool exit = false;

            using (Graphics g = Graphics.FromImage(_ImageMap))
            {
                while (dfs.Count > 0)
                {
                    Point pt = dfs.Pop();
                    int x = pt.X;
                    int y = pt.Y;

                    List<Point> spts = new List<Point> { new Point(x + 2, y), new Point(x - 2, y), new Point(x, y + 2), new Point(x, y - 2) };
                    //List<Point> spts = new List<Point>() { new Point(x + 2, y), new Point(x - 2, y), new Point(x, y + 2), new Point(x, y - 2),
                    //                                       new Point(x + 2, y+2), new Point(x - 2, y-2), new Point(x-2, y + 2), new Point(x+2, y - 2)};

                    while (spts.Count > 0)
                    {
                        int idx = _Rand.Next(spts.Count);
                        Point spt = spts[idx];

                        spts.RemoveAt(idx);

                        int sx = spt.X;
                        int sy = spt.Y;
                        if (sx >= 0 && sx < mapWidth && sy >= 0 && sy < mapHeight && _PathMap[sx, sy] == 0)
                        {
                            _LinkPoint[sx, sy] = pt;
                            dfs.Push(spt);

                        }

                        _PathMap[x, y] = 1;

                        g.DrawLine(_DrawPen, new Point(_LinkPoint[x, y].X * ScaleLevel + ScaleLevel, _LinkPoint[x, y].Y * ScaleLevel + ScaleLevel), new Point(x * ScaleLevel + ScaleLevel, y * ScaleLevel + ScaleLevel));

                        if (x == mapWidth - 1 && !exit)
                        {
                            exit = true;
                            g.DrawLine(_DrawPen, new Point(x * ScaleLevel + ScaleLevel * 5, y * ScaleLevel + ScaleLevel), new Point(x * ScaleLevel + ScaleLevel, y * ScaleLevel + ScaleLevel));
                        }
                    }

                    dr++;
                    if (dr > 1)
                    {
                        picMaze.Refresh();
                        dr = 0;
                    }
                }
            }

            buttonCancel.Enabled = true;
            buttonBuild.Enabled = false;
            picMaze.Invalidate();
        }
    }
}

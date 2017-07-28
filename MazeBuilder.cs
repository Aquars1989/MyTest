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
        private MapPoint[,] _LinkMap = null;
        private Image _ImageMap = null;

        private int ScaleLevel = 8;
        private Pen _DrawPen;
        private Random _Rand = new Random();

        public MazeBuilder()
        {
            InitializeComponent();
            buttonClear.Enabled = false;
            _DrawPen = new Pen(Color.White, ScaleLevel) { Alignment = System.Drawing.Drawing2D.PenAlignment.Center };
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            _ImageMap = null;
            _LinkMap = null;
            buttonSave.Enabled = false;
            buttonClear.Enabled = false;
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
            int imageWidth = picMaze.Width;
            int imageHeight = picMaze.Height;
            int mapWidth = (imageWidth / ScaleLevel) - 2;
            int mapHeight = (imageHeight / ScaleLevel) - 2;
            mapWidth -= (mapWidth + 1) % 2;
            mapHeight -= (mapHeight + 1) % 2;

            int paddingHorizontal = (int)((imageWidth - mapWidth * ScaleLevel) / 2 * 1.5);
            int paddingVertical = (int)((imageHeight - mapHeight * ScaleLevel) / 2 * 1.5);

            _LinkMap = new MapPoint[mapWidth, mapHeight];
            _ImageMap = new Bitmap(imageWidth, imageHeight);

            Stack<Point> dfs = new Stack<Point>();
            Point originPoint = new Point(0, 0);

            dfs.Push(originPoint);
            _LinkMap[originPoint.X, originPoint.Y] = new MapPoint()
            {
                LinkPoint = new Point(-4, 0),
                Visit = true
            };

            bool exitBuilded = false;
            using (Graphics g = Graphics.FromImage(_ImageMap))
            {
                g.Clear(Color.Black);
                g.TranslateTransform(paddingHorizontal, paddingVertical);
                while (dfs.Count > 0)
                {
                    Point pt = dfs.Pop();
                    int x = pt.X;
                    int y = pt.Y;

                    List<Point> nextPoints = new List<Point> { new Point(x + 2, y), new Point(x - 2, y), new Point(x, y + 2), new Point(x, y - 2) };
                    //List<Point> spts = new List<Point>() { new Point(x + 2, y), new Point(x - 2, y), new Point(x, y + 2), new Point(x, y - 2),
                    //                                       new Point(x + 2, y+2), new Point(x - 2, y-2), new Point(x-2, y + 2), new Point(x+2, y - 2)};

                    while (nextPoints.Count > 0)
                    {
                        int idx = _Rand.Next(nextPoints.Count);
                        Point nextPoint = nextPoints[idx];

                        nextPoints.RemoveAt(idx);

                        int nextX = nextPoint.X;
                        int nextY = nextPoint.Y;
                        if (nextX >= 0 && nextX < mapWidth && nextY >= 0 && nextY < mapHeight && !_LinkMap[nextX, nextY].Visit)
                        {
                            _LinkMap[nextX, nextY].LinkPoint = pt;
                            dfs.Push(nextPoint);
                        }
                    }

                    _LinkMap[x, y].Visit = true;
                    g.DrawLine(_DrawPen, new Point(_LinkMap[x, y].LinkPoint.X * ScaleLevel, _LinkMap[x, y].LinkPoint.Y * ScaleLevel), new Point(x * ScaleLevel, y * ScaleLevel));

                    if (x == mapWidth - 1 && !exitBuilded)
                    {
                        exitBuilded = true;
                        g.DrawLine(_DrawPen, new Point(x * ScaleLevel, y * ScaleLevel), new Point(x * ScaleLevel + paddingHorizontal, y * ScaleLevel));
                    }

                    picMaze.Refresh();
                }
            }

            buttonSave.Enabled = true;
            buttonClear.Enabled = true;
            buttonBuild.Enabled = false;
            picMaze.Invalidate();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (_ImageMap != null && ImageFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _ImageMap.Save(ImageFileDialog.FileName, ImageFormat.Png);
            }
        }

        public struct MapPoint
        {
            public Point LinkPoint { get; set; }
            public bool Visit { get; set; }
        }
    }
}

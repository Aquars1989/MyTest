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
    public partial class MazeSearch : Form
    {
        private int[,] _PathMap = null;
        private int[,] _Map = null;
        private List<Point> _PathPots = null;

        private Point _Point1 = new Point(-1, -1);
        public Point Point1
        {
            get { return _Point1; }
            set
            {
                _Point1 = value;
                labelFrom.Text = "Form(" + (_Point1.X < 0 ? "--" : Point1.X + "," + Point1.Y) + ")";
                picMaze.Invalidate();
            }
        }

        private Point _Point2 = new Point(-1, -1);
        public Point Point2
        {
            get { return _Point2; }
            set
            {
                _Point2 = value;
                labelTo.Text = "To(" + (_Point2.X < 0 ? "--" : _Point2.X + "," + _Point2.Y) + ")";
                _PathMap = null;
                _PathPots = null;
                picMaze.Invalidate();
            }
        }

      

        public MazeSearch()
        {
            InitializeComponent();
            SetMode(0);
        }

        private void oBuCancel_Click(object sender, EventArgs e)
        {
            SetMode(0);
        }
        
        private void picMaze_Paint(object sender, PaintEventArgs e)
        {
            if (_PathPots != null)
            {
                foreach (Point pt in _PathPots)
                {
                    e.Graphics.FillRectangle(Brushes.Red, pt.X - 1, pt.Y - 1, 2, 2);
                }
            }

            //if (MoveMap != null)
            //{
            //    for (int i = 0; i < MoveMap.GetLength(0); i++)
            //    {
            //        for (int j = 0; j < MoveMap.GetLength(1); j++)
            //        {
            //            if (MoveMap[i, j] > 0)
            //            {
            //                e.Graphics.FillRectangle(Brushes.Red, i, j, 1, 1);
            //            }
            //        }
            //    }
            //}


            if (Point1.X >= 0)
            {
                e.Graphics.FillEllipse(Brushes.MediumSlateBlue, Point1.X - 5, Point1.Y - 5, 10, 10);
            }

            if (Point2.X >= 0)
            {
                e.Graphics.FillEllipse(Brushes.Coral, Point2.X - 5, Point2.Y - 5, 10, 10);
            }
        }

        private void picMaze_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case System.Windows.Forms.MouseButtons.Left:
                    Point1 = _Map[e.X, e.Y] == 0 ? e.Location : new Point(-1, -1);
                    break;
                case System.Windows.Forms.MouseButtons.Right:
                    Point2 = _Map[e.X, e.Y] == 0 ? e.Location : new Point(-1, -1);
                    break;
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (Point1.X < 0 || Point2.X < 0) return;

            int wid = _Map.GetLength(0);
            int hei = _Map.GetLength(1);

            _PathMap = new int[wid, hei];
            Stack<Point> dfs = new Stack<Point>();
            dfs.Push(Point2);
            _PathMap[Point2.X, Point2.Y] = 1;

            int dr = 0;
            while (dfs.Count > 0)
            {
                Point pt = dfs.Pop();
                int x = pt.X;
                int y = pt.Y;

                if (pt.Equals(Point1)) break;

                Point[] spts = new Point[] { new Point(x + 1, y), new Point(x - 1, y), new Point(x, y + 1), new Point(x, y - 1) };
                //Point[] spts = new Point[] { new Point(x + 1, y), new Point(x - 1, y), new Point(x, y + 1), new Point(x, y - 1),
                //                             new Point(x + 1, y+1), new Point(x - 1, y-1), new Point(x-1, y + 1), new Point(x+1, y - 1)};

                foreach (Point spt in spts)
                {
                    int sx = spt.X;
                    int sy = spt.Y;
                    if (sx >= 0 && sx < wid && sy >= 0 && sy < hei && _Map[sx, sy] == 0 && _PathMap[sx, sy] == 0)
                    {
                        _PathMap[sx, sy] = _PathMap[x, y] + 1;
                        dfs.Push(spt);
                    }
                }
                dr++;
                //if (dr > 10)
                //{
                //    oPic.Refresh();
                //    dr = 0;
                //}
            }

            if (_PathMap[Point1.X, Point1.Y] == 0) _PathPots = null;

            _PathPots = new List<Point>();
            _PathPots.Add(Point1);
            Point fpt = Point1;

            while (_PathMap[fpt.X, fpt.Y] > 1)
            {
                int x = fpt.X;
                int y = fpt.Y;

                Point[] spts = new Point[] { new Point(x + 1, y), new Point(x - 1, y), new Point(x, y + 1), new Point(x, y - 1) };
                //Point[] spts = new Point[] { new Point(x + 1, y), new Point(x - 1, y), new Point(x, y + 1), new Point(x, y - 1) ,
                //                             new Point(x + 1, y+1), new Point(x - 1, y-1), new Point(x-1, y + 1), new Point(x+1, y - 1)};

                bool fid = false;
                foreach (Point spt in spts)
                {
                    int sx = spt.X;
                    int sy = spt.Y;
                    if (sx >= 0 && sx < wid && sy >= 0 && sy < hei && _PathMap[sx, sy] == _PathMap[x, y] - 1)
                    {
                        _PathPots.Add(spt);
                        fpt = spt;
                        fid = true;
                        break;
                    }
                }
                if (!fid)
                {
                    return;
                }
            }
            picMaze.Invalidate();
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            if (ImageFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string imagePath = ImageFileDialog.FileName;
                try
                {
                    Bitmap mazeImage = new Bitmap(imagePath);
                    BuildMap(mazeImage);
                    picMaze.Image = mazeImage;
                }
                catch
                {
                    return;
                }

                SetMode(1);
            }
        }

        private void BuildMap(Bitmap img)
        {
            unsafe
            {
                _Map = new int[img.Width, img.Height];
                Rectangle rtg = new Rectangle(0, 0, img.Width, img.Height);
                BitmapData bd = img.LockBits(rtg, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                IntPtr ip = bd.Scan0;
                byte* p = (byte*)ip.ToPointer();
                for (int j = 0; j < img.Height; j++)
                {
                    for (int i = 0; i < img.Width; i++)
                    {
                        if (p[0] + p[1] + p[2] < 500)
                        {
                            _Map[i, j] = 1;
                        }
                        p += 4;
                    }
                }

                img.UnlockBits(bd);
            }
        }

        private void SetMode(int mode)
        {
            switch (mode)
            {
                case 0:
                    buttonImport.Enabled = true;
                    buttonCancel.Visible = false;
                    buttonStart.Visible = false;
                    picMaze.Visible = false;
                    _Map = null;
                    picMaze.Image = null;
                    labelFrom.Visible = false;
                    labelTo.Visible = false;
                    Size = new Size(210, 110);
                    break;
                case 1:
                    buttonImport.Enabled = false;
                    buttonCancel.Visible = true;
                    buttonStart.Visible = true;
                    picMaze.Visible = true;
                    Point1 = new Point(-1, -1);
                    Point2 = new Point(-1, -1);
                    labelFrom.Visible = true;
                    labelTo.Visible = true;

                    Size = new Size(Math.Max(picMaze.Left + picMaze.Width, buttonStart.Left + buttonStart.Width) + 50, picMaze.Top + picMaze.Height + 50);
                    break;
            }
        }

    }
}

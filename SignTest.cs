using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyTest
{
    public partial class SignTest : Form
    {
        private bool _Write = false;
        private GraphicsPath _DrawPath = new GraphicsPath() { FillMode = FillMode.Winding };
        private List<List<Point>> _Lines = new List<List<Point>>();
        private List<Point> _LinePoints;
        private Point _LastPoint;
        private Stopwatch _SWatch = new Stopwatch();

        public SignTest()
        {
            DoubleBuffered = true;
            InitializeComponent();
        }

        private void Sign_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case System.Windows.Forms.MouseButtons.Left:
                    _Write = true;
                    _LastPoint = e.Location;
                    _LinePoints = null;
                    break;
            }
        }

        private void Sign_MouseMove(object sender, MouseEventArgs e)
        {
            if (_Write)
            {
                if (_LastPoint != e.Location)
                {
                    if (_LinePoints == null)
                    {
                        _LinePoints = new List<Point>();
                        _Lines.Add(_LinePoints);
                        _LinePoints.Add(_LastPoint);
                    }

                    _LinePoints.Add(e.Location);
                    _LastPoint = e.Location;
                    Invalidate();
                }
            }
        }

        private void Sign_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case System.Windows.Forms.MouseButtons.Left:
                    _Write = false;
                    if (_LinePoints != null)
                    {
                        if (_LinePoints.Count > 2)
                        {
                            _DrawPath.AddPolygon(GetLinePath(_LinePoints));
                        }
                        else
                        {
                            _Lines.Remove(_LinePoints);
                        }
                        _LinePoints = null;
                    }
                    break;
            }
        }

        private void Sign_Paint(object sender, PaintEventArgs e)
        {
            _SWatch.Restart();
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillPath(Brushes.Black, _DrawPath);
            if (_LinePoints != null && _LinePoints.Count > 2)
            {
                e.Graphics.FillPolygon(Brushes.Black, GetLinePath(_LinePoints), FillMode.Winding);
            }

            _SWatch.Stop();
            e.Graphics.DrawString(_SWatch.Elapsed.Ticks.ToString(), Font, Brushes.Red, 5, 5);
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            List<string> outTexts = new List<string>();
            foreach (var line in _Lines)
            {
                StringBuilder lineStr = new StringBuilder();
                foreach (var point in line)
                {
                    lineStr.AppendFormat((lineStr.Length == 0 ? "" : "|") + "{0},{1}", point.X, point.Y);
                }
                outTexts.Add(lineStr.ToString());
            }
            File.WriteAllLines("test.txt", outTexts);
        }

        private void btnLoad1_Click(object sender, EventArgs e)
        {
            ClearLines();

            if (!File.Exists("test.txt")) return;
            string[] inTexts = File.ReadAllLines("test.txt");
            foreach (string lineStr in inTexts)
            {
                List<Point> pots = new List<Point>();
                string[] potStrs = lineStr.Split('|');
                foreach (string pot in potStrs)
                {
                    string[] xy = pot.Split(',');
                    pots.Add(new Point(int.Parse(xy[0]), int.Parse(xy[1])));
                }
                _Lines.Add(pots);
                _DrawPath.AddPolygon(GetLinePath(pots));
            }

            Invalidate();
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter msWriter = new BinaryWriter(ms))
            using (FileStream fs = new FileStream("test.sgn", FileMode.Create))
            using (GZipStream gzip = new GZipStream(fs, CompressionMode.Compress))
            {
                foreach (var line in _Lines)
                {
                    int minVal = int.MaxValue, maxVal = int.MinValue;
                    Point[] writePots = new Point[line.Count];

                    writePots[0] = line[0];
                    for (int i = 1; i < line.Count; i++)
                    {
                        writePots[i] = new Point(line[i].X - line[i - 1].X, line[i].Y - line[i - 1].Y); //紀錄下個點的位移值以縮減資料大小
                        minVal = Math.Min(minVal, line[i].X);
                        minVal = Math.Min(minVal, line[i].Y);
                        maxVal = Math.Max(maxVal, line[i].X);
                        maxVal = Math.Max(maxVal, line[i].Y);
                    }

                    bool byteType = minVal >= sbyte.MinValue && maxVal <= sbyte.MaxValue; //資料是否可用sbyte紀錄
                    msWriter.Write((short)(line.Count * (byteType ? 1 : -1)));            //如可用sbyte紀錄,紀錄列數為正數,否為負數
                    for (int i = 0; i < writePots.Length; i++)
                    {
                        if (i > 0 && byteType)
                        {
                            msWriter.Write((sbyte)writePots[i].X);
                            msWriter.Write((sbyte)writePots[i].Y);
                        }
                        else
                        {
                            msWriter.Write((short)writePots[i].X);
                            msWriter.Write((short)writePots[i].Y);
                        }
                    }
                }

                gzip.Write(ms.ToArray(), 0, (int)ms.Length);
                msWriter.Close();
                ms.Close();
                gzip.Close();
                fs.Close();
            }
        }

        private void btnLoad2_Click(object sender, EventArgs e)
        {
            ClearLines();
            if (!File.Exists("test.sgn")) return;
            using (FileStream fs = new FileStream("test.sgn", FileMode.Open))
            using (GZipStream gzip = new GZipStream(fs, CompressionMode.Decompress))
            using (MemoryStream ms = new MemoryStream())
            using (BinaryReader br = new BinaryReader(ms))
            {
                byte[] block = new byte[1024];
                try
                {
                    int bytesRead = gzip.Read(block, 0, block.Length);
                    while (bytesRead > 0)
                    {
                        ms.Write(block, 0, bytesRead);
                        bytesRead = gzip.Read(block, 0, block.Length);
                    }
                }
                catch
                {
                    return;
                }
                finally
                {
                    gzip.Close();
                    fs.Close();
                }

                if (ms.Length == 0) return;

                ms.Position = 0;
                while (ms.Position < ms.Length)
                {
                    short cot = br.ReadInt16();
                    bool byteType = true;
                    if (cot < 0)
                    {
                        byteType = false;
                        cot = (short)-cot;
                    }

                    List<Point> pots = new List<Point>();
                    short x = br.ReadInt16();
                    short y = br.ReadInt16();
                    pots.Add(new Point(x, y));
                    for (int i = 1; i < cot; i++)
                    {
                        short fx = byteType ? br.ReadSByte() : br.ReadInt16();
                        short fy = byteType ? br.ReadSByte() : br.ReadInt16();
                        pots.Add(new Point(pots[i - 1].X + fx, pots[i - 1].Y + fy));
                    }

                    _Lines.Add(pots);
                    _DrawPath.AddPolygon(GetLinePath(pots));
                }
                ms.Close();
                br.Close();
            }

            Invalidate();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearLines();
            Invalidate();
        }

        private void ClearLines()
        {
            _DrawPath.Dispose();
            _DrawPath = new GraphicsPath() { FillMode = FillMode.Winding };
            _Lines.Clear();
        }

        private PointF[] GetLinePath(List<Point> points)
        {
            int potCot = points.Count;
            PointF[] result = new PointF[potCot * 2 - 2];
            for (int i = 0; i < potCot; i++)
            {
                if (i == 0 || i == potCot - 1)
                {
                    result[i] = points[i];
                }
                else
                {
                    double dist = Math.Sqrt(Math.Pow(points[i - 1].X - points[i].X, 2) + Math.Pow(points[i - 1].Y - points[i].Y, 2));
                    double lwid = dist / 8F;
                    if (lwid > 1.3) lwid = 1.3;
                    lwid = 2 - lwid;
                    float dX = points[i + 1].X - points[i - 1].X;
                    float dY = points[i + 1].Y - points[i - 1].Y;
                    double roation = Math.Atan2(dY, dX) + (90 / 180D * Math.PI);
                    float fxX = (float)(Math.Cos(roation) * lwid);
                    float fxY = (float)(Math.Sin(roation) * lwid);
                    float offsetX1 = points[i].X + fxX;
                    float offsetY1 = points[i].Y + fxY;
                    float offsetX2 = points[i].X - fxX;
                    float offsetY2 = points[i].Y - fxY;
                    result[i] = new PointF(offsetX1, offsetY1);
                    result[result.Length - i] = new PointF(offsetX2, offsetY2);
                }
            }
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace MyTest
{
    public partial class ImageConvert : Form
    {
        private const int _AllowanceHalfWidth = 20;
        private const int _ImagePaddingV = 20;
        private const int _ImagePaddingH = 20;
        private Font _AllowanceFont = new Font("Arial", 10);
        private StringFormat _AllowanceFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

        private bool _MouseDown;
        private Point _MouseDownPoint;
        private int _HoverIndex = -1;
        private int _PickInfoIndex = 0;
        private List<PickInfo> _PickInfos = new List<PickInfo>();

        private double _Scale = 1;
        private Bitmap _BaseImage = null;      //原始圖片
        private Bitmap _ScaleImage = null;     //預覽圖片
        private Bitmap _ConvertedImage = null; //預覽變更圖片
        private int[,] _Map;
        private Rectangle _ImageRect = new Rectangle();

        public ImageConvert()
        {

            InitializeComponent();

            PropertyInfo doubleBuffered = splitContainer1.Panel1.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            if (doubleBuffered != null)
            {
                doubleBuffered.SetValue(splitContainer1.Panel1, true, null);
                doubleBuffered.SetValue(splitContainer1.Panel2, true, null);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                saveFileDialog1.InitialDirectory = openFileDialog1.InitialDirectory;
                _PickInfos.Clear();
                _BaseImage = new Bitmap(openFileDialog1.FileName);
                double maxHei = 500;
                double maxWid = 500;
                double scale = Math.Max(_BaseImage.Height / maxHei, _BaseImage.Width / maxWid);
                Size newSize = new Size((int)(_BaseImage.Width / scale), (int)(_BaseImage.Height / scale));
                _ScaleImage = new Bitmap(_BaseImage, newSize); //產生縮小預覽圖片
                _ConvertedImage = new Bitmap(_ScaleImage.Width, _ScaleImage.Height);
                _Map = new int[_ScaleImage.Width, _ScaleImage.Height]; //快取配置圖
                PickColor(_ScaleImage, _ConvertedImage, _PickInfos, 0);
                SetImageSize();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Bitmap exportImage = new Bitmap(_BaseImage.Width, _BaseImage.Height);
                PickColor(_BaseImage, exportImage, _PickInfos, 0);

                string type = Path.GetExtension(saveFileDialog1.FileName).ToLower();
                switch (type)
                {
                    case ".bmp":
                        exportImage.Save(saveFileDialog1.FileName, ImageFormat.Bmp);
                        break;
                    case ".jpg":
                        exportImage.Save(saveFileDialog1.FileName, ImageFormat.Jpeg);
                        break;
                    case ".gif":
                        exportImage.Save(saveFileDialog1.FileName, ImageFormat.Gif);
                        break;
                    case ".png":
                        exportImage.Save(saveFileDialog1.FileName, ImageFormat.Png);
                        break;
                    default:
                        goto case ".bmp";
                }
            }
        }

        private void splitContainer1_SizeChanged(object sender, EventArgs e)
        {
            SetImageSize();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
            if (_ScaleImage != null)
            {
                e.Graphics.DrawImage(_ScaleImage, _ImageRect);

                foreach (PickInfo pickInfo in _PickInfos)
                {
                    Rectangle rectFull = new Rectangle(pickInfo.ScalePoint.X - _AllowanceHalfWidth, pickInfo.ScalePoint.Y - _AllowanceHalfWidth, _AllowanceHalfWidth * 2, _AllowanceHalfWidth * 2);
                    using (SolidBrush fillEllipseBrush = new SolidBrush(Color.FromArgb(140, 255 - pickInfo.Color.R, 255 - pickInfo.Color.G, 255 - pickInfo.Color.B)))
                    {
                        e.Graphics.FillEllipse(fillEllipseBrush, rectFull);
                    }

                    int allowWid = (int)(_AllowanceHalfWidth * (pickInfo.Allowance / 255F));
                    if (allowWid > 0)
                    {
                        Rectangle rectAllowance = new Rectangle(pickInfo.ScalePoint.X - allowWid, pickInfo.ScalePoint.Y - allowWid, allowWid * 2, allowWid * 2);
                        using (SolidBrush allowanceBrush = new SolidBrush(Color.FromArgb(200, pickInfo.Color)))
                        {
                            e.Graphics.FillEllipse(allowanceBrush, rectAllowance);
                        }
                    }
                    e.Graphics.DrawEllipse(Pens.Red, rectFull);
                    //e.Graphics.DrawString(pickInfo.Allowance.ToString(), _AllowanceFont, Brushes.Red, rectFull, _AllowanceFormat);
                }
            }
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
            if (_ConvertedImage != null)
            {
                e.Graphics.DrawImage(_ConvertedImage, _ImageRect);
            }
        }

        private void SetImageSize()
        {
            if (_ScaleImage == null) return;

            double maxHei = splitContainer1.Panel1.Height - _ImagePaddingV * 2;
            double maxWid = splitContainer1.Panel1.Width - _ImagePaddingH * 2;
            _Scale = Math.Max(_ScaleImage.Height / maxHei, _ScaleImage.Width / maxWid);

            Size size = new Size((int)(_ScaleImage.Width / _Scale), (int)(_ScaleImage.Height / _Scale));
            Point loc = new Point((splitContainer1.Panel1.Width - size.Width) / 2, (splitContainer1.Panel1.Height - size.Height) / 2);
            _ImageRect = new Rectangle(loc, size);

            foreach (PickInfo pickInfo in _PickInfos)
            {
                int x = (int)(pickInfo.PickPoint.X / _Scale) + _ImageRect.Left;
                int y = (int)(pickInfo.PickPoint.Y / _Scale) + _ImageRect.Top;
                Point pot = new Point(x, y);
                pickInfo.ScalePoint = new Point(x, y);
            }

            splitContainer1.Invalidate(true);
        }

        private void BuildMap(int[,] map, Bitmap baseImage, List<PickInfo> pickInfos, PickInfo main)
        {
            unsafe
            {
                Rectangle rect = new Rectangle(0, 0, baseImage.Width, baseImage.Height);
                BitmapData baseData = baseImage.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                IntPtr basePtr = baseData.Scan0;
                byte* baseP = (byte*)basePtr.ToPointer();

                for (int y = 0; y < baseImage.Height; y++)
                {
                    for (int x = 0; x < baseImage.Width; x++)
                    {
                        byte r = baseP[2];
                        byte g = baseP[1];
                        byte b = baseP[0];

                        bool match = false;
                        foreach (var pickInfo in pickInfos)
                        {
                            if (pickInfo == main) continue;

                            if (Math.Abs(pickInfo.Color.R - r) <= pickInfo.Allowance &&
                                Math.Abs(pickInfo.Color.G - g) <= pickInfo.Allowance &&
                                Math.Abs(pickInfo.Color.B - b) <= pickInfo.Allowance)
                            {
                                match = true;
                                break;
                            }
                        }

                        if (match)
                        {
                            map[x, y] = -1;
                        }
                        else
                        {
                            int dR = Math.Abs(main.Color.R - r);
                            int dG = Math.Abs(main.Color.G - g);
                            int dB = Math.Abs(main.Color.B - b);
                            map[x, y] = Math.Max(Math.Max(dR, dG), dB);
                        }
                        baseP += 4;
                    }
                }
                baseImage.UnlockBits(baseData);
            }
        }

        private void PickColor(Bitmap baseImage, Bitmap convertImage, List<PickInfo> pickInfos, int mode)
        {
            unsafe
            {
                Rectangle rect = new Rectangle(0, 0, baseImage.Width, baseImage.Height);
                BitmapData baseData = baseImage.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                BitmapData convertData = convertImage.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                IntPtr basePtr = baseData.Scan0;
                IntPtr convertPtr = convertData.Scan0;
                byte* baseP = (byte*)basePtr.ToPointer();
                byte* convertP = (byte*)convertPtr.ToPointer();

                for (int y = 0; y < baseImage.Height; y++)
                {
                    for (int x = 0; x < baseImage.Width; x++)
                    {
                        byte r2 = convertP[2];
                        byte g2 = convertP[1];
                        byte b2 = convertP[0];
                        bool gray = r2 == g2 && g2 == b2;
                        if (mode == 0 || (mode > 0 && gray) || (mode < 0 && !gray))
                        {
                            byte r = baseP[2];
                            byte g = baseP[1];
                            byte b = baseP[0];
                            convertP[3] = baseP[3];

                            bool match = false;
                            foreach (var pickInfo in pickInfos)
                            {
                                if (Math.Abs(pickInfo.Color.R - r) <= pickInfo.Allowance &&
                                    Math.Abs(pickInfo.Color.G - g) <= pickInfo.Allowance &&
                                    Math.Abs(pickInfo.Color.B - b) <= pickInfo.Allowance)
                                {
                                    match = true;
                                    break;
                                }
                            }

                            if (match)
                            {
                                convertP[0] = b;
                                convertP[1] = g;
                                convertP[2] = r;
                            }
                            else
                            {
                                byte v = (byte)((r + g + b) / 3);
                                convertP[0] = v;
                                convertP[1] = v;
                                convertP[2] = v;
                            }
                        }

                        convertP += 4;
                        baseP += 4;
                    }
                }
                baseImage.UnlockBits(baseData);
                convertImage.UnlockBits(convertData);
            }
        }

        private void PickColor(Bitmap baseImage, Bitmap convertImage, int[,] map, int oldVal, int newVal)
        {
            if (oldVal == newVal) return;

            unsafe
            {
                Rectangle rect = new Rectangle(0, 0, baseImage.Width, baseImage.Height);
                BitmapData baseData = baseImage.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                BitmapData convertData = convertImage.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                IntPtr basePtr = baseData.Scan0;
                IntPtr convertPtr = convertData.Scan0;
                byte* baseP = (byte*)basePtr.ToPointer();
                byte* convertP = (byte*)convertPtr.ToPointer();
                bool plus = newVal > oldVal;
                if (plus)
                {
                    for (int y = 0; y < baseImage.Height; y++)
                    {
                        for (int x = 0; x < baseImage.Width; x++)
                        {
                            if (map[x, y] >= 0 && map[x, y] > oldVal && map[x, y] <= newVal)
                            {
                                byte r = baseP[2];
                                byte g = baseP[1];
                                byte b = baseP[0];
                                convertP[0] = b;
                                convertP[1] = g;
                                convertP[2] = r;
                            }
                            baseP += 4;
                            convertP += 4;
                        }
                    }
                }
                else
                {
                    for (int y = 0; y < baseImage.Height; y++)
                    {
                        for (int x = 0; x < baseImage.Width; x++)
                        {
                            if (map[x, y] >= 0 && map[x, y] <= oldVal && map[x, y] > newVal)
                            {
                                byte r = baseP[2];
                                byte g = baseP[1];
                                byte b = baseP[0];
                                byte v = (byte)((r + g + b) / 3);
                                convertP[0] = v;
                                convertP[1] = v;
                                convertP[2] = v;
                            }
                            baseP += 4;
                            convertP += 4;
                        }
                    }
                }
                baseImage.UnlockBits(baseData);
                convertImage.UnlockBits(convertData);
            }
        }

        private void splitContainer1_Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_MouseDown)
            {
                PickInfo pictInfo = _PickInfos[_PickInfoIndex];
                int offset = e.X - _MouseDownPoint.X;
                if (offset < 0) offset = 0;
                else if (offset > 255) offset = 255;


                PickColor(_ScaleImage, _ConvertedImage, _Map, pictInfo.Allowance, offset);
                pictInfo.Allowance = offset;
                splitContainer1.Panel1.Invalidate(_ImageRect, true);
                splitContainer1.Panel2.Invalidate( true);
            }
            else
            {
                _HoverIndex = -1;
                if (e.X >= _ImageRect.Left && e.X <= _ImageRect.Left + _ImageRect.Width &&
                    e.Y >= _ImageRect.Top && e.Y <= _ImageRect.Top + _ImageRect.Height)
                {
                    Cursor = Cursors.Cross;

                    double checkDist = Math.Pow(_AllowanceHalfWidth, 2);
                    for (int i = 0; i < _PickInfos.Count; i++)
                    {
                        PickInfo pickInfo = _PickInfos[i];
                        if (Math.Pow(pickInfo.ScalePoint.X - e.X, 2) + Math.Pow(pickInfo.ScalePoint.Y - e.Y, 2) < checkDist)
                        {
                            Cursor = Cursors.Hand;
                            _HoverIndex = i;
                            return;
                        }
                    }
                    Cursor = Cursors.Cross;
                }
                else
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private void splitContainer1_Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X >= _ImageRect.Left && e.X <= _ImageRect.Left + _ImageRect.Width &&
                e.Y >= _ImageRect.Top && e.Y <= _ImageRect.Top + _ImageRect.Height)
            {
                if (_HoverIndex >= 0)
                {
                    switch (e.Button)
                    {
                        case System.Windows.Forms.MouseButtons.Left:
                            _MouseDown = true;
                            _PickInfoIndex = _HoverIndex;
                            _MouseDownPoint = new Point(e.Location.X - _PickInfos[_HoverIndex].Allowance, e.Location.Y);
                            break;
                        case System.Windows.Forms.MouseButtons.Right:
                            _PickInfos.RemoveAt(_HoverIndex);
                            _HoverIndex = -1;
                            PickColor(_ScaleImage, _ConvertedImage, _PickInfos, -1);
                            splitContainer1.Invalidate(true);
                            break;
                    }
                }
                else
                {
                    switch (e.Button)
                    {
                        case System.Windows.Forms.MouseButtons.Left:
                            _MouseDown = true;
                            _MouseDownPoint = e.Location;
                            double scale = _ScaleImage.Width / (double)_ImageRect.Width;
                            int x = (int)((e.X - _ImageRect.Left) * scale);
                            int y = (int)((e.Y - _ImageRect.Top) * scale);
                            Point pot = new Point(x, y);

                            _PickInfos.Add(new PickInfo() { Allowance = 0, PickPoint = pot, ScalePoint = e.Location, Color = _ScaleImage.GetPixel(x, y) });
                            _PickInfoIndex = _PickInfos.Count - 1;

                            BuildMap(_Map, _ScaleImage, _PickInfos, _PickInfos[_PickInfoIndex]);
                            PickColor(_ScaleImage, _ConvertedImage, _Map, -1, 0);
                            splitContainer1.Invalidate(true);
                            break;
                    }
                }
            }
        }

        private void splitContainer1_Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            _MouseDown = false;
        }

        private class PickInfo
        {
            public Point PickPoint;
            public Point ScalePoint;
            public Color Color;
            public int Allowance;
        }
    }
}

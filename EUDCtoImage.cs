using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyTest
{
    public partial class EUDCtoImage : Form
    {
        private Dictionary<int, Image> _EUDCImg = new Dictionary<int, Image>();
        
        public EUDCtoImage()
        {
            InitializeComponent();
        }
        
        private void buttonExport_Click(object sender, EventArgs e)
        {
            _EUDCImg.Clear();

            Directory.CreateDirectory(@"D:\eudc\");

            Font font = new Font("標楷體", 48);
            Bitmap ft = new Bitmap(64, 64);
            Bitmap ept = new Bitmap(64, 64);


            using (Graphics g = Graphics.FromImage(ft))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;

                //0xF8FF 全部
                for (int i = 0xE000; i <= 0xE310; i++)
                {
                    string c = ((char)i).ToString();

                    g.Clear(Color.Transparent);
                    g.DrawString(c, font, Brushes.Black, -11, 3);

                    if (!IsImageEmpty(ft))
                    {
                        _EUDCImg.Add(i, new Bitmap(ft));
                        //ft.Save(@"D:\eudc\" + Convert.ToString(i, 16) + ".png", System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
            panelOutput.Invalidate();
        }

        private void panelOutput_Paint(object sender, PaintEventArgs e)
        {
            DrawStr(txtInput.Text, new Font("標楷體", 12), e.Graphics, 0, 0);
            DrawStr(txtInput.Text, new Font("標楷體", 24), e.Graphics, 0, 100);
            DrawStr(txtInput.Text, new Font("標楷體", 42), e.Graphics, 0, 200);
            DrawStr(txtInput.Text, new Font("標楷體", 60), e.Graphics, 0, 300);
        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {
            panelOutput.Invalidate();
        }

        private void DrawStr(string str, Font font, Graphics g, int x, int y)
        {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            int lei = (int)g.MeasureString("國", font).Height;
            int hei = (int)(64 * (font.Size / 48F));
            int top = y;// (lei - hei) / 2 + y;
            int left = x;
            StringBuilder strbuffer = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                int code = str[i];
                if (code >= 0xE000 && code <= 0xF848)
                {
                    if (strbuffer.Length > 0)
                    {
                        string drawPart = strbuffer.ToString();
                        int wid = (int)g.MeasureString(drawPart, font).Width;

                        g.DrawString(drawPart, font, Brushes.Black, left, y);
                        left += wid - (int)((font.Size - 6) / 4);
                        strbuffer.Clear();
                    }
                    else
                    {
                        left += (int)((font.Size - 6) / 4);
                    }

                    Image img;
                    if (_EUDCImg.TryGetValue(code, out img))
                    {
                        g.DrawImage(img, left, top, hei, hei);
                    }
                    left += hei - (int)((font.Size - 6) / 4);
                }
                else
                {
                    strbuffer.Append(str[i]);
                }
            }

            if (strbuffer.Length > 0)
            {
                string drawPart = strbuffer.ToString();
                g.DrawString(drawPart, font, Brushes.Black, left, y);
            }
        }

        private bool IsImageEmpty(Bitmap img)
        {
            int wid = img.Width;
            int hei = img.Height;

            Rectangle rtg = new Rectangle(0, 0, wid, hei);
            int nPx = wid * hei;

            bool empty = true;

            BitmapData bd = img.LockBits(rtg, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            IntPtr source_scan = bd.Scan0;
            unsafe
            {
                byte* source_p = (byte*)source_scan.ToPointer();
                for (int j = 0; j < nPx; j++)
                {
                    int a1 = source_p[0];
                    int a2 = source_p[1];
                    int a3 = source_p[2];
                    int a4 = source_p[3];

                    bool lA = source_p[3] > 0;
                    if (lA)
                    {
                        empty = false;
                        break;
                    }
                    source_p += 4;
                }
            }
            img.UnlockBits(bd);

            return empty;
        }
    }
}

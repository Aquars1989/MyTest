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

            Font fontDraw = new Font("標楷體", 48);
            Bitmap drawImage = new Bitmap(64, 64);


            using (Graphics g = Graphics.FromImage(drawImage))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;

                //0xF8FF 
                for (int i = 0xE000; i <= 0xF848; i++)
                {
                    string c = ((char)i).ToString();

                    g.Clear(Color.Transparent);
                    g.DrawString(c, fontDraw, Brushes.Black, -11, 3);

                    if (!IsImageEmpty(drawImage))
                    {
                        _EUDCImg.Add(i, new Bitmap(drawImage));
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

            int lineHeight = (int)g.MeasureString("國", font).Height;
            int imageSize = (int)(64 * (font.Size / 48F));
            int drawTop = y;// (lei - hei) / 2 + y;
            int drawLeft = x;

            StringBuilder stringBuffer = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                int code = str[i];
                if (code >= 0xE000 && code <= 0xF848)
                {
                    if (stringBuffer.Length > 0)
                    {
                        string drawString = stringBuffer.ToString();
                        int drawWidth = (int)g.MeasureString(drawString, font).Width;

                        g.DrawString(drawString, font, Brushes.Black, drawLeft, y);
                        drawLeft += drawWidth - (int)((font.Size - 6) / 4);
                        stringBuffer.Clear();
                    }
                    else
                    {
                        drawLeft += (int)((font.Size - 6) / 4);
                    }
                    
                    Image drawImage;
                    if (_EUDCImg.TryGetValue(code, out drawImage))
                    {
                        g.DrawImage(drawImage, drawLeft, drawTop, imageSize, imageSize);
                    }
                    drawLeft += imageSize - (int)((font.Size - 6) / 4);
                }
                else
                {
                    stringBuffer.Append(str[i]);
                }
            }
            
            if (stringBuffer.Length > 0)
            {
                string drawPart = stringBuffer.ToString();
                g.DrawString(drawPart, font, Brushes.Black, drawLeft, y);
            }
        }

        private bool IsImageEmpty(Bitmap img)
        {
            int imageWidth = img.Width;
            int imageHeight = img.Height;

            int px = imageWidth * imageHeight;

            bool result = true;
            BitmapData imagedata = img.LockBits(new Rectangle(0, 0, imageWidth, imageHeight), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            IntPtr source_scan = imagedata.Scan0;
            unsafe
            {
                byte* source_p = (byte*)source_scan.ToPointer();
                for (int j = 0; j < px; j++)
                {
                    if (source_p[3] > 0)
                    {
                        result = false;
                        break;
                    }
                    source_p += 4;
                }
            }
            img.UnlockBits(imagedata);
            return result;
        }
    }
}

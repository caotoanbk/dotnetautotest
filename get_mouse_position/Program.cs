using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace get_mouse_position
{
    class Program
    {
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);
        static Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
        static void Main(string[] args)
        {
            while(true){
                Point cursor = new Point();
                GetCursorPos(ref cursor);
                Console.WriteLine("{0} - {1}",cursor.X,cursor.Y);
                var c = GetColorAt(cursor);
                Console.WriteLine("R-{0} G-{1} B-{2} A-{3}",c.R,c.G,c.B,c.A);
                Thread.Sleep(500);
            }
        }

        public static Color GetColorAt(Point location)
        {
            using var gdest = Graphics.FromImage(screenPixel);
            using var gsrc = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr hSrcDC = gsrc.GetHdc();
            IntPtr hDC = gdest.GetHdc();
            int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
            gdest.ReleaseHdc();
            gsrc.ReleaseHdc();
            return screenPixel.GetPixel(0, 0);
        }
    }
}

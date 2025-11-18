using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace get_mouse_position
{
    public class ScreenBitmap
    {
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        [DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        private static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        static Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);

        // static void Main(string[] args)
        // {
        //     SetProcessDPIAware(); // tránh lệch tọa độ do DPI virtualization
        //     while (true)
        //     {
        //         Point cursor = new Point();
        //         GetCursorPos(ref cursor);
        //         var c1 = GetColorAt(cursor);
        //         var c2 = GetColorAt_BitBlt(cursor);
        //         var c3 = GetColorAt_GetPixel(cursor);
        //         Console.WriteLine($"Pos {cursor.X},{cursor.Y} CopyFromScreen R{c1.R} G{c1.G} B{c1.B} | BitBlt R{c2.R} G{c2.G} B{c2.B} | GetPixel R{c3.R} G{c3.G} B{c3.B}");
        //         Thread.Sleep(300);
        //     }
        // }

        public static Color GetColorAt(Point location)
        {
            using var bmp = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using var g = Graphics.FromImage(bmp);
            g.CopyFromScreen(location.X, location.Y, 0, 0, new Size(1, 1));
            return bmp.GetPixel(0, 0);
        }

        public static Color GetColorAt_BitBlt(Point location)
        {
            using var bmp = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using var gdest = Graphics.FromImage(bmp);
            using var gsrc = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr hSrcDC = gsrc.GetHdc();
            IntPtr hDC = gdest.GetHdc();
            int ok = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
            gdest.ReleaseHdc(hDC);
            gsrc.ReleaseHdc(hSrcDC);
            if (ok == 0) return Color.Empty; // BitBlt thất bại
            return bmp.GetPixel(0, 0);
        }

        public static Color GetColorAt_GetPixel(Point p)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, p.X, p.Y);
            ReleaseDC(IntPtr.Zero, hdc);
            // GetPixel trả về 0x00BBGGRR
            int r = (int)(pixel & 0x000000FF);
            int g = (int)((pixel & 0x0000FF00) >> 8);
            int b = (int)((pixel & 0x00FF0000) >> 16);
            return Color.FromArgb(r, g, b);
        }
    }
}

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

class Program
{
    static void Main()
    {
        var bounds = Screen.PrimaryScreen.Bounds;
        using (Bitmap bmp = new Bitmap(bounds.Width, bounds.Height))
        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            bmp.Save("screenshot.png", ImageFormat.Png);
        }

        Console.WriteLine("Screenshot saved.");
    }
}


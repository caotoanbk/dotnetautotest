using System;
using System.Threading;
using System.Threading.Tasks;
using Screen;
using Mouse;
using System.Drawing;
using PingLib;
using LoggingLibrary;
using send_ethernet;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Tesseract;
using get_mouse_position;

namespace auto_click_by_pos
{
    
    public static class PixConverter
    {
        public static Pix ToPix(Bitmap bitmap)
        {
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;
                return Pix.LoadFromMemory(ms.ToArray());
            }
        }
    }
    class Program
    {
        static Point START_BUTTON = new(1391, 234);
        static Point INTERACTIVE_BUTTON = new(1289, 131);
        static Point WUF_SEND = new(899, 729);
        static Point START_AUTO_SEND_NM = new(951, 754);
        static Point BENCH_RESET = new(279, 192);
        static Point PASS_LABEL = new(2183, 622);//0 128 0
        static Point CLR_DLT = new(85, 755);
        static Point PCAN_START_TRACE = new(1591, 89);
        static Point PCAN_STOP_TRACE = new(1654, 89);
        const bool usePcan = false;
        
        static List<string> reproducingCmds = new List<string>
        {
        };
        static void SaveFailImg()
        {
            // Folder path relative to current directory
            string folderPath = @"../SCREENSHOT";

            // Create directory if it doesn't exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Create a timestamp string safe for filenames
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string filePath = Path.Combine(folderPath, $"NG_{timestamp}.png");

            // Get screen size
            Rectangle bounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;

            // Create a bitmap with screen size
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                // Create graphics object from bitmap
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    // Copy from screen
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }

                // Save to file (PNG, JPEG, BMP supported)
                bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
            }
        }
        static string GetFailCmd(string measureStr)
        {
            string result = "";

            Point top_left = new(59, 177);
            Point bottom_right = new(574, 209);
            Point cmd_result = new(623, 191);
            Point measure_top_left = new(1014, 179);
            Point measure_bottom_right = new(1274, 207);
            // int height = bottom_right.Y - top_left.Y;
            int height = 26;
            for (int i = 0; i < 50; i++)
            {
                Color failColor = ScreenBitmap.GetColorAt(new Point(cmd_result.X, cmd_result.Y));
                if (failColor.R == 220 || failColor.G == 20 || failColor.B == 60)
                {

                    int width = bottom_right.X - top_left.X;
                    int height2 = bottom_right.Y - top_left.Y;

                    Rectangle rect = new Rectangle(top_left.X, top_left.Y, width, height2);
                    using (Bitmap screenshot = new Bitmap(rect.Width, rect.Height))
                    {
                        using (Graphics g = Graphics.FromImage(screenshot))
                        {
                            g.CopyFromScreen(rect.Location, Point.Empty, rect.Size);
                        }

                        // Optional: Save screenshot for debug
                        screenshot.Save("screenshot.png", System.Drawing.Imaging.ImageFormat.Png);

                        // OCR
                        string tessDataPath = "./tessdata";
                        using (var engine = new TesseractEngine(tessDataPath, "eng", EngineMode.Default))
                        using (var pix = PixConverter.ToPix(screenshot)) //Convert Bitmap to Pix
                        using (var page = engine.Process(pix))
                        {
                            result = page.GetText();

                            // Console.WriteLine("Text in captured screen area:");
                            // Console.WriteLine(result.Trim());
                        }

                    }
                    break;
                }

                top_left.Y += height;
                bottom_right.Y += height;
                cmd_result.Y += height;
                measure_top_left.Y += height;
                measure_bottom_right.Y += height;
            }


            return result;
        }
        static void Main(string[] args)
        {
            ScreenBitmap.SetProcessDPIAware(); // Make sure to get correct screen coordinates on high-DPI displays 
            if (args.Length > 0 && args[0] == "toan")
            {
                int testTime = 0;
                Console.WriteLine("---Auto Click Start!---");
                while (true)
                {
                    Color finishColor = ScreenBitmap.GetColorAt(new Point(PASS_LABEL.X, PASS_LABEL.Y));
                    Thread.Sleep(500);
                    while (finishColor.R != 0 || finishColor.G != 130 || finishColor.B != 0)
                    {
                        finishColor = ScreenBitmap.GetColorAt(new Point(PASS_LABEL.X, PASS_LABEL.Y));
                        Thread.Sleep(500);
                        if (finishColor.R == 222 || finishColor.G == 20 || finishColor.B == 57)
                        {
                            Console.WriteLine("FAIL DETECTED! Saving image and extracting command... then exit.");
                            SaveFailImg();
                            string measureResult = "";
                            string failCmd = GetFailCmd(measureResult);
                            Console.WriteLine(failCmd);
                            //failCmd.Trim() == "[TCU_STD] START_TOOL" ||

                            bool isFound = reproducingCmds.Any(cmd => failCmd.Trim().Contains(cmd));

                            // if (!isFound)
                            // {
                            //     break;
                            // }

                            if (usePcan)
                            {
                                MouseClick.DoMouseLeftClick(PCAN_STOP_TRACE.X, PCAN_STOP_TRACE.Y, false);
                                Thread.Sleep(2000);
                            }
                            Environment.Exit(0);
                        }

                    }

                    if (usePcan)
                    {
                        Thread.Sleep(300);
                        MouseClick.DoMouseLeftClick(PCAN_STOP_TRACE.X, PCAN_STOP_TRACE.Y, false);
                    }
                    testTime++;
                    Console.WriteLine("Test Again! " + testTime.ToString());
                    Thread.Sleep(2000);
                    MouseClick.DoMouseLeftDoubleClick(START_BUTTON.X, START_BUTTON.Y, true);
                    // Thread.Sleep(300);
                    // MouseClick.DoMouseLeftClick(START_BUTTON.X, START_BUTTON.Y, true);
                    // Thread.Sleep(300);
                    if (usePcan)
                    {
                        Thread.Sleep(300);
                        MouseClick.DoMouseLeftClick(PCAN_START_TRACE.X, PCAN_START_TRACE.Y, false);
                    }
                    Thread.Sleep(3000);
                }
            }
            else
            {
                while (true)
                {
                    Point cursor = new();
                    ScreenBitmap.GetCursorPos(ref cursor);
                    var c1 = ScreenBitmap.GetColorAt(cursor);
                    var c2 = ScreenBitmap.GetColorAt_BitBlt(cursor);
                    var c3 = ScreenBitmap.GetColorAt_GetPixel(cursor);
                    Console.WriteLine($"Pos {cursor.X},{cursor.Y} CopyFromScreen R{c1.R} G{c1.G} B{c1.B} | BitBlt R{c2.R} G{c2.G} B{c2.B} | GetPixel R{c3.R} G{c3.G} B{c3.B}");
                    Thread.Sleep(300);
                }
            }
        }
    }
}

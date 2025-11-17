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
        public class PingParam
        {
            public string address { get; set; }
            public int delay { get; set; }
        }
        public static async void PingAfterDelay(object obj)
        {
            PingParam param = obj as PingParam;
            // Wait for 15 seconds
            await Task.Delay(param.delay);
            // Execute the task
            var result = PingClass.PingHost(param.address);
            Console.WriteLine("Ping " + param.address + " " + result.ToString() + " after " + param.delay.ToString());
        }
        static Logger LogObj;
        static Point interfaceStart = new(1970, 81);
        static Point interfaceFinish = new(1754, 411);
        static Point interfaceQuit = new(2724, 80);

        static Point subClick1 = new(2045, 113);

        static Point oneStart = new(91, 47);
        static Point oneFinish = new(1754, 411);

        static Point oneQuit = new(612, 88);

        static Point twoStart = new(2971, 85);
        static Point twoFinish = new(3762, 329);

        static Point twoQuit = new(2968, 86);

        static Point startPoint;
        static Point finishPoint;
        static Point quitPoint;

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

        static string GetJudgeLabel()
        {
            string result = "TESTING";
            Point top_left = new(1503, 439);
            Point bottom_right = new(1648, 487);
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

            return result;
        }

        static string GetFailCmd(string measureStr)
        {
            string result = "";
            bool flag = false;

            Point top_left = new(59, 177);
            Point bottom_right = new(574, 209);
            Point cmd_result = new(623, 191);
            Point measure_top_left = new(1014, 179);
            Point measure_bottom_right = new(1274, 207);
            // int height = bottom_right.Y - top_left.Y;
            int height = 26;



            for (int i = 0; i < 50; i++)
            {
                Color failColor = ScreenBitmap.GetColorAt(cmd_result.X, cmd_result.Y);
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
        static async Task Main(string[] args)
        {
            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("yyMMdd");
            LogObj = new(formattedDate);
            int testTime = 0;
            int delayClick = 2500;
            if (args[0] == "ping")
            {
                PingParam parametersBam = new PingParam
                {
                    delay = 15000,
                    address = "160.48.249.98"
                };
                PingParam parametersNad = new PingParam
                {
                    delay = 15000,
                    address = "160.48.249.97"
                };
                PingParam parametersBam2 = new PingParam
                {
                    delay = 20000,
                    address = "160.48.249.98"
                };
                PingParam parametersNad2 = new PingParam
                {
                    delay = 20000,
                    address = "160.48.249.97"
                };
                PingParam parametersBam3 = new PingParam
                {
                    delay = 30000,
                    address = "160.48.249.98"
                };
                PingParam parametersNad3 = new PingParam
                {
                    delay = 30000,
                    address = "160.48.249.97"
                };
                PingParam parametersBam5 = new PingParam
                {
                    delay = 50000,
                    address = "160.48.249.98"
                };
                PingParam parametersNad5 = new PingParam
                {
                    delay = 50000,
                    address = "160.48.249.97"
                };
                PingParam parametersBam6 = new PingParam
                {
                    delay = 65000,
                    address = "160.48.249.98"
                };
                PingParam parametersNad6 = new PingParam
                {
                    delay = 65000,
                    address = "160.48.249.97"
                };
                Thread thread = new Thread(new ParameterizedThreadStart(PingAfterDelay));
                Thread thread2 = new Thread(new ParameterizedThreadStart(PingAfterDelay));
                Thread thread3 = new Thread(new ParameterizedThreadStart(PingAfterDelay));
                Thread thread4 = new Thread(new ParameterizedThreadStart(PingAfterDelay));
                Thread thread5 = new Thread(new ParameterizedThreadStart(PingAfterDelay));
                Thread thread6 = new Thread(new ParameterizedThreadStart(PingAfterDelay));
                Thread thread7 = new Thread(new ParameterizedThreadStart(PingAfterDelay));
                Thread thread8 = new Thread(new ParameterizedThreadStart(PingAfterDelay));
                Thread thread9 = new Thread(new ParameterizedThreadStart(PingAfterDelay));
                Thread thread10 = new Thread(new ParameterizedThreadStart(PingAfterDelay));

                thread.Start(parametersBam);
                thread2.Start(parametersNad);
                thread3.Start(parametersBam2);
                thread4.Start(parametersNad2);
                thread5.Start(parametersBam3);
                thread6.Start(parametersNad3);
                thread7.Start(parametersBam5);
                thread8.Start(parametersNad5);
                thread9.Start(parametersBam6);
                thread10.Start(parametersNad6);

                Thread.Sleep(20000);
                /*
                bool isConnected = await EthernetCommand.Connect("160.48.249.98", 20000);
                if (isConnected)
                {
                    Console.WriteLine("Connected to 160.48.249.98");
                    List<byte> startToolReq = new List<byte> { 0x4B, 0x55, 0x00, 0x00, 0x0C, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x5A, 0x4D, 0x7E };// start tool
                    List<byte> startToolRes = new List<byte> { 0x18, 0xE4, 0x7E };// start tool
                    bool startToolResult =  EthernetCommand.SendPacket(startToolReq,startToolRes);
                    if(startToolResult){
                        Console.WriteLine("Start Tool Success!");
                    }
                    else
                    {
                        Console.WriteLine("Start Tool Failed!");
                    }

                }
                else
                {
                    Console.WriteLine("Failed to connect to 160.48.249.98");
                }*/


                while (true) ;
            }
            if (args[0] == "1")
            {
                startPoint = oneStart;
                finishPoint = oneFinish;
                quitPoint = oneQuit;
            }
            else if (args[0] == "2")
            {
                startPoint = twoStart;
                finishPoint = twoFinish;
                quitPoint = twoQuit;
            }
            else if (args[0] == "toan")
            {
                Console.WriteLine("toan");
                Point START_BUTTON = new(869, 129);
                Point INTERACTIVE_BUTTON = new(1289, 131);
                Point WUF_SEND = new(899, 729);
                Point START_AUTO_SEND_NM = new(951, 754);
                Point BENCH_RESET = new(279, 192);
                Point PASS_LABEL = new(1364, 495);//0 128 0
                Point FAIL_LABEL = new(1364, 491);// 220 20 60
                Point CLR_DLT = new(85, 755);
                Point PCAN_START_TRACE = new(1591, 89);
                Point PCAN_STOP_TRACE = new(1654, 89);
                bool usePcan = false;
                var reproducingCmds = new List<string>
                {
                };
                string judgeLabel = GetJudgeLabel();
                Console.WriteLine("judgeLabel:" + judgeLabel);
                while (true)
                {
                    // Color finishColor = ScreenBitmap.GetColorAt(PASS_LABEL.X, PASS_LABEL.Y);
                    // Console.WriteLine(finishColor);
                    judgeLabel = GetJudgeLabel();
                    Console.WriteLine(judgeLabel);
                    Thread.Sleep(500);
                    while (judgeLabel != "PASS")
                    {
                        // Console.WriteLine($"finishColor: {finishColor.R}, {finishColor.G}, {finishColor.B}");
                        // Console.WriteLine($"PASS_POS: {PASS_LABEL.X}, {PASS_LABEL.Y}");
                        judgeLabel = GetJudgeLabel();
                        Thread.Sleep(500);
                        if (judgeLabel == "FAIL")
                        {
                            Console.WriteLine("FAIL keep power and delay wait 5S");
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
                                Thread.Sleep(2000);
                                MouseClick.DoMouseLeftClick(PCAN_STOP_TRACE.X, PCAN_STOP_TRACE.Y, false);
                                Thread.Sleep(200);
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
                        Thread.Sleep(300);
                        MouseClick.DoMouseLeftClick(PCAN_STOP_TRACE.X, PCAN_STOP_TRACE.Y, false);
                    }
                    testTime++;
                    Console.WriteLine("Test Again! " + testTime.ToString());
                    Thread.Sleep(300);
                    MouseClick.DoMouseLeftDoubleClick(START_BUTTON.X, START_BUTTON.Y, false);
                    Thread.Sleep(300);
                    MouseClick.DoMouseLeftDoubleClick(START_BUTTON.X, START_BUTTON.Y, false);
                    if (usePcan)
                    {
                        Thread.Sleep(300);
                        MouseClick.DoMouseLeftClick(PCAN_START_TRACE.X, PCAN_START_TRACE.Y, false);
                        Thread.Sleep(300);
                        MouseClick.DoMouseLeftClick(PCAN_START_TRACE.X, PCAN_START_TRACE.Y, false);
                    }
                    Thread.Sleep(3500);
                }
            }
            else if (args[0] == "test_maita")
            {
                while (true)
                {
                    Point START_BUTTON = new(72, 79);
                    Point INTERACTIVE_BUTTON = new(700, 77);
                    Point BENCH_RESET = new(149, 228);
                    Point PASS_LABEL = new(1372, 514);//0 128 0
                    Point FAIL_LABEL = new(1372, 514);// 220 20 60
                    Point CLR_DLT = new(85, 755);

                    Color finishColor = ScreenBitmap.GetColorAt(PASS_LABEL.X, PASS_LABEL.Y);
                    Thread.Sleep(500);
                    while (finishColor.R != 0 || finishColor.G != 128 || finishColor.B != 0)
                    {
                        finishColor = ScreenBitmap.GetColorAt(PASS_LABEL.X, PASS_LABEL.Y);
                        Thread.Sleep(500);
                        Color failColor = ScreenBitmap.GetColorAt(FAIL_LABEL.X, FAIL_LABEL.Y);
                        Thread.Sleep(500);
                        if (failColor.R == 220 || failColor.G == 20 || failColor.B == 60)
                        {
                            Console.WriteLine("FAIL keep power and delay wait 5S");
                            Thread.Sleep(5000);
                            MouseClick.DoMouseLeftDoubleClick(INTERACTIVE_BUTTON.X, INTERACTIVE_BUTTON.Y, true);
                            Thread.Sleep(800);
                            MouseClick.DoMouseLeftDoubleClick(INTERACTIVE_BUTTON.X, INTERACTIVE_BUTTON.Y, true);
                            Thread.Sleep(800);
                            MouseClick.DoMouseLeftDoubleClick(INTERACTIVE_BUTTON.X, INTERACTIVE_BUTTON.Y, true);
                            Thread.Sleep(800);
                            MouseClick.DoMouseLeftDoubleClick(BENCH_RESET.X, BENCH_RESET.Y, true);
                            Thread.Sleep(2000);
                            MouseClick.DoMouseLeftDoubleClick(BENCH_RESET.X, BENCH_RESET.Y, true);
                            Thread.Sleep(2000);
                            MouseClick.DoMouseLeftDoubleClick(BENCH_RESET.X, BENCH_RESET.Y, true);
                            Thread.Sleep(2000);
                            MouseClick.DoMouseLeftDoubleClick(BENCH_RESET.X, BENCH_RESET.Y, true);
                            Thread.Sleep(2000);
                            MouseClick.DoMouseLeftDoubleClick(BENCH_RESET.X, BENCH_RESET.Y, true);
                            Environment.Exit(0);
                        }

                    }
                    Thread.Sleep(500);
                    testTime++;
                    Console.WriteLine("Test Again! " + testTime.ToString());
                    Thread.Sleep(700);
                    MouseClick.DoMouseLeftDoubleClick(START_BUTTON.X, START_BUTTON.Y, false);
                    MouseClick.DoMouseLeftDoubleClick(START_BUTTON.X, START_BUTTON.Y, false);
                    Thread.Sleep(700);
                    // MouseClick.DoMouseLeftDoubleClick(CLR_DLT.X, CLR_DLT.Y, true);
                    Thread.Sleep(2000);
                }
            }
            else if (args[0] == "test_diag")
            {
                while (true)
                {
                    Point START_BUTTON = new(70, 57);
                    Point INTERACTIVE_BUTTON = new(693, 57);
                    Point STOP_BUTTON = new(216, 57);
                    Point WUF_CMD = new(200, 499);
                    Point CANM_CMD = new(210, 535);
                    Point PASS_LABEL = new(1367, 484);//0 128 0
                    Point FAIL_LABEL = new(1367, 484);// 220 20 60
                    Point CLR_DLT = new(84, 862);

                    Color finishColor = ScreenBitmap.GetColorAt(PASS_LABEL.X, PASS_LABEL.Y);
                    Thread.Sleep(500);
                    while (finishColor.R != 0 || finishColor.G != 128 || finishColor.B != 0)
                    {
                        finishColor = ScreenBitmap.GetColorAt(PASS_LABEL.X, PASS_LABEL.Y);
                        Thread.Sleep(500);
                        Color failColor = ScreenBitmap.GetColorAt(FAIL_LABEL.X, FAIL_LABEL.Y);
                        Thread.Sleep(500);
                        if (failColor.R == 220 || failColor.G == 20 || failColor.B == 60)
                        {
                            Console.WriteLine("FAIL keep power and delay wait 1000s");
                            Thread.Sleep(200);
                            MouseClick.DoMouseLeftDoubleClick(INTERACTIVE_BUTTON.X, INTERACTIVE_BUTTON.Y, true);
                            Thread.Sleep(500);
                            MouseClick.DoMouseLeftDoubleClick(INTERACTIVE_BUTTON.X, INTERACTIVE_BUTTON.Y, true);
                            Thread.Sleep(500);
                            MouseClick.DoMouseLeftDoubleClick(WUF_CMD.X, WUF_CMD.Y, true);
                            Thread.Sleep(600);
                            MouseClick.DoMouseLeftDoubleClick(CANM_CMD.X, CANM_CMD.Y, true);
                            Environment.Exit(0);
                        }

                    }
                    Thread.Sleep(500);
                    testTime++;
                    Console.WriteLine("Test Again! " + testTime.ToString());
                    Thread.Sleep(500);
                    MouseClick.DoMouseLeftDoubleClick(START_BUTTON.X, START_BUTTON.Y, true);
                    Thread.Sleep(500);
                    MouseClick.DoMouseLeftDoubleClick(CLR_DLT.X, CLR_DLT.Y, true);
                    PingParam parametersBam = new PingParam
                    {
                        delay = 15000,
                        address = "160.48.249.98"
                    };
                    PingParam parametersNad = new PingParam
                    {
                        delay = 15000,
                        address = "160.48.249.97"
                    };
                    Thread thread = new Thread(new ParameterizedThreadStart(PingAfterDelay));
                    Thread thread2 = new Thread(new ParameterizedThreadStart(PingAfterDelay));
                    thread.Start(parametersBam);
                    thread2.Start(parametersNad);
                    Thread.Sleep(2000);
                }
            }
            else if (args[0] == "aging")
            {
                while (true)
                {
                    Point POWER_ON = new(1417, 174);
                    Point POWER_OFF = new(1460, 139);
                    Point CLR_LOG = new(1430, 68);
                    Point START_TEST = new(343, 125);

                    Point PCAN_1 = new(1550, 786);
                    Point PCAN_2 = new(1550, 800);
                    Point PCAN_3 = new(1550, 817);
                    Point PCAN_4 = new(1550, 835);
                    Point PCAN_5 = new(1550, 851);

                    Point IN_01 = new(512, 92);
                    Point IN_02 = new(760, 393);
                    Point OUT_01 = new(1264, 90);

                    Color finishColor = ScreenBitmap.GetColorAt(367, 121);
                    Color finishColor2 = ScreenBitmap.GetColorAt(661, 422);
                    Thread.Sleep(500);
                    while (finishColor.R != 0 || finishColor.G != 128 || finishColor.B != 0)
                    {
                        finishColor = ScreenBitmap.GetColorAt(367, 121);
                        Thread.Sleep(500);
                    }

                    testTime++;
                    Console.WriteLine("Test Again! " + testTime.ToString());
                    // RESET SEQUENCE
                    //MouseClick.DoMouseLeftDoubleClick(CLR_LOG.X,CLR_LOG.Y,true);
                    Thread.Sleep(3000);
                    MouseClick.DoMouseLeftDoubleClick(POWER_OFF.X, POWER_OFF.Y, true);
                    Thread.Sleep(1000);
                    MouseClick.DoMouseLeftDoubleClick(PCAN_1.X, PCAN_1.Y, true);
                    Thread.Sleep(3000);
                    // TEST SEQUENCE
                    MouseClick.DoMouseLeftDoubleClick(POWER_ON.X, POWER_ON.Y, true);
                    Thread.Sleep(3000);
                    MouseClick.DoMouseLeftDoubleClick(PCAN_1.X, PCAN_1.Y, true);
                    Thread.Sleep(800);
                    MouseClick.DoMouseLeftDoubleClick(PCAN_2.X, PCAN_2.Y, true);
                    Thread.Sleep(800);
                    MouseClick.DoMouseLeftDoubleClick(PCAN_3.X, PCAN_3.Y, true);
                    Thread.Sleep(800);
                    MouseClick.DoMouseLeftDoubleClick(PCAN_4.X, PCAN_4.Y, true);
                    Thread.Sleep(800);
                    MouseClick.DoMouseLeftDoubleClick(PCAN_5.X, PCAN_5.Y, true);
                    Thread.Sleep(1000);

                    // test_01
                    MouseClick.DoMouseLeftDoubleClick(IN_01.X, IN_01.Y, true);
                    Thread.Sleep(800);
                    MouseClick.DoMouseLeftDoubleClick(START_TEST.X, START_TEST.Y, true);
                    Thread.Sleep(800);
                    MouseClick.DoMouseLeftDoubleClick(OUT_01.X, OUT_01.Y, true);
                    Thread.Sleep(800);
                    // test_02
                    MouseClick.DoMouseLeftDoubleClick(IN_02.X, IN_02.Y, true);
                    Thread.Sleep(800);
                    MouseClick.DoMouseLeftDoubleClick(START_TEST.X, START_TEST.Y, true);
                    Thread.Sleep(800);
                    MouseClick.DoMouseLeftDoubleClick(OUT_01.X, OUT_01.Y, true);
                    Thread.Sleep(800);
                    //END TEST
                }
            }
            else if (args[0] == "test_nad_crash")
            {
                while (true)
                {
                    Point START_BUTTON = new(103, 57);
                    Point INTERACTIVE_BUTTON = new(693, 57);
                    Point STOP_BUTTON = new(216, 57);
                    Point WUF_CMD = new(200, 400);
                    Point CANM_CMD = new(210, 430);
                    Point PASS_LABEL = new(1384, 498);//0 128 0
                    Point FAIL_LABEL = new(1384, 498);// 220 20 60
                    Point CLR_DLT = new(94, 703);

                    Color finishColor = ScreenBitmap.GetColorAt(PASS_LABEL.X, PASS_LABEL.Y);
                    Thread.Sleep(500);
                    while (finishColor.R != 0 || finishColor.G != 128 || finishColor.B != 0)
                    {
                        finishColor = ScreenBitmap.GetColorAt(PASS_LABEL.X, PASS_LABEL.Y);
                        Thread.Sleep(500);
                        Color failColor = ScreenBitmap.GetColorAt(FAIL_LABEL.X, FAIL_LABEL.Y);
                        Thread.Sleep(500);
                        if (failColor.R == 220 || failColor.G == 20 || failColor.B == 60)
                        {
                            Console.WriteLine("FAIL keep power and delay wait 1000s");
                            Thread.Sleep(500);
                            MouseClick.DoMouseLeftDoubleClick(INTERACTIVE_BUTTON.X, INTERACTIVE_BUTTON.Y, true);
                            Thread.Sleep(1000);
                            MouseClick.DoMouseLeftDoubleClick(WUF_CMD.X, WUF_CMD.Y, true);
                            Thread.Sleep(500);
                            MouseClick.DoMouseLeftDoubleClick(CANM_CMD.X, CANM_CMD.Y, true);
                            Thread.Sleep(1000000);
                            MouseClick.DoMouseLeftDoubleClick(STOP_BUTTON.X, STOP_BUTTON.Y, true);
                            Environment.Exit(0);
                        }

                    }
                    Thread.Sleep(500);
                    testTime++;
                    Console.WriteLine("Test Again! " + testTime.ToString());
                    Thread.Sleep(500);
                    MouseClick.DoMouseLeftDoubleClick(START_BUTTON.X, START_BUTTON.Y, true);
                    Thread.Sleep(500);
                    MouseClick.DoMouseLeftDoubleClick(CLR_DLT.X, CLR_DLT.Y, true);
                    Thread.Sleep(2000);
                }
            }
            else if (args[0] == "only_nad_check")
            {
                while (true)
                {
                    Color finishColor = ScreenBitmap.GetColorAt(1155, 407);
                    Thread.Sleep(500);
                    while (finishColor.R != 0 || finishColor.G != 128 || finishColor.B != 0)
                    {
                        finishColor = ScreenBitmap.GetColorAt(1155, 407);
                        Thread.Sleep(1000);
                    }
                    testTime++;
                    Console.WriteLine("Test Again! " + testTime.ToString());
                    Point START = new(72, 56);

                    MouseClick.DoMouseLeftDoubleClick(START.X, START.Y, true);
                    Thread.Sleep(1000);
                }
            }
            else
            {
                startPoint = interfaceStart;
                finishPoint = interfaceFinish;
                quitPoint = interfaceQuit;
            }

            while (true)
            {
                Color finishColor = ScreenBitmap.GetColorAt(finishPoint.X, finishPoint.Y);
                // remote 222 20 57
                // station 220 20 60
                // untra 222 16 57
                /*if(finishColor.R == 222 && finishColor.G == 20 && finishColor.B == 57){
                    Console.WriteLine("Error!");
                    MouseClick.DoMouseLeftDoubleClick(quitPoint.X,quitPoint.Y,true);
                    Thread.Sleep(1000);
                    MouseClick.DoMouseLeftDoubleClick(quitPoint.X,quitPoint.Y,true);
                    Thread.Sleep(1000);
                    MouseClick.DoMouseLeftDoubleClick(quitPoint.X,quitPoint.Y,true);
                    Console.WriteLine("close");
                    return;
                }*/
                // remote 0 130 0
                // station 0 128 0
                // ultra 0 132 0
                if (finishColor.R == 0 && finishColor.G == 128 && finishColor.B == 0)
                {
                    testTime++;
                    Console.WriteLine("Test Again! " + testTime.ToString());
                    Thread.Sleep(delayClick);
                    MouseClick.DoMouseLeftDoubleClick(startPoint.X, startPoint.Y, true);
                    Thread.Sleep(200);
                    MouseClick.DoMouseLeftDoubleClick(startPoint.X, startPoint.Y, true);
                    Thread.Sleep(500);
                }
            }
        }
    }
}

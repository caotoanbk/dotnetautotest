using System;
using System.Threading;
using System.Threading.Tasks;
using Screen;
using Mouse;
using System.Drawing;
using PingLib;
using LoggingLibrary;
using send_ethernet;

namespace auto_click_by_pos
{
    class Program
    {
        public class PingParam{
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
            Console.WriteLine("Ping " + param.address + " "+ result.ToString());
        }
        static Logger LogObj;
        static Point interfaceStart = new(1970,81);
        static Point interfaceFinish = new(1754,411);
        static Point interfaceQuit = new(2724,80);

        static Point subClick1 = new(2045,113);

        static Point oneStart = new(91,47);
        static Point oneFinish =  new(1754,411);

        static Point oneQuit = new(612,88);

        static Point twoStart = new(2971,85);
        static Point twoFinish = new(3762,329);

        static Point twoQuit = new(2968,86);

        static Point startPoint;
        static Point finishPoint;
        static Point quitPoint;
        static async Task Main(string[] args)
        {
            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("yyMMdd");
            LogObj = new(formattedDate);
            int testTime = 0;
            int delayClick = 2500;
            if(args[0]=="ping"){
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
                Thread.Sleep(20000);
                
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
                }
                

                while(true);
            }
            if(args[0]=="1"){
                startPoint = oneStart;
                finishPoint = oneFinish;
                quitPoint = oneQuit;
            }
            else if(args[0]=="2"){
                startPoint = twoStart;
                finishPoint = twoFinish;
                quitPoint = twoQuit;
            }
            else if(args[0]=="test_diag"){
                while(true){
                    Point START_BUTTON = new(70,57);
                    Point INTERACTIVE_BUTTON = new(693,57);
                    Point STOP_BUTTON = new(216,57);
                    Point WUF_CMD = new(200,499);
                    Point CANM_CMD = new(210,535);
                    Point PASS_LABEL = new(1734,399);//0 128 0
                    Point FAIL_LABEL = new(1712,414);// 220 20 60
                    Point CLR_DLT = new(84,862);

                    Color finishColor = ScreenBitmap.GetColorAt(PASS_LABEL.X,PASS_LABEL.Y);
                    Thread.Sleep(500);
                    while(finishColor.R != 0 || finishColor.G != 128 || finishColor.B != 0 ){
                        finishColor = ScreenBitmap.GetColorAt(PASS_LABEL.X,PASS_LABEL.Y);
                        Thread.Sleep(500);
                        Color failColor = ScreenBitmap.GetColorAt(FAIL_LABEL.X,FAIL_LABEL.Y);
                        Thread.Sleep(500);
                        if(failColor.R == 220 || failColor.G == 20 || failColor.B == 60){
                            Console.WriteLine("FAIL keep power and delay wait 1000s");
                            Thread.Sleep(200);
                            MouseClick.DoMouseLeftDoubleClick(INTERACTIVE_BUTTON.X,INTERACTIVE_BUTTON.Y,true);
                            Thread.Sleep(500);
                            MouseClick.DoMouseLeftDoubleClick(WUF_CMD.X,WUF_CMD.Y,true);
                            Thread.Sleep(600);
                            MouseClick.DoMouseLeftDoubleClick(CANM_CMD.X,CANM_CMD.Y,true);
                            Environment.Exit(0);
                        }

                    }
                    Thread.Sleep(500);
                    testTime++;
                    Console.WriteLine("Test Again! " + testTime.ToString());
                    Thread.Sleep(500);
                    MouseClick.DoMouseLeftDoubleClick(START_BUTTON.X,START_BUTTON.Y,true);
                    Thread.Sleep(500);
                    MouseClick.DoMouseLeftDoubleClick(CLR_DLT.X,CLR_DLT.Y,true);
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
            else if(args[0]=="aging"){
                while(true){
                    Point POWER_ON = new(1417,174);
                    Point POWER_OFF = new(1460,139);
                    Point CLR_LOG = new(1430,68);
                    Point START_TEST = new(343,125);
                    
                    Point PCAN_1 = new(1550,786);
                    Point PCAN_2 = new(1550,800);
                    Point PCAN_3 = new(1550,817);
                    Point PCAN_4 = new(1550,835);
                    Point PCAN_5 = new(1550,851);
                    
                    Point IN_01 = new(512,92);
                    Point IN_02 = new(760,393);
                    Point OUT_01 = new(1264,90);

                    Color finishColor = ScreenBitmap.GetColorAt(367,121);
                    Color finishColor2 = ScreenBitmap.GetColorAt(661,422);
                    Thread.Sleep(500);
                    while(finishColor.R != 0 || finishColor.G != 128 || finishColor.B != 0 ){
                        finishColor = ScreenBitmap.GetColorAt(367,121);
                        Thread.Sleep(500);
                    }
                    
                    testTime++;
                    Console.WriteLine("Test Again! " + testTime.ToString());
                    // RESET SEQUENCE
                    //MouseClick.DoMouseLeftDoubleClick(CLR_LOG.X,CLR_LOG.Y,true);
                    Thread.Sleep(3000);
                    MouseClick.DoMouseLeftDoubleClick(POWER_OFF.X,POWER_OFF.Y,true);
                    Thread.Sleep(1000);
                    MouseClick.DoMouseLeftDoubleClick(PCAN_1.X,PCAN_1.Y,true);
                    Thread.Sleep(3000);
                    // TEST SEQUENCE
                    MouseClick.DoMouseLeftDoubleClick(POWER_ON.X,POWER_ON.Y,true);
                    Thread.Sleep(3000);
                    MouseClick.DoMouseLeftDoubleClick(PCAN_1.X,PCAN_1.Y,true);
                    Thread.Sleep(800);
                    MouseClick.DoMouseLeftDoubleClick(PCAN_2.X,PCAN_2.Y,true);
                    Thread.Sleep(800);
                    MouseClick.DoMouseLeftDoubleClick(PCAN_3.X,PCAN_3.Y,true);
                    Thread.Sleep(800);
                    MouseClick.DoMouseLeftDoubleClick(PCAN_4.X,PCAN_4.Y,true);
                    Thread.Sleep(800);
                    MouseClick.DoMouseLeftDoubleClick(PCAN_5.X,PCAN_5.Y,true);
                    Thread.Sleep(1000);
                    
                    // test_01
                    MouseClick.DoMouseLeftDoubleClick(IN_01.X,IN_01.Y,true);
                    Thread.Sleep(800);
                    MouseClick.DoMouseLeftDoubleClick(START_TEST.X,START_TEST.Y,true);
                    Thread.Sleep(800);
                    MouseClick.DoMouseLeftDoubleClick(OUT_01.X,OUT_01.Y,true);
                    Thread.Sleep(800);
                    // test_02
                    MouseClick.DoMouseLeftDoubleClick(IN_02.X,IN_02.Y,true);
                    Thread.Sleep(800);
                    MouseClick.DoMouseLeftDoubleClick(START_TEST.X,START_TEST.Y,true);
                    Thread.Sleep(800);
                    MouseClick.DoMouseLeftDoubleClick(OUT_01.X,OUT_01.Y,true); 
                    Thread.Sleep(800);
                    //END TEST
                }
            }
            else if(args[0]=="test_nad_crash"){
                while(true){
                    Point START_BUTTON = new(103,57);
                    Point INTERACTIVE_BUTTON = new(693,57);
                    Point STOP_BUTTON = new(216,57);
                    Point WUF_CMD = new(200,400);
                    Point CANM_CMD = new(210,430);
                    Point PASS_LABEL = new(1384,498);//0 128 0
                    Point FAIL_LABEL = new(1384,498);// 220 20 60
                    Point CLR_DLT = new(94,703);

                    Color finishColor = ScreenBitmap.GetColorAt(PASS_LABEL.X,PASS_LABEL.Y);
                    Thread.Sleep(500);
                    while(finishColor.R != 0 || finishColor.G != 128 || finishColor.B != 0 ){
                        finishColor = ScreenBitmap.GetColorAt(PASS_LABEL.X,PASS_LABEL.Y);
                        Thread.Sleep(500);
                        Color failColor = ScreenBitmap.GetColorAt(FAIL_LABEL.X,FAIL_LABEL.Y);
                        Thread.Sleep(500);
                        if(failColor.R == 220 || failColor.G == 20 || failColor.B == 60){
                            Console.WriteLine("FAIL keep power and delay wait 1000s");
                            Thread.Sleep(500);
                            MouseClick.DoMouseLeftDoubleClick(INTERACTIVE_BUTTON.X,INTERACTIVE_BUTTON.Y,true);
                            Thread.Sleep(1000);
                            MouseClick.DoMouseLeftDoubleClick(WUF_CMD.X,WUF_CMD.Y,true);
                            Thread.Sleep(500);
                            MouseClick.DoMouseLeftDoubleClick(CANM_CMD.X,CANM_CMD.Y,true);
                            Thread.Sleep(1000000);
                            MouseClick.DoMouseLeftDoubleClick(STOP_BUTTON.X,STOP_BUTTON.Y,true);
                            Environment.Exit(0);
                        }

                    }
                    Thread.Sleep(500);
                    testTime++;
                    Console.WriteLine("Test Again! " + testTime.ToString());
                    Thread.Sleep(500);
                    MouseClick.DoMouseLeftDoubleClick(START_BUTTON.X,START_BUTTON.Y,true);
                    Thread.Sleep(500);
                    MouseClick.DoMouseLeftDoubleClick(CLR_DLT.X,CLR_DLT.Y,true);
                    Thread.Sleep(2000);
                }
            }
            else if(args[0]=="only_nad_check"){
                while(true){
                    Color finishColor = ScreenBitmap.GetColorAt(1155,407);
                    Thread.Sleep(500);
                    while(finishColor.R != 0 || finishColor.G != 128 || finishColor.B != 0){
                        finishColor = ScreenBitmap.GetColorAt(1155,407);
                        Thread.Sleep(1000);
                    }
                    testTime++;
                    Console.WriteLine("Test Again! " + testTime.ToString());
                    Point START = new(72,56);
                    
                    MouseClick.DoMouseLeftDoubleClick(START.X,START.Y,true);
                    Thread.Sleep(1000);
                }
            }
            else{
                startPoint = interfaceStart;
                finishPoint = interfaceFinish;
                quitPoint = interfaceQuit;
            }
            
            while(true){
                Color finishColor = ScreenBitmap.GetColorAt(finishPoint.X,finishPoint.Y);
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
                if(finishColor.R == 0 && finishColor.G == 128 && finishColor.B == 0){
                    testTime++;
                    Console.WriteLine("Test Again! " + testTime.ToString());
                    Thread.Sleep(delayClick);
                    MouseClick.DoMouseLeftDoubleClick(startPoint.X,startPoint.Y,true);
                    Thread.Sleep(200);
                    MouseClick.DoMouseLeftDoubleClick(startPoint.X,startPoint.Y,true);
                    Thread.Sleep(500);
                }
            }
        }
    }
}

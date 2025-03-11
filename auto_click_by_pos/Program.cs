using System;
using System.Threading;
using Screen;
using Mouse;
using System.Drawing;

namespace auto_click_by_pos
{
    

    class Program
    {
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
        static void Main(string[] args)
        {
            int testTime = 0;
            int delayClick = 2500;
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
            else if(args[0]=="aging"){
                while(true){
                    testTime++;
                    Console.WriteLine("Test Again! " + testTime.ToString());
                    Point POWER_ON = new(1417,174);
                    Point POWER_OFF = new(1460,139);
                    Point CLR_LOG = new(1430,68);
                    Point START_TEST = new(343,125);
                    
                    Point PCAN_1 = new(1550,886);
                    Point PCAN_2 = new(1550,900);
                    Point PCAN_3 = new(1550,917);
                    Point PCAN_4 = new(1550,935);
                    Point PCAN_5 = new(1550,951);
                    // RESET SEQUENCE
                    //MouseClick.DoMouseLeftDoubleClick(CLR_LOG.X,CLR_LOG.Y,true);
                    Thread.Sleep(500);
                    MouseClick.DoMouseLeftDoubleClick(POWER_OFF.X,POWER_OFF.Y,true);
                    Thread.Sleep(400);
                    MouseClick.DoMouseLeftDoubleClick(PCAN_1.X,PCAN_1.Y,true);
                    Thread.Sleep(1000);
                    // TEST SEQUENCE
                    MouseClick.DoMouseLeftDoubleClick(POWER_ON.X,POWER_ON.Y,true);
                    Thread.Sleep(400);
                    MouseClick.DoMouseLeftDoubleClick(START_TEST.X,START_TEST.Y,true);
                    Thread.Sleep(6000);
                    MouseClick.DoMouseLeftDoubleClick(PCAN_1.X,PCAN_1.Y,true);
                    Thread.Sleep(500);
                    MouseClick.DoMouseLeftDoubleClick(PCAN_2.X,PCAN_2.Y,true);
                    Thread.Sleep(500);
                    MouseClick.DoMouseLeftDoubleClick(PCAN_3.X,PCAN_3.Y,true);
                    Thread.Sleep(500);
                    MouseClick.DoMouseLeftDoubleClick(PCAN_4.X,PCAN_4.Y,true);
                    Thread.Sleep(500);
                    MouseClick.DoMouseLeftDoubleClick(PCAN_5.X,PCAN_5.Y,true);
                    Thread.Sleep(140000);
                    //END TEST
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

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;

namespace Mouse
{
    public static class MouseClick
    {
        [DllImport("user32.dll")]
        public static extern long SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("user32.dll",CharSet=CharSet.Auto, CallingConvention=CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, uint cButtons, uint dwExtraInfo);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;

        private const int MOUSEEVENTF_MOVE = 0x01;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        public static void DoMouseLeftClick(int x,int y,bool returnCursor)
        {
            Point currentPoint = new();
            GetCursorPos(ref currentPoint);
            SetCursorPos(x,y);
            mouse_event(MOUSEEVENTF_LEFTDOWN , 0, 0, 0, 0);
            Thread.Sleep(50);
            mouse_event( MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            Thread.Sleep(50);
            if(returnCursor)SetCursorPos(currentPoint.X,currentPoint.Y);
        }
        public static void DoMouseLeftDoubleClick(int x,int y,bool returnCursor)
        {
            Point currentPoint = new();
            GetCursorPos(ref currentPoint);
            SetCursorPos(x,y);
            mouse_event(MOUSEEVENTF_LEFTDOWN , 0, 0, 0, 0);
            Thread.Sleep(50);
            mouse_event( MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            Thread.Sleep(50);
            if(returnCursor)SetCursorPos(currentPoint.X,currentPoint.Y);
        }
        public static void DoMouseRightClick(int x,int y,bool returnCursor)
        {
            Point currentPoint = new();
            GetCursorPos(ref currentPoint);
            SetCursorPos(x,y);
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
            SetCursorPos(currentPoint.X,currentPoint.Y);
            if(returnCursor)SetCursorPos(currentPoint.X,currentPoint.Y);
        }

    }
    
}

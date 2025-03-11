using System;

namespace lampda
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(func1(1));
        }
        static int func1(int x) => x+10;
    }
}

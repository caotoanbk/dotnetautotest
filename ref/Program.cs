using System;

var b = new a(1,1);
var(c,d) = b;
Console.WriteLine(c.ToString()+d.ToString());
class a{
    int W;
    int H;

    public a(int w,int h) => (W,H)=(w,h);
    public void Deconstruct(out int w,out int h) => (w,h)=(W,H);
}
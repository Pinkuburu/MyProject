using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace 多线程测试
{
    class Program
    {
        static void Main(string[] args)
        {
            My m = new My();
            m.x = 2;
            m.y = 3;

            Thread t = new Thread(new ThreadStart(m.C));
            t.Start();



            Console.Read();  
        }
    }

    class My
    {
        public int x, y;

        public void C()
        {
            Console.WriteLine("x={0},y={1}", this.x, this.y);
        }
    }  
}

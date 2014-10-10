using System;
using System.Threading;

namespace Thread_Test
{
    class Program
    {
        private event EventHandler OnLearn;
        static void Main(string[] args)
        {
            Program p = new Program();
            Thread td_a = new Thread(new ParameterizedThreadStart(p.NoParameter));
            td_a.Name = "Thread A";
            td_a.Start(50);
            Console.ReadKey();
        }

        public void NoParameter(object ms)
        {
            int interval = Convert.ToInt32(ms);
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(Thread.CurrentThread.Name + "系统当前时间毫秒值：" + DateTime.Now.Millisecond.ToString());
                Thread.Sleep(interval);//让线程暂停 
            }
            OnLearn += new EventHandler(Program_OnLearn);
        }

        void Program_OnLearn(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}

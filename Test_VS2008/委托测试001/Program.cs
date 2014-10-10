using System;
using System.Collections.Generic;
using System.Text;

namespace 委托测试001
{
    // 热水器
    public class Heater
    {
        private int temperature;
        public delegate void BoilHandler(int param);   //声明委托
        public event BoilHandler BoilEvent;        //声明事件

        // 烧水
        public void BoilWater()
        {
            for (int i = 0; i <= 100; i++)
            {
                temperature = i;

                if (temperature > 95)
                {
                    if (BoilEvent != null)
                    { //如果有对象注册
                        BoilEvent(temperature);  //调用所有注册对象的方法
                    }
                }
            }
        }
    }

    // 警报器
    public class Alarm
    {
        public void MakeAlert(int param)
        {
            Console.WriteLine("Alarm：嘀嘀嘀，水已经 {0} 度了：", param);
        }
    }

    // 显示器
    public class Display
    {
        public static void ShowMsg(int param)
        { //静态方法
            Console.WriteLine("Display：水快烧开了，当前温度：{0}度。", param);
        }
    }

    class Program
    {
        static void Main()
        {
            Heater heater = new Heater();
            Alarm alarm = new Alarm();

            heater.BoilEvent += alarm.MakeAlert;    //注册方法
            heater.BoilEvent += (new Alarm()).MakeAlert;   //给匿名对象注册方法
            heater.BoilEvent += Display.ShowMsg;       //注册静态方法

            heater.BoilWater();   //烧水，会自动调用注册过对象的方法
            Console.ReadKey();
        }
    }
}

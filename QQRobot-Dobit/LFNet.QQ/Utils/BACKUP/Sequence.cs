using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Utils
{
   public class Sequence
    {
       private int _sequence;
       public Sequence()
       {
           _sequence =new Random().Next();
       }
       
       public byte[] CurrentSequence()
       {
           char c =(char) _sequence;
           return System.Text.Encoding.Default.GetBytes(c.ToString());
       }

       public byte[] GetSequence()
       {
           return NextSequence();
       }
       /// <summary>
       /// 
       /// </summary>
       /// <returns></returns>
       public byte[] NewSequence()
       {
           _sequence = new Random().Next();
           return CurrentSequence();
       }

       public byte[] NextSequence()
       {
           _sequence++;
           return CurrentSequence();
       }

       public override string ToString()
       {
           return _sequence.ToString();
       }
    }
}

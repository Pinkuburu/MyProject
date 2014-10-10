using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Events
{
   public class LoginStatusChangedEventArgs<T>:EventArgs where T:LoginStatus
    {
       public LoginStatus LoginStatus;
       public LoginStatusChangedEventArgs(LoginStatus loginStatus)
           : base()
       {
           this.LoginStatus = loginStatus; 
       }

    }
}

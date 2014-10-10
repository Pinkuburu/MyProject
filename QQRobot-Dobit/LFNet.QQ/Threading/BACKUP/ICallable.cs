

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
namespace LFNet.QQ.Threading
{
    /// <summary>
    /// 可以被提交到线程池异步处理的接口
    /// </summary>
    public interface ICallable
    {
        /// <summary>
        /// 是否已经在运行
    /// </summary>
        /// <value></value>
        bool IsRunning { get; }
        /// <summary>
        /// WaitCallback回调
    /// </summary>
        /// <param name="state">The state.</param>
        void Call(object state);
    }
    /// <summary>
    /// 可以被提交到线程池定时运行的接口
    /// </summary>
    public interface IRunable : IDisposable
    {
        /// <summary>是否已经在运行
    /// </summary>
        /// <value></value>
        bool IsRunning { get; }
        /// <summary>
    /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="timedOut">if set to <c>true</c> [timed out].</param>
        void Run(object state, bool timedOut);
        /// <summary>
        /// 注册在线程池后的信号变量
    /// </summary>
        /// <value></value>
        WaitHandle WaitHandler { get; set; }
        /// <summary>
        /// 注册后的对象
    /// </summary>
        /// <value></value>
        RegisteredWaitHandle RegisterdHandler { get; set; }
    }
}

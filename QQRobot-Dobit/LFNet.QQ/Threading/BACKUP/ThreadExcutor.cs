
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LFNet.QQ.Threading
{
    /// <summary>
    /// 利用线程池来异步执行线程
    /// </summary>
    public class ThreadExcutor
    {
        /// <summary>提交一个线程等待执行
    /// </summary>
        /// <param name="callable">The callable.</param>
        /// <param name="state">The state.</param>
        public static void Submit(ICallable callable, object state)
        {
            if (!callable.IsRunning)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(callable.Call), state);
            }
        }
        /// <summary>
        /// 注册一个轮循线程
    /// </summary>
        /// <param name="runnable">The runnable.</param>
        /// <param name="state">The state.</param>
        /// <param name="interval">The interval.</param>
        public static void RegisterIntervalObject(IRunable runnable, object state, long interval, bool onlyOnce)
        {
            // if (runnable.RegisterdHandler == null)
            // {
            runnable.WaitHandler = new AutoResetEvent(false);
            runnable.RegisterdHandler = ThreadPool.RegisterWaitForSingleObject(runnable.WaitHandler, new WaitOrTimerCallback(runnable.Run), state, interval, onlyOnce);
            // }
        }

        /// <summary>注销轮循线程
        /// Uns the register intervalu object.
        /// </summary>
        /// <param name="runnable">The runnable.</param>
        public static void UnRegisterIntervaluObject(IRunable runnable)
        {
            if (runnable.RegisterdHandler != null && runnable.WaitHandler != null)
            {
                runnable.RegisterdHandler.Unregister(runnable.WaitHandler);
            }
        }
    }
}


using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Threading
{
    /// <summary>
    /// 保持登录状态触发器
    /// </summary>
    public class KeepAliveTrigger : IRunable
    {
        private QQClient client;
        public KeepAliveTrigger(QQClient client)
        {
            this.client = client;
            ThreadExcutor.RegisterIntervalObject(this, this, QQGlobal.QQ_INTERVAL_KEEP_ALIVE, false);
        }
        #region IRunable Members

        /// <summary>
    /// </summary>
        /// <value></value>
        public bool IsRunning
        {
            get;
            private set;
        }

        /// <summary>
    /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="timedOut">if set to <c>true</c> [timed out].</param>
        public void Run(object state, bool timedOut)
        {
            if (!IsRunning)
            {
                lock (this)
                {
                    if (!IsRunning)
                    {
                        IsRunning = true;
                        this.client.KeepAlive();
                        IsRunning = false;
                    }
                }
            }

        }

        /// <summary>
    /// </summary>
        /// <value></value>
        public System.Threading.WaitHandle WaitHandler
        {
            get;
            set;
        }

        /// <summary>
    /// </summary>
        /// <value></value>
        public System.Threading.RegisteredWaitHandle RegisterdHandler
        {
            get;
            set;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.WaitHandler != null && this.RegisterdHandler != null)
            {
                RegisterdHandler.Unregister(this.WaitHandler);
            }
        }

        #endregion
    }
}

namespace _36JI_
{
    using System;
    using System.Threading;

    internal class GThread
    {
        private bool m_bExitThread = false;
        private ManualResetEvent m_ExitEvent = new ManualResetEvent(false);
        private Thread m_Thread;
        private ManualResetEvent m_WaitEvent = new ManualResetEvent(false);
        private Mutex m_WorkMutex = new Mutex();
        protected EWorkType m_WorkType;

        protected void DoWork(EWorkType eWork)
        {
            this.m_WorkMutex.WaitOne();
            this.m_WorkType = eWork;
            this.m_WaitEvent.Set();
            this.m_WorkMutex.ReleaseMutex();
            Thread.Sleep(100);
        }

        public void Exit()
        {
            if (this.m_Thread.IsAlive)
            {
                this.m_WorkMutex.WaitOne();
                this.m_bExitThread = true;
                this.m_WaitEvent.Set();
                this.m_WorkMutex.ReleaseMutex();
            }
            if (!this.m_ExitEvent.WaitOne(0x3e8, false))
            {
                this.m_Thread.Abort();
            }
            this.m_ExitEvent.Close();
            this.m_WaitEvent.Close();
            this.m_WorkMutex.Close();
        }

        private void MyThreadProc()
        {
            while (true)
            {
                this.m_WaitEvent.WaitOne();
                if (this.m_bExitThread)
                {
                    break;
                }
                this.m_WorkMutex.WaitOne();
                this.Working();
                this.m_WaitEvent.Reset();
                this.m_WorkMutex.ReleaseMutex();
            }
            this.m_ExitEvent.Set();
        }

        protected void RunThread()
        {
            this.m_Thread = new Thread(new ThreadStart(this.MyThreadProc));
            this.m_Thread.Start();
        }

        public void WaitWork()
        {
            this.m_WorkMutex.WaitOne();
            this.m_WorkMutex.ReleaseMutex();
        }

        protected virtual void Working()
        {
        }

        protected enum EWorkType
        {
            EIdle,
            ELogin,
            EUpdateNationAndCity,
            EUpdateCityInfo
        }
    }
}


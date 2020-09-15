using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TikTokMonitor
{

    class TikTok
    {
        public void tick(bool running)
        {
            lock (this)
            {
                if (!running)
                {
                    Monitor.Pulse(this);
                    return;
                }
                Console.Write("Tick ");
                Monitor.Pulse(this);
                Monitor.Wait(this);
            }
        }
        public void tock(bool running)
        {
            lock (this)
            {
                if (!running)
                {
                    Monitor.Pulse(this);
                    return;
                }
                Console.WriteLine("Tock");
                Monitor.Pulse(this);
                Monitor.Wait(this);
            }
        }
    }
    class MyThread
    {
        public Thread thread;
        TikTok TikTok;
        public MyThread(string name, TikTok tt)
        {
            thread = new Thread(this.run);
            TikTok = tt;
            thread.Name = name;
            thread.Start();
        }
        void run()
        {
            if (thread.Name == "Tick")
            {
                for (int i = 0; i < 5; i++)
                    TikTok.tick(true);
                TikTok.tick(false);

            }
            else
            {
                for (int i = 0; i < 5; i++)
                    TikTok.tock(true);
                TikTok.tock(false);
            }
        }
    }

  
      
    
    class Program
    { 
        private const int threads = 3;
        private const int workitems = 20;
        private static Object locker = new object();
        static void Worker()
        {
            while (true)
            {
                lock (locker)
                {
                    Monitor.Wait(locker);
                }
                Console.WriteLine("{0} access the website c-sharpcorner", Thread.CurrentThread.Name);
                Thread.Sleep(100);
            }
        }
        static void Main(string[] args)
        {
            //TikTok tt = new TikTok();
            //MyThread mt1 = new MyThread("Tick", tt);
            //MyThread mt2 = new MyThread("Tock", tt);
            //mt1.thread.Join();
            //mt2.thread.Join();
            //Console.WriteLine("Clock  Stopped");
            //Console.Read();

            Thread[] t = new Thread[threads];
            for (int k = 0; k < threads; k++)
            {
                t[k] = new Thread(new ThreadStart(Worker));
                t[k].Name = "user " + k;
                t[k].IsBackground = true;
                t[k].Start();
            }
            for (int i = 0; i < workitems; i++)
            {
                Thread.Sleep(1000);
                lock (locker)
                {
                    Monitor.Pulse(locker);
                }
            }
            Console.Read();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using PullToScxtpt.Model;

namespace PullToScxtpt
{
    public partial class PullInfoService : ServiceBase
    {
        private static object LockObject = new Object();
        // 检查更新锁
        private static int CheckUpDateLock = 0;
        TaskTimer timer1;  //计时器
        TaskTimer timer2;  //计时器
        TaskTimer timer3;  //计时器
        Sender sender = new Sender();
        public PullInfoService()
        {

       
            InitializeComponent();
        }

        protected override void OnStart(string[] args)

        {
            try
            {
                Thread.Sleep(1000 * 10);

                Sender sender = new Sender();
                //    sender.InserPersonInfo();
            
                timer1 = new TaskTimer(sender);
                timer1.Interval = 72;  //设置计时器事件间隔执行时间 2小时
                timer1.Elapsed += new System.Timers.ElapsedEventHandler(TMStart1_Elapsed);
                timer1.Enabled = true;

                timer2 = new TaskTimer(sender);
                timer2.Interval = 72;  //设置计时器事件间隔执行时间
                timer2.Elapsed += new System.Timers.ElapsedEventHandler(TMStart2_Elapsed);
                timer2.Enabled = true;
              
                //timer3 = new TaskTimer(sender);
                //timer3.Interval = 72;  //设置计时器事件间隔执行时间
                //timer3.Elapsed += new System.Timers.ElapsedEventHandler(TMStart3_Elapsed);
                //timer3.Enabled = true;

            }
            catch (Exception e)
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter("C:\\log.txt", true))

                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + e.Message+e.StackTrace);
                }
            }
         
        }



        protected override void OnPause()
        {
            //服务暂停执行代码
            base.OnPause();
        }
        protected override void OnContinue()
        {
            //服务恢复执行代码
            base.OnContinue();
        }
        protected override void OnShutdown()
        {
            //系统即将关闭执行代码
            base.OnShutdown();
        }

        private void TMStart1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
           
            //执行SQL语句或其他操作
            // 加锁检查更新锁
            lock (LockObject)
            {
                if (CheckUpDateLock == 0)CheckUpDateLock = 1;
                else return;
            }
            TaskTimer timer = (TaskTimer)sender;
            timer.sender.InserCompanyInfo();
            //More code goes here.
            //具体实现功能的方法
            // 解锁更新检查锁
            lock (LockObject)
            {
                CheckUpDateLock = 0;
            }
          
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("C:\\" + 1 + "log.txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "Start.");
            }
        }

        private void TMStart2_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            //执行SQL语句或其他操作
            //执行SQL语句或其他操作
            // 加锁检查更新锁
            lock (LockObject)
            {
                if (CheckUpDateLock == 0) CheckUpDateLock = 1;
                else return;
            }
            TaskTimer timer = (TaskTimer)sender;
            timer.sender.InserPersonResume();
            //More code goes here.
            //具体实现功能的方法
            // 解锁更新检查锁
            lock (LockObject)
            {
                CheckUpDateLock = 0;
            }
           
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("C:\\" + 2 + "log.txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "Start.");
            }
        }
        private void TMStart3_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            //执行SQL语句或其他操作
            lock (LockObject)
            {
                if (CheckUpDateLock == 0) CheckUpDateLock = 1;
                else return;
            }
            TaskTimer timer = (TaskTimer)sender;
           // timer.sender.InserPersonInfo();
            //More code goes here.
            //具体实现功能的方法
            // 解锁更新检查锁
            lock (LockObject)
            {
                CheckUpDateLock = 0;
            }
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("C:\\" + 3 + "log.txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "Start.");
            }
        }

    }
}

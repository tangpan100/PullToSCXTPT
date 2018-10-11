using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace PullToScxtpt
{
    public partial class PullInfoService : ServiceBase
    {
        System.Timers.Timer timer1;  //计时器
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
              //  sender.InserCompanyInfo();
                sender.InserPersonInfo();
                //timer1 = new System.Timers.Timer();
                //timer1.Interval = 7200000;  //设置计时器事件间隔执行时间 2小时
                //timer1.Elapsed += new System.Timers.ElapsedEventHandler(TMStart1_Elapsed);
                //timer1.Enabled = true;
          
                //using (System.IO.StreamWriter sw = new System.IO.StreamWriter("C:\\log.txt", true))

                //{

                //    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "Start.");

                //}


            }
            catch (Exception e)
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter("C:\\log.txt", true))

                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + e.Message);
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
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("C:\\" + 1 + "log.txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "Start.");
            }
        }
    }
}

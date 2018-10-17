using System;
using System.Threading;
using PullToScxtpt_px.Helper;
using PullToScxtpt_px.Model;
using Topshelf;

namespace PullToScxtpt_px
{
    partial class PullInfoService : ServiceControl
    {

        private static object LockObject = new Object();
        // 检查更新锁
        private static int CheckUpDateLock = 0;
    
        private static System.Threading.Timer timer1 = null;  //计时器
        private static System.Threading.Timer timer2;  //计时器
        private static System.Threading.Timer timer3;  //计时器
        private int dueTime = 1000*2;//(单位毫秒)
        private int period = 1000 * 60 * 60*2;//(单位毫秒)
        private static Sender sender = new Sender();

        public bool Start(HostControl hostControl)
        {
            try
            {

                //sender.InserPersonInfo();
                //sender.InserCompanyInfo();
                //sender.InserPersonResume();
                timer1 = new System.Threading.Timer(TMStart1_Elapsed, null, Timeout.Infinite, Timeout.Infinite);
                timer1.Change(dueTime, period);
                timer2 = new System.Threading.Timer(TMStart2_Elapsed, null, Timeout.Infinite, Timeout.Infinite);
                timer2.Change(dueTime, period);
                timer3 = new System.Threading.Timer(TMStart3_Elapsed, null, Timeout.Infinite, Timeout.Infinite);
                timer3.Change(dueTime, period);
            }
            catch (Exception ex)
            {
                LogHelper.GetLog(this).Info(string.Format("DATE： {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))+"异常："+ex.Message);
            }
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            return true;
        }

        private void TMStart1_Elapsed(object state)
        {
            // 加锁检查更新锁
            //lock (LockObject)
            //{
            //    if (CheckUpDateLock == 0) CheckUpDateLock = 1;
            //    else return;
            //}




            try
            {
                sender.InserCompanyInfo();
            }
            catch (Exception ex)
            {

                LogHelper.GetLog(this).Error(string.Format("DATE： {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) + "异常：插入公司信息" + ex.Message);
            }
            //// 解锁更新检查锁
            //lock (LockObject)
            //{
            //    CheckUpDateLock = 0;
            //}


        }

        private void TMStart2_Elapsed(object state)
        {
            //// 加锁检查更新锁
            //lock (LockObject)
            //{
            //    if (CheckUpDateLock == 0) CheckUpDateLock = 1;
            //    else return;
            //}


            try
            {
                sender.InserPersonInfo();

            }
            catch (Exception ex)
            {

                LogHelper.GetLog(this).Error(string.Format("DATE： {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) + "异常：插入个人信息" + ex.Message);
            }


            //// 解锁更新检查锁
            //lock (LockObject)
            //{
            //    CheckUpDateLock = 0;
            //}


        }
        private void TMStart3_Elapsed(object state)
        {
            //// 加锁检查更新锁
            //lock (LockObject)
            //{
            //    if (CheckUpDateLock == 0) CheckUpDateLock = 1;
            //    else return;
            //}



            try
            {

                sender.InserPersonResume();
            }
            catch (Exception ex)
            {

                LogHelper.GetLog(this).Error(string.Format("DATE： {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) + "异常：插入个人简历信息" + ex.Message);
            }

            //// 解锁更新检查锁
            //lock (LockObject)
            //{
            //    CheckUpDateLock = 0;
            //}
        }

    }



}


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
        /// <summary>
        /// 校验时间
        /// </summary>
        public static DateTime CheckTime = DateTime.Now;
        /// <summary>
        /// 停止发送
        /// </summary>
        public static bool StopSend = false;
        /// <summary>
        /// 定时发送间隔时间
        /// </summary>
        public static int Interval = 60 * 1000 * 60 * 2;

        /// <summary>
        /// 标识是否正在运行
        /// </summary>
        public static bool IsRunning1 = false;
        public static bool IsRunning2 = false;
        public static bool IsRunning3 = false;
        public static bool IsRunning4 = false;

        private static int delay = 0;

        private static bool IsLock1 = false;
        private static bool IsLock2 = false;
        private static bool IsLock3 = false;
        private static bool IsLock4 = false;

        private static System.Threading.Timer timer1;  //计时器
        private static System.Threading.Timer timer2;  //计时器
        private static System.Threading.Timer timer3;  //计时器
        private static System.Threading.Timer timer4;  //计时器
        private int dueTime = 1000 * 2;//(单位毫秒)
        private int period = 1000 * 60 * 60 * 2;//(单位毫秒)
        private static Sender sender = new Sender();

        public bool Start(HostControl hostControl)
        {
            try
            {

                //if (
                ////核对状态
                //!PullInfoService.IsRunning1 ||
                ////核对时间，防止定时器意外终止，精确到分
                //PullInfoService.CheckTime.ToString("yyyy-MM-dd HH:mm") != DateTime.Now.ToString("yyyy-MM-dd HH:mm"))
                //{
                //    PullInfoService.IsRunning1 = false;
                //    //  PullInfoService.delay = delay;
                //    //如果定时器还存在，则销毁
                //    if (PullInfoService.timer1 != null)
                //    {
                //        PullInfoService.timer1.Dispose();
                //    }
                //    PullInfoService.timer1 = new System.Threading.Timer(PullInfoService.TMStart1_Elapsed, null, dueTime, period);
                //}

                if (
                //核对状态
                !PullInfoService.IsRunning1 ||
                //核对时间，防止定时器意外终止，精确到分
                PullInfoService.CheckTime.ToString("yyyy-MM-dd HH:mm") != DateTime.Now.ToString("yyyy-MM-dd HH:mm"))
                {
                    PullInfoService.IsRunning2 = false;
                    //  PullInfoService.delay = delay;
                    //如果定时器还存在，则销毁
                    if (PullInfoService.timer2 != null)
                    {
                        PullInfoService.timer2.Dispose();
                    }
                    PullInfoService.timer2 = new System.Threading.Timer(PullInfoService.TMStart2_Elapsed, null, dueTime, period);
                }

                //if (
                // //核对状态
                // !PullInfoService.IsRunning1 ||
                // //核对时间，防止定时器意外终止，精确到分
                // PullInfoService.CheckTime.ToString("yyyy-MM-dd HH:mm") != DateTime.Now.ToString("yyyy-MM-dd HH:mm"))
                //{
                //    PullInfoService.IsRunning1 = false;
                //    //  PullInfoService.delay = delay;
                //    //如果定时器还存在，则销毁
                //    if (PullInfoService.timer3 != null)
                //    {
                //        PullInfoService.timer3.Dispose();
                //    }
                //    PullInfoService.timer3 = new System.Threading.Timer(PullInfoService.TMStart3_Elapsed, null, dueTime, period);
                //}

                //if (
                //   //核对状态
                //   !PullInfoService.IsRunning1 ||
                //   //核对时间，防止定时器意外终止，精确到分
                //   PullInfoService.CheckTime.ToString("yyyy-MM-dd HH:mm") != DateTime.Now.ToString("yyyy-MM-dd HH:mm"))
                //{
                //    PullInfoService.IsRunning4 = false;
                //    //  PullInfoService.delay = delay;
                //    //如果定时器还存在，则销毁
                //    if (PullInfoService.timer4 != null)
                //    {
                //        PullInfoService.timer4.Dispose();
                //    }
                //    PullInfoService.timer4 = new System.Threading.Timer(PullInfoService.TMStart4_Elapsed, null, dueTime, period);
                //}

                //sender.InserPersonInfo();
                //sender.InserCompanyInfo();
                //sender.InserPersonResume();
                // sender.InserCompanyjob();
                //timer1 = new System.Threading.Timer(TMStart1_Elapsed, null,dueTime, period);

                //timer2 = new System.Threading.Timer(TMStart2_Elapsed, null, dueTime, period);



            }
            catch (Exception ex)
            {
                LogHelper.GetLog(this).Info(string.Format("DATE： {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) + "异常：" + ex.Message + "||" + ex.StackTrace);
            }
            return true;
        }


        public bool Stop(HostControl hostControl)
        {
            return true;
        }

        private static void TMStart1_Elapsed(object state)
        {
            //延时执行
            if (PullInfoService.delay > 0)
            {
                System.Threading.Thread.Sleep(PullInfoService.delay);
                PullInfoService.delay = 0;
            }
            //已经在执行了，就不再执行，直接执行完
            if (PullInfoService.IsLock1)
            {
                return;
            }
            try
            {
                //锁定
                PullInfoService.IsLock1 = true;

                //发送
                sender.InserCompanyInfo();
            }
            catch (Exception ex)
            {

                LogHelper.GetLog(typeof(PullInfoService)).Error(string.Format("DATE： {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) + "异常：插入公司信息" + ex.Message + "||" + ex.StackTrace);
            }
            finally
            {
                //设置校验时间
                PullInfoService.CheckTime = DateTime.Now;
                PullInfoService.IsRunning1 = true;
                //解锁
                PullInfoService.IsLock1 = false;
            }







        }

        private static void TMStart2_Elapsed(object state)
        {
            //延时执行
            if (PullInfoService.delay > 0)
            {
                System.Threading.Thread.Sleep(PullInfoService.delay);
                PullInfoService.delay = 0;
            }
            //已经在执行了，就不再执行，直接执行完
            if (PullInfoService.IsLock3)
            {
                return;
            }
            try
            {
                //锁定
                PullInfoService.IsLock3 = true;

                //发送
                sender.InserPersonInfo();
            }
            catch (Exception ex)
            {

                LogHelper.GetLog(typeof(PullInfoService)).Error(string.Format("DATE： {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) + "异常：插入个人信息" + ex.Message + "||" + ex.StackTrace);
            }
            finally
            {
                //设置校验时间
                PullInfoService.CheckTime = DateTime.Now;
                PullInfoService.IsRunning3 = true;
                //解锁
                PullInfoService.IsLock3 = false;
            }

        }
        private static void TMStart3_Elapsed(object state)
        {
            //延时执行
            if (PullInfoService.delay > 0)
            {
                System.Threading.Thread.Sleep(PullInfoService.delay);
                PullInfoService.delay = 0;
            }
            //已经在执行了，就不再执行，直接执行完
            if (PullInfoService.IsLock3)
            {
                return;
            }
            try
            {
                //锁定
                PullInfoService.IsLock3 = true;

                //发送
                sender.InserPersonResume();
            }
            catch (Exception ex)
            {

                LogHelper.GetLog(typeof(PullInfoService)).Error(string.Format("DATE： {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) + "异常：插入个人简历" + ex.Message + "||" + ex.StackTrace);
            }
            finally
            {
                //设置校验时间
                PullInfoService.CheckTime = DateTime.Now;
                PullInfoService.IsRunning3 = true;
                //解锁
                PullInfoService.IsLock3 = false;
            }





        }

        private static void TMStart4_Elapsed(object state)
        {
            //延时执行
            if (PullInfoService.delay > 0)
            {
                System.Threading.Thread.Sleep(PullInfoService.delay);
                PullInfoService.delay = 0;
            }
            //已经在执行了，就不再执行，直接执行完
            if (PullInfoService.IsLock4)
            {
                return;
            }
            try
            {
                //锁定
                PullInfoService.IsLock4 = true;

                //发送
                sender.InserCompanyjob();
            }
            catch (Exception ex)
            {

                LogHelper.GetLog(typeof(PullInfoService)).Error(string.Format("DATE： {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) + "异常：插入招聘信息" + ex.Message + "||" + ex.StackTrace);
            }
            finally
            {
                //设置校验时间
                PullInfoService.CheckTime = DateTime.Now;
                PullInfoService.IsRunning4 = true;
                //解锁
                PullInfoService.IsLock4 = false;
            }

        }

    }

}


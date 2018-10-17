using System.IO;
using System.Reflection;

using Topshelf;

namespace PullToScxtpt_px
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            string assemblyFilePath = Assembly.GetExecutingAssembly().Location;
            string assemblyDirPath = Path.GetDirectoryName(assemblyFilePath);
            string configFilePath = assemblyDirPath + "\\log4net.config";
            log4net.Config.XmlConfigurator.Configure(new FileInfo(configFilePath));
 
            HostFactory.Run(x =>
            {

             //   x.UseLog4Net("log4net.config");
                x.RunAsLocalSystem();
                x.Service(settings => new PullInfoService());


                x.SetDescription("推送人才网站信息到四川协同平台");
                x.SetDisplayName("PullToScxtpt_px");
                x.SetServiceName("PullInfoService");
            });
        }
    }
}

using System;
using System.IO;
using System.Text;
using System.Web;

namespace PullToScxtpt.Helper
{
    /// <summary>
    /// 应用程序上下文
    /// </summary>
    public class AppContext
    {
        static AppContext()
        {
            AppClientConfig = IsWebApp() ? null : LoadAppClientConfig();
            AppServerConfig = IsWebApp() ? LoadAppServerConfig() : null;
        }

        /// <summary>
        /// 客户端配置文件
        /// </summary>
        public static ClientAppConfig AppClientConfig { get; set; }

        /// <summary>
        /// 服务端配置文件
        /// </summary>
        public static ServerAppConfig AppServerConfig { get; set; }

        static ClientAppConfig LoadAppClientConfig()
        {
            string fileContent = string.Empty;
            string filePath = string.Empty;
            //filePath = Path.Combine(Application.StartupPath, "ClientAppConfig.xml");
            filePath = "ClientAppConfig.xml";
            ClientAppConfig _ApplicationConfig = new ClientAppConfig();
            fileContent = FileUtility.ReadFile(filePath);
            object obj = XmlUtil.Deserialize(typeof(ClientAppConfig), fileContent);
            _ApplicationConfig = obj as ClientAppConfig;
            return _ApplicationConfig;
        }

        static ServerAppConfig LoadAppServerConfig()
        {
            string fileContent = string.Empty;
            string filePath = string.Empty;
            filePath = Path.Combine(HttpRuntime.AppDomainAppPath, "ServiceAppConfig.xml");
            //filePath = Path.Combine(HttpRuntime.BinDirectory, "ServiceAppConfig.xml");
            ServerAppConfig _ApplicationConfig = new ServerAppConfig();
            fileContent = FileUtility.ReadFile(filePath);
            object obj = XmlUtil.Deserialize(typeof(ServerAppConfig), fileContent);
            _ApplicationConfig = obj as ServerAppConfig;
            return _ApplicationConfig;
        }

        /// <summary>  
        /// 判断是否是web程序  
        /// </summary>  
        /// <returns>true：是，false：winform</returns>  
        static bool IsWebApp()
        {
            bool flag = false;
            if (HttpContext.Current != null)
            {
                flag = true;
            }
            //否则是winform程序  
            return flag;
        }

        private class FileUtility
        {
            #region 写文件

            /// <summary>
            /// 文件流转换为Byte字节数组
            /// </summary>
            /// <param name="fileFullName">文件全路径</param>
            /// <returns></returns>
            public static byte[] ConvertFileStream2Byte(string fileFullName)
            {
                byte[] fileByts = null;
                using (FileStream fileStream = new FileStream(fileFullName, FileMode.Open))
                {
                    fileByts = new byte[fileStream.Length];
                    for (int i = 0; i < fileStream.Length; i++)
                    {
                        fileByts[i] = (byte)fileStream.ReadByte();
                    }
                    return fileByts;
                };
            }

            /// <summary>
            /// 写文件
            /// </summary>
            /// <param name="path">文件路径</param>
            /// <param name="value">文件内容</param>
            public static bool WriteFile(string path, string value)
            {
                try
                {
                    if (File.Exists(path) == false)
                    {
                        using (FileStream f = File.Create(path))
                        { }
                    }

                    using (var streamWriter = new StreamWriter(path, true, Encoding.UTF8))
                    {
                        streamWriter.WriteLine(value);
                    }

                    return true;
                }
                catch
                {
                    return false;
                }
            }


            #region 将内容写入文本文件(如果文件path存在就打开，不存在就新建)
            /// <summary>
            /// 将内容写入文本文件(如果文件path存在就打开，不存在就新建)
            /// </summary>
            /// <param name="FilePath">文件路径</param>
            /// <param name="WriteStr">要写入的内容</param>
            /// <param name="FileModes">写入模式：append 是追加写, CreateNew 是覆盖</param>
            public static void WriteStrToTxtFile(string FilePath, string WriteStr, FileMode FileModes = FileMode.Create)
            {
                FileStream fst = new FileStream(FilePath, FileModes);
                StreamWriter swt = new StreamWriter(fst, System.Text.Encoding.GetEncoding("utf-8"));
                swt.WriteLine(WriteStr);
                swt.Close();
                fst.Close();
            }

            #endregion

            #endregion 写文件

            #region 读文件

            /// <summary>
            /// 读文件
            /// </summary>
            /// <param name="path">文件路径</param>
            /// <returns></returns>
            public static string ReadFile(string path)
            {
                string result = string.Empty;

                if (File.Exists(path) == false)
                {
                    return result;
                }

                try
                {
                    using (var streamReader = new StreamReader(path, Encoding.UTF8))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }
                catch (Exception)
                {
                    result = string.Empty;
                }

                return result;
            }

            #endregion 读文件

            #region 删除文件

            /// <summary>
            /// 删除文件
            /// </summary>
            /// <param name="path">路径</param>
            public static bool FileDel(string path)
            {
                try
                {
                    File.Delete(path);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            #endregion 删除文件

            #region 获取指定文件详细属性

            /// <summary>
            /// 获取指定文件详细属性
            /// </summary>
            /// <param name="filePath">文件详细路径</param>
            /// <returns>FileInfo</returns>
            public static FileInfo GetFileAttibe(string filePath)
            {
                try
                {
                    return new FileInfo(filePath);
                }
                catch
                {
                    return default(FileInfo);
                }
            }

            #endregion 获取指定文件详细属性

        }
    }
}
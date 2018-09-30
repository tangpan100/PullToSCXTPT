namespace PullToScxtpt.Helper
{
    /// <summary>
    /// 客户端配置信息
    /// </summary>
    public class ClientAppConfig
    {
        /// <summary>
        /// 远程web服务器IP地址 
        /// </summary>
        public string systemKey { get; set; }

        /// <summary>
        /// Web服务SoapHead账号
        /// </summary>
        public string source { get; set; }

        /// <summary>
        ///  Web服务SoapHead密码
        /// </summary>
        public string signature { get; set; }

        public string jybh { get; set; }

    }
}
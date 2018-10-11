using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PullToScxtpt
{
    /// <summary>
    /// 响应数据
    /// </summary>
    public class XmlResponse
    {
        /// <summary>
        /// 结果代码值
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 结果信息
        /// </summary>
        public string msg { get; set; }
    }
}

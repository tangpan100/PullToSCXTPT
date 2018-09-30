using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;
using System.Xml.Serialization;

namespace PullToScxtpt.Helper
{
    public class RequestSOAPHeader : System.Web.Services.Protocols.SoapHeader
    {
        public RequestSOAPHeader()
        {
            Namespaces = new XmlSerializerNamespaces();
        }
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Namespaces { get; set; }
        /// <summary>
        /// 1、名称空间（固定）
        /// </summary>
        //   public string nameSpace { get; set; }
        /// <summary>
        /// 2、接入系统标识（协同平台分配）
        /// </summary>
        /// 
     
        public string systemKey { get; set; }
        /// <summary>
        /// 3、入参xml
        /// </summary>
        /// 
     
        public string source { get; set; }
        /// <summary>
        /// 4、参数签名
        /// </summary>
        /// 
      
        public string signature { get; set; }
        /// <summary>
        /// 5、交易服务标识
        /// </summary>
        /// 
        [XmlElement(Namespace = "http://webservice.common.yinhai.com/")]
        public string jybh { get; set; }
    }
}

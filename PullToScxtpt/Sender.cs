using PullToScxtpt.Helper;
using System.Collections.Generic;
using Newtonsoft.Json;
using static PullToScxtpt.Scxtpt.YinHaiBusiness;
using PullToScxtpt.Scxtpt;
using System.Text;
using System.Net;
using System.IO;
using System;

namespace PullToScxtpt
{
    public  class Sender
    {


        // SCXTPT.callBusiness bs = new SCXTPT.callBusiness();
        public  void CallWebService()
        {

            string privateKey = System.Configuration.ConfigurationManager.AppSettings["privateKey"].ToString();
            string systemKey = System.Configuration.ConfigurationManager.AppSettings["systemKey"].ToString();
            CompanyInfoService companyInfoService = new CompanyInfoService();

            List<CompanyInfo> clist = companyInfoService.QueryCompanyInfo();
            string inputxml = XmlUtil.Serializer(typeof(List<CompanyInfo>), clist);
            inputxml = inputxml.Replace("\r\n", "").Replace("    ", "").Replace("  ", "").Substring(21).Replace("<CompanyInfo>", "").Replace("</CompanyInfo>", "");
            string xmlzy = XmlUtil.ConvertXml(inputxml);
            xmlzy = xmlzy.Replace(" ", "");
          
            //签名
            string sign = RSAHelper.RSASignPEM(xmlzy, privateKey, "SHA1", "UTF-8");

            //按照前面描述的SOAP结构，构造SOAP信息            string soap = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +                          "<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">" +                              "<soap:Body>" +                                  "<LoginAction xmlns=\"http://tempuri.org/\">" +                                      "<pin>"+uname+"</pin>" +                                      "<pas>"+pwd+"</pas>" +                                  "</LoginAction>" +                              "</soap:Body>" +                          "</soap:Envelope>";             //将SOAP字符串信息转换成Byte数组，用于后面的流传输            byte[] bytData = Encoding.UTF8.GetBytes(soap.ToString());             //创建一个HttpWebRequest实例，地址http://localhost:7887/WebService1.asmx是我发布到本地IIS上的接口            HttpWebRequest request = System.Net.WebRequest.Create(new Uri("http://localhost:7887/WebService1.asmx")) as HttpWebRequest;            //按照SOAP结构中描述的给各个属性赋值            request.Method = "POST";//POST方式传输            request.Host = "localhost";//主机名或IP地址            request.ContentType = "text/xml; charset=utf-8";//传输内容类型及编码格式            request.ContentLength = bytData.Length;//传输内容长度             //注意这里的SOAPAction，看它的value值，是指向了默认命名空间下的LoginAction方法            //通常成熟的接口中都有自定义的SOAP节点（我认为），来告诉服务我要调用那个方法，所以在这种情况下我们把这里的SOAPAction的value值置成空            //（如果你不明白的话，请忽略上面那句话，总之你要知道SOAPAction就是告诉服务我们要调用哪个接口方法）            request.Headers.Add("SOAPAction", "http://tempuri.org/LoginAction");                        //注意！！这里就是身份验证！！如果没有认证，但是IIS却开启了Windows身份验证，就会报401错误！！切记！！            request.Credentials = MyCred();            request.Timeout = 100000;//设置超时时间             //用GetRequestStream()方法来获取一个流，它发出的请求将数据发送到Internet资源供给接口            Stream newStream = request.GetRequestStream();            //将数据写入该流            newStream.Write(bytData, 0, bytData.Length);//写入参数            newStream.Close();             //服务响应            HttpWebResponse res;            try            {                //获取一个响应                res = (HttpWebResponse)request.GetResponse();            }            catch (WebException ex)            {                res = (HttpWebResponse)ex.Response;            }            //将响应写入StreamReader            StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);            //读取流转换成字符串            string ret = sr.ReadToEnd();             //关闭资源            sr.Close();            res.Close();            newStream.Close();


            //将SOAP字符串信息转换成Byte数组，用于后面的流传输            byte[] bytData = Encoding.UTF8.GetBytes(soap.ToString());             //创建一个HttpWebRequest实例，地址http://localhost:7887/WebService1.asmx是我发布到本地IIS上的接口            HttpWebRequest request = System.Net.WebRequest.Create(new Uri("http://localhost:7887/WebService1.asmx")) as HttpWebRequest;            //按照SOAP结构中描述的给各个属性赋值            request.Method = "POST";//POST方式传输            request.Host = "localhost";//主机名或IP地址            request.ContentType = "text/xml; charset=utf-8";//传输内容类型及编码格式            request.ContentLength = bytData.Length;//传输内容长度             //注意这里的SOAPAction，看它的value值，是指向了默认命名空间下的LoginAction方法            //通常成熟的接口中都有自定义的SOAP节点（我认为），来告诉服务我要调用那个方法，所以在这种情况下我们把这里的SOAPAction的value值置成空            //（如果你不明白的话，请忽略上面那句话，总之你要知道SOAPAction就是告诉服务我们要调用哪个接口方法）            request.Headers.Add("SOAPAction", "http://tempuri.org/LoginAction");                        //注意！！这里就是身份验证！！如果没有认证，但是IIS却开启了Windows身份验证，就会报401错误！！切记！！            request.Credentials = MyCred();            request.Timeout = 100000;//设置超时时间             //用GetRequestStream()方法来获取一个流，它发出的请求将数据发送到Internet资源供给接口            Stream newStream = request.GetRequestStream();            //将数据写入该流            newStream.Write(bytData, 0, bytData.Length);//写入参数            newStream.Close();             //服务响应            HttpWebResponse res;            try            {                //获取一个响应                res = (HttpWebResponse)request.GetResponse();            }            catch (WebException ex)            {                res = (HttpWebResponse)ex.Response;            }            //将响应写入StreamReader            StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);            //读取流转换成字符串            string ret = sr.ReadToEnd();             //关闭资源            sr.Close();            res.Close();            newStream.Close();

            string soap = GetSoapInsertStr(systemKey, xmlzy, sign);

            //将SOAP字符串信息转换成Byte数组，用于后面的流传输
            byte[] bytData = Encoding.UTF8.GetBytes(soap);

            //创建一个HttpWebRequest实例，地址http://localhost:7887/WebService1.asmx是我发布到本地IIS上的接口
            HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(new Uri("http://119.6.84.89:8225/xtpt/services/yinHaiBusiness?wsdl"));
            //按照SOAP结构中描述的给各个属性赋值
            request.Method = "POST";//POST方式传输
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.1.4322)";

            request.ContentType = "text/xml; charset=utf-8";//传输内容类型及编码格式
            request.ContentLength = bytData.Length;//传输内容长度

           
            request.Timeout = 100000;//设置超时时间

            //用GetRequestStream()方法来获取一个流，它发出的请求将数据发送到Internet资源供给接口
            Stream newStream = request.GetRequestStream();
            //将数据写入该流
            newStream.Write(bytData, 0, bytData.Length);//写入参数
            newStream.Close();

            //服务响应
            HttpWebResponse res;
            try
            {
                //获取一个响应
                res = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }
            //将响应写入StreamReader
            StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
            //读取流转换成字符串
            string ret = sr.ReadToEnd();

            //关闭资源
            sr.Close();
            res.Close();
            newStream.Close();

        }

        string GetSoapInsertStr(string systemKey,string source,string sign)
        {
            StringBuilder soap = new StringBuilder();
            soap.Append("<?xml version='1.0' encoding='UTF - 8'?>");
            soap.Append("<soap:Envelope xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>");
            soap.Append("<soap:Header>");
            soap.Append("<in:system xmlns:in=\"http://yinhai.com\">");
            soap.Append("<systemKey>" + systemKey + "</systemKey>");
            soap.Append("<source>" + source + "");
            soap.Append("</source>");
            soap.Append("<signature>" + sign + "");
            soap.Append("</signature>");
            soap.Append("<jybh>scggzp_save_ab01</jybh>");
            soap.Append("</in:system>");
            soap.Append("</soap:Header>");
            soap.Append("<soap:Body>");
            soap.Append("<ns2:callBusiness xmlns:ns2='http://webservice.common.yinhai.com/'>");
            soap.Append("<inputxml>" + source + "");
            soap.Append("</inputxml>");
            soap.Append("</ns2:callBusiness>");
            soap.Append("</soap:Body>");
            soap.Append("</soap:Envelope>");

            return soap.ToString();
        }

    }
}

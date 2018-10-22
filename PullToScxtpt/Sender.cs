using PullToScxtpt_px.Helper;
using System.Collections.Generic;


using System.Text;
using System.Net;
using System.IO;
using System;
using System.Data.SqlClient;
using PullToScxtpt_px.Model;
using PullToScxtpt_px.Service;
using System.Collections;

namespace PullToScxtpt_px
{
    public  class Sender
    {
        readonly string saveCompanyInfo = "scggzp_save_ab01";
        readonly string savePersonInfo = "scggzp_save_ac01";
        readonly string savePersonResume = "scggzp_save_cc20";
        readonly string privateKey = System.Configuration.ConfigurationManager.AppSettings["privateKey"].ToString();
        readonly string systemKey = System.Configuration.ConfigurationManager.AppSettings["systemKey"].ToString();

        // SCXTPT.callBusiness bs = new SCXTPT.callBusiness();
        public void InserCompanyInfo() 
        {
            CompanyInfoService companyInfoService = new CompanyInfoService();
            List<CompanyInfo> clist = companyInfoService.QueryCompanyInfo();
            for (int i = 0; i < clist.Count; i++)
            {
                string inputxml = XmlUtil.Serializer(typeof(CompanyInfo), clist[i]);
                inputxml = inputxml.Replace("\r\n", "").Replace("    ", "").Replace("  ", "").Substring(21).Replace("<CompanyInfo>", "").Replace("</CompanyInfo>", "");
                string ret = GetWsResult(privateKey, systemKey, ref inputxml, saveCompanyInfo);
                if (ret.Contains("success"))
                {
                    SqlHelper.ExecuteNonQuery("insert into PullInfoRecord(number,UpdateTime,type)values(@number,@UpdateTime,@type)",
                        new SqlParameter("@number", clist[i].aab001),
                        new SqlParameter("@UpdateTime", DateTime.Now.ToLocalTime()),
                        new SqlParameter("@type", "企业信息")

                        );
                 
                }
                else
                {
                    SqlHelper.ExecuteNonQuery("insert into PullIInfoErrorRecord(number,UpdateTime,type,errorMsg)values(@number,@UpdateTime,@type,@errorMsg)",
                     new SqlParameter("@number", clist[i].aab001),
                     new SqlParameter("@UpdateTime", DateTime.Now.ToLocalTime()),
                     new SqlParameter("@type", "企业信息"),
                      new SqlParameter("@errorMsg", ret)

                     );
                }
            }


        }
        public void InserPersonInfo()
        {
            PersonInfoService personInfoService = new PersonInfoService();
            List<PersonInfo> plist = personInfoService.QueryPersonInfo();

            for (int i = 0; i < plist.Count; i++)
            {
                string inputxml = XmlUtil.Serializer(typeof(PersonInfo), plist[i]);
                inputxml = inputxml.Replace("\r\n", "").Replace("    ", "").Replace("  ", "").Substring(21).Replace("<PersonInfo>", "").Replace("</PersonInfo>", "");
                string ret = GetWsResult(privateKey, systemKey, ref inputxml, savePersonInfo);
                if (ret.Contains("success"))
                {
                    SqlHelper.ExecuteNonQuery("insert into PullInfoRecord(number,UpdateTime,type)values(@number,@UpdateTime,@type)",
                        new SqlParameter("@number", plist[i].aac001),
                        new SqlParameter("@UpdateTime", DateTime.Now.ToLocalTime()),
                        new SqlParameter("@type", "个人信息")

                        );
                }
                else
                {
                    SqlHelper.ExecuteNonQuery("insert into PullIInfoErrorRecord(number,UpdateTime,type,errorMsg)values(@number,@UpdateTime,@type,@errorMsg)",
                     new SqlParameter("@number", plist[i].aac001),
                     new SqlParameter("@UpdateTime", DateTime.Now.ToLocalTime()),
                     new SqlParameter("@type", "个人信息"),
                      new SqlParameter("@errorMsg", ret)

                     );
                }
            }
           
        }

        public void InserPersonResume()
        {
            ResumeService resumeService = new ResumeService();
            List<PersonResume> prlist = resumeService.QueryPersonResume();

            for (int i = 0; i < prlist.Count; i++)
            {
                string inputxml = XmlUtil.Serializer(typeof(PersonResume), prlist[i]);
                inputxml = inputxml.Replace("\r\n", "").Replace("    ", "").Replace("  ", "").Substring(21).Replace("<PersonResume>", "").Replace("</PersonResume>", "");
                string ret = GetWsResult(privateKey, systemKey, ref inputxml, savePersonResume);

                if (ret.Contains("success"))
                {
                    SqlHelper.ExecuteNonQuery("insert into PullInfoRecord(number,UpdateTime,type)values(@number,@UpdateTime,@type)",
                        new SqlParameter("@number", prlist[i].acc200),
                        new SqlParameter("@UpdateTime", DateTime.Now.ToLocalTime()),
                        new SqlParameter("@type", "个人简历")

                        );
                }
                else
                {
                    SqlHelper.ExecuteNonQuery("insert into PullIInfoErrorRecord(number,UpdateTime,type,errorMsg)values(@number,@UpdateTime,@type,@errorMsg)",
                     new SqlParameter("@number", prlist[i].acc200),
                     new SqlParameter("@UpdateTime", DateTime.Now.ToLocalTime()),
                     new SqlParameter("@type", "个人简历"),
                      new SqlParameter("@errorMsg", ret)

                     );
                }
            }

        }

        private string GetWsResult(string privateKey, string systemKey, ref string inputxml,string opr)
        {
            inputxml = XmlUtil.ConvertXml(inputxml);
            inputxml = inputxml.Replace(" ", "");

            //签名
            string sign = RSAHelper.RSASignPEM(inputxml, privateKey, "SHA1", "UTF-8");

            //按照前面描述的SOAP结构，构造SOAP信息            string soap = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +                          "<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">" +                              "<soap:Body>" +                                  "<LoginAction xmlns=\"http://tempuri.org/\">" +                                      "<pin>"+uname+"</pin>" +                                      "<pas>"+pwd+"</pas>" +                                  "</LoginAction>" +                              "</soap:Body>" +                          "</soap:Envelope>";             //将SOAP字符串信息转换成Byte数组，用于后面的流传输            byte[] bytData = Encoding.UTF8.GetBytes(soap.ToString());             //创建一个HttpWebRequest实例，地址http://localhost:7887/WebService1.asmx是我发布到本地IIS上的接口            HttpWebRequest request = System.Net.WebRequest.Create(new Uri("http://localhost:7887/WebService1.asmx")) as HttpWebRequest;            //按照SOAP结构中描述的给各个属性赋值            request.Method = "POST";//POST方式传输            request.Host = "localhost";//主机名或IP地址            request.ContentType = "text/xml; charset=utf-8";//传输内容类型及编码格式            request.ContentLength = bytData.Length;//传输内容长度             //注意这里的SOAPAction，看它的value值，是指向了默认命名空间下的LoginAction方法            //通常成熟的接口中都有自定义的SOAP节点（我认为），来告诉服务我要调用那个方法，所以在这种情况下我们把这里的SOAPAction的value值置成空            //（如果你不明白的话，请忽略上面那句话，总之你要知道SOAPAction就是告诉服务我们要调用哪个接口方法）            request.Headers.Add("SOAPAction", "http://tempuri.org/LoginAction");                        //注意！！这里就是身份验证！！如果没有认证，但是IIS却开启了Windows身份验证，就会报401错误！！切记！！            request.Credentials = MyCred();            request.Timeout = 100000;//设置超时时间             //用GetRequestStream()方法来获取一个流，它发出的请求将数据发送到Internet资源供给接口            Stream newStream = request.GetRequestStream();            //将数据写入该流            newStream.Write(bytData, 0, bytData.Length);//写入参数            newStream.Close();             //服务响应            HttpWebResponse res;            try            {                //获取一个响应                res = (HttpWebResponse)request.GetResponse();            }            catch (WebException ex)            {                res = (HttpWebResponse)ex.Response;            }            //将响应写入StreamReader            StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);            //读取流转换成字符串            string ret = sr.ReadToEnd();             //关闭资源            sr.Close();            res.Close();            newStream.Close();


            //将SOAP字符串信息转换成Byte数组，用于后面的流传输            byte[] bytData = Encoding.UTF8.GetBytes(soap.ToString());             //创建一个HttpWebRequest实例，地址http://localhost:7887/WebService1.asmx是我发布到本地IIS上的接口            HttpWebRequest request = System.Net.WebRequest.Create(new Uri("http://localhost:7887/WebService1.asmx")) as HttpWebRequest;            //按照SOAP结构中描述的给各个属性赋值            request.Method = "POST";//POST方式传输            request.Host = "localhost";//主机名或IP地址            request.ContentType = "text/xml; charset=utf-8";//传输内容类型及编码格式            request.ContentLength = bytData.Length;//传输内容长度             //注意这里的SOAPAction，看它的value值，是指向了默认命名空间下的LoginAction方法            //通常成熟的接口中都有自定义的SOAP节点（我认为），来告诉服务我要调用那个方法，所以在这种情况下我们把这里的SOAPAction的value值置成空            //（如果你不明白的话，请忽略上面那句话，总之你要知道SOAPAction就是告诉服务我们要调用哪个接口方法）            request.Headers.Add("SOAPAction", "http://tempuri.org/LoginAction");                        //注意！！这里就是身份验证！！如果没有认证，但是IIS却开启了Windows身份验证，就会报401错误！！切记！！            request.Credentials = MyCred();            request.Timeout = 100000;//设置超时时间             //用GetRequestStream()方法来获取一个流，它发出的请求将数据发送到Internet资源供给接口            Stream newStream = request.GetRequestStream();            //将数据写入该流            newStream.Write(bytData, 0, bytData.Length);//写入参数            newStream.Close();             //服务响应            HttpWebResponse res;            try            {                //获取一个响应                res = (HttpWebResponse)request.GetResponse();            }            catch (WebException ex)            {                res = (HttpWebResponse)ex.Response;            }            //将响应写入StreamReader            StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);            //读取流转换成字符串            string ret = sr.ReadToEnd();             //关闭资源            sr.Close();            res.Close();            newStream.Close();

            string soap = GetSoapStr(systemKey, inputxml, sign, opr);

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
            //  XmlResponse xml = (XmlResponse)XmlUtil.Deserialize(typeof(XmlResponse), ret);
            // string status = xml.status;

            //关闭资源
            sr.Close();
            res.Close();
            newStream.Close();
            return ret;
        }

        string GetSoapStr(string systemKey, string source, string sign,string opr)
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
            soap.Append("<jybh>"+ opr + "</jybh>");
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

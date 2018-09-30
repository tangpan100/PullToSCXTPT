using System.ServiceModel;
using PullToScxtpt.Helper;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Diagnostics;
using Aop.Api.Util;
using System.Text.RegularExpressions;
using System.Text;

namespace PullToScxtpt
{
    public static class SenderInfo
    {
        public static void CallWebService()
        {

            string privateKey = System.Configuration.ConfigurationManager.AppSettings["privateKey"].ToString();
            string systemKey = System.Configuration.ConfigurationManager.AppSettings["systemKey"].ToString();
            CompanyInfoService companyInfoService = new CompanyInfoService();

            List<CompanyInfo> clist = companyInfoService.QueryCompanyInfo();
            // string str = companyInfoService.QueryCompanyInfo();
            JsonSerializerSettings jsSetting = new JsonSerializerSettings();

            jsSetting.NullValueHandling = NullValueHandling.Ignore;

            //      string clistJson = JsonConvert.SerializeObject(clist, Formatting.Indented, jsSetting);

            /**
             * 1、名称空间（固定）
             * 2、接入系统标识（协同平台分配）
             * 3、入参xml
             * 4、参数签名 
             * 5、交易服务标识
             */

            string inputxml = XmlUtil.Serializer(typeof(List<CompanyInfo>), clist);
            inputxml = inputxml.Replace("\r","").Replace("\n", "").Replace("    ", "").Replace("  ", "").Substring(21).Replace("<CompanyInfo>","").Replace("</CompanyInfo>","");
            string xmlzy = XmlUtil.ConvertXml(inputxml);
          //  xmlzy = xmlzy.Replace(" ","");
            //签名
            string sign = RSAHelper.RSASignPEM(xmlzy, privateKey, "SHA1", "UTF-8");
            //  string sign2 = RSAUtil.RSASign(inputxml2, "~\bcp.keystore"); 
            RequestSOAPHeader requestSOAPHeader = new RequestSOAPHeader();
            requestSOAPHeader.systemKey = systemKey;
            requestSOAPHeader.source = xmlzy;
            requestSOAPHeader.signature = sign;
            requestSOAPHeader.jybh = "scggzp_save_ab01";
            //PullToScxtpt.Scxtpt.YinHaiBusiness yinHaiBusinessClient = new PullToScxtpt.Scxtpt.YinHaiBusiness("http://webservice.common.yinhai.com/", "yhjy", clistJson, sign1, "scggzp_save_ab01");
            PullToScxtpt.Scxtpt.YinHaiBusiness yinHaiBusinessClient = new PullToScxtpt.Scxtpt.YinHaiBusiness(requestSOAPHeader);

            string str2 = yinHaiBusinessClient.callBusiness(xmlzy);
            //发起调用
            //string xmlResponse = yinHaiBusinessClient.callBusiness(clistJson);
            //获取返回结果(XML格式)

            //   XmlResponse xmlResponseObj = JsonConvert.DeserializeObject<XmlResponse>(xmlResponse);

            //  string code = xmlResponseObj.Code;

            //  System.Diagnostics.Debug.WriteLine(code);

        }


    }
}

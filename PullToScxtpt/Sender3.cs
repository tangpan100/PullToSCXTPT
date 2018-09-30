//using System.ServiceModel;
//using PullToScxtpt.Helper;
//using System.Collections.Generic;
//using Newtonsoft.Json;
//using System;

//namespace PullToScxtpt
//{
//    class Sender3
//    {
//        // SCXTPT.callBusiness bs = new SCXTPT.callBusiness();
//        public static void CallWebService()
//        {

//            string privateKey = System.Configuration.ConfigurationManager.AppSettings["privateKey"].ToString();
//            string systemKey = System.Configuration.ConfigurationManager.AppSettings["systemKey"].ToString();
//            CompanyInfoService companyInfoService = new CompanyInfoService();

//         //   List<CompanyInfo> clist = companyInfoService.QueryCompanyInfo();

//            JsonSerializerSettings jsSetting = new JsonSerializerSettings
//            {
//                NullValueHandling = NullValueHandling.Ignore
//            };

//           // string clistJson = JsonConvert.SerializeObject(clist, Formatting.Indented, jsSetting);
//            //创建client，wsdlUrl地址格式：业务协同管理平台访问地址+/services/yinHaiBusiness?wsdl
//            //client创建开销比较大，一般需要使用线程池进行缓存
//            //Client client = dcf.createClient("http://localhost:8080/dubbo-admin/services/yinHaiBusiness?wsdl");
//            //HTTPConduit http = (HTTPConduit)client.getConduit();
//            //设置连接超时参数
//            //HTTPClientPolicy hcp = new HTTPClientPolicy();
//            //hcp.setConnectionTimeout(2000);
//            //hcp.setReceiveTimeout(200000);
//            //http.setClient(hcp);
//            //参数xml，必须有根节点，二级节点对应入参对象属性，也允许直接传json，例如{"aac001","001"}
//            //  string xmlInput = clistJson;
//            //参数签名 1、入参串 2、 证书（不传默认读取bcp.keystore）
//            //string sign = RSAcryption.sign(xmlInput, "cdsiapp.keystore");
//            //string sign = RSAcryption.
//            //设置报文头信息
//            /**
//             * 1、名称空间（固定）
//             * 2、接入系统标识（协同平台分配）
//             * 3、入参xml
//             * 4、参数签名 
//             * 5、交易服务标识
//             */

//            //签名
//            string sign1 = RSAHelper.RSASignPEM(clistJson, privateKey, "SHA1", "UTF-8");
//            //加密
//            // string signInput = RSAHelper.EncryptPEM(privateKey,sign1,"UTF-8");


//            Dictionary<string, object> properties = new Dictionary<string, object>
//            {
//                { "systemKey", "yhjy" },
//                { "source", clistJson },
//                { "signature", sign1 },
//                { "jybh", "scggzp_save_ab01" }
//            };

//           // PullToScxtpt.SCXTPT.YinHaiBusinessClient client = new SCXTPT.YinHaiBusinessClient("123",1);

//        //    RequestSOAPHeader header = new RequestSOAPHeader();
//         //   header.SystemKey = "123";
//            //client
//            //EndpointAddressBuilder end =  new EndpointAddressBuilder(client.Endpoint.Address);
//            //end.Headers.Add(header);

//          //  string str = client.callBusiness("234");

     
          
//        }

//    }
//}

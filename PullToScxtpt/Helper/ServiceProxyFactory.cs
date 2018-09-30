using System;
using System.Linq;
using System.Reflection;
namespace PullToScxtpt.Helper
{
    /// <summary>
    /// WebService服务代理工厂类
    /// </summary>
    public sealed class ServiceProxyFactory
    {




        /// <summary>
        /// 统一入口获取指定T类型的WebService服务实例（推介使用）
        /// </summary>
        /// <typeparam name="T">服务代理类</typeparam>
        /// <returns></returns>
        public static T GetFirstOrDefaultService<T>() where T : System.Web.Services.Protocols.SoapHttpClientProtocol
        {
            Type type = typeof(T); 
            //string webServerUrlFullPath = Prefix_WebServiceURL + string.Format("/{0}/{0}.asmx", type.Name);
            string webServerUrlFullPath = "http://119.6.84.89:8225/xtpt/services/yinHaiBusiness?wsdl";
            object objServiceInstance = Reflect<T>.CreateObjInstance(type.FullName, type.Assembly.ManifestModule.Assembly.FullName, bCache: true);
            ReflectUtility.SetProperty(objServiceInstance, "Url", webServerUrlFullPath);
            ReflectUtility.SetProperty(objServiceInstance, "EnableDecompression", true);
            Type certificateType = type.Assembly.GetTypes().Cast<Type>().Where(s => s.Namespace == type.Namespace
                                                                             && s.BaseType == typeof(System.Web.Services.Protocols.SoapHeader)).FirstOrDefault();
            if (certificateType != null)
            {
                object objCertificate = Reflect<System.Web.Services.Protocols.SoapHeader>.CreateObjInstance(certificateType.FullName,
                                                                                                            certificateType.Assembly.ManifestModule.Assembly.FullName,
                                                                                                            bCache: true);
                ReflectUtility.SetProperty(objCertificate, "systemKey", AppContext.AppClientConfig.systemKey);
                ReflectUtility.SetProperty(objCertificate, "source", AppContext.AppClientConfig.source);
                ReflectUtility.SetProperty(objCertificate, "signature", AppContext.AppClientConfig.signature);
                ReflectUtility.SetProperty(objCertificate, "jybh", AppContext.AppClientConfig.jybh);
 
            }
            return objServiceInstance as T;
        }
    }
}
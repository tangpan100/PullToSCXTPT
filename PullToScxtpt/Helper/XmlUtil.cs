using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace PullToScxtpt.Helper
{


    /// <summary>
    /// Xml序列化与反序列化
    /// </summary>
    public class XmlUtil
    {
        #region 反序列化

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static object Deserialize(Type type, string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(type);
            return xmldes.Deserialize(stream);
        }

        #endregion 反序列化

        #region 序列化

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serializer(Type type, object obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type, new XmlRootAttribute("input"));
            try
            {
                XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces();

                Namespaces.Add(string.Empty, string.Empty);


                //序列化对象
                xml.Serialize(Stream, obj, Namespaces);

            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }

        #endregion 序列化


        #region XML转义字符处理

        /// <summary>

        /// XML转义字符处理

        /// </summary>

        public static string ConvertXml(string xml)

        {
            xml = (char)1 + xml;   //为了避免首字母为要替换的字符，前加前缀
            //for (int intNext = 0; true;)

            //{

            //    int intIndexOf = xml.IndexOf("&", intNext);

            //    intNext = intIndexOf + 1;  //避免&被重复替换

            //    if (intIndexOf <= 0)

            //    {

            //        break;

            //    }

            //    else

            //    {

            //        xml = xml.Substring(0, intIndexOf) + "&amp;" + xml.Substring(intIndexOf + 1);

            //    }

            //}



            for (; true;)

            {

                int intIndexOf = xml.IndexOf("<");

                if (intIndexOf <= 0)

                {

                    break;

                }

                else

                {

                    xml = xml.Substring(0, intIndexOf) + "&lt;" + xml.Substring(intIndexOf + 1);

                }

            }



            for (; true;)

            {

                int intIndexOf = xml.IndexOf(">");

                if (intIndexOf <= 0)

                {

                    break;

                }

                else

                {

                    xml = xml.Substring(0, intIndexOf) + "&gt;" + xml.Substring(intIndexOf + 1);

                }

            }



            //for (; true;)

            //{

            //    int intIndexOf = xml.IndexOf("\"");

            //    if (intIndexOf <= 0)

            //    {

            //        break;

            //    }

            //    else

            //    {

            //        xml = xml.Substring(0, intIndexOf) + "&quot;" + xml.Substring(intIndexOf + 1);

            //    }

            //}



            return xml.Replace(((char)1).ToString(), "");



        }

        #endregion


        #region XML转义字符处理

        /// <summary>

        /// XML转义字符处理

        /// </summary>

        public static string ConvertXml2(string xml)

        {



            xml = (char)1 + xml;   //为了避免首字母为要替换的字符，前加前缀



            for (int intNext = 0; true;)

            {

                int intIndexOf = xml.IndexOf("&", intNext);

                intNext = intIndexOf + 1;  //避免&被重复替换

                if (intIndexOf <= 0)

                {

                    break;

                }

                else

                {

                    xml = xml.Substring(0, intIndexOf) + "&amp;" + xml.Substring(intIndexOf + 1);

                }

            }



            for (; true;)

            {

                int intIndexOf = xml.IndexOf("<");

                if (intIndexOf <= 0)

                {

                    break;

                }

                else

                {

                    xml = xml.Substring(0, intIndexOf) + "&lt;" + xml.Substring(intIndexOf + 1);

                }

            }



            for (; true;)

            {

                int intIndexOf = xml.IndexOf(">");

                if (intIndexOf <= 0)

                {

                    break;

                }

                else

                {

                    xml = xml.Substring(0, intIndexOf) + "&gt;" + xml.Substring(intIndexOf + 1);

                }

            }



            for (; true;)

            {

                int intIndexOf = xml.IndexOf("\"");

                if (intIndexOf <= 0)

                {

                    break;

                }

                else

                {

                    xml = xml.Substring(0, intIndexOf) + "&quot;" + xml.Substring(intIndexOf + 1);

                }

            }



            return xml.Replace(((char)1).ToString(), "");



        }

        #endregion



        //static string sign(string plainData)
        //{
        //    ISigner signer = SignerUtilities.GetSigner("SHA1withRSA");
        //    RsaKeyParameters key = new RsaKeyParameters(true, new BigInteger(modulus, 16), new BigInteger(privateExponent, 16));
        //    signer.Init(true, key);
        //    byte[] plainBytes = Encoding.UTF8.GetBytes(plainData);
        //    signer.BlockUpdate(plainBytes, 0, plainBytes.Length);
        //    byte[] signBytes = signer.GenerateSignature();
        //    return Convert.ToBase64String(signBytes);
        //}
        //static bool verify(string plainData, string sign)
        //{
        //    ISigner signer = SignerUtilities.GetSigner("SHA1withRSA");
        //    RsaKeyParameters key = new RsaKeyParameters(false, new BigInteger(modulus, 16), new BigInteger(publicExponent, 16));
        //    signer.Init(false, key);
        //    byte[] signBytes = Convert.FromBase64String(sign);
        //    byte[] plainBytes = Encoding.UTF8.GetBytes(plainData);
        //    signer.BlockUpdate(plainBytes, 0, plainBytes.Length);
        //    bool ret = signer.VerifySignature(signBytes);
        //    return ret;
        //}
    }
}

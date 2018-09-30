using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
/************************************************************
 * 关于hashAlgorithm参数值有：MD5、SHA1、SHA256、SHA384、SHA512
 * 重要的事情说三遍，不懂的自己恶补去。
 * RSA加密解密：私钥解密，公钥加密。
 * RSA数字签名-俗称加签验签：私钥加签，公钥验签。  
 * RSA加密解密：私钥解密，公钥加密。
 * RSA数字签名-俗称加签验签：私钥加签，公钥验签。 
 * RSA加密解密：私钥解密，公钥加密。
 * RSA数字签名-俗称加签验签：私钥加签，公钥验签。 
 * 
 * 秘钥格式：1、JAVA和C#用是PKCS#8  
 *           2、PEM用的是普通的PKCS#1
 * 
 * List<string> hashAlgorithmList = new List<string>() { "MD5", "SHA1", "SHA256", "SHA384", "SHA512" };//{MD5、RIPEMD160、SHA1、SHA256、SHA384、SHA512}
 * 
 * 
 * kevin整理
 */

public static class RSAHelper
{
    public static string PubKey = "自己用下面生成的";  //公钥
    public static string PriKey = "自己用下面生成的";   //私钥

    #region 创建公钥私钥
    public struct RsaKey
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }
    /// <summary>
    /// 创建CSharp公钥私钥(生产1024位pkcs8秘钥)
    /// </summary>
    /// <returns></returns>
    static public RsaKey GEN_CSharpKey()
    {
        var rsa = new RSACryptoServiceProvider();
        return new RsaKey()
        {
            //写死加快执行速度
            PublicKey = rsa.ToXmlString(false),   //生成公钥 自己生成填到上面
            PrivateKey = rsa.ToXmlString(true)   //生成私钥 自己生成填到上面
        };
    }
    /// <summary>
    /// 创建JAVA公钥私钥(生产1024位pkcs8秘钥)
    /// </summary>
    /// <returns></returns>
    static public RsaKey GEN_JAVAKey()
    {
        var rsa = new RSACryptoServiceProvider();
        return new RsaKey()
        {
            //写死加快执行速度
            PublicKey = rsa.ConvertToJavaPublicKey(rsa.ToXmlString(false)),   //生成公钥 自己生成填到上面
            PrivateKey = rsa.ConvertToJavaPrivateKey(rsa.ToXmlString(true))   //生成私钥 自己生成填到上面
        };
    }
    #endregion

    #region 加密后的字符串反序列化成类和原类值比对
    /// <summary>
    /// 加密后的字符串反序列化成类和原类值比对
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tOri"></param>
    /// <param name="strEncry"></param>
    /// <returns></returns>
    public static bool Compare<T>(T tOri, string strEncry)
    {
        try
        {
            var decryStr = DecryptJava(strEncry, PriKey);
            var flag = true;
            T tDecry = JsonConvert.DeserializeObject<T>(decryStr);
            PropertyInfo[] propertyInfos1 = tOri.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propertyInfo in propertyInfos1)
            {
                var name = propertyInfo.Name;
                var val1 = propertyInfo.GetValue(tOri, null);
                var val2 = tDecry.GetType().GetProperty(name).GetValue(tDecry, null);
                if (val1.ToString() != val2.ToString())
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }
        catch (Exception)
        {
            return false;
        }
    }
    #endregion

    #region  加密

    /// <summary>
    /// RSA加密
    /// </summary>
    /// <param name="publicKeyJava"></param>
    /// <param name="data"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string EncryptJava(string publicKeyJava, string data, string encoding = "UTF-8")
    {
#if false
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        byte[] cipherbytes;
        rsa.FromPublicKeyJavaString(publicKeyJava);

        //☆☆☆☆.NET 4.6以后特有☆☆☆☆
        //HashAlgorithmName hashName = new System.Security.Cryptography.HashAlgorithmName(hashAlgorithm);
        //RSAEncryptionPadding padding = RSAEncryptionPadding.OaepSHA512;//RSAEncryptionPadding.CreateOaep(hashName);//.NET 4.6以后特有               
        //cipherbytes = rsa.Encrypt(Encoding.GetEncoding(encoding).GetBytes(data), padding);
        //☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆

        //☆☆☆☆.NET 4.6以前请用此段代码☆☆☆☆
        cipherbytes = rsa.Encrypt(Encoding.GetEncoding(encoding).GetBytes(data), false);
        
        return Convert.ToBase64String(cipherbytes);
#else
        if (string.IsNullOrEmpty(data))
        {
            return string.Empty;
        }

        if (string.IsNullOrWhiteSpace(publicKeyJava))
        {
            throw new ArgumentException("Invalid Public Key");
        }

        using (var rsa = new RSACryptoServiceProvider())
        {
            var inputBytes = Encoding.GetEncoding(encoding).GetBytes(data);//有含义的字符串转化为字节流

            rsa.FromPublicKeyJavaString(publicKeyJava);//载入公钥
            int bufferSize = (rsa.KeySize / 8) - 11;//单块最大长度
            var buffer = new byte[bufferSize];
            using (MemoryStream inputStream = new MemoryStream(inputBytes),
                 outputStream = new MemoryStream())
            {
                while (true)
                { //分段加密
                    int readSize = inputStream.Read(buffer, 0, bufferSize);
                    if (readSize <= 0)
                    {
                        break;
                    }

                    var temp = new byte[readSize];
                    Array.Copy(buffer, 0, temp, 0, readSize);
                    var encryptedBytes = rsa.Encrypt(temp, false);
                    outputStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                }
                return Convert.ToBase64String(outputStream.ToArray());//转化为字节流方便传输
            }
        }
#endif
    }
    /// <summary>
    /// RSA加密
    /// </summary>
    /// <param name="publicKeyCSharp"></param>
    /// <param name="data"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string EncryptCSharp(string publicKeyCSharp, string data, string encoding = "UTF-8")
    {
#if false
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        byte[] cipherbytes;
        rsa.FromXmlString(publicKeyCSharp);

        //☆☆☆☆.NET 4.6以后特有☆☆☆☆
        //HashAlgorithmName hashName = new System.Security.Cryptography.HashAlgorithmName(hashAlgorithm);
        //RSAEncryptionPadding padding = RSAEncryptionPadding.OaepSHA512;//RSAEncryptionPadding.CreateOaep(hashName);//.NET 4.6以后特有               
        //cipherbytes = rsa.Encrypt(Encoding.GetEncoding(encoding).GetBytes(data), padding);
        //☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆

        //☆☆☆☆.NET 4.6以前请用此段代码☆☆☆☆
        cipherbytes = rsa.Encrypt(Encoding.GetEncoding(encoding).GetBytes(data), false);

        return Convert.ToBase64String(cipherbytes);
#else
        if (string.IsNullOrEmpty(data))
        {
            return string.Empty;
        }

        if (string.IsNullOrWhiteSpace(publicKeyCSharp))
        {
            throw new ArgumentException("Invalid Public Key");
        }

        using (var rsa = new RSACryptoServiceProvider())
        {
            var inputBytes = Encoding.GetEncoding(encoding).GetBytes(data);//有含义的字符串转化为字节流

            rsa.FromXmlString(publicKeyCSharp);//载入公钥
            int bufferSize = (rsa.KeySize / 8) - 11;//单块最大长度
            var buffer = new byte[bufferSize];
            using (MemoryStream inputStream = new MemoryStream(inputBytes),
                 outputStream = new MemoryStream())
            {
                while (true)
                { //分段加密
                    int readSize = inputStream.Read(buffer, 0, bufferSize);
                    if (readSize <= 0)
                    {
                        break;
                    }

                    var temp = new byte[readSize];
                    Array.Copy(buffer, 0, temp, 0, readSize);
                    var encryptedBytes = rsa.Encrypt(temp, false);
                    outputStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                }
                return Convert.ToBase64String(outputStream.ToArray());//转化为字节流方便传输
            }
        }
#endif
    }

    /// <summary>
    /// RSA加密PEM秘钥（普通PKCS#1格式）
    /// </summary>
    /// <param name="publicKeyPEM"></param>
    /// <param name="data"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string EncryptPEM(string publicKeyPEM, string data, string encoding = "UTF-8")
    {
#if false
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        byte[] cipherbytes;
        rsa.LoadPublicKeyPEM(publicKeyPEM);

        //☆☆☆☆.NET 4.6以后特有☆☆☆☆
        //HashAlgorithmName hashName = new System.Security.Cryptography.HashAlgorithmName(hashAlgorithm);
        //RSAEncryptionPadding padding = RSAEncryptionPadding.OaepSHA512;//RSAEncryptionPadding.CreateOaep(hashName);//.NET 4.6以后特有               
        //cipherbytes = rsa.Encrypt(Encoding.GetEncoding(encoding).GetBytes(data), padding);
        //☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆

        //☆☆☆☆.NET 4.6以前请用此段代码☆☆☆☆
        cipherbytes = rsa.Encrypt(Encoding.GetEncoding(encoding).GetBytes(data), false);

        return Convert.ToBase64String(cipherbytes);
#else
        if (string.IsNullOrEmpty(data))
        {
            return string.Empty;
        }

        if (string.IsNullOrWhiteSpace(publicKeyPEM))
        {
            throw new ArgumentException("Invalid Public Key");
        }

        using (var rsa = new RSACryptoServiceProvider())
        {
            var inputBytes = Encoding.GetEncoding(encoding).GetBytes(data);//有含义的字符串转化为字节流

            rsa.LoadPublicKeyPEM(publicKeyPEM);//载入公钥
            int bufferSize = (rsa.KeySize / 8) - 11;//单块最大长度
            var buffer = new byte[bufferSize];
            using (MemoryStream inputStream = new MemoryStream(inputBytes),
                 outputStream = new MemoryStream())
            {
                while (true)
                { //分段加密
                    int readSize = inputStream.Read(buffer, 0, bufferSize);
                    if (readSize <= 0)
                    {
                        break;
                    }

                    var temp = new byte[readSize];
                    Array.Copy(buffer, 0, temp, 0, readSize);
                    var encryptedBytes = rsa.Encrypt(temp, false);
                    outputStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                }
                return Convert.ToBase64String(outputStream.ToArray());//转化为字节流方便传输
            }
        }
#endif
    }
    #endregion

    #region 解密


    /// <summary>
    /// RSA解密
    /// </summary>
    /// <param name="privateKeyJava"></param>
    /// <param name="data"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string DecryptJava(string privateKeyJava, string data, string encoding = "UTF-8")
    {
#if false
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        byte[] cipherbytes;
        rsa.FromPrivateKeyJavaString(privateKeyJava);
        //☆☆☆☆.NET 4.6以后特有☆☆☆☆
        //RSAEncryptionPadding padding = RSAEncryptionPadding.CreateOaep(new System.Security.Cryptography.HashAlgorithmName(hashAlgorithm));//.NET 4.6以后特有        
        //cipherbytes = rsa.Decrypt(Encoding.GetEncoding(encoding).GetBytes(data), padding);
        //☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆

        //☆☆☆☆.NET 4.6以前请用此段代码☆☆☆☆
        cipherbytes = rsa.Decrypt(Convert.FromBase64String(data), false);

        return Encoding.GetEncoding(encoding).GetString(cipherbytes);

#else
        if (string.IsNullOrEmpty(data))
        {
            return string.Empty;
        }

        if (string.IsNullOrWhiteSpace(privateKeyJava))
        {
            throw new ArgumentException("Invalid Private Key");
        }

        using (var rsa = new RSACryptoServiceProvider())
        {
            var inputBytes = Convert.FromBase64String(data);
            rsa.FromPrivateKeyJavaString(privateKeyJava);
            int bufferSize = rsa.KeySize / 8;
            var buffer = new byte[bufferSize];
            using (MemoryStream inputStream = new MemoryStream(inputBytes),
                 outputStream = new MemoryStream())
            {
                while (true)
                {
                    int readSize = inputStream.Read(buffer, 0, bufferSize);
                    if (readSize <= 0)
                    {
                        break;
                    }

                    var temp = new byte[readSize];
                    Array.Copy(buffer, 0, temp, 0, readSize);
                    var rawBytes = rsa.Decrypt(temp, false);
                    outputStream.Write(rawBytes, 0, rawBytes.Length);
                }
                return Encoding.GetEncoding(encoding).GetString(outputStream.ToArray());
            }
        }
#endif
    }
    /// <summary>
    /// RSA解密
    /// </summary>
    /// <param name="privateKeyCSharp"></param>
    /// <param name="data"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string DecryptCSharp(string privateKeyCSharp, string data, string encoding = "UTF-8")
    {
#if false
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        byte[] cipherbytes;
        rsa.FromXmlString(privateKeyCSharp);
        //☆☆☆☆.NET 4.6以后特有☆☆☆☆
        //RSAEncryptionPadding padding = RSAEncryptionPadding.CreateOaep(new System.Security.Cryptography.HashAlgorithmName(hashAlgorithm));//.NET 4.6以后特有        
        //cipherbytes = rsa.Decrypt(Encoding.GetEncoding(encoding).GetBytes(data), padding);
        //☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆

        //☆☆☆☆.NET 4.6以前请用此段代码☆☆☆☆
        cipherbytes = rsa.Decrypt(Convert.FromBase64String(data), false);

        return Encoding.GetEncoding(encoding).GetString(cipherbytes);
#else
        if (string.IsNullOrEmpty(data))
        {
            return string.Empty;
        }

        if (string.IsNullOrWhiteSpace(privateKeyCSharp))
        {
            throw new ArgumentException("Invalid Private Key");
        }

        using (var rsa = new RSACryptoServiceProvider())
        {
            var inputBytes = Convert.FromBase64String(data);
            rsa.FromXmlString(privateKeyCSharp);
            int bufferSize = rsa.KeySize / 8;
            var buffer = new byte[bufferSize];
            using (MemoryStream inputStream = new MemoryStream(inputBytes),
                 outputStream = new MemoryStream())
            {
                while (true)
                {
                    int readSize = inputStream.Read(buffer, 0, bufferSize);
                    if (readSize <= 0)
                    {
                        break;
                    }

                    var temp = new byte[readSize];
                    Array.Copy(buffer, 0, temp, 0, readSize);
                    var rawBytes = rsa.Decrypt(temp, false);
                    outputStream.Write(rawBytes, 0, rawBytes.Length);
                }
                return Encoding.GetEncoding(encoding).GetString(outputStream.ToArray());
            }
        }
#endif
    }
    /// <summary>
    /// RSA解密（普通PKCS#1格式）
    /// </summary>
    /// <param name="privateKeyPEM"></param>
    /// <param name="data"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string DecryptPEM(string privateKeyPEM, string data, string encoding = "UTF-8")
    {
#if false
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        byte[] cipherbytes;
        rsa.LoadPrivateKeyPEM(privateKeyPEM);
        //☆☆☆☆.NET 4.6以后特有☆☆☆☆
        //RSAEncryptionPadding padding = RSAEncryptionPadding.CreateOaep(new System.Security.Cryptography.HashAlgorithmName(hashAlgorithm));//.NET 4.6以后特有        
        //cipherbytes = rsa.Decrypt(Encoding.GetEncoding(encoding).GetBytes(data), padding);
        //☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆

        //☆☆☆☆.NET 4.6以前请用此段代码☆☆☆☆
        cipherbytes = rsa.Decrypt(Convert.FromBase64String(data), false);

        return Encoding.GetEncoding(encoding).GetString(cipherbytes);
#else
        if (string.IsNullOrEmpty(data))
        {
            return string.Empty;
        }

        if (string.IsNullOrWhiteSpace(privateKeyPEM))
        {
            throw new ArgumentException("Invalid Private Key");
        }

        using (var rsa = new RSACryptoServiceProvider())
        {
            var inputBytes = Convert.FromBase64String(data);
            rsa.LoadPrivateKeyPEM(privateKeyPEM);
            int bufferSize = rsa.KeySize / 8;
            var buffer = new byte[bufferSize];
            using (MemoryStream inputStream = new MemoryStream(inputBytes),
                 outputStream = new MemoryStream())
            {
                while (true)
                {
                    int readSize = inputStream.Read(buffer, 0, bufferSize);
                    if (readSize <= 0)
                    {
                        break;
                    }

                    var temp = new byte[readSize];
                    Array.Copy(buffer, 0, temp, 0, readSize);
                    var rawBytes = rsa.Decrypt(temp, false);
                    outputStream.Write(rawBytes, 0, rawBytes.Length);
                }
                return Encoding.GetEncoding(encoding).GetString(outputStream.ToArray());
            }
        }
#endif
    }
    #endregion


    #region 加签

    /// <summary>
    /// RSA签名
    /// </summary>
    /// <param name="privateKeyJava">私钥</param>
    /// <param name="data">待签名的内容</param>
    /// <returns></returns>
    public static string RSASignJava(string data, string privateKeyJava, string hashAlgorithm, string encoding = "UTF-8")
    {
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        rsa.FromPrivateKeyJavaString(privateKeyJava);//加载私钥
                                                     //RSAPKCS1SignatureFormatter RSAFormatter = new RSAPKCS1SignatureFormatter(rsa);
                                                     ////设置签名的算法为MD5 MD5withRSA 签名
                                                     //RSAFormatter.SetHashAlgorithm(hashAlgorithm);


        var dataBytes = Encoding.GetEncoding(encoding).GetBytes(data);
        var HashbyteSignature = rsa.SignData(dataBytes, hashAlgorithm);
        return Convert.ToBase64String(HashbyteSignature);

        //byte[] HashbyteSignature = ConvertToRgbHash(data, encoding);

        //byte[] dataBytes =Encoding.GetEncoding(encoding).GetBytes(data);
        //HashbyteSignature = rsa.SignData(dataBytes, hashAlgorithm);
        //return Convert.ToBase64String(HashbyteSignature);
        //执行签名 
        //EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);
        //return Convert.ToBase64String(RSAFormatter.CreateSignature(HashbyteSignature));
        //return result.Replace("=", string.Empty).Replace('+', '-').Replace('/', '_');
    }
    /// <summary>
    /// RSA签名
    /// </summary>
    /// <param name="privateKeyPEM">私钥</param>
    /// <param name="data">待签名的内容</param>
    /// <returns></returns>
    public static string RSASignPEM(string data, string privateKeyPEM, string hashAlgorithm , string encoding = "UTF-8")
    {
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        rsa.LoadPrivateKeyPEM(privateKeyPEM);//加载私钥   
        var dataBytes = Encoding.GetEncoding(encoding).GetBytes(data);
        var HashbyteSignature = rsa.SignData(dataBytes, hashAlgorithm);
        string str16= byteToHexStr(HashbyteSignature).ToLower();
        string str = Convert.ToBase64String(HashbyteSignature);
        return str16;
    }
    /// <summary>
    /// RSA签名CSharp
    /// </summary>
    /// <param name="privateKeyCSharp">私钥</param>
    /// <param name="data">待签名的内容</param>
    /// <returns></returns>
    public static string RSASignCSharp(string data, string privateKeyCSharp, string hashAlgorithm , string encoding = "UTF-8")
    {
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(privateKeyCSharp);//加载私钥   
        var dataBytes = Encoding.GetEncoding(encoding).GetBytes(data);
        var HashbyteSignature = rsa.SignData(dataBytes, hashAlgorithm);
        return Convert.ToBase64String(HashbyteSignature);
    }
    /// <summary>
    /// RSA签名
    /// </summary>
    /// <param name="privateKey">私钥</param>
    /// <param name="content">待签名内容</param>
    /// <returns></returns>
    public static string Signature(string privateKey, string content, Encoding encode)
    {
        RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();
        rsaProvider.FromXmlString(privateKey);
        SHA1 sha1 = SHA1.Create();
        byte[] signature = rsaProvider.SignData(encode.GetBytes(content), sha1);
        return Convert.ToBase64String(signature);
    }
    #endregion

    #region 验签

    /// <summary> 
    /// 验证签名-方法一
    /// </summary>
    /// <param name="data"></param>
    /// <param name="publicKeyJava"></param>
    /// <param name="hashAlgorithm"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static bool VerifyJava(string data, string publicKeyJava, string signature, string hashAlgorithm = "MD5", string encoding = "UTF-8")
    {
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        //导入公钥，准备验证签名
        rsa.FromPublicKeyJavaString(publicKeyJava);
        //返回数据验证结果
        byte[] Data = Encoding.GetEncoding(encoding).GetBytes(data);
        byte[] rgbSignature = Convert.FromBase64String(signature);

        return rsa.VerifyData(Data, hashAlgorithm, rgbSignature);

        //return SignatureDeformatter(publicKeyJava, data, signature);

        //return CheckSign(publicKeyJava, data, signature);

        //return rsa.VerifyData(Encoding.GetEncoding(encoding).GetBytes(data), "MD5", Encoding.GetEncoding(encoding).GetBytes(signature));
    }
    /// <summary> 
    /// 验证签名PEM
    /// </summary>
    /// <param name="data"></param>
    /// <param name="signature"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static bool VerifyPEM(string data, string publicKeyPEM, string signature, string hashAlgorithm = "MD5", string encoding = "UTF-8")
    {
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        //导入公钥，准备验证签名
        rsa.LoadPublicKeyPEM(publicKeyPEM);
        //返回数据验证结果
        byte[] Data = Encoding.GetEncoding(encoding).GetBytes(data);
        byte[] rgbSignature = Convert.FromBase64String(signature);

        return rsa.VerifyData(Data, hashAlgorithm, rgbSignature);
    }

    /// <summary> 
    /// 验证签名CSharp
    /// </summary>
    /// <param name="data"></param>
    /// <param name="signature"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static bool VerifyCSharp(string data, string publicKeyCSharp, string signature, string hashAlgorithm = "MD5", string encoding = "UTF-8")
    {
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        //导入公钥，准备验证签名
        rsa.LoadPublicKeyPEM(publicKeyCSharp);
        //返回数据验证结果
        byte[] Data = Encoding.GetEncoding(encoding).GetBytes(data);
        byte[] rgbSignature = Convert.FromBase64String(signature);

        return rsa.VerifyData(Data, hashAlgorithm, rgbSignature);
    }

    #region 签名验证-方法二
    /// <summary>
    /// 签名验证
    /// </summary>
    /// <param name="publicKey">公钥</param>
    /// <param name="p_strHashbyteDeformatter">待验证的用户名</param>
    /// <param name="signature">注册码</param>
    /// <returns>签名是否符合</returns>
    public static bool SignatureDeformatter(string publicKey, string data, string signature, string hashAlgorithm = "MD5")
    {
        try
        {
            byte[] rgbHash = ConvertToRgbHash(data);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //导入公钥，准备验证签名
            rsa.FromPublicKeyJavaString(publicKey);

            RSAPKCS1SignatureDeformatter deformatter = new RSAPKCS1SignatureDeformatter(rsa);
            deformatter.SetHashAlgorithm("MD5");
            byte[] rgbSignature = Convert.FromBase64String(signature);
            if (deformatter.VerifySignature(rgbHash, rgbSignature))
            {
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 签名数据转化为RgbHash
    /// </summary>
    /// <param name="data"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static byte[] ConvertToRgbHash(string data, string encoding = "UTF-8")
    {
        using (MD5 md5 = new MD5CryptoServiceProvider())
        {
            byte[] bytes_md5_in = Encoding.GetEncoding(encoding).GetBytes(data);
            return md5.ComputeHash(bytes_md5_in);
        }
    }
    #endregion

    #region 签名验证-方法三
    /// <summary>
    /// 验证签名
    /// </summary>
    /// <param name="data">原始数据</param>
    /// <param name="sign">签名</param>
    /// <returns></returns>
    public static bool CheckSign(string publicKey, string data, string sign, string encoding = "UTF-8")
    {

        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        rsa.FromPublicKeyJavaString(publicKey);
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

        byte[] Data = Encoding.GetEncoding(encoding).GetBytes(data);
        byte[] rgbSignature = Convert.FromBase64String(sign);
        if (rsa.VerifyData(Data, md5, rgbSignature))
        {
            return true;
        }
        return false;

    }
  
        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
    {
        string returnStr = "";
        if (bytes != null)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                returnStr += bytes[i].ToString("X2");
            }
        }
        return returnStr;
    }
    #endregion
    #endregion

}

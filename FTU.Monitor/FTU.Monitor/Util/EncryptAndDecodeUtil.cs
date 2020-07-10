using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows;
using FTU.Monitor.ViewModel;

namespace FTU.Monitor.Util
{
    /// <summary>
    /// EncryptAndDecodeUtil 的摘要说明
    /// author: liyan
    /// date：2018/4/18 15:15:36
    /// desc：AES加密和解密工具类
    /// version: 1.0
    /// </summary>
    public static class EncryptAndDecodeUtil
    {
        /// <summary>
        /// 获取密钥
        /// </summary>
        private static string Key
        {
            get
            {
                return @")O[NB]6,YF}+efcaj{+oESb9d8>Z'e9M";
            }
        }

        /// <summary>
        /// 获取向量
        /// </summary>
        private static string IV
        {
            get
            {
                return @"L+\~f4,Ir)b$=pkf";
            }
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainStr">明文字符串</param>
        /// <returns>密文</returns>
        public static string AESEncrypt(string plainStr)
        {
            // 获取密钥字节数组
            byte[] bKey = Encoding.UTF8.GetBytes(Key);
            // 获取向量字节数组
            byte[] bIV = Encoding.UTF8.GetBytes(IV);
            // 获取明文字符串字节数组
            byte[] byteArray = Encoding.UTF8.GetBytes(plainStr);
            // 定义加密后的字符串
            string encrypt = null;

            // 创建加密对象以执行 System.Security.Cryptography.Rijndael 算法
            Rijndael aes = Rijndael.Create();
            try
            {
                // 使用初始化为零的可扩展容量初始化 System.IO.MemoryStream 类的新实例
                using (MemoryStream mStream = new MemoryStream())
                {
                    // 用目标数据流、要使用的转换和流的模式初始化 System.Security.Cryptography.CryptoStream 类的新实例
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        // 将一字节序列写入当前的 System.Security.Cryptography.CryptoStream，并将通过写入的字节数提前该流的当前位置
                        cStream.Write(byteArray, 0, byteArray.Length);
                        // 用缓冲区的当前状态更新基础数据源或储存库，随后清除缓冲区
                        cStream.FlushFinalBlock();
                        // 将 8 位无符号整数的数组转换为其用 Base64 数字编码的等效字符串表示形式
                        encrypt = Convert.ToBase64String(mStream.ToArray());
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("AES加密异常:" + ex.Message);
            }
            // 释放 System.Security.Cryptography.SymmetricAlgorithm 类使用的所有资源
            aes.Clear();

            return encrypt;
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainStr">明文字符串</param>
        /// <param name="returnNull">加密失败时是否返回 null，false 返回 String.Empty</param>
        /// <returns>密文</returns>
        public static string AESEncrypt(string plainStr, bool returnNull)
        {
            // 获取AES加密后的字符串
            string encrypt = AESEncrypt(plainStr);
            return returnNull ? encrypt : (encrypt == null ? String.Empty : encrypt);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptStr">密文字符串</param>
        /// <returns>明文</returns>
        public static string AESDecrypt(string encryptStr)
        {
            // 获取密钥字节数组
            byte[] bKey = Encoding.UTF8.GetBytes(Key);
            // 获取向量字节数组
            byte[] bIV = Encoding.UTF8.GetBytes(IV);
            // 将指定的字符串（它将二进制数据编码为 Base64 数字）转换为等效的 8 位无符号整数数组
            byte[] byteArray = Convert.FromBase64String(encryptStr);
            // 定义解密后的字符串
            string decrypt = null;

            // 创建加密对象以执行 System.Security.Cryptography.Rijndael 算法
            Rijndael aes = Rijndael.Create();
            try
            {
                // 使用初始化为零的可扩展容量初始化 System.IO.MemoryStream 类的新实例
                using (MemoryStream mStream = new MemoryStream())
                {
                    // 用目标数据流、要使用的转换和流的模式初始化 System.Security.Cryptography.CryptoStream 类的新实例
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        // 将一字节序列写入当前的 System.Security.Cryptography.CryptoStream，并将通过写入的字节数提前该流的当前位置
                        cStream.Write(byteArray, 0, byteArray.Length);
                        // 用缓冲区的当前状态更新基础数据源或储存库，随后清除缓冲区
                        cStream.FlushFinalBlock();
                        // 将指定字节数组中的所有字节解码为一个字符串
                        decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("AES加密异常:" + ex.Message);
            }
            // 释放 System.Security.Cryptography.SymmetricAlgorithm 类使用的所有资源
            aes.Clear();

            return decrypt;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptStr">密文字符串</param>
        /// <param name="returnNull">解密失败时是否返回 null，false 返回 String.Empty</param>
        /// <returns>明文</returns>
        public static string AESDecrypt(string encryptStr, bool returnNull)
        {
            // 获取AES解密后的字符串
            string decrypt = AESDecrypt(encryptStr);
            return returnNull ? decrypt : (decrypt == null ? String.Empty : decrypt);
        }

        /// <summary>
        /// 将对象转换为Json格式
        /// </summary>
        /// <param name="obj">被转换的对象</param>
        /// <returns>转换后的Json字符串</returns>
        public static string GetJson(object obj)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            return json.Serialize(obj);
        }

        /// <summary>
        /// Json反序列化，就是把Json格式的字符串转换成对应的数据对象
        /// </summary>
        /// <param name="json">Json字符串</param>
        /// <returns>反序列化后的数据对象</returns>
        public static IList<T> JsonToList<T>(this string json)
        {
            Console.WriteLine(json);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IList<T> objs = serializer.Deserialize<IList<T>>(json);
            return objs;
        }

        /// <summary>
        /// Json反序列化，就是把Json格式的字符串转换成对应的数据对象
        /// </summary>
        /// <param name="json">Json字符串</param>
        /// <returns>反序列化后的数据对象</returns>
        public static T JsonToObject<T>(this string json)
        {
            Console.WriteLine(json);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            T obj = serializer.Deserialize<T>(json);
            return obj;
        }

        /// <summary>
        /// AES加密方法
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns>加密后的字节</returns>
         public static byte[] AESEncryptOld(string plainText)
        {
            byte[] inputByteArray = Encoding.UTF8.GetBytes(plainText);
            string strKey = "dongbinhuiasxiny";
            byte[] key = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            SymmetricAlgorithm des = Rijndael.Create();
            des.Padding = PaddingMode.Zeros;
            des.Key = Encoding.UTF8.GetBytes(strKey);
            des.IV = key;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            byte[] cipherBytes = ms.ToArray();//得到加密后的字节数组

            cs.Close();
            ms.Close();

            return cipherBytes;
        }

        /// <summary>
        /// 文件解密方法
        /// </summary>
        /// <param name="cipherText">密文</param>
        /// <returns>明文</returns>
         public static string AESDecryptOld(byte[] cipherText)
         {
             string strKey = "dongbinhuiasxiny";
             byte[] key = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
             SymmetricAlgorithm des = Rijndael.Create();
             des.Padding = PaddingMode.Zeros;
             des.Key = Encoding.UTF8.GetBytes(strKey);
             des.IV = key;
             byte[] decryptBytes = new byte[cipherText.Length];
             MemoryStream ms = new MemoryStream(cipherText);
             CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read);
             cs.Read(decryptBytes, 0, decryptBytes.Length);
             
             cs.Close();
             ms.Close();

             // 将Json后面的padding字符
             string plaintex = Encoding.UTF8.GetString(decryptBytes);
             int index = plaintex.LastIndexOf('}');
             if (index == -1)
             {
                 MessageBox.Show("解密后的json格式错误");
                 return "";
             }
             return plaintex.Remove(index + 1, plaintex.Length - index - 1);
         }

        /// <summary>
        /// 将终端序列号加入到即将导出的文件中
        /// </summary>
        /// <param name="json"></param>
        public static void AddProgramVersionToJson(ref string json)
        {
            int beginLocation = json.IndexOf('[');
            json = json.Insert(beginLocation + 1, InherentParameterViewModel.programVersion);
        }

        /// <summary>
        /// 将终端序列号从导入的文件中提取出来并删除
        /// </summary>
        /// <param name="json">点表对应的json字符串</param>
        /// <returns>即将导入的点表所携带的程序版本号</returns>
        public static string RemoveProgramVersionFromJson(ref string json)
        {
            int programVersionStringBeginLocation = json.IndexOf('[');
            int programVersionStringEndLocation = json.IndexOf('{');
            int programVersionStringLenthFromJson = programVersionStringEndLocation - programVersionStringBeginLocation - 1;
            string programVersionFromJson = json.Substring(programVersionStringBeginLocation + 1, programVersionStringLenthFromJson);
            json = json.Remove(programVersionStringBeginLocation + 1, programVersionStringLenthFromJson);
            return programVersionFromJson;
        }
    }
}

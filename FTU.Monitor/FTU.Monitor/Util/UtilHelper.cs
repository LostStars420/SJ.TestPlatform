using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace FTU.Monitor.Util
{
    /// <summary>
    /// UtilHelper 的摘要说明
    /// author: songminghao
    /// date：2017/10/12 9:48:09
    /// desc：常用方法工具类
    /// version: 1.0
    /// </summary>
    public class UtilHelper
    {
        /// <summary>
        /// List集合拼接为字符串
        /// </summary>
        /// <param name="buf">List集合</param>
        /// <returns></returns>
        public static string ListToString(List<byte> buf)
        {
            if (buf == null || buf.Count == 0) 
            {
                return "";
            }

            StringBuilder builder = new StringBuilder();//用于显示报文

            for (int i = 0; i < buf.Count; i++)
            {
                builder.Append(buf[i].ToString("X2") + " ");
            }

            return builder.ToString();
        }

        /// <summary>
        /// 字节数组拼接为字符串
        /// </summary>
        /// <param name="buf">字节数组</param>
        /// <returns></returns>
        public static string ListToString(byte[] buf)
        {
            if (buf == null || buf.Length == 0)
            {
                return "";
            }

            StringBuilder builder = new StringBuilder();//用于显示报文

            for (int i = 0; i < buf.Length; i++)
            {
                builder.Append(buf[i].ToString("X2") + " ");
            }

            return builder.ToString();
        }

        /// <summary>
        /// 字节数组拼接为字符串
        /// </summary>
        /// <param name="buf">字节数组</param>
        /// <param name="length">字节数组转换的长度</param>
        /// <returns></returns>
        public static string ListToString(byte[] buf, int length)
        {
            if (buf == null || buf.Length == 0 || length <= 0 || buf.Length < length)
            {
                return "";
            }

            StringBuilder builder = new StringBuilder();//用于显示报文

            for (int i = 0; i < length; i++)
            {
                builder.Append(buf[i].ToString("X2") + " ");
            }

            return builder.ToString();
        }

        /// <summary>
        /// 判断对象是否为空
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static bool IsEmpty(object obj) 
        {
            if(obj == null)
            {
                return true;
            }

            if (obj is IEnumerable)
            {
                XmlNodeList xmlNodeList = (XmlNodeList)obj;
                return xmlNodeList.Count == 0;
            }

            throw new Exception("not support this type");
        }

        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>true，代表空；false，代表非空</returns>
        public static bool IsEmpty(string str)
        {
            if (str == null || "".Equals(str.Trim()))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 判断集合是否为空
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>true，代表空；false，代表非空</returns>
        public static bool IsListEmpty(IList<Object> list)
        {
            if (list == null || list.Count == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// CRC16校验
        /// </summary>
        /// <param name="data">待校验字节集合</param>
        /// <param name="start">校验起始位置(从零开始)</param>
        /// <param name="count">校验长度</param>
        /// <returns></returns>
        public static UInt16 CRC16Check(List<byte> data, int start, int count)
        {
            // CRC16校验值
            UInt16 Reg_CRC = 0xFFFF;

            if (data == null || data.Count < count)
            {
                return 0;
            }

            for (int i = start; i < start + count; i++)
            {
                Reg_CRC ^= data[i];

                for (int j = 0; j < 8; j++)
                {
                    if ((UInt16)(Reg_CRC & 0x0001) == 1)
                    {
                        Reg_CRC = (UInt16)((UInt16)(Reg_CRC >> 1) ^ 0xA001);
                    }
                    else
                    {
                        Reg_CRC >>= 1;
                    }
                        
                }
            }

            return (UInt16)((UInt16)(Reg_CRC << 8) | (UInt16)(Reg_CRC >> 8)); 
        }

        /// <summary>
        /// 是否忙碌
        /// </summary>
        /// <param name="abled"></param>
        public static void CursorEnabled(bool abled)
        { 
            if (abled)
            {
                Messenger.Default.Send<string>("Enabled", "PassMainViewCommand");
            }
            else
            {
                Messenger.Default.Send<string>("UnAbled", "PassMainViewCommand");
            }
        }

        /// <summary>
        /// 获取终端序列号的最后一个.之前的编号
        /// </summary>
        /// <param name="allSeriaNumber">所有终端序列号</param>
        /// <returns>终端序列号的最后一个.之前的编号的字符串</returns>
        public static string GetTerminalSeriaNumber(string allSeriaNumber)
        {
            if(allSeriaNumber == null || allSeriaNumber == "")
            {
                return "";
            }
            int lastPointIndex = allSeriaNumber.LastIndexOf('.');
            if(lastPointIndex == -1)
            {
                return "";
            }
            return allSeriaNumber.Substring(0,lastPointIndex);
        }
    }
}

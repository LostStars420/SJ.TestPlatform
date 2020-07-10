using System;

namespace FTU.Monitor.Util
{
    /// <summary>
    /// LogHelper 的摘要说明
    /// author: songminghao
    /// date：2017/10/11 10:08:27
    /// desc：日志输出工具类
    /// version: 1.0
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// 输出调试信息到日志
        /// </summary>
        /// <param name="t">类型</param>
        /// <param name="ex">异常</param>
        public static void Debug(Type t, Exception ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Debug("Debug", ex);
        }

        /// <summary>
        /// 输出调试信息到日志
        /// </summary>
        /// <param name="t">类型</param>
        /// <param name="msg">信息描述</param>
        public static void Debug(Type t, string msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Debug(msg);
        }

        /// <summary>
        /// 输出一般信息到日志
        /// </summary>
        /// <param name="t">类型</param>
        /// <param name="msg">信息描述</param>
        public static void Info(Type t, string msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Info(msg);
        }

        /// <summary>
        /// 输出警告信息到日志
        /// </summary>
        /// <param name="t">类型</param>
        /// <param name="ex">异常</param>
        public static void Warn(Type t, Exception ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Warn("Warn", ex);
        }

        /// <summary>
        /// 输出警告信息到日志
        /// </summary>
        /// <param name="t">类型</param>
        /// <param name="msg">信息描述</param>
        public static void Warn(Type t, string msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Warn(msg);
        }

        /// <summary>
        /// 输出一般错误信息到日志
        /// </summary>
        /// <param name="t">类型</param>
        /// <param name="ex">异常</param>
        public static void Error(Type t, Exception ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Error("Error", ex);
        }
         
        /// <summary>
        /// 输出一般错误信息到日志
        /// </summary>
        /// <param name="t">类型</param>
        /// <param name="msg">信息描述</param>
        public static void Error(Type t, string msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Error(msg);
        }

        /// <summary>
        /// 输出致命错误信息到日志
        /// </summary>
        /// <param name="t">类型</param>
        /// <param name="ex">异常</param>
        public static void Fatal(Type t, Exception ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Fatal("Fatal", ex);
        }

        /// <summary>
        /// 输出致命错误信息到日志 
        /// </summary>
        /// <param name="t">类型</param>
        /// <param name="msg">信息描述</param>
        public static void Fatal(Type t, string msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Fatal(msg);
        }
    
    }
}

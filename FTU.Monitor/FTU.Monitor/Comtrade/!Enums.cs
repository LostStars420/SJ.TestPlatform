/*
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace FTU.Monitor.Comtrade
{
	
	/// <summary>
	/// Comtrade标准版本年号枚举
	/// </summary>	
    internal enum ComtradeVersion
	{
		V1991=0,
		V1999=1,
		V2013=2,
	};

    /// <summary>
    /// ComtradeVersionConverter 的摘要说明
    /// author: songminghao
    /// date：2017/12/08 11:08:09
    /// desc：录波标准版本年号转换类
    /// version: 1.0
    /// </summary>
	internal static class ComtradeVersionConverter
	{
        /// <summary>
        /// 获取录波标准版本年号对应枚举数据
        /// </summary>
        /// <param name="year">年号</param>
        /// <returns></returns>
        internal static ComtradeVersion Get(string year)
		{
            if (year == null)
            {
                return ComtradeVersion.V1991;
            }
                
            if (year == "1991")
            {
                return ComtradeVersion.V1991;
            }
                
            if (year == "1999")
            {
                return ComtradeVersion.V1999;
            }
                
            if (year == "2013")
            {
                return ComtradeVersion.V2013;
            }
            
            // 默认返回1991年号版本
			return ComtradeVersion.V1991;
		}
	}
	
	/// <summary>
	/// 数据文件类型枚举
	/// </summary>
	public enum DataFileType
	{	
		/// <summary>
        /// 无定义
		/// </summary>		
		Undefined = 0,

		/// <summary>
        /// ASCII码
		/// </summary>
		ASCII,

		/// <summary>
        /// 二进制格式
		/// </summary>
		Binary,

		/// <summary>
        /// 二进制格式32
		/// </summary>
		Binary32,

		/// <summary>
        /// 浮点型格式32
		/// </summary>
		Float32
	}

    /// <summary>
    /// DataFileTypeConverter 的摘要说明
    /// author: songminghao
    /// date：2017/12/08 11:08:09
    /// desc：数据文件类型名称转换类
    /// version: 1.0
    /// </summary>
	internal static class DataFileTypeConverter
	{
        /// <summary>
        /// 获取数据文件类型名称对应枚举数据
        /// </summary>
        /// <param name="dateFileType">数据文件类型名称</param>
        /// <returns></returns>
		internal static DataFileType Get(string dateFileType)
		{
            // 将dateFileType字符串转换为小写形式
            dateFileType = dateFileType.ToLowerInvariant();

            if (dateFileType == "ascii")
            {
                return DataFileType.ASCII;
            }

            if (dateFileType == "binary")
            {
                return DataFileType.Binary;
            }

            if (dateFileType == "binary32")
            {
                return DataFileType.Binary32;
            }

            if (dateFileType == "float32")
            {
                return DataFileType.Float32;
            }
                
			throw new InvalidOperationException("Undefined *.dat file format");
		}
	}
		
	
}

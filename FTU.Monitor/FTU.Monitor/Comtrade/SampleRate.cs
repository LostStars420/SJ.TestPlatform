/*
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace FTU.Monitor.Comtrade
{
    /// <summary>
    /// SampleRate 的摘要说明
    /// author: songminghao
    /// date：2017/12/08 14:32:09
    /// desc：包含采样速率及该速率下最终采样数
    /// version: 1.0
    /// </summary>
	internal class SampleRate
	{
		/// <summary>
		/// 采样速率
        /// 单位：赫兹。必要字段，实数，数字，最小长度=1字符，最大长度=32个字符，可使用标准浮点标记法
		/// </summary>
		readonly public double samplingFrequency = 0;

        /// <summary>
        /// 在采样速率下，最终采样数。
        /// 必要字段，整数，数字，最小长度=1个字符，最大长度=10个字符，最小值=0，最大值=9999999999
        /// </summary>
		readonly public int lastSampleNumber=0;
		
        /// <summary>
        /// 有参构造方法
        /// </summary>
        /// <param name="sampleRateLine">一行包含采样速率及该速率下最终采样数的数据</param>
		public SampleRate(string sampleRateLine)
		{
            // 以逗号隔开，获取子字符串数组
			var values=sampleRateLine.Split(GlobalSettings.commaDelimiter);

            try
            {
                this.samplingFrequency = Convert.ToDouble(values[0].Trim(GlobalSettings.whiteSpace), System.Globalization.CultureInfo.InvariantCulture);
                this.lastSampleNumber = Convert.ToInt32(values[1].Trim(GlobalSettings.whiteSpace));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
			
		}
	}
}

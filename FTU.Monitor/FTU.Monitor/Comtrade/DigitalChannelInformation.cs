/*
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace FTU.Monitor.Comtrade
{
    /// <summary>
    /// DigitalChannelInformation 的摘要说明
    /// author: songminghao
    /// date：2017/12/08 14:32:09
    /// desc：状态（数字）通道信息类：Dn, ch_id, ph, ccbm, y
    /// version: 1.0
    /// </summary>
    public class DigitalChannelInformation
	{
        /// <summary>
        /// 状态通道索引编号Dn
        /// 状态通道索引编号：必要字段，数字，整数，最小长度=1个字符，最大长度=6个字符，最小值=1，最大值=999999.
        /// 不要求前导零或空格.顺序计数从1到状态通道的总数，不必考虑记录装置通道的数目
        /// </summary>
		private int index=0;
		
		/// <summary>
        /// 设置和获取状态通道索引编号Dn
		/// </summary>
		public int Index
		{
			get
            {
				return this.index;
			}
			internal set
            {
				this.index=value;
			}
		}

        /// <summary>
        /// 通道名ch_id
        /// 可选字段，字母数字，最小长度=0字符，最大长度=64字符
        /// readonly表示只能在构造方法中赋值或声明时赋值
        /// </summary>
		readonly public string name = string.Empty;

        /// <summary>
        /// 通道相别标识ph
        /// 可选字段，字母数字，最小长度=0字符，最大长度=2字符
        /// </summary>
		readonly public string phase = string.Empty;

        /// <summary>
        /// 被监视的电路元件ccbm
        /// 可选字段，字母数字，最小长度=0字符，最大长度=64字符
        /// </summary>
		readonly public string circuitComponent = string.Empty;

		/// <summary>
		/// 状态通道正常状态（仅应用于状态通道），即一次设备处于稳定运行时的输入状态。
        /// 必要字段，整数，数字，最小长度=1个字符，最大长度=1个字符。有效值仅为0或1。
        /// 状态通道的正常状态没有携带有关状态输入的实际表示信息，仅仅表示是无源触点（分或合）或是电压（带电或不带电）。其目的是
        /// 规定“1”代表正常还是异常状态
		/// </summary>
		readonly public bool normalState = false;
		
		/// <summary>
		/// 有参构造方法
		/// </summary>
        /// <param name="name">通道名</param>
        /// <param name="phase">通道相别标识</param>
		public DigitalChannelInformation(string name, string phase)
		{
			this.name = name;
			this.phase = phase;
		}
		
        /// <summary>
        /// 有参构造方法
        /// </summary>
        /// <param name="digitalLine">一行状态通道信息数据</param>
        internal DigitalChannelInformation(string digitalLine)
        {
            // 以逗号隔开，获取子字符串数组
			var values = digitalLine.Split(GlobalSettings.commaDelimiter);

            try
            {
                this.index = Convert.ToInt32(values[0].Trim(GlobalSettings.whiteSpace), System.Globalization.CultureInfo.InvariantCulture);
                this.name = values[1].Trim(GlobalSettings.whiteSpace);
                this.phase = values[2].Trim(GlobalSettings.whiteSpace);
                this.circuitComponent = values[3].Trim(GlobalSettings.whiteSpace);

                if (values.Length > 4)
                {
                    //some files not include this part of line
                    this.normalState = Convert.ToBoolean(Convert.ToInt32(values[4].Trim(GlobalSettings.whiteSpace), System.Globalization.CultureInfo.InvariantCulture));
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
			
		}
		
        /// <summary>
        /// 将状态通道信息内容转换为字符串
        /// </summary>
        /// <returns></returns>
		internal string ToCFGString()
		{
            return this.Index.ToString() + GlobalSettings.commaDelimiter
                + this.name + GlobalSettings.commaDelimiter
                + this.phase + GlobalSettings.commaDelimiter
                + this.circuitComponent + GlobalSettings.commaDelimiter
                + (this.normalState ? "1" : "0");
		}

	}
}

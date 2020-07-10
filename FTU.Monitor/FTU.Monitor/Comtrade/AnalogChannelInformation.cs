/*
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace FTU.Monitor.Comtrade
{
    /// <summary>
    /// AnalogChannelInformation 的摘要说明
    /// author: songminghao
    /// date：2017/12/08 11:44:09
    /// desc：模拟通道信息类：An, ch_id, ph, ccbm, uu, a, b, skew, min, max, primary, secondary, PS
    /// version: 1.0
    /// </summary>
	public class AnalogChannelInformation
	{
        /// <summary>
        /// 模拟通道索引编号An
        /// 模拟通道索引编号：必要字段，数字，整数，最小长度=1个字符，最大长度=6个字符，最小值=1，最大值=999999.
        /// 不要求前导零或空格.顺序计数从1到模拟通道的总数，不必考虑记录装置通道的数目
        /// </summary>
		private int index = 0;
		
		/// <summary>
        /// 设置和获取模拟通道索引编号An
		/// </summary>
		public int Index
        {
			get
            {
				return this.index;
			}
			internal set
            {
				this.index = value;
			}
		}		
		
		/// <summary>
		/// 通道标识ch_id
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
		/// 通道单位uu（例如，kV、kA）
        /// 必要字段，字母，最小长度=1字符，最大长度=32字符。如果存在物理量标准名称，物理量的单位应使用标准名称或
        /// IEEE/ANSI或IEC标准中规定的名称或缩写。数字倍率不应该包括在内，可采用标准的倍率，
        /// 如，k（千）、m（千分之一）、M（百万）
		/// </summary>
		readonly public string units = "NONE";
		
        /// <summary>
        /// 通道增益系数a
        /// 必要字段，实数，数字，最小长度=1字符，最大长度=32字符。可使用标准浮点标记法
        /// </summary>
		internal double a = 1.0;
	    
        /// <summary>
        /// 通道偏移因子b
        /// 必要字段，实数，数字，最小长度=1字符，最大长度=32字符。可使用标准浮点标记法
        /// 通道转换因子是ax+b。数据文件（.DAT）中“x”的存储数据值应对应于采用上述规定单位（uu）的ax+b采样值。
        /// 据数学分析规则，采样数据“x”乘以增益系数“a”，加上偏移因子“b”。通过转换因子对数据值处理还原为原始采样值。
        /// </summary>
		internal double b = 0;
		
        /// <summary>
        /// 从采样时段起始的通道时间时滞（单位：us）
        /// 可选字段，实数，最小长度=1字符，最大长度=32字符。可以使用标准浮点计数法
        /// 这个域提供在记录的采样时段内，各个通道的采样间时间差信息。如，在一个具有一个A/D转换器和8个通道的装置中，没有同步采样，
        /// 保持以1ms的采样率运行。第一次采样在时标timestamp表示的时间开始，在采样时段内，相邻通道的采样时间相互依次滞后125us。
        /// 这种情况下，相继通道的采样时滞为：0us、125us、250us、375us等
        /// </summary>
		readonly internal double skew = 0;
		
		/// <summary>
		/// 该通道数值范围中的最小值（可能数值范围内的下限）min
        /// 必要字段，整数，数字，最小长度=1个字符，最大长度=6个字符，最小值=-99999，最大值=99999
        /// （在二进制数据文件中，数值范围限制在-32767~+32767之间）
		/// </summary>
		private double min = float.MinValue;
		
		/// <summary>
        /// 设置和获取该通道数值范围中的最小值（可能数值范围内的下限）min
		/// </summary>
		public double Min
		{
			get
            {
				return this.min;
			}
			internal set
            {
				this.min=value;
			}			
		}

        /// <summary>
        /// 该通道数值范围中的最大值（可能数值范围内的上限）max
        /// 必要字段，整数，数字，最小长度=1个字符，最大长度=6个字符，最小值=-99999，最大值=99999
        /// （在二进制数据文件中，数值范围限制在-32767~+32767之间）
        /// </summary>
		private double max = float.MaxValue;

		/// <summary>
        /// 设置和获取该通道数值范围中的最大值（可能数值范围内的下限）max
		/// </summary>
		public double Max
		{
			get
            {
				return this.max;
			}
			internal set
            {
				this.max=value;
			}			
		}
		
		/// <summary>
		/// 通道电压或电流互感器变比一次因子primary
        /// 必要字段，实数，数字，最小长度=1个字符，最大长度=32字符
		/// </summary>
		readonly public double primary = 1.0;

		/// <summary>
        /// 通道电压或电流互感器变比二次因子secondary
        /// 必要字段，实数，数字，最小长度=1个字符，最大长度=32字符
		/// </summary>
		readonly public double secondary = 1.0;
		
        /// <summary>
        /// 说明通道转换因子方程ax+b得到的值还原为一次（P）还是二次（S）值的标识
        /// 必要字段，字母，数字，最小长度=1个字符，最大长度=1字符。有效字符仅为：p、P、s、S。详细说明请参见文档
        /// </summary>
		readonly internal bool isPrimary = true;	
		
		/// <summary>
		/// 有参构造方法
		/// </summary>
        /// <param name="name">通道标识</param>
        /// <param name="phase">通道相别标识</param>
		public AnalogChannelInformation(string name, string phase)
		{
			this.name = name;
			this.phase = phase;
		}
		
        /// <summary>
        /// 有参构造方法
        /// </summary>
        /// <param name="analogLine">一行模拟通道信息数据</param>
		internal AnalogChannelInformation(string analogLine)
		{
            // 以逗号隔开，获取子字符串数组
			var values=analogLine.Split(GlobalSettings.commaDelimiter);
            try
            {
                this.index = Convert.ToInt32(values[0].Trim(GlobalSettings.whiteSpace), System.Globalization.CultureInfo.InvariantCulture);
                this.name = values[1].Trim(GlobalSettings.whiteSpace);
                this.phase = values[2].Trim(GlobalSettings.whiteSpace);
                this.circuitComponent = values[3].Trim(GlobalSettings.whiteSpace);
                this.units = values[4].Trim(GlobalSettings.whiteSpace);
                this.a = Convert.ToDouble(values[5].Trim(GlobalSettings.whiteSpace), System.Globalization.CultureInfo.InvariantCulture);
                this.b = Convert.ToDouble(values[6].Trim(GlobalSettings.whiteSpace), System.Globalization.CultureInfo.InvariantCulture);
                this.skew = Convert.ToDouble(values[7].Trim(GlobalSettings.whiteSpace), System.Globalization.CultureInfo.InvariantCulture);
                this.min = Convert.ToDouble(values[8].Trim(GlobalSettings.whiteSpace), System.Globalization.CultureInfo.InvariantCulture);
                this.max = Convert.ToDouble(values[9].Trim(GlobalSettings.whiteSpace), System.Globalization.CultureInfo.InvariantCulture);
                this.primary = Convert.ToDouble(values[10].Trim(GlobalSettings.whiteSpace), System.Globalization.CultureInfo.InvariantCulture);
                this.secondary = Convert.ToDouble(values[11].Trim(GlobalSettings.whiteSpace), System.Globalization.CultureInfo.InvariantCulture);

                string isPrimaryText = values[12].Trim(GlobalSettings.whiteSpace);
                if (isPrimaryText == "S" || isPrimaryText == "s")
                {
                    this.isPrimary = false;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
						
		}
		
        /// <summary>
        /// 将模拟通道信息内容转换为字符串
        /// </summary>
        /// <returns></returns>
		internal string ToCFGString()
		{
            return this.Index.ToString() + GlobalSettings.commaDelimiter 
                + this.name + GlobalSettings.commaDelimiter
                + this.phase + GlobalSettings.commaDelimiter
                + this.circuitComponent + GlobalSettings.commaDelimiter
                + this.units + GlobalSettings.commaDelimiter
                + this.a.ToString(System.Globalization.CultureInfo.InvariantCulture) + GlobalSettings.commaDelimiter
                + this.b.ToString(System.Globalization.CultureInfo.InvariantCulture) + GlobalSettings.commaDelimiter
                + this.skew.ToString(System.Globalization.CultureInfo.InvariantCulture) + GlobalSettings.commaDelimiter
                + this.min.ToString(System.Globalization.CultureInfo.InvariantCulture) + GlobalSettings.commaDelimiter
                + this.max.ToString(System.Globalization.CultureInfo.InvariantCulture) + GlobalSettings.commaDelimiter
                + this.primary.ToString(System.Globalization.CultureInfo.InvariantCulture) + GlobalSettings.commaDelimiter
                + this.secondary.ToString(System.Globalization.CultureInfo.InvariantCulture) + GlobalSettings.commaDelimiter
                + (this.isPrimary ? "P" : "S");
		}

	}
}

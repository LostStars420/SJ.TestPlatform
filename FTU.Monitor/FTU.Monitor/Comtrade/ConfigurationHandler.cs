/*
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace FTU.Monitor.Comtrade
{
    /// <summary>
    /// ConfigurationHandler 的摘要说明
    /// author: songminghao
    /// date：2017/12/08 10:49:09
    /// desc：读录波配置文件（.cfg）类
    /// version: 1.0
    /// </summary>
    public class ConfigurationHandler
	{
        #region 第一行

		/// <summary>
		/// 厂站名
		/// </summary>
		private string stationName = string.Empty;

		/// <summary>
		/// 记录装置标识
		/// </summary>
        private string deviceId = string.Empty;

        /// <summary>
        /// Comtrade 标准版本年号（if not presented suppose that v1991）
        /// </summary>
		internal ComtradeVersion version = ComtradeVersion.V1991;

        #endregion 第一行

        #region 第二行

        /// <summary>
        /// 通道总数
        /// </summary>
        internal int total = 0;

        /// <summary>
        /// 模拟通道总数
        /// </summary>
		internal int analogChannelsCount = 0;

        /// <summary>
        /// 状态通道总数
        /// </summary>
		internal int digitalChannelsCount = 0;

        #endregion 第二行

        /// <summary>
        /// 模拟通道信息集合
        /// </summary>
        private List<AnalogChannelInformation> _analogChannelInformations;		

		/// <summary>
		/// 设置和获取模拟通道信息集合
		/// </summary>
        public IReadOnlyList<AnalogChannelInformation> AnalogChannelInformations
		{
			get
            {
				return this._analogChannelInformations;
			}
		}
	    
        /// <summary>
        /// 状态通道信息集合
        /// </summary>
		private List<DigitalChannelInformation> _digitalChannelInformations;

		/// <summary>
        /// 设置和获取状态通道信息集合
		/// </summary>
		public IReadOnlyList<DigitalChannelInformation> DigitalChannelInformations
		{
			get
            {
				return this._digitalChannelInformations;
			}
		}

		/// <summary>
		/// 通道频率lf，在文件中单独列一行
        /// lf：标称通道频率，单位：Hz（如，50、60、33.333）
        /// 可选字段，实数，数字，最小长度=0个字符，最大长度=32个字符。可采用标准浮点标记
		/// </summary>
		public double frequency = 50.0;

        #region 采样速率信息，包括采样速率信息和给定速率下数据采样数

        /// <summary>
        /// 数据文件中采样速率数
        /// 必要字段，整数，数字，最小长度=1个字符，最大长度=3个字符，最小值=0，最大值=999
        /// </summary>
        internal int samplingRateCount = 0;

        /// <summary>
        /// 包含采样速率及该速率下最终采样数的对象集合，长度和samplingRateCount一致
        /// </summary>
        internal List<SampleRate> sampleRates;

        #endregion 采样速率信息
        
        /// <summary>
        /// 数据文件类型
        /// 标识数据文件类型为ASCII文件，或是二进制文件
        /// 必要字段，字母，大小写无关，最小长度=5个字符，最大长度=6个字符。仅允许=ASCII或ascii，BINARY或binary
        /// </summary>
		internal DataFileType dataFileType = DataFileType.Undefined;	

        /// <summary>
        /// 时标倍率因子Timemult
        /// 该域用作数据文件中的时标（timestamp）域倍率因子，容许以COMTRADE格式存放长持续时间记录。时标以微秒为基本单位。
        /// 从数据文件中第一个数据采样到该数据文件中任一个时标的该数据采样所经过的时间等于该数据采样时标乘以配置文件中
        /// 时标倍率因子（timestamp * timemult）
        /// Timemult:数据文件中时间差域的倍率因子。
        /// 必要字段，实数，数字，最小长度=1个字符，最大长度=32个字符。可使用标准浮点标记法
        /// </summary>
		internal double timeMultiplicationFactor = 1.0;
		
        /// <summary>
        /// 有参构造方法
        /// </summary>
        /// <param name="fullPathToFileCFG">cfg配置文件全路径</param>
		internal ConfigurationHandler(string fullPathToFileCFG)
		{
			this.Parse(System.IO.File.ReadAllLines(fullPathToFileCFG));            
		}
		
        /// <summary>
        /// 字符串数组解析
        /// </summary>
        /// <param name="strings">字符串数组</param>
		internal void Parse(string[] strings)
		{
            // 解析配置文件中的第一行数据
			ParseFirstLine(strings[0]);
            // 解析配置文件中的第二行数据
			ParseSecondLine(strings[1]);
			
            // 初始化模拟通道信息集合
            this._analogChannelInformations = new List<AnalogChannelInformation>();
            for (int i = 0; i < this.analogChannelsCount; i++)
            {
                this._analogChannelInformations.Add(new AnalogChannelInformation(strings[2 + i]));
            }

            // 初始化状态通道信息集合
            this._digitalChannelInformations = new List<DigitalChannelInformation>();
            for (int i = 0; i < this.digitalChannelsCount; i++)
            {
                this._digitalChannelInformations.Add(new DigitalChannelInformation(strings[2 + this.analogChannelsCount + i]));
            }

            // 转到通道频率所在行索引
            var strIndex = 2 + this.analogChannelsCount + this.digitalChannelsCount;
            // 解析配置文件中的通道频率所在行数据
            ParseFrequenceLine(strings[strIndex++]);

            // 解析配置文件中的采样速率数所在行数据
			ParseNumberOfSampleRates(strings[strIndex++]);

            // 初始化包含采样速率及该速率下最终采样数的对象集合
            this.sampleRates = new List<SampleRate>();
            if (this.samplingRateCount == 0)
            {
                this.sampleRates.Add(new SampleRate(strings[strIndex++]));
            }
            else
            {
                for (int i = 0; i < this.samplingRateCount; i++)
                {
                    this.sampleRates.Add(new SampleRate(strings[strIndex + i]));
                }
                strIndex += this.samplingRateCount;
            }
			
			// 跳过两行日期/时标
            strIndex += 2;

            // 解析配置文件中的数据文件类型所在行数据
			ParseDataFileType(strings[strIndex++]);
			
			ParseTimeMultiplicationFactor(strings[strIndex++]);
			//strIndex++;
			
		}
		
        /// <summary>
        /// 解析配置文件中的第一行数据
        /// </summary>
        /// <param name="firstLine">第一行数据字符串</param>
		private void ParseFirstLine(string firstLine)
		{
            // 将第一行字符串中的空格转换为空字符串
			firstLine = firstLine.Replace(GlobalSettings.whiteSpace.ToString(), string.Empty);

            // 以逗号隔开，获取子字符串数组
			var values = firstLine.Split(GlobalSettings.commaDelimiter);

            if (values.Length <= 0)
            {
                return;
            }

            // 场站名
            this.stationName = values[0];

            if (values.Length == 2)
            {
                // 记录装置标识
                this.deviceId = values[1];
                return;
            }

			if(values.Length > 2)
            {
                // 标准版本年号
				this.version = ComtradeVersionConverter.Get(values[2]);
                return;
			}			
		}

        /// <summary>
        /// 解析配置文件中的第二行数据
        /// </summary>
        /// <param name="secondLine">第二行数据字符串</param>
        private void ParseSecondLine(string secondLine)
		{
            // 将第二行字符串中的空格转换为空字符串
			secondLine = secondLine.Replace(GlobalSettings.whiteSpace.ToString(), string.Empty);

            // 以逗号隔开，获取子字符串数组
			var values=secondLine.Split(GlobalSettings.commaDelimiter);

            if (values.Length != 3)
            {
                throw new Exception("录波配置文件(.cfg)第二数据通道编号和通道类型有错误");
            }

            try
            {
                this.total = Convert.ToInt32(values[0].Trim(GlobalSettings.whiteSpace), System.Globalization.CultureInfo.InvariantCulture);
                this.analogChannelsCount = Convert.ToInt32(values[1].TrimEnd('A'), System.Globalization.CultureInfo.InvariantCulture);
                this.digitalChannelsCount = Convert.ToInt32(values[2].TrimEnd('D'), System.Globalization.CultureInfo.InvariantCulture);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
		}
		
        /// <summary>
        /// 解析配置文件中的通道频率所在行数据
        /// </summary>
        /// <param name="frequenceLine">配置文件中的通道频率所在行数据</param>
		private void ParseFrequenceLine(string frequenceLine)
		{
            this.frequency = Convert.ToDouble(frequenceLine.Trim(GlobalSettings.whiteSpace), System.Globalization.CultureInfo.InvariantCulture);
		}
		
        /// <summary>
        /// 解析配置文件中的采样速率数所在行数据
        /// </summary>
        /// <param name="str">配置文件中的采样速率数所在行数据</param>
		private void ParseNumberOfSampleRates(string str)
		{
            this.samplingRateCount = Convert.ToInt32(str.Trim(GlobalSettings.whiteSpace), System.Globalization.CultureInfo.InvariantCulture);
		}
		
        /// <summary>
        /// 解析配置文件中的数据文件类型所在行数据
        /// </summary>
        /// <param name="str">配置文件中的数据文件类型所在行数据</param>
		private void ParseDataFileType(string str)
		{
            this.dataFileType = DataFileTypeConverter.Get(str.Trim(GlobalSettings.whiteSpace));
		}
		
        /// <summary>
        /// 解析配置文件中的时标倍率因子所在行数据
        /// </summary>
        /// <param name="str">配置文件中的时标倍率因子所在行数据</param>
		private void ParseTimeMultiplicationFactor(string str)
		{
            this.timeMultiplicationFactor = Convert.ToDouble(str.Trim(GlobalSettings.whiteSpace), System.Globalization.CultureInfo.InvariantCulture);
		}

        /// <summary>
        /// 将配置文件中的厂站名、标准版本年号、记录装置标识、模拟通道总数、状态通道总数、通道频率、采样速率数以及采样数集合大小转换为字符串
        /// </summary>
        /// <returns></returns>
        internal string ToCFGString()
        {
            return "stationName:" + this.stationName + "\n"
                + "version:" + this.version + "\n"
                + "deviceId:" + this.deviceId.ToString() + "\n"
                + "analogChannelsCount:" + this.analogChannelsCount.ToString() + "\n"
                + "digitalChannelsCount:" + this.digitalChannelsCount.ToString() + "\n"
                + "frequency:" + this.frequency.ToString() + "\n"
                + "samplingRateCount:" + this.samplingRateCount + "\n"
                + "sampleRates.Count:" + this.sampleRates.Count + "\n";
        }

	}
}
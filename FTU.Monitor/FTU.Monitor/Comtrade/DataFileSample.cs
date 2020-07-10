/*
 */
using System;
using System.Collections.Generic;

namespace FTU.Monitor.Comtrade
{
    /// <summary>
    /// DataFileSample 的摘要说明
    /// author: songminghao
    /// date：2017/12/08 23:26:09
    /// desc：录波数据文件每一行数据（.dat）类
    /// version: 1.0
    /// </summary>
	internal class DataFileSample
	{
        /// <summary>
        /// 采样编号
        /// 必要字段，数字，整数
        /// </summary>
		public int number;
		
		/// <summary>
		/// 时标
		/// </summary>
		public int timestamp;

        /// <summary>
        /// 模拟通道数据数组
        /// </summary>
		public double[] analogs;

        /// <summary>
        /// 状态通道数据数组
        /// </summary>
		public bool[] digitals;
		
        /// <summary>
        /// 有参构造方法
        /// </summary>
        /// <param name="number">采样编号</param>
        /// <param name="timestamp">时标</param>
        /// <param name="analogs">模拟通道数据数组</param>
        /// <param name="digitals">状态通道数据数组</param>
        public DataFileSample(int number, int timestamp, double[] analogs, bool[] digitals)
        {
            this.number = number;
            this.timestamp = timestamp;
            this.analogs = analogs;
            this.digitals = digitals;
        }
		
        /// <summary>
        /// 有参构造方法
        /// </summary>
        /// <param name="asciiLine">ASCII字符串行数据</param>
        /// <param name="analogCount">模拟通道总数</param>
        /// <param name="digitalCount">状态通道总数</param>
        public DataFileSample(string asciiLine, int analogCount, int digitalCount)
        {
            // 处理ASCII字符串行数据空格字符
            asciiLine = asciiLine.Replace(GlobalSettings.whiteSpace.ToString(), string.Empty);
            // 以逗号分隔ASCII字符串行数据，获取分隔后的数组
            var strings = asciiLine.Split(GlobalSettings.commaDelimiter);

            // 判断个数是否一致
            if (strings.Length != 2 + analogCount + digitalCount)
            {
                throw new Exception("录波配置文件中模拟通道个数和状态通道个数和ASCII类型的数据文件中每一行实际个数不一致，数据存在错误");
            }

            // 初始化模拟通道数据数组
            this.analogs = new double[analogCount];
            // 初始化状态通道数据数组
            this.digitals = new bool[digitalCount];

            this.number = Convert.ToInt32(strings[0]);
            this.timestamp = Convert.ToInt32(strings[1]);

            // 获取模拟通道数据数组数值
            for (int i = 0; i < analogCount; i++)
            {
                if (strings[i + 2] != string.Empty)
                {
                    //by Standart, can be missing value. In that case by default=0
                    this.analogs[i] = Convert.ToDouble(strings[i + 2], System.Globalization.CultureInfo.InvariantCulture);
                }
            }

            // 获取状态通道数据数组数值
            for (int i = 0; i < digitalCount; i++)
            {
                this.digitals[i] = Convert.ToBoolean(Convert.ToInt32(strings[i + 2 + analogCount]));
            }
        }
		
        /// <summary>
        /// 有参构造方法
        /// </summary>
        /// <param name="bytes">数据文件每一行字节数组</param>
        /// <param name="dataFileType">数据文件类型</param>
        /// <param name="analogCount">模拟通道总数</param>
        /// <param name="digitalCount">状态通道总数</param>
		public DataFileSample(byte[] bytes, DataFileType dataFileType, int analogCount, int digitalCount)
		{
            // 初始化模拟通道数据数组
            this.analogs = new double[analogCount];

            // 初始化状态通道数据数组
            this.digitals = new bool[digitalCount];

            // 采样编号
            this.number = System.BitConverter.ToInt32(bytes, 0);
            // 时标
            this.timestamp = System.BitConverter.ToInt32(bytes, 4);

            // 状态通道数据初始字节索引
            int digitalByteStart;

            // 判断数据文件类型是不是二进制格式
            if (dataFileType == DataFileType.Binary)
            {
                // 给模拟通道数据数组赋值
                for (int i = 0; i < analogCount; i++)
                {
                    // 将两个字节的数据转换为相应的模拟通道数据
                    this.analogs[i] = System.BitConverter.ToInt16(bytes, 8 + i * 2);
                }

                // 设置状态通道数据初始字节索引
                digitalByteStart = 8 + 2 * analogCount;
            }
            else
            {
                if (dataFileType == DataFileType.Binary32)
                {
                    for (int i = 0; i < analogCount; i++)
                    {
                        this.analogs[i] = System.BitConverter.ToInt32(bytes, 8 + i * 4);
                    }
                }
                else if (dataFileType == DataFileType.Float32)
                {
                    for (int i = 0; i < analogCount; i++)
                    {
                        this.analogs[i] = System.BitConverter.ToSingle(bytes, 8 + i * 4);
                    }
                }

                digitalByteStart = 8 + 4 * analogCount;
            }

            // 获取状态通道采样数据所占字节总数
            int digitalByteCount = DataFileHandler.GetDigitalByteCount(digitalCount);

            for (int i = 0; i < digitalCount; i++)
            {
                int digitalByteIterator = i / 8;
                // 获取每一个状态通道采样数据数值：第i个状态通道数据数值为：右移i位并和1相与（因为字节的最低位是状态通道的开始位）
                this.digitals[i] = Convert.ToBoolean((bytes[digitalByteStart + digitalByteIterator] >> (i - digitalByteIterator * 8)) & 1);
            }			
		}
		
		public string ToASCIIDAT()
		{
			string result=string.Empty;
			result+=this.number.ToString();
			result+=GlobalSettings.commaDelimiter+
				this.timestamp.ToString();
			foreach(var analog in this.analogs){
				result+=GlobalSettings.commaDelimiter+
					analog.ToString(System.Globalization.CultureInfo.InvariantCulture);
			}
			foreach(var digital in this.digitals){
				result+=GlobalSettings.commaDelimiter+
					System.Convert.ToInt32(digital).ToString(System.Globalization.CultureInfo.InvariantCulture);
			}			
			
			return result;
		}
		
		
		public byte[] ToByteDAT(DataFileType dataFileType, IReadOnlyList<AnalogChannelInformation> analogInformations)
		{
			var result=new byte[DataFileHandler.GetByteCount(this.analogs.Length,this.digitals.Length,dataFileType)];
			int analogOneChannelLength= dataFileType == DataFileType.Binary ? 2 : 4;
			int digitalByteStart=8+analogOneChannelLength*this.analogs.Length;
			
			System.BitConverter.GetBytes(this.number).CopyTo(result,0);
			System.BitConverter.GetBytes(this.timestamp).CopyTo(result,4);
			
			switch (dataFileType) {
				case DataFileType.Binary:
					this.AnalogsToBinaryDAT(result, analogInformations);
					break;
				case DataFileType.Binary32:
					this.AnalogsToBinary32DAT(result, analogInformations);
					break;
				case DataFileType.Float32:
					this.AnalogsToFloat32DAT(result);
					break;
				default:
					throw new InvalidOperationException("Not supported file type DAT");
			}
						
			this.DigitalsToDAT(result,digitalByteStart);
			return result;
		}
				
		void AnalogsToBinaryDAT(byte[] result, IReadOnlyList<AnalogChannelInformation> analogInformations)
		{			
			for(int i=0;i<this.analogs.Length;i++){
				short s=(short)((this.analogs[i]-analogInformations[i].b)/analogInformations[i].a);
				System.BitConverter.GetBytes(s).CopyTo(result,8+i*2);
			}			
		}	

		void AnalogsToBinary32DAT(byte[] result, IReadOnlyList<AnalogChannelInformation> analogInformations)
		{			
			for(int i=0;i<this.analogs.Length;i++){
				int s=(int)((this.analogs[i]-analogInformations[i].b)/analogInformations[i].a);
				System.BitConverter.GetBytes(s).CopyTo(result,8+i*4);
			}			
		}		
		
		void AnalogsToFloat32DAT(byte[] result)
		{
			for(int i=0;i<this.analogs.Length;i++){
				System.BitConverter.GetBytes((float)this.analogs[i]).CopyTo(result,8+i*4);
			}		
		}
		
		void DigitalsToDAT(byte[] result, int digitalByteStart)
		{
			int byteIndex=0;
			byte s=0;
			for(int i=0;i<this.digitals.Length;i++){				
				s=(byte)(System.Convert.ToInt32(s)|(System.Convert.ToInt32(this.digitals[i])<<(i-byteIndex*8)));
				
				if((i+1)%8==0 || (i+1)==this.digitals.Length){
					result[digitalByteStart+byteIndex]=s;
					s=0;
					byteIndex++;					
				}
			}
		}
	}
}

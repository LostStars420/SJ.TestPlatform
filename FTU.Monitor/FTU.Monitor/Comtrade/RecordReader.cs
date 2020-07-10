/*
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace FTU.Monitor.Comtrade
{
    /// <summary>
    /// RecordReader 的摘要说明
    /// author: songminghao
    /// date：2017/12/08 9:52:09
    /// desc：读录波文件数据类
    /// version: 1.0
    /// </summary>
    public class RecordReader
    {
		/// <summary>
		/// 读录波配置文件（.cfg）
		/// </summary>
        public ConfigurationHandler Configuration
        {
			get;
			private set;
		}
		
        /// <summary>
        /// 读录波数据文件（.dat）
        /// </summary>
        public	DataFileHandler data;

        /// <summary>
        /// 有参构造方法
        /// </summary>
        /// <param name="fullPathToFile">文件完整路径</param>
		public RecordReader(string fullPathToFile)
		{
			this.OpenFile(fullPathToFile);
		}
		
        /// <summary>
        /// 打开并读取文件内容
        /// </summary>
        /// <param name="fullPathToFile">文件完整路径</param>
		internal void OpenFile(string fullPathToFile)
		{			
            // 获取指定路径字符串的目录信息
			string path = System.IO.Path.GetDirectoryName(fullPathToFile);

            // 获取不具有扩展名的指定路径字符串的文件名
			string filenameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fullPathToFile);

            // 获取指定的路径字符串的扩展名
			string extension = System.IO.Path.GetExtension(fullPathToFile).ToLower();
			
            // 判断文件扩展名是不是'.cff'
			if(extension == GlobalSettings.extentionCFF){
				// 抛出异常
				throw new NotImplementedException("*.cff not supported");
			}

            // 判断文件扩展名是不是'.cfg'或'.dat'
            if (extension == GlobalSettings.extentionCFG || extension == GlobalSettings.extentionDAT)
            {
                // 读录波配置文件
                this.Configuration = new ConfigurationHandler(System.IO.Path.Combine(path, filenameWithoutExtension + ".cfg"));
                // 读录波数据文件
                this.data = new DataFileHandler(System.IO.Path.Combine(path, filenameWithoutExtension + ".dat"), this.Configuration);
            }
            else
            {
                throw new InvalidOperationException("Unsupported file extentions. Must be *.cfg, *.dat, *.cff");
            }
			
		}

        /// <summary>
        /// Get common for all channels set of timestamps
        /// </summary>
        /// <returns>In microSeconds</returns>
        public IReadOnlyList<double> GetTimeLine()
        {
            var list = new double[this.data.samples.Length];

            if (this.Configuration.samplingRateCount == 0 ||
               (Math.Abs(this.Configuration.sampleRates[0].samplingFrequency) < 0.01d))
            {
                //use timestamps in samples
                for (int i = 0; i < this.data.samples.Length; i++)
                {
                    list[i] = this.data.samples[i].timestamp * this.Configuration.timeMultiplicationFactor;
                }
            }
            else
            {//use calculated by samplingFrequency
                double currentTime = 0;
                int sampleRateIndex = 0;
                const double secondToMicrosecond = 1000000;
                for (int i = 0; i < this.data.samples.Length; i++)
                {
                    list[i] = currentTime;
                    if (i >= this.Configuration.sampleRates[sampleRateIndex].lastSampleNumber)
                    {
                        sampleRateIndex++;
                    }

                    currentTime += secondToMicrosecond / this.Configuration.sampleRates[sampleRateIndex].samplingFrequency;
                }
            }

            return list;
        }

        /// <summary>
        /// Return sequence of values choosen analog channel
        /// </summary>
        public IReadOnlyList<double> GetAnalogPrimaryChannel(int channelNumber)
        {
            double Kt = 1;
            if (this.Configuration.AnalogChannelInformations[channelNumber].isPrimary == false)
            {
                Kt = this.Configuration.AnalogChannelInformations[channelNumber].primary /
                    this.Configuration.AnalogChannelInformations[channelNumber].secondary;
            }

            var list = new double[this.data.samples.Length];
            for (int i = 0; i < this.data.samples.Length; i++)
            {
                list[i] = (this.data.samples[i].analogs[channelNumber] * this.Configuration.AnalogChannelInformations[channelNumber].a +
                         this.Configuration.AnalogChannelInformations[channelNumber].b) * Kt;
            }
            return list;
        }

        /// <summary>
        /// Return sequence of values choosen digital channel
        /// </summary>
        public IReadOnlyList<bool> GetDigitalChannel(int channelNumber)
        {
            var list = new bool[this.data.samples.Length];
            for (int i = 0; i < this.data.samples.Length; i++)
            {
                list[i] = this.data.samples[i].digitals[channelNumber];
            }
            return list;
        }
		
	}
}

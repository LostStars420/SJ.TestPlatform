using System;
/*
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System.Linq;

namespace FTU.Monitor.Comtrade
{
    /// <summary>
    /// DataFileHandler 的摘要说明
    /// author: songminghao
    /// date：2017/12/08 19:54:09
    /// desc：读录波数据文件（.dat）类
    /// version: 1.0
    /// </summary>
    public class DataFileHandler
    {
        /// <summary>
        /// 录波数据文件（.dat）每行数据解析后的对象集合
        /// </summary>
        internal DataFileSample[] samples;

        /// <summary>
        /// 采样编号所占字节大小
        /// </summary>
        internal const int sampleNumberLength = 4;

        /// <summary>
        /// 时标所占字节大小
        /// </summary>
        internal const int timeStampLength = 4;

        /// <summary>
        /// 状态采样数据每16个状态通道分为两字节一组
        /// </summary>
        internal const int digital16ChannelLength = 2;
        
        /// <summary>
        /// 获取状态通道采样数据所占字节总数
        /// </summary>
        /// <param name="digitalChannelsCount">状态通道总数</param>
        /// <returns></returns>
        internal static int GetDigitalByteCount(int digitalChannelsCount)
        {
            return (digitalChannelsCount / 16 + (digitalChannelsCount % 16 == 0 ? 0 : 1)) * DataFileHandler.digital16ChannelLength;
        }

        /// <summary>
        /// 获取二进制数据文件每一行字节总数
        /// </summary>
        /// <param name="analogsChannelsCount">模拟通道总数</param>
        /// <param name="digitalChannelsCount">状态通道总数</param>
        /// <param name="dataFileType">数据文件类型</param>
        /// <returns></returns>
        internal static int GetByteCount(int analogsChannelsCount, int digitalChannelsCount, DataFileType dataFileType)
        {
            // 单个模拟通道采样数据所占字节大小
            int analogOneChannelLength = dataFileType == DataFileType.Binary ? 2 : 4;

            // 返回文件字节总数
            return DataFileHandler.sampleNumberLength 
                + DataFileHandler.timeStampLength
                + analogsChannelsCount * analogOneChannelLength
                + DataFileHandler.GetDigitalByteCount(digitalChannelsCount);
        }

        /// <summary>
        /// dat数据文件采样数据总数
        /// </summary>
        public int samplesCount = 0;

        /// <summary>
        /// 有参构造方法
        /// </summary>
        /// <param name="fullPathToFileDAT">dat数据文件全路径</param>
        /// <param name="configuration">读录波配置文件（.cfg）类对象</param>
        internal DataFileHandler(string fullPathToFileDAT, ConfigurationHandler configuration)
        {
            if (configuration.sampleRates == null || configuration.sampleRates.Count == 0)
            {
                throw new Exception("录波配置文件采样频率下的采样个数不存在，数据存在错误");
            }

            // 获取dat数据文件采样数据总数
            for (int i = 0; i < configuration.sampleRates.Count; i++)
            {
                samplesCount += configuration.sampleRates[i].lastSampleNumber;
            }
            
            // 初始化集合
            this.samples = new DataFileSample[samplesCount];

            // 判断文件数据类型是不是二进制格式
            if (configuration.dataFileType == DataFileType.Binary || configuration.dataFileType == DataFileType.Binary32 || configuration.dataFileType == DataFileType.Float32)
            {
                // 读取文件所有数据字节
                var fileContent = System.IO.File.ReadAllBytes(fullPathToFileDAT);

                // 获取二进制数据文件每一行字节总数
                int oneSampleLength = DataFileHandler.GetByteCount(configuration.analogChannelsCount, configuration.digitalChannelsCount, configuration.dataFileType);

                if (samplesCount * oneSampleLength != Convert.ToInt32(fileContent.Length))
                {
                    throw new Exception("录波配置文件采样频率下的采样个数和数据文件中实际个数不一致，数据存在错误");
                }

                for (int i = 0; i < samplesCount; i++)
                {
                    var bytes = new byte[oneSampleLength];

                    // 获取每一行数据字节
                    for (int j = 0; j < oneSampleLength; j++)
                    {
                        bytes[j] = fileContent[i * oneSampleLength + j];
                    }

                    // 将每一行数据字节进行解析并保存到解析后的对象集合中
                    this.samples[i] = new DataFileSample(bytes, configuration.dataFileType, configuration.analogChannelsCount, configuration.digitalChannelsCount);

                }

                return;
                
            }

            // 判断文件数据类型是不是ASCII格式
            if (configuration.dataFileType == DataFileType.ASCII)
            {
                // 读取文件所有数据行
                var strings = System.IO.File.ReadAllLines(fullPathToFileDAT);

                //去除空字符串 removing empty strings (when *.dat file not following Standart)
                strings = strings.Where(x => x != string.Empty).ToArray();

                // 遍历dat数据文件采样数据总数
                for (int i = 0; i < samplesCount; i++)
                {
                    this.samples[i] = new DataFileSample(strings[i], configuration.analogChannelsCount, configuration.digitalChannelsCount);
                }

                return;
            }

        }

    }
}

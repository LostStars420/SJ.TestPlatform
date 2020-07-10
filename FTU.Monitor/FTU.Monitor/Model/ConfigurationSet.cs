using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;

namespace FTU.Monitor.Model
{
    /// <summary>
    /// ConfigurationSet 的摘要说明
    /// author: zhengshuiqing
    /// date：2017/10/10 21:25:23
    /// desc：下发参数配置Model
    /// version: 1.0
    /// </summary>
    public class ConfigurationSet : ObservableObject
    {

        #region 串口接口配置
        /// <summary>
        /// 使用串口列表
        /// </summary>
        private List<string> _UartPortItems;

        /// <summary>
        /// 设置和获取使用串口列表信息
        /// </summary>
        public List<string> UartPortItems
        {
            get
            {
                return this._UartPortItems;
            }
            set
            {
                this._UartPortItems = value;
                RaisePropertyChanged(() => UartPortItems);
            }
        }

        /// <summary>
        /// 串口列表索引
        /// </summary>
        private int _UartPortIndex;

        /// <summary>
        /// 设置和获取串口列表索引
        /// </summary>
        public int UartPortIndex
        {
            get
            {
                return this._UartPortIndex;
            }
            set
            {
                this._UartPortIndex = value;
                RaisePropertyChanged(() => UartPortIndex);
            }
        }
        /// <summary>
        /// 波特率列表
        /// </summary>
        private List<string> _UartBaudRateItems;

        /// <summary>
        /// 设置和获取波特率列表
        /// </summary>
        public List<string> UartBaudRateItems
        {
            get
            {
                return this._UartBaudRateItems;
            }
            set
            {
                this._UartBaudRateItems = value;
                RaisePropertyChanged(() => UartBaudRateItems);
            }
        }

        /// <summary>
        /// 波特率列表索引
        /// </summary>
        private int _UartBaudRateIndex;

        /// <summary>
        /// 设置和获取波特率列表索引
        /// </summary>
        public int UartBaudRateIndex
        {
            get
            {
                return this._UartBaudRateIndex;
            }
            set
            {
                this._UartBaudRateIndex = value;
                RaisePropertyChanged(() => UartBaudRateIndex);
            }
        }

        /// <summary>
        /// 数据位下拉列表
        /// </summary>
        private List<UInt16> _UartWordLengthItems;

        /// <summary>
        /// 设置和获取数据位下拉列表
        /// </summary>
        public List<UInt16> UartWordLengthItems
        {
            get
            {
                return this._UartWordLengthItems;
            }
            set
            {
                this._UartWordLengthItems = value;
                RaisePropertyChanged(() => UartWordLengthItems);
            }
        }

        /// <summary>
        /// 数据位下拉列表索引
        /// </summary>
        private int _UartWordLengthIndex;

        /// <summary>
        /// 设置和获取数据位下拉列表索引
        /// </summary>
        public int UartWordLengthIndex
        {
            get
            {
                return this._UartWordLengthIndex;
            }
            set
            {
                this._UartWordLengthIndex = value;
                RaisePropertyChanged(() => UartWordLengthIndex);
            }
        }

        /// <summary>
        /// 停止位下拉列表
        /// </summary>
        private List<UInt16> _UartStopBitsItems;

        /// <summary>
        /// 设置和获取停止位下拉列表
        /// </summary>
        public List<UInt16> UartStopBitsItems
        {
            get
            {
                return this._UartStopBitsItems;
            }
            set
            {
                this._UartStopBitsItems = value;
                RaisePropertyChanged(() => UartStopBitsItems);
            }
        }

        /// <summary>
        /// 停止位下拉列表索引
        /// </summary>
        private int _UartStopBitsIndex;

        /// <summary>
        /// 设置和获取停止位下拉列表索引
        /// </summary>
        public int UartStopBitsIndex
        {
            get
            {
                return this._UartStopBitsIndex;
            }
            set
            {
                this._UartStopBitsIndex = value;
                RaisePropertyChanged(() => UartStopBitsIndex);
            }
        }

        /// <summary>
        /// 校验位下拉列表
        /// </summary>
        private List<string> _UartParityItems;

        /// <summary>
        /// 设置和获取校验位下拉列表
        /// </summary>
        public List<string> UartParityItems
        {
            get
            {
                return this._UartParityItems;
            }
            set
            {
                this._UartParityItems = value;
                RaisePropertyChanged(() => UartParityItems);
            }
        }

        /// <summary>
        /// 校验位下拉列表索引
        /// </summary>
        private int _UartParityIndex;

        /// <summary>
        /// 设置和获取校验位下拉列表索引
        /// </summary>
        public int UartParityIndex
        {
            get
            {
                return this._UartParityIndex;
            }
            set
            {
                this._UartParityIndex = value;
                RaisePropertyChanged(() => UartParityIndex);
            }
        }

        #endregion 串口接口配置


        #region 串口应用配置

        /// <summary>
        /// 模式列表，非平衡1，平衡2
        /// </summary>
        private List<string> _UartBalanModeItems;

        /// <summary>
        /// 设置和获取模式列表
        /// </summary>
        public List<string> UartBalanModeItems
        {
            get
            {
                return this._UartBalanModeItems;
            }
            set
            {
                this._UartBalanModeItems = value;
                RaisePropertyChanged(() => UartBalanModeItems);
            }
        }

        /// <summary>
        /// 模式列表索引
        /// </summary>
        private int _UartBalanModeIndex;

        /// <summary>
        /// 设置和获取模式列表索引
        /// </summary>
        public int UartBalanModeIndex
        {
            get
            {
                return this._UartBalanModeIndex;
            }
            set
            {
                this._UartBalanModeIndex = value;
                RaisePropertyChanged(() => UartBalanModeIndex);
            }
        }

        /// <summary>
        /// 从站地址
        /// </summary>
        private UInt16 _UartSourceAddr;

        /// <summary>
        /// 设置和获取从站地址
        /// </summary>
        public UInt16 UartSourceAddr
        {
            get
            {
                return this._UartSourceAddr;
            }
            set
            {
                this._UartSourceAddr = value;
                RaisePropertyChanged(() => UartSourceAddr);
            }
        }

        /// <summary>
        /// 从站地址长度下拉列表
        /// </summary>
        private List<UInt16> _UartLinkAddrSizeItems;

        /// <summary>
        /// 设置和获取从站地址长度下拉列表
        /// </summary>
        public List<UInt16> UartLinkAddrSizeItems
        {
            get
            {
                return this._UartLinkAddrSizeItems;
            }
            set
            {
                this._UartLinkAddrSizeItems = value;
                RaisePropertyChanged(() => UartLinkAddrSizeItems);
            }
        }

        /// <summary>
        /// 从站地址长度下拉列表索引
        /// </summary>
        private int _UartLinkAddrSizeIndex;

        /// <summary>
        /// 设置和获取从站地址长度下拉列表索引
        /// </summary>
        public int UartLinkAddrSizeIndex
        {
            get
            {
                return this._UartLinkAddrSizeIndex;
            }
            set
            {
                this._UartLinkAddrSizeIndex = value;
                RaisePropertyChanged(() => UartLinkAddrSizeIndex);
            }
        }

        /// <summary>
        /// 传送原因长度下拉列表
        /// </summary>
        private List<UInt16> _UartASDUCotSizeItems;

        /// <summary>
        /// 设置和获取传送原因长度下拉列表
        /// </summary>
        public List<UInt16> UartASDUCotSizeItems
        {
            get
            {
                return this._UartASDUCotSizeItems;
            }
            set
            {
                this._UartASDUCotSizeItems = value;
                RaisePropertyChanged(() => UartASDUCotSizeItems);
            }
        }

        /// <summary>
        /// 传送原因长度下拉列表索引
        /// </summary>
        private int _UartASDUCotSizeIndex;

        /// <summary>
        /// 设置和获取传送原因长度下拉列表索引
        /// </summary>
        public int UartASDUCotSizeIndex
        {
            get
            {
                return this._UartASDUCotSizeIndex;
            }
            set
            {
                this._UartASDUCotSizeIndex = value;
                RaisePropertyChanged(() => UartASDUCotSizeIndex);
            }
        }

        /// <summary>
        /// ASDU地址
        /// </summary>
        private UInt16 _UartASDUAddr;

        /// <summary>
        /// 设置和获取ASDU地址
        /// </summary>
        public UInt16 UartASDUAddr
        {
            get
            {
                return this._UartASDUAddr;
            }
            set
            {
                this._UartASDUAddr = value;
                RaisePropertyChanged(() => UartASDUAddr);
            }
        }

        /// <summary>
        /// ASDU地址长度下拉列表
        /// </summary>
        private List<UInt16> _UartASDUAddrSizeItems;

        /// <summary>
        /// 设置和获取ASDU地址长度下拉列表
        /// </summary>
        public List<UInt16> UartASDUAddrSizeItems
        {
            get
            {
                return this._UartASDUAddrSizeItems;
            }
            set
            {
                this._UartASDUAddrSizeItems = value;
                RaisePropertyChanged(() => UartASDUAddrSizeItems);
            }
        }

        /// <summary>
        /// ASDU地址长度下拉列表索引
        /// </summary>
        private int _UartASDUAddrSizeIndex;

        /// <summary>
        /// 设置和获取ASDU地址长度下拉列表索引
        /// </summary>
        public int UartASDUAddrSizeIndex
        {
            get
            {
                return this._UartASDUAddrSizeIndex;
            }
            set
            {
                this._UartASDUAddrSizeIndex = value;
                RaisePropertyChanged(() => UartASDUAddrSizeIndex);
            }
        }

        #endregion 串口应用配置


        #region 网口接口配置

        /// <summary>
        /// MAC地址
        /// </summary>
        private UInt16[] _NetMac;

        /// <summary>
        /// 设置和获取MAC地址
        /// </summary>
        public UInt16[] NetMac
        {
            get
            {
                return this._NetMac;
            }
            set
            {
                this._NetMac = value;
                RaisePropertyChanged(() => NetMac);
            }
        }

        /// <summary>
        /// IP地址1
        /// </summary>
        private UInt16[] _NetIPOne;

        /// <summary>
        /// 设置和获取IP地址1
        /// </summary>
        public UInt16[] NetIPOne
        {
            get
            {
                return this._NetIPOne;
            }
            set
            {
                this._NetIPOne = value;
                RaisePropertyChanged(() => NetIPOne);
            }
        }

        /// <summary>
        /// IP地址2
        /// </summary>
        private UInt16[] _NetIPTwo;

        /// <summary>
        /// 设置和获取IP地址2
        /// </summary>
        public UInt16[] NetIPTwo
        {
            get
            {
                return this._NetIPTwo;
            }
            set
            {
                this._NetIPTwo = value;
                RaisePropertyChanged(() => NetIPTwo);
            }
        }

        /// <summary>
        /// 子网掩码
        /// </summary>
        private UInt16[] _NetNetmask;

        /// <summary>
        /// 设置和获取子网掩码
        /// </summary>
        public UInt16[] NetNetmask
        {
            get
            {
                return this._NetNetmask;
            }
            set
            {
                this._NetNetmask = value;
                RaisePropertyChanged(() => NetNetmask);
            }
        }

        /// <summary>
        /// 网关
        /// </summary>
        private UInt16[] _NetGateway;

        /// <summary>
        /// 设置和获取网关
        /// </summary>
        public UInt16[] NetGateway
        {
            get
            {
                return this._NetGateway;
            }
            set
            {
                this._NetGateway = value;
                RaisePropertyChanged(() => NetGateway);
            }
        }

        /// <summary>
        /// DNS信息
        /// </summary>
        private UInt16[] _NetDNS;

        /// <summary>
        /// 设置和获取DNS信息
        /// </summary>
        public UInt16[] NetDNS
        {
            get
            {
                return this._NetDNS;
            }
            set
            {
                this._NetDNS = value;
                RaisePropertyChanged(() => NetDNS);
            }
        }

        #endregion 网口接口配置


        #region 网口应用配置

        /// <summary>
        /// 从站地址
        /// </summary>
        private UInt16 _NetSourceAddr;

        /// <summary>
        /// 设置和获取从站地址
        /// </summary>
        public UInt16 NetSourceAddr
        {
            get
            {
                return this._NetSourceAddr;
            }
            set
            {
                this._NetSourceAddr = value;
                RaisePropertyChanged(() => NetSourceAddr);
            }
        }

        /// <summary>
        /// ASDU地址
        /// </summary>
        private UInt16 _NetASDUAddr;

        /// <summary>
        /// 设置和获取ASDU地址
        /// </summary>
        public UInt16 NetASDUAddr
        {
            get
            {
                return this._NetASDUAddr;
            }
            set
            {
                this._NetASDUAddr = value;
                RaisePropertyChanged(() => NetASDUAddr);
            }
        }

        #endregion 网口应用配置

        #region 设备参数配置

        /// <summary>
        /// 设备ID
        /// </summary>
        private string _DeviceID;

        /// <summary>
        /// 设置和获取设备ID
        /// </summary>
        public string DeviceID
        {
            get
            {
                return this._DeviceID;
            }
            set
            {
                this._DeviceID = value;
                RaisePropertyChanged(() => DeviceID);
            }
        }

        #endregion 设备参数配置


        #region 遥信配置

        /// <summary>
        /// 遥信原始点号
        /// </summary>
        private UInt16[] _YXAddr;

        /// <summary>
        /// 设置和获取遥信原始点号
        /// </summary>
        public UInt16[] YXAddr
        {
            get
            {
                return this._YXAddr;
            }
            set
            {
                this._YXAddr = value;
                RaisePropertyChanged(() => YXAddr);
            }
        }

        /// <summary>
        /// 双点标志
        /// </summary>
        private bool[] _YXDoublePoint;

        /// <summary>
        /// 设置和获取双点标志
        /// </summary>
        public bool[] YXDoublePoint
        {
            get
            {
                return this._YXDoublePoint;
            }
            set
            {
                this._YXDoublePoint = value;
                RaisePropertyChanged(() => YXDoublePoint);
            }
        }

        /// <summary>
        /// 变化标志
        /// </summary>
        private bool[] _YXIsChanged;

        /// <summary>
        /// 设置和获取变化标志
        /// </summary>
        public bool[] YXIsChanged
        {
            get
            {
                return this._YXIsChanged;
            }
            set
            {
                this._YXIsChanged = value;
                RaisePropertyChanged(() => YXIsChanged);
            }
        }

        /// <summary>
        /// 取反标志
        /// </summary>
        private bool[] _YXNegate;

        /// <summary>
        /// 设置和获取取反标志
        /// </summary>
        public bool[] YXNegate
        {
            get
            {
                return this._YXNegate;
            }
            set
            {
                this._YXNegate = value;
                RaisePropertyChanged(() => YXNegate);
            }
        }

        #endregion 遥信配置


        #region 遥测配置

        /// <summary>
        /// 遥测原始点号
        /// </summary>
        private UInt16[] _YCAddr;

        /// <summary>
        /// 设置和获取遥测原始点号
        /// </summary>
        public UInt16[] YCAddr
        {
            get
            {
                return this._YCAddr;
            }
            set
            {
                this._YCAddr = value;
                RaisePropertyChanged(() => YCAddr);
            }
        }

        #endregion 遥测配置


        #region 遥控配置

        /// <summary>
        /// 遥控原始点号
        /// </summary>
        private UInt16[] _YKAddr;

        /// <summary>
        /// 设置和获取遥控原始点号
        /// </summary>
        public UInt16[] YKAddr
        {
            get
            {
                return this._YKAddr;
            }
            set
            {
                this._YKAddr = value;
                RaisePropertyChanged(() => YKAddr);
            }
        }

        /// <summary>
        /// 取反标志
        /// </summary>
        private bool[] _YKNegate;

        /// <summary>
        /// 设置和获取取反标志
        /// </summary>
        public bool[] YKNegate
        {
            get
            {
                return this._YKNegate;
            }
            set
            {
                this._YKNegate = value;
                RaisePropertyChanged(() => YKNegate);
            }
        }

        #endregion 遥控配置

    }
}

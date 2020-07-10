using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTU.Monitor.Lib60870
{
    /// <summary>
    /// FixedValueParameterObject 的摘要说明
    /// author: songminghao
    /// date：2017/12/21 22:25:35
    /// desc：定值参数信息对象中包含的每个定值参数信息体
    /// version: 1.0
    /// </summary>
    public class FixedValueParameterObject
    {
        /// <summary>
        /// 信息体地址
        /// </summary>
        private int _IOA;

        /// <summary>
        /// 设置和获取信息体地址
        /// </summary>
        public int IOA
        {
            get
            {
                return this._IOA;
            }
            set
            {
                this._IOA = value;
            }
        }

        /// <summary>
        /// Tag类型
        /// </summary>
        private byte _tag;

        /// <summary>
        /// 设置和获取Tag类型
        /// </summary>
        public byte Tag
        {
            get
            {
                return this._tag;
            }
            set
            {
                this._tag = value;
            }
        }

        /// <summary>
        /// 数据长度
        /// </summary>
        private byte _len;

        /// <summary>
        /// 设置和获取数据长度
        /// </summary>
        public byte Len
        {
            get
            {
                return this._len;
            }
            set
            {
                this._len = value;
            }
        }

        /// <summary>
        /// 值字节数组
        /// </summary>
        private byte[] _valueBytes;

        /// <summary>
        /// 设置和获取值字节数组
        /// </summary>
        public byte[] ValueBytes
        {
            get
            {
                return this._valueBytes;
            }
            set
            {
                this._valueBytes = value;
            }
        }

        /// <summary>
        /// 无参构造方法
        /// </summary>
        public FixedValueParameterObject()
        {

        }

    }
}

using lib60870;
using System;
using System.Collections.Generic;

namespace FTU.Monitor.Lib60870
{
    /// <summary>
    /// FixedValueParameter 的摘要说明
    /// author: songminghao
    /// date：2017/12/21 21:07:35
    /// desc：读参数和定值信息对象
    /// version: 1.0
    /// </summary>
    public class FixedValueParameter : InformationObject
    {
        /// <summary>
        /// 重写父类的GetEncodedSize方法
        /// </summary>
        /// <returns></returns>
        public override int GetEncodedSize()
        {
            return 0;
        }

        /// <summary>
        /// 重写父类的Type方法,获取类型标识TypeID
        /// </summary>
        override public TypeID Type
        {
            get
            {
                // 返回读参数和定值标识202
                return TypeID.C_RS_NA_1;
            }
        }

        /// <summary>
        /// 支持顺序(可变结构限定词中的SQ位是否可以为1).返回true,代表顺序,SQ为1;返回false,代表单个,SQ为0
        /// </summary>
        override public bool SupportsSequence
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 定值区号SN,2个字节
        /// </summary>
        private UInt16 _SN;

        /// <summary>
        /// 设置和获取定值区号SN,2个字节
        /// </summary>
        public UInt16 SN
        {
            get
            {
                return this._SN;
            }
            set
            {
                this._SN = value;
            }
        }

        /// <summary>
        /// 参数特征标识,1个字节
        /// </summary>
        private byte _features;

        /// <summary>
        /// 设置和获取参数特征标识,1个字节
        /// </summary>
        public byte Features
        {
            get
            {
                return this._features;
            }
            set
            {
                this._features = value;
            }
        }

        /// <summary>
        /// 定值参数信息体对象集合
        /// </summary>
        private IList<FixedValueParameterObject> _fixedValueParameterObjectList;

        /// <summary>
        /// 设置和获取定值参数信息体对象集合
        /// </summary>
        public IList<FixedValueParameterObject> FixedValueParameterObjectList
        {
            get
            {
                return this._fixedValueParameterObjectList;
            }
            set
            {
                this._fixedValueParameterObjectList = value;
            }
        }

        /// <summary>
        /// 有参构造方法
        /// </summary>
        /// <param name="parameters">基础参数对象</param>
        /// <param name="msg">报文字节数组</param>
        /// <param name="startIndex">报文字节数组解析起始位置</param>
        /// <param name="count">可变结构限定词VSQ中number大小</param>
        internal FixedValueParameter(ConnectionParameters parameters, byte[] msg, int startIndex, int count)
        {
            // 初始化定值参数信息体对象集合
            this.FixedValueParameterObjectList = new List<FixedValueParameterObject>();

            // 获取定值区号SN
            this.SN = (UInt16)(msg[startIndex++] + msg[startIndex++] * 256);
            // 获取参数特征标识
            this.Features = msg[startIndex++];

            // 依次解析定值参数信息体
            for (int i = 0; i < count; i++)
            {
                // 定义定值参数信息体对象
                FixedValueParameterObject fixedValueParameterObject = new FixedValueParameterObject();

                // 获取定值参数信息体对象信息体地址
                fixedValueParameterObject.IOA = msg[startIndex++] + msg[startIndex++] * 256;
                if (parameters.SizeOfIOA == 3)
                {
                    fixedValueParameterObject.IOA += msg[startIndex++] * 65536;
                }

                // 获取定值参数信息体对象Tag类型
                fixedValueParameterObject.Tag = msg[startIndex++];
                // 获取定值参数信息体对象数据长度
                fixedValueParameterObject.Len = msg[startIndex++];

                // 获取定值参数信息体对象数据值字节数组
                fixedValueParameterObject.ValueBytes = new byte[fixedValueParameterObject.Len];
                for (int j = 0; j < fixedValueParameterObject.Len; j++)
                {
                    fixedValueParameterObject.ValueBytes[j] = msg[startIndex++];
                    this.FixedValueParameterObjectList.Add(fixedValueParameterObject);
                }
            }

        }

    }
}

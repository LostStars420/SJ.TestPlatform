using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTU.Monitor.Model
{
    /// <summary>
    /// COS 的摘要说明
    /// author: liyan
    /// date：2018/12/21 13:33:32
    /// desc：
    /// version: 1.0
    /// </summary>
    public class COS : ObservableObject
    {

        /// <summary>
        /// 序号
        /// </summary>
        private int _number;

        /// <summary>
        /// 获取和设置序号
        /// </summary>
        public int Number
        {
            get
            {
                return this._number;
            }
            set
            {
                this._number = value;
                RaisePropertyChanged(() => Number);
            }
        }

        /// <summary>
        /// SOE信息ID
        /// </summary>
        private string _ID;

        /// <summary>
        /// 获取和设置SOE信息ID
        /// </summary>
        public string ID
        {
            get
            {
                return this._ID;
            }
            set
            {
                this._ID = value;
                RaisePropertyChanged(() => ID);
            }
        }

        /// <summary>
        /// SOE信息名称
        /// </summary>
        private string _name;

        /// <summary>
        /// 获取和设置SOE信息名称
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        /// <summary>
        /// 遥控结果内容
        /// </summary>
        private string _content;

        /// <summary>
        /// 获取和设置遥控结果内容
        /// </summary>
        public string Content
        {
            get
            {
                return this._content;
            }
            set
            {
                this._content = value;
                RaisePropertyChanged(() => Content);
            }
        }

        /// <summary>
        /// 遥控值
        /// </summary>
        private int _value;

        /// <summary>
        /// 获取和设置遥控值
        /// </summary>
        public int Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
                RaisePropertyChanged(() => Value);
            }
        }

        /// <summary>
        /// 备注信息
        /// </summary>
        private string _comment;

        /// <summary>
        /// 获取和设置备注信息
        /// </summary>
        public string Comment
        {
            get
            {
                return this._comment;
            }
            set
            {
                this._comment = value;
                RaisePropertyChanged(() => Comment);
            }
        }
    }
}

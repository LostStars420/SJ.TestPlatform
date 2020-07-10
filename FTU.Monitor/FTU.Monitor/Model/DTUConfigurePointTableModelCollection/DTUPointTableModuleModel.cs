using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTU.Monitor.Model.DTUConfigurePointTableModelCollection
{
    /// <summary>
    /// DTUPointTableModuleModel 的摘要说明
    /// author: liyan
    /// date：2018/7/31 16:56:16
    /// desc：每个模块点表中三遥的模型
    /// version: 1.0
    /// </summary>
    public class DTUPointTableModuleModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 序号
        /// </summary>
        private int _number;

        /// <summary>
        /// 设置和获取序号
        /// </summary>
        public int Number
        {   get
            {
                return this._number;
            }
            set
            {
                this._number = value;
                OnPropertyChanged("Number");
            }
        }

        /// <summary>
        /// 点号
        /// </summary>
        private string _pointNumber;

        /// <summary>
        /// 设置和获取点号
        /// </summary>
        public string PointNumber
        {
            get
            {
                return this._pointNumber;
            }
            set
            {
                this._pointNumber = value;
            }
        }

        /// <summary>
        /// 点号对应的名称
        /// </summary>
        private string _pointName;

        /// <summary>
        /// 设置和获取点号对应的名称
        /// </summary>
        public string PointName
        {
            get
            {
                return this._pointName;
            }
            set
            {
                this._pointName = value;
            }
        }

        /// <summary>
        /// 当属性值发生改变时，更新显示界面
        /// </summary>
        /// <param name="propertyName"></param>
        protected internal virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}

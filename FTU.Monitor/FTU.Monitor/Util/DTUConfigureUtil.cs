using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTU.Monitor.Util
{
    /// <summary>
    /// DTUConfigureUtil 的摘要说明
    /// author: liyan
    /// date：2018/6/4 11:18:27
    /// desc：该类主要提供节点属性的枚举值，避免魔鬼数字
    /// version: 1.0
    /// </summary>
    public class DTUConfigureUtil
    {
        private static DTUConfigureUtil uniqueDTUConfigureUtilInstance;
        private static readonly object locker = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        private DTUConfigureUtil()
        {
            this._mSide = "M侧";
            this._nSide = "N侧";
            this._powerSupply = "供电链路";
            this._powerSupplyOne = "供电链路1";
            this._mSideChild = "M侧配置1";
            this._nSideChild = "N侧配置1";
            this._powerSupplyOneChild = "供电链路1配置1";
        }

        /// <summary>
        /// 定义公有方法提供一个全局访问点
        /// </summary>
        /// <returns></returns>
        public static DTUConfigureUtil GetInstance()
        {
            if(uniqueDTUConfigureUtilInstance == null)
            {
                bool? lockerBool = locker as bool?;
                if (lockerBool == null)
                {
                    if(uniqueDTUConfigureUtilInstance == null)
                    {
                        uniqueDTUConfigureUtilInstance = new DTUConfigureUtil();
                    }
                }
            }
            return uniqueDTUConfigureUtilInstance;
        }

        public enum NodeType
        {
            RootNode = 0,// 根节点 DTU配置项
            FirstLevelNode = 1,// 一级节点  表X
            SecondLevelNode = 2,// 二级节点 M N 供电链表
            ThirdLevelNode = 3,// 三级节点 供电链表1
            LeafNode = 4// 叶子节点 M配置项  N配置项  供电链表1配置项
        }

        /// <summary>
        /// "M侧"
        /// </summary>
        private string _mSide;

        /// <summary>
        /// 设置和获取“M侧”
        /// </summary>
        public string MSide
        {
            get
            {
                return _mSide;
            }
            set
            {
                _mSide = value;
            }
        }

        /// <summary>
        /// "N侧"
        /// </summary>
        private string _nSide;

        /// <summary>
        /// 设置和获取“N侧”
        /// </summary>
        public string NSide
        {
            get
            {
                return _nSide;
            }
            set
            {
                _nSide = value;
            }
        }

        /// <summary>
        /// "供电链路"
        /// </summary>
        private string _powerSupply;

        /// <summary>
        /// 设置和获取供电链路"
        /// </summary>
        public string PowerSupply
        {
            get
            {
                return _powerSupply;
            }
            set
            {
                _powerSupply = value;
            }
        }

        /// <summary>
        /// "供电链路1"
        /// </summary>
        private string _powerSupplyOne;

        /// <summary>
        /// 设置和获取"供电链路1"
        /// </summary>
        public string PowerSupplyOne
        {
            get
            {
                return _powerSupplyOne;
            }
            set
            {
                _powerSupplyOne = value;
            }
        }

        /// <summary>
        /// "M侧链路的子节点"
        /// </summary>
        private string _mSideChild;

        /// <summary>
        /// 设置和获取"M侧链路的子节点"
        /// </summary>
        public string MSideChild
        {
            get
            {
                return _mSideChild;
            }
            set
            {
                _mSideChild = value;
            }
        }

        /// <summary>
        /// "N侧链路的子节点"
        /// </summary>
        private string _nSideChild;

        /// <summary>
        /// 设置和获取"N侧链路的子节点"
        /// </summary>
        public string NSideChild
        {
            get
            {
                return _nSideChild;
            }
            set
            {
                _nSideChild = value;
            }
        }

        /// <summary>
        /// "供电链路1的子节点"
        /// </summary>
        private string _powerSupplyOneChild;

        /// <summary>
        /// 设置和获取"供电链路1的子节点"
        /// </summary>
        public string PowerSupplyOneChild
        {
            get
            {
                return _powerSupplyOneChild;
            }
            set
            {
                _powerSupplyOneChild = value;
            }
        }
    }
}

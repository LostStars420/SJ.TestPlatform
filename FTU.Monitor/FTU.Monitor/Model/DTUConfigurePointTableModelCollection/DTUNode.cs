using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTU.Monitor.Model.DTUConfigurePointTableModelCollection
{
    /// <summary>
    /// DTUNode 的摘要说明
    /// author: liyan
    /// date：2018/7/30 20:50:58
    /// desc：DTU三遥点表配置模板点表建模
    /// version: 1.0
    /// </summary>
    public class DTUNode
    {
        /// <summary>
        /// 节点ID
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        ///  节点名称
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 节点路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 节点类型
        /// </summary>
        public int NodeType { get; set; }

        /// <summary>
        /// 当前节点的所有子节点
        /// </summary>
        public List<DTUNode> Children { get; set; }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public DTUNode()
        {
            this.Children = new List<DTUNode>();
        }

        /// <summary>
        /// 寻找当前节点的父节点
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool FindParent(List<DTUNode> element)
        {
            string parentPath;
            int index = Path.LastIndexOf('/');
            if (index == -1)
            {
                return false;
            }
            else
            {
                parentPath = Path.Substring(0, index);
            }

            foreach (var e in element)
            {
                if (e.Path == parentPath)
                {
                    e.AddChild(this);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="child">所要添加子节点的节点</param>
        public void AddChild(DTUNode child)
        {
            Children.Add(child);
        }
    }
}

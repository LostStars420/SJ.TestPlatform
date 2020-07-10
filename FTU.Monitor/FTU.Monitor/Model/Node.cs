using System.Collections.Generic;

namespace FTU.Monitor.Model
{
    /// <summary>
    /// Node 的摘要说明 与数据库中DTUConfigure表格一一对应
    /// author: liyan
    /// date：2018/5/27 10:45:11
    /// desc：分布式参数配置单个节点的模型
    /// version: 1.0
    /// </summary>
    public class Node
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
        /// 配置项的IP  null:非配置项
        /// </summary>
        public string NodeIP { get; set; }

        /// <summary>
        /// 断路器或联络开关 0:断路器  1：联络开关
        /// </summary>
        public int NodeBreakersOrTieSwitch { get; set; }

        /// <summary>
        /// 支线或主线  0：主线 1：支线
        /// </summary>
        public int NodeMainOrBranchLine { get; set; }

        /// <summary>
        /// 当前节点的所有子节点
        /// </summary>
        public List<Node> Children { get; set; }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public Node()
        {
            this.Children = new List<Node>();
        }

        /// <summary>
        /// 寻找当前节点的父节点
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool FindParent(List<Node> element)
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
        public void AddChild(Node child)
        {
            Children.Add(child);
        }

    }
}

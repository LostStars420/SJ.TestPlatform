using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sojo.Checkplatform.libcollect
{
    public class IEC104Interface
    {
        /// <summary>
        /// 打开104连接
        /// </summary>
        public Func<string, int, bool> TestAct;

        /// <summary>
        /// 关闭104连接
        /// </summary>
        public Func<string, bool> CloseConnection; 

        /// <summary>
        /// 总召唤
        /// </summary>
        public Func<string, bool> GiAct;

        ///// <summary>
        ///// 遥控
        ///// </summary>
        //public Func<string, bool> TeleDoControl;

        /// <summary>
        /// 遥控选择
        /// </summary>
        public Func<string, bool> TeleSelect;

        /// <summary>
        /// 遥控动作
        /// </summary>
        public Func<string, bool> TeleAction;

        /// <summary>
        /// 取消
        /// </summary>
        public Func<string, bool> TeleCancel;

        /// <summary>
        /// 加载遥控点表
        /// </summary>
        public Func<object, bool> TeleReloadPoint;

        /// <summary>
        /// 遥控点号，单双点，分合的赋值
        /// </summary>
        public Func<int, byte, int, bool> ValueToParameter;

        /// <summary>
        /// 设置时间
        /// </summary>
        public Func<string, bool> Set;

        /// <summary>
        /// 读取时间
        /// </summary>
        public Func<string, bool> Read;

        /// <summary>
        /// 设置时间参数
        /// </summary>
        public Action<DateTime> SetTimeParam;

        /// <summary>
        /// 打开故障录波
        /// </summary>
        public Action<string> OpenFaultRecord;

    }

    public class IEC104InterfaceOut
    {
        /// <summary>
        /// 处理接收到的遥测数据
        /// </summary>
        public Func<List<Tuple<int, float>>, bool> GiComplted;


        /// <summary>
        /// 处理接收到的遥信数据
        /// </summary>
        public Func<List<Tuple<int, bool>>, bool> GetTelsa;


        /// <summary>
        /// 处理接收到的时间数据
        /// </summary>
        public Func<DateTime, int, bool> GetTimeData;


    }
}

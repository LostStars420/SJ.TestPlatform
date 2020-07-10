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
        /// 网络连接的委托
        /// </summary>
        public Func<string, int, bool> TestAct;
        /// <summary>
        /// 总召唤的委托
        /// </summary>
        public Func<string, bool> GiAct;




    }

    public class IEC104InterfaceOut
    {
        /// <summary>
        /// 获取列表的委托
        /// </summary>
        public Func<List<Tuple<int, float>>, bool> GiComplted;


    }
}

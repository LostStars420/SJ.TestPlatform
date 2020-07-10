using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sojo.TestPlatform.PlatformModel.Model
{
    /// <summary>
    /// 点表
    /// </summary>
    public struct TeleMeterPointTable
    {
        //电流点号
        public int Ia;   //A相电流对应的点号
        public int Ib;
        public int Ic;
        public int I0;

        //电压点号
        public int Ua;  //A相电压对应的点号
        public int Ub;
        public int Uc;
        public int U0;


        public TeleMeterPointTable(int ia, int ib, int ic, int i0, int ua, int ub, int uc, int u0)
        {
            Ia = ia;
            Ib = ib;
            Ic = ic;
            I0 = i0;
            Ua = ua;
            Ub = ub;
            Uc = uc;
            U0 = u0;
        }

    }
}

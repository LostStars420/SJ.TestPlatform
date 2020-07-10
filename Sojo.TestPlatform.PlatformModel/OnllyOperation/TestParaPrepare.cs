namespace Sojo.TestPlatform.PlatformModel.OnllyOperation
{
    using OnllyDataLib;
    // 使用 ONLLYTS 动态库中的函数
    // 定义 WIN32 中的变量类型
    using HRESULT = System.UInt32;
    using BOOL = System.Int32;

    public class TestParaPrepare
    {

        public readonly HRESULT S_OK = 0;
        public readonly BOOL TRUE = 1;
        public readonly BOOL FALSE = 0;

        public int m_nPowerMode;        //0--6路电流均 < 10A; 	1--任何一相>=10A;                               //


        //将用户试验数据修改转存到 ONLLY 数据对象
        //--------------------------------------------------------------------------------
        //对于电压电流, 数据对象的封装为
        //TestParams.Up.0.Mag       -- 注: 0,1,2...8 分别表示 9 路电压 Uabc,xyz,uvw
        //               .Ang
        //               .Fre
        //               .bDC
        //TestParams.Ip.0.Mag       -- 注: 0,1,2...8 分别表示 9 路电流 Iabc,xyz,uvw
        //               .Ang
        //               .Fre
        //               .bDC
        //TestParams.PowerMode      -- 电流源轻重载模式切换开关
        //--------------------------------------------------------------------------------
        public BOOL my_testPara_prepare(UISet[] m_Up, UISet[] m_Ip, IGenericData m_spTestParam)
        {
            string strPara;
            int i;

            m_nPowerMode = 0;

            //根据测试项目, 初始化数据对象
            HRESULT hr = OnllyTs.OTS_Win32.OTS_InitTestParams(m_spTestParam, OnllyTs.OTS_Win32.ONLLYTID_UI);
            if (hr != S_OK)
                return FALSE;

            //获取根节点
            IGenericDataNode ptrRoot = m_spTestParam.GetRootNode();
            if (ptrRoot == null)
                return FALSE;

            //给测试数据对象 m_spTestParam 赋值
            //6路电压
            for (i = 0; i < 6; i++)
            {
                strPara = "TestParams.Up." + string.Format("{0:D0}", i);
                ptrRoot.SetChildValue(strPara + ".Mag", m_Up[i].Mag);
                ptrRoot.SetChildValue(strPara + ".Ang", m_Up[i].Ang);
                ptrRoot.SetChildValue(strPara + ".Fre", m_Up[i].Fre);
                ptrRoot.SetChildValue(strPara + ".bDC", m_Up[i].bDC);
            }
            //6路电流
            for (i = 0; i < 6; i++)
            {
                strPara = "TestParams.Ip." + string.Format("{0:D0}", i);
                ptrRoot.SetChildValue(strPara + ".Mag", m_Ip[i].Mag);
                ptrRoot.SetChildValue(strPara + ".Ang", m_Ip[i].Ang);
                ptrRoot.SetChildValue(strPara + ".Fre", m_Ip[i].Fre);
                ptrRoot.SetChildValue(strPara + ".bDC", m_Ip[i].bDC);
            }
            //电流源轻重载模式
            ptrRoot.SetChildValue("TestParams.PowerMode", m_nPowerMode);
            ////动作判据
            //ptrRoot.SetChildValue("TestParams.bAndOr", 1);
            //ptrRoot.SetChildValue("TestParams.bBinSelect.0", 1);

            return TRUE;
        }


    }
}

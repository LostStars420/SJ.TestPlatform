using OnllyDataLib;
using Sojo.TestPlatform.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sojo.TestPlatform.PlatformModel.OnllyOperation
{

    // 定义 WIN32 中的变量类型
    using HRESULT = System.UInt32;
    using HANDLE = System.IntPtr;
    using HWND = System.IntPtr;
    using BOOL = System.Int32;
    using UINT16 = System.UInt16;
    using INT32 = System.Int32;
    using LONG = System.Int32;
    using ULONG = System.UInt32;
    using VARIANT = System.Object;
    using BYTE = System.Char;
    using WPARAM = System.Int32;
    using LPARAM = System.Int32;


    public class LinkDevice
    {
        private TestParaPrepare testParaPrepare;

        public readonly HRESULT S_OK = 0;
        public readonly BOOL TRUE = 1;
        public readonly BOOL FALSE = 0;


        public LinkDevice()
        {
            testParaPrepare = new TestParaPrepare();
        }


        /// <summary>
        /// 创建服务器连接对象
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public HRESULT CreateServer(HWND hWnd)
        {
            HRESULT hr = OnllyTs.OTS_Win32.OTS_CreateServer(hWnd);
            LogTrace.LogInformation("创建服务器连接对象：" + string.Format("{0:X8}", hr));
            return hr;
        }

        /// <summary>
        /// 销毁服务器连接对象
        /// </summary>
        public void DestroyServer()
        {
            OnllyTs.OTS_Win32.OTS_DestroyServer();
        }

        /// <summary>
        /// 连接设备
        /// </summary>
        /// <param name="strLink"></param>
        /// <returns></returns>
        public HRESULT ConnectDevice(string strLink)
        {
            HRESULT hr = OnllyTs.OTS_Win32.OTS_LinkDevice(strLink);
            return hr;
        }

        /// <summary>
        /// 激活测试
        /// </summary>
        /// <returns></returns>
        public HRESULT ActiveTest()
        {
            HRESULT hr = OnllyTs.OTS_Win32.OTS_ActiveTest(OnllyTs.OTS_Win32.ONLLYTID_UI);   //调用电压电流(手动测试)
            return hr;
        }

        /// <summary>
        /// 开始试验
        /// </summary>
        public HRESULT BeginTest(UISet[] m_Up, UISet[] m_Ip, IGenericData m_spTestParam)
        {
            HRESULT hr = 0;
            if (testParaPrepare.my_testPara_prepare(m_Up, m_Ip, m_spTestParam) == 1)
            {
                hr = OnllyTs.OTS_Win32.OTS_BeginTest(m_spTestParam);
            }
            return hr;
        }

        /// <summary>
        /// 结束试验
        /// </summary>
        public HRESULT StopTest()
        {
            HRESULT hr = OnllyTs.OTS_Win32.OTS_StopTest(0);
            return hr;
        }

        /// <summary>
        /// 切换测试点
        /// </summary>
        /// <returns></returns>
        public HRESULT SwitchTest(UISet[] m_Up, UISet[] m_Ip, IGenericData m_spTestParam)
        {
            HRESULT hr = 0;
            if (testParaPrepare.my_testPara_prepare(m_Up, m_Ip, m_spTestParam) == TRUE)
            {
                //切换测试点
               hr = OnllyTs.OTS_Win32.OTS_SwitchTestPoint(m_spTestParam);
            }
            return hr;
        }


        public string My_testResult_trace(OnllyResult_UI m_result)
        {
            IGenericData spTestParams = new GenericData();
            OnllyTs.OTS_Win32.OTS_GetTestResult(out spTestParams);
            //解析结果
            string strPara = null;
            if (spTestParams != null)
            {
                //获取根节点
                IGenericDataNode ptrRoot;
                ptrRoot = spTestParams.GetRootNode();
                if (ptrRoot != null)
                {
                    //获取结果接点
                    IGenericDataNode ptrResult;
                    ptrResult = ptrRoot.GetChild("TestParams.Result");
                    if (ptrResult != null)
                    {             
                        VARIANT var;

                        strPara = "TripFlag";
                        var = ptrResult.GetChildValue(strPara);
                        if (var is IntPtr)
                        {
                            string strTemp;
                            strTemp = Convert.ToString(var);
                            m_result.nTripFlag = Int32.Parse(strTemp);
                        }
                        else if (var is Int16)
                            m_result.nTripFlag = Convert.ToInt16(var);
                        else if (var is Int32)
                            m_result.nTripFlag = Convert.ToInt32(var);
                        else if (var is Int64)
                            m_result.nTripFlag = (int)Convert.ToInt64(var);
                        else
                            m_result.nTripFlag = 0;

                        strPara = "TripTime";
                        var = ptrResult.GetChildValue(strPara);
                        if (var is float)
                            m_result.fTripTime = Convert.ToSingle(var);
                        else
                            m_result.fTripTime = -1000.0f;

                        strPara = "Receive TestResult: TripFlag=";
                        strPara += string.Format("{0:D0}", m_result.nTripFlag);
                        strPara += ", TripTime=";
                        strPara += string.Format("{0:F3}", m_result.fTripTime);
                        strPara += "\r\n";
                    }
                }
            }
            return strPara;
        }


        

    }
}

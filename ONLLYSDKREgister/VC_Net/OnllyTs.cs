using System;

/// <summary>
/// Summary description for Class1
/// </summary>

//声明 OnllyTs.DLL(Win32) 中的非托管函数
namespace OnllyTs
{
    using System.Runtime.InteropServices;
    //声明 OnllyCom 组件使用
    using OnllyDataLib;
    using OnllyCalcEngineLib;
    //定义 WIN32 中的变量类型
    using HRESULT = System.UInt32;
    using HANDLE = System.IntPtr;
    using HWND = System.IntPtr;
    using BOOL = System.Int32;
    using UINT32 = System.UInt32;
    using UINT16 = System.UInt16;
    using LONG = System.Int32;
    using ULONG = System.UInt32;
    using VARIANT = System.Object;
    using WCHAR = System.Char;

    public class OTS_Win32
    {
        //Description: 
        //   创建测试服务
        //hDevMsgHostWnd为指定接收测试服务层消息的窗口句柄，消息为WM_ONLLY_DEVMSG，
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern HRESULT OTS_CreateServer(HWND hDevMsgHostWnd);

        //Description: 
        //   销毁测试服务
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public static extern HRESULT OTS_DestroyServer();

        //Description: 
        //   测试仪连机
        //   lpszLinkInfo="PC-IP:192.168.2.97; PC-PORT:2001; DEV-IP:192.168.2.231; DEV-PORT:2001;"
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public static extern HRESULT OTS_LinkDevice(string lpszLinkInfo);

        //Description: 
        //   与测试仪断开连接
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public static extern HRESULT OTS_DisconnectDevice();

        //Description: 
        //   查询开入开出端口状态
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public static extern HRESULT OTS_AskIoState();

        //Description: 
        //   发送开出量设置
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public unsafe static extern HRESULT OTS_TransBoutState(UINT16[] varBoutState, LONG nCount);

        //------------------------------------------------------------------
        // 测试组件TestID定义: 
        public const string ONLLYTID_UI = "Onlly.UITest.1";	        //电流电压
        public const string ONLLYTID_HARM = "Onlly.XBTest.1";	    //谐波试验
        public const string ONLLYTID_SHOT = "Onlly.ZZTest.1";	    //整组试验
        public const string ONLLYTID_STATE = "Onlly.ZTXLTest.1";    //状态序列(12U12I, 不支持递变)
        public const string ONLLYTID_RAMP = "Onlly.DBHCTest.1";	    //递变滑差
        public const string ONLLYTID_TRACK = "Onlly.FSXTXTest.1";	//反时限特性: I-t, V-t
        public const string ONLLYTID_SYN = "Onlly.ZDZTQTest.1";	    //自动准同期
        public const string ONLLYTID_TIMER = "Onlly.SJCLTest.1";	//时间测量: 3 个状态
        public const string ONLLYTID_DIFF = "Onlly.CDTest.1";	    //差动试验
        public const string ONLLYTID_WAVEPLAY = "Onlly.BXHFTest.1";	//波形回放
        public const string ONLLYTID_OSC = "Onlly.GLZDTest.1";	    //功率振荡
        //------------------------------------------------------------------
        //Description: 
        //   激活测试
        //   lpszTestID见上面 ONLLYTID_ 开头的定义
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public static extern HRESULT OTS_ActiveTest(string lpszTestID);

        //Description: 
        //   初始化试验参数(IGenericData 类型) 
        //   lpszTestID见上面 ONLLYTID_ 开头的定义
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public unsafe static extern HRESULT OTS_InitTestParams(IGenericData lpTestParams, string lpszTestID);

        //Description: 
        //   开始测试
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public unsafe static extern HRESULT OTS_BeginTest(IGenericData lpTestParams);

        //Description: 
        //   停止测试
        //   nFlag=0 -> 正常结束
        //   nFlag=1 -> 硬件故障结束
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public static extern HRESULT OTS_StopTest(LONG nFlag);

        //Description: 
        //   取得测试结果
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public unsafe static extern HRESULT OTS_GetTestResult(out IGenericData lppResult);

        //Description: 
        //   运行命令，扩展功能
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public unsafe static extern HRESULT OTS_Execute(ULONG nID, VARIANT pvcmd, VARIANT pret);

        //Description: 
        //   切换测试点
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public unsafe static extern HRESULT OTS_SwitchTestPoint(IGenericData lpTestParams);

        //Description: 
        //		获取 COM 组件返回的出错信息
        //      wcMsg:返回的信息字符串
        //      nSize:信息字符串的大小
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public unsafe static extern HRESULT OTS_GetFaultMsg_Com(HRESULT hr, WCHAR[] wcMsg, int nSize);

        //------------------------------------------------------------------
        //Description: 
        //   调用系统配置器界面, 显示和配置 UI 通道类型, PT/CT, 小信号比例, 数字量量程等
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public static extern HRESULT OTS_OnSystemConfig(HWND hParentWnd);

        //////////////////////////////////////////////////////////////////////////
        //数字化测试仪专用：下载 61850 配置
        //////////////////////////////////////////////////////////////////////////
        // 下载IEC61850配置程序保存的文件
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public static extern BOOL OTS_DownLoadF66CfgFile(string path);

        //////////////////////////////////////////////////////////////////////////
        //辅助的计算功能函数 
        //////////////////////////////////////////////////////////////////////////
        //Description: 
        //   获取计算引擎
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public unsafe static extern HRESULT OTS_GetCalcEngine(out ICalcEngine lppCalcEngine);

        //----------------------------------------------------------------------
        //计算功能编码 nCalcID
        //----------------------------------------------------------------------
        public const ULONG CALCID_LOADSTATE = 0;	//正常运行负荷状态计算
        public const ULONG CALCID_FAULT = 1;    	//计算短路: 常规计算模型
        public const ULONG CALCID_GPFAULT = 2;  	//计算短路: 工频变化量计算模型
        public const ULONG CALCID_ABCTOPP = 3;  	//线分量计算: ABC -> PP
        public const ULONG CALCID_PPTOABC = 4;  	//            PP -> ABC
        public const ULONG CALCID_ABCTO120 = 5; 	//序分量计算: ABC -> 120
        public const ULONG CALCID_120TOABC = 6; 	//            120 -> ABC
        public const ULONG CALCID_ABCTOPQ = 7;  	//计算三相功率

        public const ULONG CALCID_SHOT = 100;   	    //整组试验计算: 根据界面参数计算3个状态下的电压电流(三相UI)
        public const ULONG CALCID_DISTANCE = 101;	    //距离保护定值校验计算
        public const ULONG CALCID_GPDISTANCE = 102;     //工频变化量阻抗保护定值校验计算
        public const ULONG CALCID_OVERCURR = 103;       //正序过流保护定值校验计算
        public const ULONG CALCID_NEGSEQCURR = 104;     //负序过流保护定值校验计算
        public const ULONG CALCID_ZEROSEQCURR = 105;	//零序过流保护定值校验计算
        //------------------------------------------------------------------
        //Description: 
        //   初始化计算参数(GenericData 类型) 
        //   nCalcID见上面 CALCID_ 开头的定义
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public unsafe static extern HRESULT OTS_InitCalcParams(IGenericData lpCalcParams, ULONG nCalcID);
 
	}
}

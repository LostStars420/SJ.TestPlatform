#pragma once

#include "OnllyDef.h"
//--------------------------------------------------------------------------------------------
#import "OnllyData.tlb" named_guids no_namespace raw_interfaces_only
#import "OnllyCalcEngine.tlb" named_guids no_namespace raw_interfaces_only
//--------------------------------------------------------------------------------------------

#ifndef _ONLLY_OTS_STUB
	#pragma comment(lib,"OnllyTS.lib")
#endif
//--------------------------------------------------------------------------------------------
// 测试组件ID定义
//--------------------------------------------------------------------------------------------
#define ONLLYTID_UI			_T("Onlly.UITest.1")	//电流电压
#define ONLLYTID_HARM		_T("Onlly.XBTest.1")	//谐波试验
#define ONLLYTID_SHOT		_T("Onlly.ZZTest.1")	//整组试验
//
#define ONLLYTID_STATE			_T("Onlly.ZTXLTest.1")	//状态序列(12U12I, 不支持递变)
#define ONLLYTID_STATERAMP		_T("Onlly.StateRamp.1")	//状态序列(6U6I, 支持递变)
//
#define ONLLYTID_RAMP		_T("Onlly.DBHCTest.1")	//递变滑差
#define ONLLYTID_TRACK		_T("Onlly.FSXTXTest.1")	//反时限特性: I-t, V-t
#define ONLLYTID_SYN		_T("Onlly.ZDZTQTest.1")	//自动准同期
#define ONLLYTID_TIMER		_T("Onlly.SJCLTest.1")	//时间测量: 3 个状态
#define ONLLYTID_DIFF		_T("Onlly.CDTest.1")	//差动试验
#define ONLLYTID_WAVEPLAY	_T("Onlly.BXHFTest.1")	//波形回放
#define ONLLYTID_OSC		_T("Onlly.GLZDTest.1")	//功率振荡
#define ONLLYTID_SZNARI_STATE _T("Onlly.SzNari_State.1")	//深圳南瑞专用测试
#define ONLLYTID_COMP		_T("Onlly.AdjTest.1")	//Adj 出厂调试--补偿系数
#define ONLLYTID_DDL		_T("Onlly.DDL.1")		//地铁直流保护

//开入开出装置测试 IOTEST
#define ONLLYTID_IOTEST_TIMER	_T("Onlly.IOTest_Timer.1")	//跳闸矩阵计时测试
#define ONLLYTID_IOTEST_SOE		_T("Onlly.IOTest_SOE.1")	//SOE 测试
#define ONLLYTID_IOTEST_STATE	_T("Onlly.IOTest_State.1")	//状态序列


//--------------------------------------------------------------------------------------------
//计算功能编码 nCalcID(注意: 必须同..\Include\OnllyDef_Calc.h 中的定义相符)
//--------------------------------------------------------------------------------------------
#define CALCID_LOADSTATE	0		//正常运行负荷状态计算
#define	CALCID_FAULT		1		//计算短路: 常规计算模型
#define CALCID_GPFAULT		2		//计算短路: 工频变化量计算模型
#define CALCID_ABCTOPP		3		//线分量计算: ABC -> PP
#define CALCID_PPTOABC		4		//            PP -> ABC
#define CALCID_ABCTO120		5		//序分量计算: ABC -> 120
#define CALCID_120TOABC		6		//            120 -> ABC
#define CALCID_ABCTOPQ		7		//计算三相功率

#define CALCID_SHOT			100		//整组试验计算: 根据界面参数计算3个状态下的电压电流(三相UI)
#define CALCID_DISTANCE		101		//距离保护定值校验计算
#define CALCID_GPDISTANCE	102		//工频变化量阻抗保护定值校验计算
#define CALCID_OVERCURR		103		//过流保护定值校验计算
#define CALCID_NEGSEQCURR	104		//负序过流保护定值校验计算
#define CALCID_ZEROSEQCURR	105		//零序过流保护定值校验计算

#define CALCID_DIFF			120		//差动计算: 根据 Id,Ir 计算两侧所加电流 Iabc,Ixyz
#define CALCID_DIFF_KP		121		//差动计算: 补偿系数 KP123 辅助计算
#define CALCID_DIFF_SHOT	122		//差动计算: 差动整组
#define CALCID_DIFF_IRID	123		//差动计算: 根据 I1[3], I2[3] 计算三相差流和制流

#define CALCID_DIFF5		130		//电铁差动: 根据 Id,Ir 计算两侧所加电流 Iabc,Ixy

#define CALCID_OSC			140		//功率振荡: 当前发电机角下, 保护安装处 K 点的电压电流有效值
#define CALCID_OSC_ZK		141		//功率振荡: 当前发电机角下, 保护安装处 K 点的测量阻抗
//--------------------------------------------------------------------------------------------


//--------------------------------------------------------------------------------------------
//Description: 
//   创建测试服务
//   hDevMsgHostWnd为指定接收测试服务层消息的窗口句柄，消息为WM_ONLLY_DEVMSG，
//   见上面定义
HRESULT OTS_CreateServer(HWND hDevMsgHostWnd);

//Description: 
//   销毁测试服务
HRESULT OTS_DestroyServer();

//Description: 
//   测试仪联机
//   lpszLinkInfo=L"PC-IP:192.168.2.97; PC-PORT:2001; DEV-IP:192.168.2.231; DEV-PORT:2001; BSETHOSTPORT:1"
HRESULT OTS_LinkDevice(const WCHAR* lpszLinkInfo);
HRESULT OTS_LinkDevice_W(const WCHAR* lpszLinkInfo);
HRESULT OTS_LinkDevice_A(const char* lpszLinkInfo);

//Description: 
//   与测试仪断开连接
HRESULT OTS_DisconnectDevice();

//Description: 
//   查询开入开出状态
HRESULT OTS_AskIoState();

//Description: 
//   发送开出量设置
HRESULT OTS_TransBoutState(UINT16* varBoutState,LONG nCount);

//Description: 
//   激活测试
//   lpszTestID见上面 ONLLYTID_ 开头的定义
HRESULT OTS_ActiveTest(const WCHAR* lpszTestID);
HRESULT OTS_ActiveTest_W(const WCHAR* lpszTestID);
HRESULT OTS_ActiveTest_A(const char* lpszTestID);

//Description: 
//   初始化试验参数(GenericData 类型) 
//   lpszTestID见上面 ONLLYTID_ 开头的定义
HRESULT OTS_InitTestParams(IGenericData* lpTestParams, const WCHAR* lpszTestID);
HRESULT OTS_InitTestParams_W(IGenericData* lpTestParams, const WCHAR* lpszTestID);
HRESULT OTS_InitTestParams_A(IGenericData* lpTestParams, const char* lpszTestID);

//Description: 
//   开始测试
HRESULT OTS_BeginTest(IGenericData* lpTestParams);

//////////////////////////////////////////////////////////////////////////
//开始试验也可拆分为以下两个函数调用:
//   测试准备
HRESULT OTS_TestPrepare(IGenericData* lpTestParams);

//   测试运行
HRESULT OTS_TestRun();
//////////////////////////////////////////////////////////////////////////


//Description: 
//   停止测试
//   nFlag=0 -> 正常结束
//   nFlag=1 -> 硬件故障结束
HRESULT OTS_StopTest(LONG nFlag);

//Description: 
//   获取测试结果
HRESULT OTS_GetTestResult(IGenericData** lppResult);

//Description: 
//   运行命令，扩展功能, 如手动触发, PPM 触发等
HRESULT OTS_Execute(ULONG nID, VARIANT* pvcmd,VARIANT* pret);

//Description: 
//   切换测试点
HRESULT OTS_SwitchTestPoint(IGenericData* lpTestParams);

//Description: 
//   获取计算引擎
HRESULT OTS_GetCalcEngine(ICalcEngine**lppCalcEngine);

//Description: 
//   初始化计算参数(GenericData 类型) 
//   nCalcID见上面的定义
HRESULT OTS_InitCalcParams(IGenericData* lpCalcParams, ULONG nCalcID);

//Description: 
//   调用系统配置器界面, 显示和配置 UI 通道类型, PT/CT, 小信号比例, 数字量量程等
HRESULT OTS_OnSystemConfig(HWND hParentWnd);

//Description: 
//   请求查询 GPS 时间
//传入字符串格式: "GpsPos=2;Com=1;BaudRate=9600;Parity=0;StopBits=1;DataBits=8;"
//            其中 GpsPos=2;       0--内置GPS, 1--外接 GPS(测试仪), 2--外接 GPS(PC机);
//                 Com=1;          表示GPS所接 COM 的端口号, 1 表示串口1, 2 表示串口2, ...
//			       BaudRate=9600;  表示GPS所接 COM 的波特率
//			       Parity=0;       0--无校验, 1--奇校验, 2--偶校验  
//			       StopBits=1;     停止位, 1, 或 2;
//			       DataBits=8;     数据位, 7, 或 8;
HRESULT	OTS_QueryGPSTime(const WCHAR* lpszGPSLinkInfo);
HRESULT	OTS_QueryGPSTime_W(const WCHAR* lpszGPSLinkInfo);
HRESULT	OTS_QueryGPSTime_A(const char* lpszGPSLinkInfo);

//Description: 
//   获取 DA 的 模拟量输出范围模, 即区间长度
float OTS_GetDAAM();

//Description: 
//   获取硬件故障告警信息
//   wcMsg:返回的信息字符串
//   nSize:信息字符串的大小
void OTS_GetFaultMsg(DWORD dwMacType,UINT64 uFaultMsg,WCHAR* wcMsg,int nSize);
void OTS_GetFaultMsg_W(DWORD dwMacType,UINT64 uFaultMsg,WCHAR* wcMsg,int nSize);
void OTS_GetFaultMsg_A(DWORD dwMacType,UINT64 uFaultMsg,char* wcMsg,int nSize);

//Description: 
//		获取 COM 组件返回的出错信息
//      wcMsg:返回的信息字符串
//      nSize:信息字符串的大小
void OTS_GetFaultMsg_Com(HRESULT hr,WCHAR* wcMsg,int nSize);
void OTS_GetFaultMsg_Com_W(HRESULT hr,WCHAR* wcMsg,int nSize);
void OTS_GetFaultMsg_Com_A(HRESULT hr,char* wcMsg,int nSize);

//Description: 
//   测试仪内置 GPS 的对时模式设置
HRESULT OTS_SetGPSTimeMode128(UINT64 nMode1, UINT64 nMode2, BOOL bSaveDefault);


// 2015-10-21  zy 新增同步时间（ns）发送，用于实现数模同步
HRESULT OTS_SetSynTime(long nSynTime_ns);


//---------------------- ONLLY-IO 开入开出测试装置使用 --------------------
//Description: 
//   查询开入开出状态
HRESULT OTS_AskIoState_Ex();

//Description: 
//   发送开出量设置
HRESULT OTS_TransBoutState_Ex(UINT16* varBoutState,LONG BoutState_Count);


HRESULT OTS_SetOnllyConfigInfo(IGenericData* lpCfgParams);



//////////////////////////////////////////////////////////////////////////
//  增加对 821 设备的通信功能 
//////////////////////////////////////////////////////////////////////////

#define OTS_DEV701   0
#define OTS_DEV821_1 1
#define OTS_DEV821_2 2

HRESULT OTS_SendMessageToDevice(UINT32 DevID, UINT16 TestID, 
		UINT16 MsgID, UINT32 lInLength, const void* pInData,
		UINT32 lOutLength, void* pOutData);

// 下载IEC61850配置程序保存的文件
BOOL OTS_DownLoadF66CfgFile(const WCHAR* path);
BOOL OTS_DownLoadF66CfgFile_W(const WCHAR* path);
BOOL OTS_DownLoadF66CfgFile_A(const char* path);

/***************************************************************************
//-------------------------------------------------------------------------
//以下函数仅供 ONLLY 内部使用, 转移到 ONLLYSS.H 中定义
//-------------------------------------------------------------------------
//Description: 
//   取得硬件基本信息
const OnllyDeviceBaseInfo& OTS_GetBaseDeviceInfo();

//Description: 
//   获取 UI 通道类型配置信息, 含 PT, CT
const OnllyCfgData& OTS_GetCFGPTCT();

//Description: 
//   获取系统配置信息, 小信号比例, 数字量量程, 额定频率, 是否默认显示一次侧等
const OnllySystemSet& OTS_GetSystemSet();

//Description: 
//   获取 ONLLY TestServer
//Return:
//S_OK
HRESULT OTS_GetTestServer(IDispatch ** lppTestServer);

//Description: 
//   系统配置数据存储交换
//Return:
//S_OK
HRESULT OTS_DoConfigDataExchange(IGenericDataNode* lpData,BOOL bSave);

//Description: 
//   查询硬件信息
//   lpszDevInfoID = "LINK" -- 查询当前的联机字符串
//                   "DEVBASEINFO.TestSampRate" -- 查询当前机器的采样率
//                   ...
//   *pvar: 保存返回信息
HRESULT OTS_GetDeviceInfo(const WCHAR* lpszDevInfoID,VARIANT*pvar);
HRESULT OTS_GetDeviceInfo_W(const WCHAR* lpszDevInfoID,VARIANT*pvar);
HRESULT OTS_GetDeviceInfo_A(const char* lpszDevInfoID,VARIANT*pvar);

***************************************************************************/
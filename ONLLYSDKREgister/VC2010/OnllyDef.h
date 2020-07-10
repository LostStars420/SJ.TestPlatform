//*
//* file: OnllyDef.h
//* 


//测试仪最多能支持的电压或电流通道数
#define MAX_CHANNEL_COUNT     16

//测试仪最多能支持的总通道数
#define TOTAL_CHANNEL_COUNT   32

// 测试仪最多能支持的开入量或开出量个数
#define MAX_IO_COUNT          16

// 测试仪最多能支持的总开关量个数
#define TOTAL_IO_COUNT        32

//测试仪ID最大长度
#define MAX_MACHINE_ID_LENGTH 32


//=========================================支持的输出信号类型==============================================================

//小信号
#define ONLLY_DevAdjSignalType_XXH    0x10000L

//数字量
#define ONLLY_DevAdjSignalType_SZL_91 0x20000L
#define ONLLY_DevAdjSignalType_SZL_92 0x20001L

//功放
#define ONLLY_DevAdjSignalType_GF     0x30001L



//=========================================测试仪硬件型号定义=============================================================


//DevType 高16位表示硬件主型号
#define ONLLY_DEVTYPE_UNKNOWN   0x0000 //未知
#define ONLLY_DEVTYPE_A        0x0001 //A系列
#define ONLLY_DEVTYPE_D        0x0002 //D系列
#define ONLLY_DEVTYPE_F        0x0003 //F系列


//DevType 低16位表示硬件子型号

//---------------------A 系列测试仪型号定义-------------------------------
//    索引(编码): 0-8 共 9 种 (9 已淘汰, 10,11 软件专用)
//
//////////////////////////////////////////////////////////////////////
//支持旧的类型:
#define ONLLYTYPE_NUMBERS 9
#define ONLLY_DEVTYPE_A6630G       0		//0x02
#define ONLLY_DEVTYPE_A4630G       1		//x00
#define ONLLY_DEVTYPE_A4620G       2		//0x0f
#define ONLLY_DEVTYPE_A4350G       3		//0x01
#define ONLLY_DEVTYPE_A660         4		//0x06
#define ONLLY_DEVTYPE_A460         5		//0x05
#define ONLLY_DEVTYPE_A430         6		//0x03
#define ONLLY_DEVTYPE_A330         7		//0x03
#define ONLLY_DEVTYPE_A630         8		//0x06
#define ONLLY_DEVTYPE_A430D_OLD    9		//0x0b
#define ONLLY_DEVTYPE_A320_OLD     10	    //0x0a
#define ONLLY_DEVTYPE_A320_NEW     11	    //0x04
#define ONLLY_DEVTYPE_AD990		   12


//---------------------D系列测试仪型号定义---------------------------

#define ONLLY_DEVTYPE_D430       0x0001		//0x01


//---------------------F系列测试仪型号定义---------------------------

#define ONLLY_DEVTYPE_F66        0x0001    




//=========================================ONLLY 测试仪硬件信息中特性标志定义===================================================================

//  Values are 32 bit values layed out as follows:
//
//   3 3 2 2 2 2 2 2 2 2 2 2 1 1 1 1 1 1 1 1 1 1
//   1 0 9 8 7 6 5 4 3 2 1 0 9 8 7 6 5 4 3 2 1 0 9 8 7 6 5 4 3 2 1 0
//  +-+-+-+-+-------+-------+-------+-------+-----------------------+
//  |P|X|D| | Power |  XXH  | Digit |       |      Common           |
//  +---+-+-+-------+-------+-------+-------+-----------------------+

//bit_31 : 是否支持内部功放
//bit_30 : 是否支持信号输出
//bit_29 : 是否支持数字量输出
//bit_28 : 保留

//bit_27 -> bit_24 (功放子特性)

//bit_23 -> bit_20 (小信号子特性)

//bit_19 -> bit_16 (数字量子特性)

//bit_15 -> bit_12 (保留)

//bit_11 -> bit_0 (公共特性) 
//bit_0 : 是否支持直流



//=========================================ONLLY Com error define================================================================

//ONLLY defined HRESULT
//  Values are 32 bit values layed out as follows:
//
//   3 3 2 2 2 2 2 2 2 2 2 2 1 1 1 1 1 1 1 1 1 1
//   1 0 9 8 7 6 5 4 3 2 1 0 9 8 7 6 5 4 3 2 1 0 9 8 7 6 5 4 3 2 1 0
//  +---+-+-+-----------------------+---------------+---------------+
//  |Sev|C|R|     Facility          | Module Index  |   Module Code |
//  +---+-+-+-----------------------+---------------+---------------+
//
//  where
//
//      Sev - is the severity code
//
//          00 - Success
//          01 - Informational
//          10 - Warning
//          11 - Error
//
//      C - is the Customer code flag
//
//      R - is a reserved bit
//
//      Facility - is the facility code
//
//      Code - is the facility's status code : high byte is module code,low byte is

#define   ONLLY_E_FIRST    0xA0190000L

#define   MAKE_ONLLY_MODULE_ERROR(mi,mc)  (ONLLYDS_E_FIRST+MAKEWORD(mc,mi))

#define   MAKE_ONLLY_ERROR(mic)  (ONLLYDS_E_FIRST+mic)

//OnllyDS Module Index = 0x01
//Error Code Define:
#define   ONLLYDS_E_FIRST       (ONLLY_E_FIRST+0x0100)

#define   E_ONLLYDS_FAIL        ONLLYDS_E_FIRST


//1. Network Error:
//=========================================应答反馈常量定义========================================================
//执行正确
#define CFM_CODE_OK				0x00

//当前命令下位机执行失败
#define CFM_CODE_FAIL			0x01
#define E_ONLLYDS_NET_FAIL      (ONLLYDS_E_FIRST+CFM_CODE_FAIL)

//网络通讯数据CRC校验错误
#define CFM_CODE_CRC_ERROR		0x02
#define E_ONLLYDS_NET_CRC       (ONLLYDS_E_FIRST+CFM_CODE_CRC_ERROR)   

//下位机内存不足，请联系厂家予以解决
#define CFM_CODE_OUTOF_MEMORY	0x03
#define E_ONLLYDS_NET_OUTOFMEMORY   (ONLLYDS_E_FIRST+CFM_CODE_OUTOF_MEMORY)  

//当前命令下位机不支持，请联系厂家予以解决
#define CFM_CODE_NOT_SUPPORT	0x04
#define E_ONLLYDS_NET_NOIMPLE   (ONLLYDS_E_FIRST+CFM_CODE_NOT_SUPPORT)  

//网络通讯数据发送超时，可能是网络不通，请查看网络连接是否正常
#define CFM_CODE_TIMEOUT		0x05
#define E_ONLLYDS_NET_TIMEOUT   (ONLLYDS_E_FIRST+CFM_CODE_TIMEOUT)   

//网络通讯数据发送失败，可能是网络不通或者不稳定，请查看网络连接是否正常
#define CFM_CODE_SEND_ERR		0x06
#define E_ONLLYDS_NET_SEND_ERR   (ONLLYDS_E_FIRST+CFM_CODE_SEND_ERR)  
 
//下位机发现测试ID不一致，请联系厂家予以解决
#define CFM_CODE_TESTID_ERR		0x07
#define E_ONLLYDS_TESTID_ERR    (ONLLYDS_E_FIRST+CFM_CODE_TESTID_ERR) 

//下位机补偿数据未初始化，请联系厂家予以解决
#define CFM_CODE_ADJ_UNINT      0x08
#define E_ONLLYDS_ADJ_UNINT     (ONLLYDS_E_FIRST+CFM_CODE_ADJ_UNINT)  

//读取下位机数据错误，请联系厂家予以解决
#define CFM_CODE_FLASHDATA_ERR  0x09
#define E_ONLLYDS_FLASHDATAERR  (ONLLYDS_E_FIRST+CFM_CODE_FLASHDATA_ERR)  

//文件格式错误
#define CFM_CODE_FILE_ERR       0x0A
#define E_ONLLYDS_FILE_ERR	    (ONLLYDS_E_FIRST+CFM_CODE_FILE_ERR)


//发现不支持的通道配置信号输出类型，请查看系统配置中通道配置是否正确
#define E_ONLLYDS_SIGNALNOTSUPPORT	 (ONLLYDS_E_FIRST+0x0B)


//获取下位机补偿数据失败，请联系厂家予以解决
#define E_ONLLYDS_GETADJFAILED	 (ONLLYDS_E_FIRST+0x0C)




//=========================================ONLLYDS 组件事件ID===============================================================
//Device site events define

//返回发送大量数据过程中的进度
#define ODS_EVENT_SENDPROCESS     0x01

//返回网络通讯断开
#define ODS_EVENT_NET_UNCONNECTED 0x02




//=========================================ONLLY 测试组件命令ID===============================================================
//按键触发命令
#define STSCMD_KEYPRESS 0L

//GPS分脉冲触发
#define STSCMD_GPS_PPM	1L

//申请刷新数据
#define STSCMD_ASKREFRESH 2L

//波形回放
//打开波形文件
//参数为字符串，内容为 COMTRADE文件路径+用户采样率，二者用分号隔开
//如：E:\\dd.cfg;1000
#define STSCMD_LOADCOMTRADEFILE 3L

//波形回放
//打开波形文件,并下传至下位机
//参数为字符串，内容为 COMTRADE文件路径+下位机存储文件名，二者用分号隔开
//如：E:\\dd.cfg;dd.cfg
#define STSCMD_COPYCOMTRADEFILE 4L


//=========================================ONLLY 测试服务器上传消息ID定义===============================================================
//////////////////////////////////////////////////////////////////////////

#define WM_ONLLY_DEVMSG (WM_USER+0x1000)
//wParam: 消息ID;
//lParam: OnllySERVERMSG*

//Description: 
//  无法识别的报文
//Content: 
//  OnllySERVERMSG.pMsg = 原始报文内容
//  OnllySERVERMSG.uLen = 原始报文大小
#define OSMSG_Unknown  0x01


//Description: 
//  开关量状态信息上传
//Content: 
//  OnllySERVERMSG.pMsg = OnllyDeviceIOState*
//  OnllySERVERMSG.uLen = sizeof(OnllyDeviceIOState)
#define OSMSG_ioState  0x02    


//Description: 
//  试验报告信息上传
//Content: 
//  OnllySERVERMSG.pMsg = Result Data
//  OnllySERVERMSG.uLen = Result Data Length
#define OSMSG_TestResult 0x03


//Description: 
//  试验过程刷新消息 
//Content: 
//  根据试验不同 
#define OSMSG_Refresh    0x04


//Description: 
//  DSP上传硬盘容量信息上传 
//Content: 
//  FreeSize(MB?) = OnllySERVERMSG.wParam 
#define OSMSG_DiskFreeSize 0x05


//Description: 
//  硬件试验终止消息
//Content: 
//  结束原因 = OnllySERVERMSG.wParam 
#define OSMSG_TestStop    0x06


//Description: 
//  硬件试验状态更新信息上传
//Content: 
//  试验进入第n状态 = OnllySERVERMSG.wParam 
#define OSMSG_TestProcess  0x07



#define ONLLY_DevFault_Curr  0x0001
#define ONLLY_DevFault_Volt  0x0002
#define ONLLY_DevFault_Dsp   0x0003

//Description: 
//  测试仪故障告警信息上传
//Content: 
//  测试仪故障信息 = OnllySERVERMSG.wParam (value: ONLLY_DevFault_开头的宏) 
#define OSMSG_DevFault       0x08


//Description: 
//  任务完成信息上传
//Content: 
//  未确定
#define OSMSG_TaskFinshed   0x09



#define ONLLY_LINKSTATE_OFFLINE 0x00
#define ONLLY_LINKSTATE_ONLINE  0x01

//Description: 
//  连机状态改变
//Content: 
//  (详细内容) ==> 
//  OnllySERVERMSG.wParam = 连接状态 : ONLLY_LINKSTATE_ ;
//  当连接状态为 ONLLY_LINKSTATE_ONLINE 时
//  {
//		OnllySERVERMSG.pMsg = OnllyDeviceBaseInfo*
//		OnllySERVERMSG.uLen = sizeof(OnllyDeviceBaseInfo)
//  }
//  否则
//		OnllySERVERMSG.pMsg = NULL
//		OnllySERVERMSG.uLen = 0
#define OSMSG_DevLinkStateChanged   0x10


/*
//Description: 
//  syscfg改变
//Content: 
//  OnllySERVERMSG.pMsg = OnllyCFGPTCT*
//  OnllySERVERMSG.uLen = sizeof(OnllyCFGPTCT)
#define OSMSG_QueryChannelCfg   0x11

#define OSMSG_ADJReady          0x12
*/


//服务层事件定义
//
#define OSMSG_DevSiteSendEvent   0x13


//Description: 
//  加载文件事件上传
//Content: 
//  OnllySERVERMSG.wParam = 当前已读取的大小
//  OnllySERVERMSG.lParam = 文件总大小
#define OSMSG_TestModuleEvent_LoadFile  0x14


//Description: 
//  GPS对时上传
//Content: 
//	OnllySERVERMSG.pMsg = SYSTEMTIME*
//	OnllySERVERMSG.uLen = sizeof(SYSTEMTIME)
#define OSMSG_GPSTime   0x15

//------------------------------------------------------------------
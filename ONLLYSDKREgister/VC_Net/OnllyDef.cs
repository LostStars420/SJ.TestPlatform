using System;
using System.Collections.Generic;
using System.Text;

namespace OnllyTs
{
    class OnllyDef
    {
        //测试仪最多能支持的电压或电流通道数
        public const int MAX_CHANNEL_COUNT = 16;

        //测试仪最多能支持的总通道数
        public const int TOTAL_CHANNEL_COUNT = 32;

        // 测试仪最多能支持的开入量或开出量个数
        public const int MAX_IO_COUNT = 16;

        // 测试仪最多能支持的总开关量个数
        public const int TOTAL_IO_COUNT = 32;

        //测试仪ID最大长度
        public const int MAX_MACHINE_ID_LENGTH = 32;


        //=========================================支持的输出信号类型==============================================================

        //小信号
        public const System.UInt32 ONLLY_DevAdjSignalType_XXH = 0x10000;

        //数字量
        public const System.UInt32 ONLLY_DevAdjSignalType_SZL_91 = 0x20000;
        public const System.UInt32 ONLLY_DevAdjSignalType_SZL_92 = 0x20001;

        //功放
        public const System.UInt32 ONLLY_DevAdjSignalType_GF = 0x30001;



        //=========================================测试仪硬件型号定义=============================================================


        //DevType 高16位表示硬件主型号
        public const System.UInt16 ONLLY_DEVTYPE_UNKNOWN = 0x0000; //未知
        public const System.UInt16 ONLLY_DEVTYPE_A = 0x0001; //A系列
        public const System.UInt16 ONLLY_DEVTYPE_D = 0x0002; //D系列
        public const System.UInt16 ONLLY_DEVTYPE_F = 0x0003; //F系列


        //DevType 低16位表示硬件子型号

        //---------------------A 系列测试仪型号定义-------------------------------
        //    索引(编码): 0-8 共 9 种 (9 已淘汰, 10,11 软件专用)
        //
        //////////////////////////////////////////////////////////////////////
        //支持旧的类型:
        public const int ONLLYTYPE_NUMBERS = 9;
        public const int ONLLY_DEVTYPE_A6630G = 0;
        public const int ONLLY_DEVTYPE_A4630G = 1;
        public const int ONLLY_DEVTYPE_A4620G = 2;
        public const int ONLLY_DEVTYPE_A4350G = 3;
        public const int ONLLY_DEVTYPE_A660 = 4;	
        public const int ONLLY_DEVTYPE_A460 = 5;	
        public const int ONLLY_DEVTYPE_A430 = 6;	
        public const int ONLLY_DEVTYPE_A330 = 7;	
        public const int ONLLY_DEVTYPE_A630 = 8;	
        public const int ONLLY_DEVTYPE_A430D_OLD = 9;
        public const int ONLLY_DEVTYPE_A320_OLD = 10;
        public const int ONLLY_DEVTYPE_A320_NEW = 11;
        public const int ONLLY_DEVTYPE_AD990	= 12;


        //---------------------D系列测试仪型号定义---------------------------

        public const int ONLLY_DEVTYPE_D430 = 0x0001;		//0x01


        //---------------------F系列测试仪型号定义---------------------------

        public const int ONLLY_DEVTYPE_F66 = 0x0001;




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

        public const System.UInt32    ONLLY_E_FIRST = 0xA0190000;

        static public System.UInt32 MAKEWORD(System.UInt32 bLow, System.UInt32 bHigh)
        {
            return ((bLow & 0xff) | ((bHigh & 0xff) << 8));
        }

        static public System.UInt32 MAKE_ONLLY_MODULE_ERROR(System.UInt32 mi,System.UInt32 mc)
        {
            return (ONLLYDS_E_FIRST+MAKEWORD(mc,mi));
        }

        static public System.UInt32 MAKE_ONLLY_ERROR(System.UInt32 mic)
        {
            return (ONLLYDS_E_FIRST+mic);
        }

        //OnllyDS Module Index = 0x01
        //Error Code Define:
        public const System.UInt32    ONLLYDS_E_FIRST = (ONLLY_E_FIRST+0x0100);

        public const System.UInt32    E_ONLLYDS_FAIL = ONLLYDS_E_FIRST;


        //1. Network Error:
        //=========================================应答反馈常量定义========================================================
        //执行正确
        public const System.UInt32  CFM_CODE_OK = 0x00;

        //当前命令下位机执行失败
        public const System.UInt32  CFM_CODE_FAIL = 0x01;
        public const System.UInt32  E_ONLLYDS_NET_FAIL = (ONLLYDS_E_FIRST+CFM_CODE_FAIL);

        //网络通讯数据CRC校验错误
        public const System.UInt32  CFM_CODE_CRC_ERROR = 0x02;
        public const System.UInt32  E_ONLLYDS_NET_CRC = (ONLLYDS_E_FIRST+CFM_CODE_CRC_ERROR);

        //下位机内存不足，请联系厂家予以解决
        public const System.UInt32  CFM_CODE_OUTOF_MEMORY = 0x03;
        public const System.UInt32  E_ONLLYDS_NET_OUTOFMEMORY = (ONLLYDS_E_FIRST+CFM_CODE_OUTOF_MEMORY);

        //当前命令下位机不支持，请联系厂家予以解决
        public const System.UInt32  CFM_CODE_NOT_SUPPORT = 0x04;
        public const System.UInt32  E_ONLLYDS_NET_NOIMPLE = (ONLLYDS_E_FIRST+CFM_CODE_NOT_SUPPORT);

        //网络通讯数据发送超时，可能是网络不通，请查看网络连接是否正常
        public const System.UInt32  CFM_CODE_TIMEOUT	= 0x05;
        public const System.UInt32  E_ONLLYDS_NET_TIMEOUT = (ONLLYDS_E_FIRST+CFM_CODE_TIMEOUT);

        //网络通讯数据发送失败，可能是网络不通或者不稳定，请查看网络连接是否正常
        public const System.UInt32  CFM_CODE_SEND_ERR = 0x06;
        public const System.UInt32  E_ONLLYDS_NET_SEND_ERR = (ONLLYDS_E_FIRST+CFM_CODE_SEND_ERR);
         
        //下位机发现测试ID不一致，请联系厂家予以解决
        public const System.UInt32  CFM_CODE_TESTID_ERR = 0x07;
        public const System.UInt32  E_ONLLYDS_TESTID_ERR = (ONLLYDS_E_FIRST+CFM_CODE_TESTID_ERR);

        //下位机补偿数据未初始化，请联系厂家予以解决
        public const System.UInt32  CFM_CODE_ADJ_UNINT = 0x08;
        public const System.UInt32  E_ONLLYDS_ADJ_UNINT = (ONLLYDS_E_FIRST+CFM_CODE_ADJ_UNINT);

        //读取下位机数据错误，请联系厂家予以解决
        public const System.UInt32  CFM_CODE_FLASHDATA_ERR = 0x09;
        public const System.UInt32  E_ONLLYDS_FLASHDATAERR = (ONLLYDS_E_FIRST+CFM_CODE_FLASHDATA_ERR);

        //文件格式错误
        public const System.UInt32  CFM_CODE_FILE_ERR = 0x0A;
        public const System.UInt32  E_ONLLYDS_FILE_ERR = (ONLLYDS_E_FIRST+CFM_CODE_FILE_ERR);


        //发现不支持的通道配置信号输出类型，请查看系统配置中通道配置是否正确
        public const System.UInt32  E_ONLLYDS_SIGNALNOTSUPPORT = (ONLLYDS_E_FIRST+0x0B);


        //获取下位机补偿数据失败，请联系厂家予以解决
        public const System.UInt32  E_ONLLYDS_GETADJFAILED = (ONLLYDS_E_FIRST+0x0C);




        //=========================================ONLLYDS 组件事件ID===============================================================
        //Device site events define

        //返回发送大量数据过程中的进度
        public const System.UInt32  ODS_EVENT_SENDPROCESS = 0x01;

        //返回网络通讯断开
        public const System.UInt32  ODS_EVENT_NET_UNCONNECTED = 0x02;




        //=========================================ONLLY 测试组件命令ID===============================================================
        //按键触发命令
        public const System.UInt32  STSCMD_KEYPRESS = 0;

        //GPS分脉冲触发
        public const System.UInt32  STSCMD_GPS_PPM = 1;

        //申请刷新数据
        public const System.UInt32  STSCMD_ASKREFRESH = 2;

        //波形回放
        //打开波形文件
        //参数为字符串，内容为 COMTRADE文件路径+用户采样率，二者用分号隔开
        //如：E:\\dd.cfg;1000
        public const System.UInt32  STSCMD_LOADCOMTRADEFILE = 3;

        //波形回放
        //打开波形文件,并下传至下位机
        //参数为字符串，内容为 COMTRADE文件路径+下位机存储文件名，二者用分号隔开
        //如：E:\\dd.cfg;dd.cfg
        public const System.UInt32  STSCMD_COPYCOMTRADEFILE = 4;


        //=========================================ONLLY 测试服务器上传消息ID定义===============================================================
        //////////////////////////////////////////////////////////////////////////
        public const System.UInt32 WM_USER = 0x0400;
        public const System.UInt32 WM_ONLLY_DEVMSG = ( WM_USER + 0x1000);
        //wParam: 消息ID;
        //lParam: OnllySERVERMSG*

        //Description: 
        //  无法识别的报文
        //Content: 
        //  OnllySERVERMSG.pMsg = 原始报文内容
        //  OnllySERVERMSG.uLen = 原始报文大小
        public const System.UInt16  OSMSG_Unknown = 0x01;


        //Description: 
        //  开关量状态信息上传
        //Content: 
        //  OnllySERVERMSG.pMsg = OnllyDeviceIOState*
        //  OnllySERVERMSG.uLen = sizeof(OnllyDeviceIOState)
        public const System.UInt16  OSMSG_ioState = 0x02;


        //Description: 
        //  试验报告信息上传
        //Content: 
        //  OnllySERVERMSG.pMsg = Result Data
        //  OnllySERVERMSG.uLen = Result Data Length
        public const System.UInt16  OSMSG_TestResult = 0x03;


        //Description: 
        //  试验过程刷新消息 
        //Content: 
        //  根据试验不同 
        public const System.UInt16  OSMSG_Refresh = 0x04;


        //Description: 
        //  DSP上传硬盘容量信息上传 
        //Content: 
        //  FreeSize(MB?) = OnllySERVERMSG.wParam 
        public const System.UInt16  OSMSG_DiskFreeSize = 0x05;


        //Description: 
        //  硬件试验终止消息
        //Content: 
        //  结束原因 = OnllySERVERMSG.wParam 
        public const System.UInt16  OSMSG_TestStop = 0x06;


        //Description: 
        //  硬件试验状态更新信息上传
        //Content: 
        //  试验进入第n状态 = OnllySERVERMSG.wParam 
        public const System.UInt16  OSMSG_TestProcess = 0x07;



        public const System.UInt16  ONLLY_DevFault_Curr = 0x0001;
        public const System.UInt16  ONLLY_DevFault_Volt = 0x0002;
        public const System.UInt16  ONLLY_DevFault_Dsp = 0x0003;

        //Description: 
        //  测试仪故障告警信息上传
        //Content: 
        //  测试仪故障信息 = OnllySERVERMSG.wParam (value: ONLLY_DevFault_开头的宏) 
        public const System.UInt16  OSMSG_DevFault = 0x08;


        //Description: 
        //  任务完成信息上传
        //Content: 
        //  未确定
        public const System.UInt16  OSMSG_TaskFinshed = 0x09;



        public const System.UInt16  ONLLY_LINKSTATE_OFFLINE = 0x00;
        public const System.UInt16  ONLLY_LINKSTATE_ONLINE = 0x01;

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
        public const System.UInt16  OSMSG_DevLinkStateChanged = 0x10;


        /*
        //Description: 
        //  syscfg改变
        //Content: 
        //  OnllySERVERMSG.pMsg = OnllyCFGPTCT*
        //  OnllySERVERMSG.uLen = sizeof(OnllyCFGPTCT)
        public const System.UInt16  OSMSG_QueryChannelCfg   0x11

        public const System.UInt16  OSMSG_ADJReady          0x12
        */


        //服务层事件定义
        //
        public const System.UInt16  OSMSG_DevSiteSendEvent = 0x13;


        //Description: 
        //  加载文件事件上传
        //Content: 
        //  OnllySERVERMSG.wParam = 当前已读取的大小
        //  OnllySERVERMSG.lParam = 文件总大小
        public const System.UInt16  OSMSG_TestModuleEvent_LoadFile = 0x14;

        
        //Description: 
        //  GPS对时上传
        //Content: 
        //	OnllySERVERMSG.pMsg = SYSTEMTIME*
        //	OnllySERVERMSG.uLen = sizeof(SYSTEMTIME)
        public const System.UInt16 OSMSG_GPSTime = 0x15;

    }
}

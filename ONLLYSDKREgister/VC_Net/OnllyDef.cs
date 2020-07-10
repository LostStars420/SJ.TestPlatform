using System;
using System.Collections.Generic;
using System.Text;

namespace OnllyTs
{
    class OnllyDef
    {
        //�����������֧�ֵĵ�ѹ�����ͨ����
        public const int MAX_CHANNEL_COUNT = 16;

        //�����������֧�ֵ���ͨ����
        public const int TOTAL_CHANNEL_COUNT = 32;

        // �����������֧�ֵĿ������򿪳�������
        public const int MAX_IO_COUNT = 16;

        // �����������֧�ֵ��ܿ���������
        public const int TOTAL_IO_COUNT = 32;

        //������ID��󳤶�
        public const int MAX_MACHINE_ID_LENGTH = 32;


        //=========================================֧�ֵ�����ź�����==============================================================

        //С�ź�
        public const System.UInt32 ONLLY_DevAdjSignalType_XXH = 0x10000;

        //������
        public const System.UInt32 ONLLY_DevAdjSignalType_SZL_91 = 0x20000;
        public const System.UInt32 ONLLY_DevAdjSignalType_SZL_92 = 0x20001;

        //����
        public const System.UInt32 ONLLY_DevAdjSignalType_GF = 0x30001;



        //=========================================������Ӳ���ͺŶ���=============================================================


        //DevType ��16λ��ʾӲ�����ͺ�
        public const System.UInt16 ONLLY_DEVTYPE_UNKNOWN = 0x0000; //δ֪
        public const System.UInt16 ONLLY_DEVTYPE_A = 0x0001; //Aϵ��
        public const System.UInt16 ONLLY_DEVTYPE_D = 0x0002; //Dϵ��
        public const System.UInt16 ONLLY_DEVTYPE_F = 0x0003; //Fϵ��


        //DevType ��16λ��ʾӲ�����ͺ�

        //---------------------A ϵ�в������ͺŶ���-------------------------------
        //    ����(����): 0-8 �� 9 �� (9 ����̭, 10,11 ���ר��)
        //
        //////////////////////////////////////////////////////////////////////
        //֧�־ɵ�����:
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


        //---------------------Dϵ�в������ͺŶ���---------------------------

        public const int ONLLY_DEVTYPE_D430 = 0x0001;		//0x01


        //---------------------Fϵ�в������ͺŶ���---------------------------

        public const int ONLLY_DEVTYPE_F66 = 0x0001;




        //=========================================ONLLY ������Ӳ����Ϣ�����Ա�־����===================================================================

        //  Values are 32 bit values layed out as follows:
        //
        //   3 3 2 2 2 2 2 2 2 2 2 2 1 1 1 1 1 1 1 1 1 1
        //   1 0 9 8 7 6 5 4 3 2 1 0 9 8 7 6 5 4 3 2 1 0 9 8 7 6 5 4 3 2 1 0
        //  +-+-+-+-+-------+-------+-------+-------+-----------------------+
        //  |P|X|D| | Power |  XXH  | Digit |       |      Common           |
        //  +---+-+-+-------+-------+-------+-------+-----------------------+

        //bit_31 : �Ƿ�֧���ڲ�����
        //bit_30 : �Ƿ�֧���ź����
        //bit_29 : �Ƿ�֧�����������
        //bit_28 : ����

        //bit_27 -> bit_24 (����������)

        //bit_23 -> bit_20 (С�ź�������)

        //bit_19 -> bit_16 (������������)

        //bit_15 -> bit_12 (����)

        //bit_11 -> bit_0 (��������) 
        //bit_0 : �Ƿ�֧��ֱ��



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
        //=========================================Ӧ������������========================================================
        //ִ����ȷ
        public const System.UInt32  CFM_CODE_OK = 0x00;

        //��ǰ������λ��ִ��ʧ��
        public const System.UInt32  CFM_CODE_FAIL = 0x01;
        public const System.UInt32  E_ONLLYDS_NET_FAIL = (ONLLYDS_E_FIRST+CFM_CODE_FAIL);

        //����ͨѶ����CRCУ�����
        public const System.UInt32  CFM_CODE_CRC_ERROR = 0x02;
        public const System.UInt32  E_ONLLYDS_NET_CRC = (ONLLYDS_E_FIRST+CFM_CODE_CRC_ERROR);

        //��λ���ڴ治�㣬����ϵ�������Խ��
        public const System.UInt32  CFM_CODE_OUTOF_MEMORY = 0x03;
        public const System.UInt32  E_ONLLYDS_NET_OUTOFMEMORY = (ONLLYDS_E_FIRST+CFM_CODE_OUTOF_MEMORY);

        //��ǰ������λ����֧�֣�����ϵ�������Խ��
        public const System.UInt32  CFM_CODE_NOT_SUPPORT = 0x04;
        public const System.UInt32  E_ONLLYDS_NET_NOIMPLE = (ONLLYDS_E_FIRST+CFM_CODE_NOT_SUPPORT);

        //����ͨѶ���ݷ��ͳ�ʱ�����������粻ͨ����鿴���������Ƿ�����
        public const System.UInt32  CFM_CODE_TIMEOUT	= 0x05;
        public const System.UInt32  E_ONLLYDS_NET_TIMEOUT = (ONLLYDS_E_FIRST+CFM_CODE_TIMEOUT);

        //����ͨѶ���ݷ���ʧ�ܣ����������粻ͨ���߲��ȶ�����鿴���������Ƿ�����
        public const System.UInt32  CFM_CODE_SEND_ERR = 0x06;
        public const System.UInt32  E_ONLLYDS_NET_SEND_ERR = (ONLLYDS_E_FIRST+CFM_CODE_SEND_ERR);
         
        //��λ�����ֲ���ID��һ�£�����ϵ�������Խ��
        public const System.UInt32  CFM_CODE_TESTID_ERR = 0x07;
        public const System.UInt32  E_ONLLYDS_TESTID_ERR = (ONLLYDS_E_FIRST+CFM_CODE_TESTID_ERR);

        //��λ����������δ��ʼ��������ϵ�������Խ��
        public const System.UInt32  CFM_CODE_ADJ_UNINT = 0x08;
        public const System.UInt32  E_ONLLYDS_ADJ_UNINT = (ONLLYDS_E_FIRST+CFM_CODE_ADJ_UNINT);

        //��ȡ��λ�����ݴ�������ϵ�������Խ��
        public const System.UInt32  CFM_CODE_FLASHDATA_ERR = 0x09;
        public const System.UInt32  E_ONLLYDS_FLASHDATAERR = (ONLLYDS_E_FIRST+CFM_CODE_FLASHDATA_ERR);

        //�ļ���ʽ����
        public const System.UInt32  CFM_CODE_FILE_ERR = 0x0A;
        public const System.UInt32  E_ONLLYDS_FILE_ERR = (ONLLYDS_E_FIRST+CFM_CODE_FILE_ERR);


        //���ֲ�֧�ֵ�ͨ�������ź�������ͣ���鿴ϵͳ������ͨ�������Ƿ���ȷ
        public const System.UInt32  E_ONLLYDS_SIGNALNOTSUPPORT = (ONLLYDS_E_FIRST+0x0B);


        //��ȡ��λ����������ʧ�ܣ�����ϵ�������Խ��
        public const System.UInt32  E_ONLLYDS_GETADJFAILED = (ONLLYDS_E_FIRST+0x0C);




        //=========================================ONLLYDS ����¼�ID===============================================================
        //Device site events define

        //���ط��ʹ������ݹ����еĽ���
        public const System.UInt32  ODS_EVENT_SENDPROCESS = 0x01;

        //��������ͨѶ�Ͽ�
        public const System.UInt32  ODS_EVENT_NET_UNCONNECTED = 0x02;




        //=========================================ONLLY �����������ID===============================================================
        //������������
        public const System.UInt32  STSCMD_KEYPRESS = 0;

        //GPS�����崥��
        public const System.UInt32  STSCMD_GPS_PPM = 1;

        //����ˢ������
        public const System.UInt32  STSCMD_ASKREFRESH = 2;

        //���λط�
        //�򿪲����ļ�
        //����Ϊ�ַ���������Ϊ COMTRADE�ļ�·��+�û������ʣ������÷ֺŸ���
        //�磺E:\\dd.cfg;1000
        public const System.UInt32  STSCMD_LOADCOMTRADEFILE = 3;

        //���λط�
        //�򿪲����ļ�,���´�����λ��
        //����Ϊ�ַ���������Ϊ COMTRADE�ļ�·��+��λ���洢�ļ����������÷ֺŸ���
        //�磺E:\\dd.cfg;dd.cfg
        public const System.UInt32  STSCMD_COPYCOMTRADEFILE = 4;


        //=========================================ONLLY ���Է������ϴ���ϢID����===============================================================
        //////////////////////////////////////////////////////////////////////////
        public const System.UInt32 WM_USER = 0x0400;
        public const System.UInt32 WM_ONLLY_DEVMSG = ( WM_USER + 0x1000);
        //wParam: ��ϢID;
        //lParam: OnllySERVERMSG*

        //Description: 
        //  �޷�ʶ��ı���
        //Content: 
        //  OnllySERVERMSG.pMsg = ԭʼ��������
        //  OnllySERVERMSG.uLen = ԭʼ���Ĵ�С
        public const System.UInt16  OSMSG_Unknown = 0x01;


        //Description: 
        //  ������״̬��Ϣ�ϴ�
        //Content: 
        //  OnllySERVERMSG.pMsg = OnllyDeviceIOState*
        //  OnllySERVERMSG.uLen = sizeof(OnllyDeviceIOState)
        public const System.UInt16  OSMSG_ioState = 0x02;


        //Description: 
        //  ���鱨����Ϣ�ϴ�
        //Content: 
        //  OnllySERVERMSG.pMsg = Result Data
        //  OnllySERVERMSG.uLen = Result Data Length
        public const System.UInt16  OSMSG_TestResult = 0x03;


        //Description: 
        //  �������ˢ����Ϣ 
        //Content: 
        //  �������鲻ͬ 
        public const System.UInt16  OSMSG_Refresh = 0x04;


        //Description: 
        //  DSP�ϴ�Ӳ��������Ϣ�ϴ� 
        //Content: 
        //  FreeSize(MB?) = OnllySERVERMSG.wParam 
        public const System.UInt16  OSMSG_DiskFreeSize = 0x05;


        //Description: 
        //  Ӳ��������ֹ��Ϣ
        //Content: 
        //  ����ԭ�� = OnllySERVERMSG.wParam 
        public const System.UInt16  OSMSG_TestStop = 0x06;


        //Description: 
        //  Ӳ������״̬������Ϣ�ϴ�
        //Content: 
        //  ��������n״̬ = OnllySERVERMSG.wParam 
        public const System.UInt16  OSMSG_TestProcess = 0x07;



        public const System.UInt16  ONLLY_DevFault_Curr = 0x0001;
        public const System.UInt16  ONLLY_DevFault_Volt = 0x0002;
        public const System.UInt16  ONLLY_DevFault_Dsp = 0x0003;

        //Description: 
        //  �����ǹ��ϸ澯��Ϣ�ϴ�
        //Content: 
        //  �����ǹ�����Ϣ = OnllySERVERMSG.wParam (value: ONLLY_DevFault_��ͷ�ĺ�) 
        public const System.UInt16  OSMSG_DevFault = 0x08;


        //Description: 
        //  ���������Ϣ�ϴ�
        //Content: 
        //  δȷ��
        public const System.UInt16  OSMSG_TaskFinshed = 0x09;



        public const System.UInt16  ONLLY_LINKSTATE_OFFLINE = 0x00;
        public const System.UInt16  ONLLY_LINKSTATE_ONLINE = 0x01;

        //Description: 
        //  ����״̬�ı�
        //Content: 
        //  (��ϸ����) ==> 
        //  OnllySERVERMSG.wParam = ����״̬ : ONLLY_LINKSTATE_ ;
        //  ������״̬Ϊ ONLLY_LINKSTATE_ONLINE ʱ
        //  {
        //		OnllySERVERMSG.pMsg = OnllyDeviceBaseInfo*
        //		OnllySERVERMSG.uLen = sizeof(OnllyDeviceBaseInfo)
        //  }
        //  ����
        //		OnllySERVERMSG.pMsg = NULL
        //		OnllySERVERMSG.uLen = 0
        public const System.UInt16  OSMSG_DevLinkStateChanged = 0x10;


        /*
        //Description: 
        //  syscfg�ı�
        //Content: 
        //  OnllySERVERMSG.pMsg = OnllyCFGPTCT*
        //  OnllySERVERMSG.uLen = sizeof(OnllyCFGPTCT)
        public const System.UInt16  OSMSG_QueryChannelCfg   0x11

        public const System.UInt16  OSMSG_ADJReady          0x12
        */


        //������¼�����
        //
        public const System.UInt16  OSMSG_DevSiteSendEvent = 0x13;


        //Description: 
        //  �����ļ��¼��ϴ�
        //Content: 
        //  OnllySERVERMSG.wParam = ��ǰ�Ѷ�ȡ�Ĵ�С
        //  OnllySERVERMSG.lParam = �ļ��ܴ�С
        public const System.UInt16  OSMSG_TestModuleEvent_LoadFile = 0x14;

        
        //Description: 
        //  GPS��ʱ�ϴ�
        //Content: 
        //	OnllySERVERMSG.pMsg = SYSTEMTIME*
        //	OnllySERVERMSG.uLen = sizeof(SYSTEMTIME)
        public const System.UInt16 OSMSG_GPSTime = 0x15;

    }
}

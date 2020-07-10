//*
//* file: OnllyDef.h
//* 


//�����������֧�ֵĵ�ѹ�����ͨ����
#define MAX_CHANNEL_COUNT     16

//�����������֧�ֵ���ͨ����
#define TOTAL_CHANNEL_COUNT   32

// �����������֧�ֵĿ������򿪳�������
#define MAX_IO_COUNT          16

// �����������֧�ֵ��ܿ���������
#define TOTAL_IO_COUNT        32

//������ID��󳤶�
#define MAX_MACHINE_ID_LENGTH 32


//=========================================֧�ֵ�����ź�����==============================================================

//С�ź�
#define ONLLY_DevAdjSignalType_XXH    0x10000L

//������
#define ONLLY_DevAdjSignalType_SZL_91 0x20000L
#define ONLLY_DevAdjSignalType_SZL_92 0x20001L

//����
#define ONLLY_DevAdjSignalType_GF     0x30001L



//=========================================������Ӳ���ͺŶ���=============================================================


//DevType ��16λ��ʾӲ�����ͺ�
#define ONLLY_DEVTYPE_UNKNOWN   0x0000 //δ֪
#define ONLLY_DEVTYPE_A        0x0001 //Aϵ��
#define ONLLY_DEVTYPE_D        0x0002 //Dϵ��
#define ONLLY_DEVTYPE_F        0x0003 //Fϵ��


//DevType ��16λ��ʾӲ�����ͺ�

//---------------------A ϵ�в������ͺŶ���-------------------------------
//    ����(����): 0-8 �� 9 �� (9 ����̭, 10,11 ���ר��)
//
//////////////////////////////////////////////////////////////////////
//֧�־ɵ�����:
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


//---------------------Dϵ�в������ͺŶ���---------------------------

#define ONLLY_DEVTYPE_D430       0x0001		//0x01


//---------------------Fϵ�в������ͺŶ���---------------------------

#define ONLLY_DEVTYPE_F66        0x0001    




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

#define   ONLLY_E_FIRST    0xA0190000L

#define   MAKE_ONLLY_MODULE_ERROR(mi,mc)  (ONLLYDS_E_FIRST+MAKEWORD(mc,mi))

#define   MAKE_ONLLY_ERROR(mic)  (ONLLYDS_E_FIRST+mic)

//OnllyDS Module Index = 0x01
//Error Code Define:
#define   ONLLYDS_E_FIRST       (ONLLY_E_FIRST+0x0100)

#define   E_ONLLYDS_FAIL        ONLLYDS_E_FIRST


//1. Network Error:
//=========================================Ӧ������������========================================================
//ִ����ȷ
#define CFM_CODE_OK				0x00

//��ǰ������λ��ִ��ʧ��
#define CFM_CODE_FAIL			0x01
#define E_ONLLYDS_NET_FAIL      (ONLLYDS_E_FIRST+CFM_CODE_FAIL)

//����ͨѶ����CRCУ�����
#define CFM_CODE_CRC_ERROR		0x02
#define E_ONLLYDS_NET_CRC       (ONLLYDS_E_FIRST+CFM_CODE_CRC_ERROR)   

//��λ���ڴ治�㣬����ϵ�������Խ��
#define CFM_CODE_OUTOF_MEMORY	0x03
#define E_ONLLYDS_NET_OUTOFMEMORY   (ONLLYDS_E_FIRST+CFM_CODE_OUTOF_MEMORY)  

//��ǰ������λ����֧�֣�����ϵ�������Խ��
#define CFM_CODE_NOT_SUPPORT	0x04
#define E_ONLLYDS_NET_NOIMPLE   (ONLLYDS_E_FIRST+CFM_CODE_NOT_SUPPORT)  

//����ͨѶ���ݷ��ͳ�ʱ�����������粻ͨ����鿴���������Ƿ�����
#define CFM_CODE_TIMEOUT		0x05
#define E_ONLLYDS_NET_TIMEOUT   (ONLLYDS_E_FIRST+CFM_CODE_TIMEOUT)   

//����ͨѶ���ݷ���ʧ�ܣ����������粻ͨ���߲��ȶ�����鿴���������Ƿ�����
#define CFM_CODE_SEND_ERR		0x06
#define E_ONLLYDS_NET_SEND_ERR   (ONLLYDS_E_FIRST+CFM_CODE_SEND_ERR)  
 
//��λ�����ֲ���ID��һ�£�����ϵ�������Խ��
#define CFM_CODE_TESTID_ERR		0x07
#define E_ONLLYDS_TESTID_ERR    (ONLLYDS_E_FIRST+CFM_CODE_TESTID_ERR) 

//��λ����������δ��ʼ��������ϵ�������Խ��
#define CFM_CODE_ADJ_UNINT      0x08
#define E_ONLLYDS_ADJ_UNINT     (ONLLYDS_E_FIRST+CFM_CODE_ADJ_UNINT)  

//��ȡ��λ�����ݴ�������ϵ�������Խ��
#define CFM_CODE_FLASHDATA_ERR  0x09
#define E_ONLLYDS_FLASHDATAERR  (ONLLYDS_E_FIRST+CFM_CODE_FLASHDATA_ERR)  

//�ļ���ʽ����
#define CFM_CODE_FILE_ERR       0x0A
#define E_ONLLYDS_FILE_ERR	    (ONLLYDS_E_FIRST+CFM_CODE_FILE_ERR)


//���ֲ�֧�ֵ�ͨ�������ź�������ͣ���鿴ϵͳ������ͨ�������Ƿ���ȷ
#define E_ONLLYDS_SIGNALNOTSUPPORT	 (ONLLYDS_E_FIRST+0x0B)


//��ȡ��λ����������ʧ�ܣ�����ϵ�������Խ��
#define E_ONLLYDS_GETADJFAILED	 (ONLLYDS_E_FIRST+0x0C)




//=========================================ONLLYDS ����¼�ID===============================================================
//Device site events define

//���ط��ʹ������ݹ����еĽ���
#define ODS_EVENT_SENDPROCESS     0x01

//��������ͨѶ�Ͽ�
#define ODS_EVENT_NET_UNCONNECTED 0x02




//=========================================ONLLY �����������ID===============================================================
//������������
#define STSCMD_KEYPRESS 0L

//GPS�����崥��
#define STSCMD_GPS_PPM	1L

//����ˢ������
#define STSCMD_ASKREFRESH 2L

//���λط�
//�򿪲����ļ�
//����Ϊ�ַ���������Ϊ COMTRADE�ļ�·��+�û������ʣ������÷ֺŸ���
//�磺E:\\dd.cfg;1000
#define STSCMD_LOADCOMTRADEFILE 3L

//���λط�
//�򿪲����ļ�,���´�����λ��
//����Ϊ�ַ���������Ϊ COMTRADE�ļ�·��+��λ���洢�ļ����������÷ֺŸ���
//�磺E:\\dd.cfg;dd.cfg
#define STSCMD_COPYCOMTRADEFILE 4L


//=========================================ONLLY ���Է������ϴ���ϢID����===============================================================
//////////////////////////////////////////////////////////////////////////

#define WM_ONLLY_DEVMSG (WM_USER+0x1000)
//wParam: ��ϢID;
//lParam: OnllySERVERMSG*

//Description: 
//  �޷�ʶ��ı���
//Content: 
//  OnllySERVERMSG.pMsg = ԭʼ��������
//  OnllySERVERMSG.uLen = ԭʼ���Ĵ�С
#define OSMSG_Unknown  0x01


//Description: 
//  ������״̬��Ϣ�ϴ�
//Content: 
//  OnllySERVERMSG.pMsg = OnllyDeviceIOState*
//  OnllySERVERMSG.uLen = sizeof(OnllyDeviceIOState)
#define OSMSG_ioState  0x02    


//Description: 
//  ���鱨����Ϣ�ϴ�
//Content: 
//  OnllySERVERMSG.pMsg = Result Data
//  OnllySERVERMSG.uLen = Result Data Length
#define OSMSG_TestResult 0x03


//Description: 
//  �������ˢ����Ϣ 
//Content: 
//  �������鲻ͬ 
#define OSMSG_Refresh    0x04


//Description: 
//  DSP�ϴ�Ӳ��������Ϣ�ϴ� 
//Content: 
//  FreeSize(MB?) = OnllySERVERMSG.wParam 
#define OSMSG_DiskFreeSize 0x05


//Description: 
//  Ӳ��������ֹ��Ϣ
//Content: 
//  ����ԭ�� = OnllySERVERMSG.wParam 
#define OSMSG_TestStop    0x06


//Description: 
//  Ӳ������״̬������Ϣ�ϴ�
//Content: 
//  ��������n״̬ = OnllySERVERMSG.wParam 
#define OSMSG_TestProcess  0x07



#define ONLLY_DevFault_Curr  0x0001
#define ONLLY_DevFault_Volt  0x0002
#define ONLLY_DevFault_Dsp   0x0003

//Description: 
//  �����ǹ��ϸ澯��Ϣ�ϴ�
//Content: 
//  �����ǹ�����Ϣ = OnllySERVERMSG.wParam (value: ONLLY_DevFault_��ͷ�ĺ�) 
#define OSMSG_DevFault       0x08


//Description: 
//  ���������Ϣ�ϴ�
//Content: 
//  δȷ��
#define OSMSG_TaskFinshed   0x09



#define ONLLY_LINKSTATE_OFFLINE 0x00
#define ONLLY_LINKSTATE_ONLINE  0x01

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
#define OSMSG_DevLinkStateChanged   0x10


/*
//Description: 
//  syscfg�ı�
//Content: 
//  OnllySERVERMSG.pMsg = OnllyCFGPTCT*
//  OnllySERVERMSG.uLen = sizeof(OnllyCFGPTCT)
#define OSMSG_QueryChannelCfg   0x11

#define OSMSG_ADJReady          0x12
*/


//������¼�����
//
#define OSMSG_DevSiteSendEvent   0x13


//Description: 
//  �����ļ��¼��ϴ�
//Content: 
//  OnllySERVERMSG.wParam = ��ǰ�Ѷ�ȡ�Ĵ�С
//  OnllySERVERMSG.lParam = �ļ��ܴ�С
#define OSMSG_TestModuleEvent_LoadFile  0x14


//Description: 
//  GPS��ʱ�ϴ�
//Content: 
//	OnllySERVERMSG.pMsg = SYSTEMTIME*
//	OnllySERVERMSG.uLen = sizeof(SYSTEMTIME)
#define OSMSG_GPSTime   0x15

//------------------------------------------------------------------
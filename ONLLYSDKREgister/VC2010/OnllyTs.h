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
// �������ID����
//--------------------------------------------------------------------------------------------
#define ONLLYTID_UI			_T("Onlly.UITest.1")	//������ѹ
#define ONLLYTID_HARM		_T("Onlly.XBTest.1")	//г������
#define ONLLYTID_SHOT		_T("Onlly.ZZTest.1")	//��������
//
#define ONLLYTID_STATE			_T("Onlly.ZTXLTest.1")	//״̬����(12U12I, ��֧�ֵݱ�)
#define ONLLYTID_STATERAMP		_T("Onlly.StateRamp.1")	//״̬����(6U6I, ֧�ֵݱ�)
//
#define ONLLYTID_RAMP		_T("Onlly.DBHCTest.1")	//�ݱ们��
#define ONLLYTID_TRACK		_T("Onlly.FSXTXTest.1")	//��ʱ������: I-t, V-t
#define ONLLYTID_SYN		_T("Onlly.ZDZTQTest.1")	//�Զ�׼ͬ��
#define ONLLYTID_TIMER		_T("Onlly.SJCLTest.1")	//ʱ�����: 3 ��״̬
#define ONLLYTID_DIFF		_T("Onlly.CDTest.1")	//�����
#define ONLLYTID_WAVEPLAY	_T("Onlly.BXHFTest.1")	//���λط�
#define ONLLYTID_OSC		_T("Onlly.GLZDTest.1")	//������
#define ONLLYTID_SZNARI_STATE _T("Onlly.SzNari_State.1")	//��������ר�ò���
#define ONLLYTID_COMP		_T("Onlly.AdjTest.1")	//Adj ��������--����ϵ��
#define ONLLYTID_DDL		_T("Onlly.DDL.1")		//����ֱ������

//���뿪��װ�ò��� IOTEST
#define ONLLYTID_IOTEST_TIMER	_T("Onlly.IOTest_Timer.1")	//��բ�����ʱ����
#define ONLLYTID_IOTEST_SOE		_T("Onlly.IOTest_SOE.1")	//SOE ����
#define ONLLYTID_IOTEST_STATE	_T("Onlly.IOTest_State.1")	//״̬����


//--------------------------------------------------------------------------------------------
//���㹦�ܱ��� nCalcID(ע��: ����ͬ..\Include\OnllyDef_Calc.h �еĶ������)
//--------------------------------------------------------------------------------------------
#define CALCID_LOADSTATE	0		//�������и���״̬����
#define	CALCID_FAULT		1		//�����·: �������ģ��
#define CALCID_GPFAULT		2		//�����·: ��Ƶ�仯������ģ��
#define CALCID_ABCTOPP		3		//�߷�������: ABC -> PP
#define CALCID_PPTOABC		4		//            PP -> ABC
#define CALCID_ABCTO120		5		//���������: ABC -> 120
#define CALCID_120TOABC		6		//            120 -> ABC
#define CALCID_ABCTOPQ		7		//�������๦��

#define CALCID_SHOT			100		//�����������: ���ݽ����������3��״̬�µĵ�ѹ����(����UI)
#define CALCID_DISTANCE		101		//���뱣����ֵУ�����
#define CALCID_GPDISTANCE	102		//��Ƶ�仯���迹������ֵУ�����
#define CALCID_OVERCURR		103		//����������ֵУ�����
#define CALCID_NEGSEQCURR	104		//�������������ֵУ�����
#define CALCID_ZEROSEQCURR	105		//�������������ֵУ�����

#define CALCID_DIFF			120		//�����: ���� Id,Ir �����������ӵ��� Iabc,Ixyz
#define CALCID_DIFF_KP		121		//�����: ����ϵ�� KP123 ��������
#define CALCID_DIFF_SHOT	122		//�����: �����
#define CALCID_DIFF_IRID	123		//�����: ���� I1[3], I2[3] �����������������

#define CALCID_DIFF5		130		//�����: ���� Id,Ir �����������ӵ��� Iabc,Ixy

#define CALCID_OSC			140		//������: ��ǰ���������, ������װ�� K ��ĵ�ѹ������Чֵ
#define CALCID_OSC_ZK		141		//������: ��ǰ���������, ������װ�� K ��Ĳ����迹
//--------------------------------------------------------------------------------------------


//--------------------------------------------------------------------------------------------
//Description: 
//   �������Է���
//   hDevMsgHostWndΪָ�����ղ��Է������Ϣ�Ĵ��ھ������ϢΪWM_ONLLY_DEVMSG��
//   �����涨��
HRESULT OTS_CreateServer(HWND hDevMsgHostWnd);

//Description: 
//   ���ٲ��Է���
HRESULT OTS_DestroyServer();

//Description: 
//   ����������
//   lpszLinkInfo=L"PC-IP:192.168.2.97; PC-PORT:2001; DEV-IP:192.168.2.231; DEV-PORT:2001; BSETHOSTPORT:1"
HRESULT OTS_LinkDevice(const WCHAR* lpszLinkInfo);
HRESULT OTS_LinkDevice_W(const WCHAR* lpszLinkInfo);
HRESULT OTS_LinkDevice_A(const char* lpszLinkInfo);

//Description: 
//   ������ǶϿ�����
HRESULT OTS_DisconnectDevice();

//Description: 
//   ��ѯ���뿪��״̬
HRESULT OTS_AskIoState();

//Description: 
//   ���Ϳ���������
HRESULT OTS_TransBoutState(UINT16* varBoutState,LONG nCount);

//Description: 
//   �������
//   lpszTestID������ ONLLYTID_ ��ͷ�Ķ���
HRESULT OTS_ActiveTest(const WCHAR* lpszTestID);
HRESULT OTS_ActiveTest_W(const WCHAR* lpszTestID);
HRESULT OTS_ActiveTest_A(const char* lpszTestID);

//Description: 
//   ��ʼ���������(GenericData ����) 
//   lpszTestID������ ONLLYTID_ ��ͷ�Ķ���
HRESULT OTS_InitTestParams(IGenericData* lpTestParams, const WCHAR* lpszTestID);
HRESULT OTS_InitTestParams_W(IGenericData* lpTestParams, const WCHAR* lpszTestID);
HRESULT OTS_InitTestParams_A(IGenericData* lpTestParams, const char* lpszTestID);

//Description: 
//   ��ʼ����
HRESULT OTS_BeginTest(IGenericData* lpTestParams);

//////////////////////////////////////////////////////////////////////////
//��ʼ����Ҳ�ɲ��Ϊ����������������:
//   ����׼��
HRESULT OTS_TestPrepare(IGenericData* lpTestParams);

//   ��������
HRESULT OTS_TestRun();
//////////////////////////////////////////////////////////////////////////


//Description: 
//   ֹͣ����
//   nFlag=0 -> ��������
//   nFlag=1 -> Ӳ�����Ͻ���
HRESULT OTS_StopTest(LONG nFlag);

//Description: 
//   ��ȡ���Խ��
HRESULT OTS_GetTestResult(IGenericData** lppResult);

//Description: 
//   ���������չ����, ���ֶ�����, PPM ������
HRESULT OTS_Execute(ULONG nID, VARIANT* pvcmd,VARIANT* pret);

//Description: 
//   �л����Ե�
HRESULT OTS_SwitchTestPoint(IGenericData* lpTestParams);

//Description: 
//   ��ȡ��������
HRESULT OTS_GetCalcEngine(ICalcEngine**lppCalcEngine);

//Description: 
//   ��ʼ���������(GenericData ����) 
//   nCalcID������Ķ���
HRESULT OTS_InitCalcParams(IGenericData* lpCalcParams, ULONG nCalcID);

//Description: 
//   ����ϵͳ����������, ��ʾ������ UI ͨ������, PT/CT, С�źű���, ���������̵�
HRESULT OTS_OnSystemConfig(HWND hParentWnd);

//Description: 
//   �����ѯ GPS ʱ��
//�����ַ�����ʽ: "GpsPos=2;Com=1;BaudRate=9600;Parity=0;StopBits=1;DataBits=8;"
//            ���� GpsPos=2;       0--����GPS, 1--��� GPS(������), 2--��� GPS(PC��);
//                 Com=1;          ��ʾGPS���� COM �Ķ˿ں�, 1 ��ʾ����1, 2 ��ʾ����2, ...
//			       BaudRate=9600;  ��ʾGPS���� COM �Ĳ�����
//			       Parity=0;       0--��У��, 1--��У��, 2--żУ��  
//			       StopBits=1;     ֹͣλ, 1, �� 2;
//			       DataBits=8;     ����λ, 7, �� 8;
HRESULT	OTS_QueryGPSTime(const WCHAR* lpszGPSLinkInfo);
HRESULT	OTS_QueryGPSTime_W(const WCHAR* lpszGPSLinkInfo);
HRESULT	OTS_QueryGPSTime_A(const char* lpszGPSLinkInfo);

//Description: 
//   ��ȡ DA �� ģ���������Χģ, �����䳤��
float OTS_GetDAAM();

//Description: 
//   ��ȡӲ�����ϸ澯��Ϣ
//   wcMsg:���ص���Ϣ�ַ���
//   nSize:��Ϣ�ַ����Ĵ�С
void OTS_GetFaultMsg(DWORD dwMacType,UINT64 uFaultMsg,WCHAR* wcMsg,int nSize);
void OTS_GetFaultMsg_W(DWORD dwMacType,UINT64 uFaultMsg,WCHAR* wcMsg,int nSize);
void OTS_GetFaultMsg_A(DWORD dwMacType,UINT64 uFaultMsg,char* wcMsg,int nSize);

//Description: 
//		��ȡ COM ������صĳ�����Ϣ
//      wcMsg:���ص���Ϣ�ַ���
//      nSize:��Ϣ�ַ����Ĵ�С
void OTS_GetFaultMsg_Com(HRESULT hr,WCHAR* wcMsg,int nSize);
void OTS_GetFaultMsg_Com_W(HRESULT hr,WCHAR* wcMsg,int nSize);
void OTS_GetFaultMsg_Com_A(HRESULT hr,char* wcMsg,int nSize);

//Description: 
//   ���������� GPS �Ķ�ʱģʽ����
HRESULT OTS_SetGPSTimeMode128(UINT64 nMode1, UINT64 nMode2, BOOL bSaveDefault);


// 2015-10-21  zy ����ͬ��ʱ�䣨ns�����ͣ�����ʵ����ģͬ��
HRESULT OTS_SetSynTime(long nSynTime_ns);


//---------------------- ONLLY-IO ���뿪������װ��ʹ�� --------------------
//Description: 
//   ��ѯ���뿪��״̬
HRESULT OTS_AskIoState_Ex();

//Description: 
//   ���Ϳ���������
HRESULT OTS_TransBoutState_Ex(UINT16* varBoutState,LONG BoutState_Count);


HRESULT OTS_SetOnllyConfigInfo(IGenericData* lpCfgParams);



//////////////////////////////////////////////////////////////////////////
//  ���Ӷ� 821 �豸��ͨ�Ź��� 
//////////////////////////////////////////////////////////////////////////

#define OTS_DEV701   0
#define OTS_DEV821_1 1
#define OTS_DEV821_2 2

HRESULT OTS_SendMessageToDevice(UINT32 DevID, UINT16 TestID, 
		UINT16 MsgID, UINT32 lInLength, const void* pInData,
		UINT32 lOutLength, void* pOutData);

// ����IEC61850���ó��򱣴���ļ�
BOOL OTS_DownLoadF66CfgFile(const WCHAR* path);
BOOL OTS_DownLoadF66CfgFile_W(const WCHAR* path);
BOOL OTS_DownLoadF66CfgFile_A(const char* path);

/***************************************************************************
//-------------------------------------------------------------------------
//���º������� ONLLY �ڲ�ʹ��, ת�Ƶ� ONLLYSS.H �ж���
//-------------------------------------------------------------------------
//Description: 
//   ȡ��Ӳ��������Ϣ
const OnllyDeviceBaseInfo& OTS_GetBaseDeviceInfo();

//Description: 
//   ��ȡ UI ͨ������������Ϣ, �� PT, CT
const OnllyCfgData& OTS_GetCFGPTCT();

//Description: 
//   ��ȡϵͳ������Ϣ, С�źű���, ����������, �Ƶ��, �Ƿ�Ĭ����ʾһ�β��
const OnllySystemSet& OTS_GetSystemSet();

//Description: 
//   ��ȡ ONLLY TestServer
//Return:
//S_OK
HRESULT OTS_GetTestServer(IDispatch ** lppTestServer);

//Description: 
//   ϵͳ�������ݴ洢����
//Return:
//S_OK
HRESULT OTS_DoConfigDataExchange(IGenericDataNode* lpData,BOOL bSave);

//Description: 
//   ��ѯӲ����Ϣ
//   lpszDevInfoID = "LINK" -- ��ѯ��ǰ�������ַ���
//                   "DEVBASEINFO.TestSampRate" -- ��ѯ��ǰ�����Ĳ�����
//                   ...
//   *pvar: ���淵����Ϣ
HRESULT OTS_GetDeviceInfo(const WCHAR* lpszDevInfoID,VARIANT*pvar);
HRESULT OTS_GetDeviceInfo_W(const WCHAR* lpszDevInfoID,VARIANT*pvar);
HRESULT OTS_GetDeviceInfo_A(const char* lpszDevInfoID,VARIANT*pvar);

***************************************************************************/
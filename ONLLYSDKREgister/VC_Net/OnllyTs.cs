using System;

/// <summary>
/// Summary description for Class1
/// </summary>

//���� OnllyTs.DLL(Win32) �еķ��йܺ���
namespace OnllyTs
{
    using System.Runtime.InteropServices;
    //���� OnllyCom ���ʹ��
    using OnllyDataLib;
    using OnllyCalcEngineLib;
    //���� WIN32 �еı�������
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
        //   �������Է���
        //hDevMsgHostWndΪָ�����ղ��Է������Ϣ�Ĵ��ھ������ϢΪWM_ONLLY_DEVMSG��
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern HRESULT OTS_CreateServer(HWND hDevMsgHostWnd);

        //Description: 
        //   ���ٲ��Է���
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public static extern HRESULT OTS_DestroyServer();

        //Description: 
        //   ����������
        //   lpszLinkInfo="PC-IP:192.168.2.97; PC-PORT:2001; DEV-IP:192.168.2.231; DEV-PORT:2001;"
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public static extern HRESULT OTS_LinkDevice(string lpszLinkInfo);

        //Description: 
        //   ������ǶϿ�����
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public static extern HRESULT OTS_DisconnectDevice();

        //Description: 
        //   ��ѯ���뿪���˿�״̬
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public static extern HRESULT OTS_AskIoState();

        //Description: 
        //   ���Ϳ���������
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public unsafe static extern HRESULT OTS_TransBoutState(UINT16[] varBoutState, LONG nCount);

        //------------------------------------------------------------------
        // �������TestID����: 
        public const string ONLLYTID_UI = "Onlly.UITest.1";	        //������ѹ
        public const string ONLLYTID_HARM = "Onlly.XBTest.1";	    //г������
        public const string ONLLYTID_SHOT = "Onlly.ZZTest.1";	    //��������
        public const string ONLLYTID_STATE = "Onlly.ZTXLTest.1";    //״̬����(12U12I, ��֧�ֵݱ�)
        public const string ONLLYTID_RAMP = "Onlly.DBHCTest.1";	    //�ݱ们��
        public const string ONLLYTID_TRACK = "Onlly.FSXTXTest.1";	//��ʱ������: I-t, V-t
        public const string ONLLYTID_SYN = "Onlly.ZDZTQTest.1";	    //�Զ�׼ͬ��
        public const string ONLLYTID_TIMER = "Onlly.SJCLTest.1";	//ʱ�����: 3 ��״̬
        public const string ONLLYTID_DIFF = "Onlly.CDTest.1";	    //�����
        public const string ONLLYTID_WAVEPLAY = "Onlly.BXHFTest.1";	//���λط�
        public const string ONLLYTID_OSC = "Onlly.GLZDTest.1";	    //������
        //------------------------------------------------------------------
        //Description: 
        //   �������
        //   lpszTestID������ ONLLYTID_ ��ͷ�Ķ���
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public static extern HRESULT OTS_ActiveTest(string lpszTestID);

        //Description: 
        //   ��ʼ���������(IGenericData ����) 
        //   lpszTestID������ ONLLYTID_ ��ͷ�Ķ���
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public unsafe static extern HRESULT OTS_InitTestParams(IGenericData lpTestParams, string lpszTestID);

        //Description: 
        //   ��ʼ����
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public unsafe static extern HRESULT OTS_BeginTest(IGenericData lpTestParams);

        //Description: 
        //   ֹͣ����
        //   nFlag=0 -> ��������
        //   nFlag=1 -> Ӳ�����Ͻ���
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public static extern HRESULT OTS_StopTest(LONG nFlag);

        //Description: 
        //   ȡ�ò��Խ��
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public unsafe static extern HRESULT OTS_GetTestResult(out IGenericData lppResult);

        //Description: 
        //   ���������չ����
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public unsafe static extern HRESULT OTS_Execute(ULONG nID, VARIANT pvcmd, VARIANT pret);

        //Description: 
        //   �л����Ե�
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public unsafe static extern HRESULT OTS_SwitchTestPoint(IGenericData lpTestParams);

        //Description: 
        //		��ȡ COM ������صĳ�����Ϣ
        //      wcMsg:���ص���Ϣ�ַ���
        //      nSize:��Ϣ�ַ����Ĵ�С
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public unsafe static extern HRESULT OTS_GetFaultMsg_Com(HRESULT hr, WCHAR[] wcMsg, int nSize);

        //------------------------------------------------------------------
        //Description: 
        //   ����ϵͳ����������, ��ʾ������ UI ͨ������, PT/CT, С�źű���, ���������̵�
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public static extern HRESULT OTS_OnSystemConfig(HWND hParentWnd);

        //////////////////////////////////////////////////////////////////////////
        //���ֻ�������ר�ã����� 61850 ����
        //////////////////////////////////////////////////////////////////////////
        // ����IEC61850���ó��򱣴���ļ�
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public static extern BOOL OTS_DownLoadF66CfgFile(string path);

        //////////////////////////////////////////////////////////////////////////
        //�����ļ��㹦�ܺ��� 
        //////////////////////////////////////////////////////////////////////////
        //Description: 
        //   ��ȡ��������
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public unsafe static extern HRESULT OTS_GetCalcEngine(out ICalcEngine lppCalcEngine);

        //----------------------------------------------------------------------
        //���㹦�ܱ��� nCalcID
        //----------------------------------------------------------------------
        public const ULONG CALCID_LOADSTATE = 0;	//�������и���״̬����
        public const ULONG CALCID_FAULT = 1;    	//�����·: �������ģ��
        public const ULONG CALCID_GPFAULT = 2;  	//�����·: ��Ƶ�仯������ģ��
        public const ULONG CALCID_ABCTOPP = 3;  	//�߷�������: ABC -> PP
        public const ULONG CALCID_PPTOABC = 4;  	//            PP -> ABC
        public const ULONG CALCID_ABCTO120 = 5; 	//���������: ABC -> 120
        public const ULONG CALCID_120TOABC = 6; 	//            120 -> ABC
        public const ULONG CALCID_ABCTOPQ = 7;  	//�������๦��

        public const ULONG CALCID_SHOT = 100;   	    //�����������: ���ݽ����������3��״̬�µĵ�ѹ����(����UI)
        public const ULONG CALCID_DISTANCE = 101;	    //���뱣����ֵУ�����
        public const ULONG CALCID_GPDISTANCE = 102;     //��Ƶ�仯���迹������ֵУ�����
        public const ULONG CALCID_OVERCURR = 103;       //�������������ֵУ�����
        public const ULONG CALCID_NEGSEQCURR = 104;     //�������������ֵУ�����
        public const ULONG CALCID_ZEROSEQCURR = 105;	//�������������ֵУ�����
        //------------------------------------------------------------------
        //Description: 
        //   ��ʼ���������(GenericData ����) 
        //   nCalcID������ CALCID_ ��ͷ�Ķ���
        [DllImport("OnllyTs.DLL", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.Cdecl)]
        public unsafe static extern HRESULT OTS_InitCalcParams(IGenericData lpCalcParams, ULONG nCalcID);
 
	}
}

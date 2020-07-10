/*
 *  CauseOfTransmission.cs
 */

using System;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace lib60870
{
    //    ԭ�� ��Cause �ã�UI6[1..6]<0..63>
    //<0> �ã� δ��
    //<1> �ã� ���ڡ�ѭ�� per/cyc
    //<2> �ã� ����ɨ�� back
    //<3> �ã� ͻ��(�Է�) spont
    //<4> �ã� ��ʼ�� init
    //<5> �ã� ������߱����� req
    //<6> �ã� ���� act
    //<7> �ã� ����ȷ�� actcon
    //<8> �ã� ֹͣ���� deact
    //<9> �ã� ֹͣ����ȷ�� deactcon
    //<10> �ã� ������ֹ actterm
    //<11> �ã� Զ����������ķ�����Ϣ retrem
    //<12> �ã� ������������ķ�����Ϣ retloc
    //<13> �ã� �ļ����� file
    //<14..19>�ã� Ϊ���ױ�׼���ݷ�Χ����
    //<20> �ã� ��Ӧվ�ٻ� introgen
    //<21> �ã� ��Ӧ�� 1 ���ٻ� inro1
    //<22> �ã� ��Ӧ�� 2 ���ٻ� inro2
    //<29> �ã� ��Ӧ�� 9 ���ٻ� inro9
    //<30> �ã� ��Ӧ�� 10 ���ٻ� inro10
    //<42..43> �ã� Ϊ���ױ�׼���ݷ�Χ����
    //<44> �ã� δ֪�����ͱ�ʶ
    //<45> �ã� δ֪�Ĵ���ԭ��
    //<46> �ã� δ֪��Ӧ�÷������ݵ�Ԫ������ַ
    //<47> �ã� δ֪����Ϣ�����ַ
    //<48..63>�� �� ����Ӧ����������(ר�÷�Χ

    /// <summary>
    /// ����ԭ���б�,�����˴���ԭ�����
    /// </summary>
	public enum CauseOfTransmission {
        /// <summary>
        /// ����,ѭ�� per/cyc 1
        /// </summary>
		PERIODIC = 1,
        /// <summary>
        /// ����ɨ�� back 2
        /// </summary>
		BACKGROUND_SCAN = 2,
        /// <summary>
        /// ͻ�����Է��� 3
        /// </summary>
		SPONTANEOUS = 3,
        /// <summary>
        /// ��ʼ�� init 4
        /// </summary>
		INITIALIZED = 4,
        /// <summary>
        /// ������߱����� Request 5
        /// </summary>
		REQUEST = 5,
        /// <summary>
        /// ���� act= 6
        /// </summary>
		ACTIVATION = 6,
        /// <summary>
        /// ����ȷ�� actcon= 7
        /// </summary>
		ACTIVATION_CON = 7,
        /// <summary>
        /// ֹͣ���� deact= 8
        /// </summary>
		DEACTIVATION = 8,
        /// <summary>
        /// ֹͣ����ȷ�� deactcon= 9
        /// </summary>
		DEACTIVATION_CON = 9,
        /// <summary>
        /// ������ֹ ActivateTermination= 10
        /// </summary>
		ACTIVATION_TERMINATION = 10,
        /// <summary>
        /// Զ����������ķ�����Ϣ retrem= 11
        /// </summary>
		RETURN_INFO_REMOTE = 11,
        /// <summary>
        /// ������������ķ�����Ϣ retloc= 12
        /// </summary>
		RETURN_INFO_LOCAL = 12,
        /// <summary>
        /// �ļ����� file
        /// </summary>
		FILE_TRANSFER =	13,
		AUTHENTICATION = 14,
		MAINTENANCE_OF_AUTH_SESSION_KEY = 15,
		MAINTENANCE_OF_USER_ROLE_AND_UPDATE_KEY = 16,
        //<14..19>�ã� Ϊ���ױ�׼���ݷ�Χ����

        /// <summary>
        /// ��Ӧվ�ٻ� introgen
        /// </summary>
		INTERROGATED_BY_STATION = 20, 
		INTERROGATED_BY_GROUP_1 = 21, 
		INTERROGATED_BY_GROUP_2 = 22, 
		INTERROGATED_BY_GROUP_3 = 23,
		INTERROGATED_BY_GROUP_4 = 24, 
		INTERROGATED_BY_GROUP_5 = 25, 
		INTERROGATED_BY_GROUP_6 = 26, 
		INTERROGATED_BY_GROUP_7 = 27,
		INTERROGATED_BY_GROUP_8 = 28, 
		INTERROGATED_BY_GROUP_9 = 29, 
		INTERROGATED_BY_GROUP_10 = 30, 
		INTERROGATED_BY_GROUP_11 = 31, 
		INTERROGATED_BY_GROUP_12 = 32, 
		INTERROGATED_BY_GROUP_13 = 33, 
		INTERROGATED_BY_GROUP_14 = 34, 
		INTERROGATED_BY_GROUP_15 = 35, 
		INTERROGATED_BY_GROUP_16 = 36, 
		REQUESTED_BY_GENERAL_COUNTER = 37, 
		REQUESTED_BY_GROUP_1_COUNTER = 38, 
		REQUESTED_BY_GROUP_2_COUNTER = 39,
		REQUESTED_BY_GROUP_3_COUNTER = 40, 
		REQUESTED_BY_GROUP_4_COUNTER = 41,
        /// <summary>
        /// δ֪�����ͱ�ʶ unknownTypeID= 44
        /// </summary>
		UNKNOWN_TYPE_ID = 44,
        /// <summary>
        /// δ֪�Ĵ���ԭ�� UnknownTypeCaseTransmission=	45
        /// </summary>
		UNKNOWN_CAUSE_OF_TRANSMISSION =	45,
        /// <summary>
        /// δ֪��Ӧ�÷������ݵ�Ԫ��ַ UnknownAppDataPublicAddress= 46
        /// </summary>
		UNKNOWN_COMMON_ADDRESS_OF_ASDU = 46,
        /// <summary>
        /// δ֪��Ϣ�����ַ UnknownInformationObjectAddress= 47
        /// </summary>
		UNKNOWN_INFORMATION_OBJECT_ADDRESS = 47,

        /// <summary>
        /// �л���ֵ������ԭ�򣬼�����ֹ
        /// </summary>
        CHANGE_FIXED_AREA_ACTIVATION_CON = 47,
        /// <summary>
        /// �������ļ���д��
        /// </summary>
        NOT_ALLOWED_WRITE_FIEL = 0x2F,

        /// <summary>
        /// ң�ز���ʧ��
        /// </summary>
        TELECONTROL_FAIL = 0x2F
	}
	
}


using System;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace lib60870
{

	public enum TypeID {
        /// <summary>
        /// ������Ϣ 1
        /// </summary>
		M_SP_NA_1 = 1,
        /// <summary>
        /// ��ʱ��ĵ�����Ϣ 2
        /// </summary>
		M_SP_TA_1 = 2,
        /// <summary>
        /// ˫����Ϣ 3
        /// </summary>
		M_DP_NA_1 = 3,
        /// <summary>
        /// ��ʱ���˫����Ϣ 4
        /// </summary>
		M_DP_TA_1 = 4,
        /// <summary>
        /// ��λ����Ϣ 5
        /// </summary>
		M_ST_NA_1 = 5,
        /// <summary>
        /// ��ʱ��Ĳ�λ����Ϣ 6
        /// </summary>
		M_ST_TA_1 = 6,
        /// <summary>
        /// 32���ش� 7
        /// </summary>
		M_BO_NA_1 = 7,
        /// <summary>
        /// ��ʱ���32���ش� 8
        /// </summary>
		M_BO_TA_1 = 8,
        /// <summary>
        /// ����ֵ����һ��ֵ 9
        /// </summary>
		M_ME_NA_1 = 9,
		M_ME_TA_1 = 10,
        /// <summary>
        /// ����ֵ����Ȼ�ֵ  11
        /// </summary>
		M_ME_NB_1 = 11,
		M_ME_TB_1 = 12,
        /// <summary>
        /// ����ֵ���̸����� 13
        /// </summary>
		M_ME_NC_1 = 13,
        /// <summary>
        /// ��ʱ��Ĳ���ֵ���̸����� 14
        /// </summary>
		M_ME_TC_1 = 14,
        /// <summary>
        /// �ۻ��� 15
        /// </summary>
		M_IT_NA_1 = 15,
        /// <summary>
        /// ��ʱ����ۻ��� 16
        /// </summary>
		M_IT_TA_1 = 16,
        /// <summary>
        /// ��ʱ��ļ̵籣���豸�¼� 17
        /// </summary>
		M_EP_TA_1 = 17,
        /// <summary>
        /// ��ʱ��ļ̵籣�����������¼� 18
        /// </summary>
		M_EP_TB_1 = 18,
        /// <summary>
        /// ��ʱ��ļ̵籣�����������·��Ϣ 19
        /// </summary>
		M_EP_TC_1 = 19,
        /// <summary>
        /// ����״̬��λ����ĳ��鵥����Ϣ 20
        /// </summary>
		M_PS_NA_1 = 20,
        /// <summary>
        /// ����ֵ������Ʒ�������Ĺ�һ��ֵ 21
        /// </summary>
		M_ME_ND_1 = 21,
        /// <summary>
        /// ��CP56Time2aʱ��ĵ�����Ϣ 30
        /// </summary>
		M_SP_TB_1 = 30,
        /// <summary>
        /// ��CP56Time2aʱ���˫����Ϣ 31
        /// </summary>
		M_DP_TB_1 = 31,
        /// <summary>
        /// ��CP56Time2aʱ��Ĳ�λ����Ϣ 32
        /// </summary>
		M_ST_TB_1 = 32,
        /// <summary>
        /// ��CP56Time2aʱ���32λ�� 33
        /// </summary>
		M_BO_TB_1 = 33,
        /// <summary>
        /// ��CP56Time2aʱ��Ĳ���ֵ����һ��ֵ 34
        /// </summary>
		M_ME_TD_1 = 34,
        /// <summary>
        /// ��CP56Time2aʱ��Ĳ���ֵ����Ȼ�ֵ 35
        /// </summary>
		M_ME_TE_1 = 35,
        /// <summary>
        /// ��CP56Time2aʱ��Ĳ���ֵ���̸����� 36
        /// </summary>
		M_ME_TF_1 = 36,
        /// <summary>
        /// ��CP56Time2aʱ����ۼ�ֵ 37
        /// </summary>
		M_IT_TB_1 = 37,
        /// <summary>
        /// ��CP56Time2aʱ��ļ̵籣��װ���¼� 38
        /// </summary>
		M_EP_TD_1 = 38,
        /// <summary>
        /// ��CP56Time2aʱ��ļ̵籣��װ�ó��������¼� 39
        /// </summary>
		M_EP_TE_1 = 39,
        /// <summary>
        /// ��CP56Time2aʱ��ļ̵籣��װ�ó��������·��Ϣ 40
        /// </summary>
		M_EP_TF_1 = 40,
        /// <summary>
        /// ����ֵ��Ϣ 42
        /// </summary>
        M_FT_NA_1 = 42,
        /// <summary>
        /// �������� 45
        /// </summary>
		C_SC_NA_1 = 45,
        /// <summary>
        /// ˫������ 46
        /// </summary>
		C_DC_NA_1 = 46,
        /// <summary>
        /// ���������� 47
        /// </summary>
		C_RC_NA_1 = 47,
        /// <summary>
        /// �趨ֵ�����һ��ֵ 48
        /// </summary>
		C_SE_NA_1 = 48,
        /// <summary>
        /// �趨ֵ�����Ȼ�ֵ 49
        /// </summary>
		C_SE_NB_1 = 49,
        /// <summary>
        /// �趨ֵ����̸�����  50
        /// </summary>
		C_SE_NC_1 = 50,
        /// <summary>
        /// 32���ش� 51
        /// </summary>
		C_BO_NA_1 = 51,
        /// <summary>
        /// ��CP56Time2aʱ��ĵ�������
        /// </summary>
		C_SC_TA_1 = 58,
        /// <summary>
        /// ��CP56Time2aʱ���˫������
        /// </summary>
		C_DC_TA_1 = 59,
        /// <summary>
        /// ��CP56Time2aʱ��Ĳ���������
        /// </summary>
		C_RC_TA_1 = 60,
        /// <summary>
        /// ��CP56Time2aʱ����趨ֵ�����һ��ֵ
        /// </summary>
		C_SE_TA_1 = 61,
        /// <summary>
        /// ��CP56Time2aʱ����趨ֵ�����Ȼ�ֵ
        /// </summary>
		C_SE_TB_1 = 62,
        /// <summary>
        /// ��CP56Time2aʱ����趨ֵ����̸�����
        /// </summary>
		C_SE_TC_1 = 63,
        /// <summary>
        ///��CP56Time2aʱ���32���ش�
        /// </summary>
		C_BO_TA_1 = 64,
        /// <summary>
        /// ��ʼ������ 70
        /// </summary>
		M_EI_NA_1 = 70,
        /// <summary>
        /// ���ٻ� 100
        /// </summary>
		C_IC_NA_1 = 100,
        /// <summary>
        /// ���������ٻ����� 101
        /// </summary>
		C_CI_NA_1 = 101,
        /// <summary>
        /// ������ 102
        /// </summary>
		C_RD_NA_1 = 102,
        /// <summary>
        /// ʱ��ͬ������ 103
        /// </summary>
		C_CS_NA_1 = 103,
		C_TS_NA_1 = 104,
        /// <summary>
        /// ��λ�������� 105
        /// </summary>
		C_RP_NA_1 = 105,
		C_CD_NA_1 = 106,
        /// <summary>
        /// ��CP56Time2aʱ��Ĳ�������
        /// </summary>
		C_TS_TA_1 = 107,
        /// <summary>
        /// ����ֵ��������һ��ֵ 110
        /// </summary>
		P_ME_NA_1 = 110,		
        /// <summary>
        ///  ����ֵ��������Ȼ�ֵ111
        /// </summary>
        P_ME_NB_1 = 111,
        /// <summary>
        ///  ����ֵ�������̸����� 112
        /// </summary>
        P_ME_NC_1 = 112,
        /// <summary>
        /// �������� 
        /// </summary>
        P_AC_NA_1 = 113,
      /*  
       * /// <summary>
        /// �ļ��Ѿ�׼���� SQ=0
        /// </summary>
        F_FR_NA_1 = 120,
        /// <summary>
        /// ���Ѿ�׼����
        /// </summary>
        F_SR_NA_1 = 121,*/
        /// <summary>
        /// �ٻ�Ŀ¼��ѡ���ļ����ٻ��ļ����ٻ���
        /// </summary>
        F_SC_NA_1 = 122,
        /// <summary>
        /// ���Ľڣ����Ķˣ�
        /// </summary>
        F_LS_NA_1 = 123,
        /// <summary>
        /// ȷ���ļ���ȷ�Ͻ�
        /// </summary>
        F_AF_NA_1 = 124,
        /// <summary>
        /// ��
        /// </summary>
        F_SG_NA_1 = 125,
        /// <summary>
        /// Ŀ¼{�հ׻�X��ֻ�ڼ��ӣ���׼��������Ч}
        /// </summary>
        F_DR_TA_1 = 126,
        /// <summary>
        /// ��ѯ��־(QueryLog)
        /// </summary>
        F_SC_NB_1 = 127,      
        /// <summary>
        /// �л���ֵ�� 200
        /// </summary>
        C_SR_NA_1 = 200,
        /// <summary>
        /// ����ֵ���� 201
        /// </summary>
        C_RR_NA_1 = 201,
        /// <summary>
        /// �������Ͷ�ֵ 202
        /// </summary>
        C_RS_NA_1 = 202,
        /// <summary>
        /// д�����Ͷ�ֵ 203
        /// </summary>
        C_WS_NA_1 = 203,
       
        /// <summary>
        /// �ۼ������̸����� 
        /// </summary>
        M_IT_NB_1 = 206,

        /// <summary>
        /// ��CP56Time2aʱ����ۼ������̸�����
        /// </summary>
        M_IT_TC_1 = 207,
        /// <summary>
        /// �ļ����� 210
        /// </summary>
        F_FR_NA_1=210,
        /// <summary>
        /// ��������������� 211
        /// </summary>
        F_SR_NA_1=211,
    
	}
	
}

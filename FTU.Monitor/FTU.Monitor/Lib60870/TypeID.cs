
using System;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace lib60870
{

	public enum TypeID {
        /// <summary>
        /// 单点信息 1
        /// </summary>
		M_SP_NA_1 = 1,
        /// <summary>
        /// 带时标的单点信息 2
        /// </summary>
		M_SP_TA_1 = 2,
        /// <summary>
        /// 双点信息 3
        /// </summary>
		M_DP_NA_1 = 3,
        /// <summary>
        /// 带时标的双点信息 4
        /// </summary>
		M_DP_TA_1 = 4,
        /// <summary>
        /// 步位置信息 5
        /// </summary>
		M_ST_NA_1 = 5,
        /// <summary>
        /// 带时标的步位置信息 6
        /// </summary>
		M_ST_TA_1 = 6,
        /// <summary>
        /// 32比特串 7
        /// </summary>
		M_BO_NA_1 = 7,
        /// <summary>
        /// 带时标的32比特串 8
        /// </summary>
		M_BO_TA_1 = 8,
        /// <summary>
        /// 测量值，归一化值 9
        /// </summary>
		M_ME_NA_1 = 9,
		M_ME_TA_1 = 10,
        /// <summary>
        /// 测量值，标度化值  11
        /// </summary>
		M_ME_NB_1 = 11,
		M_ME_TB_1 = 12,
        /// <summary>
        /// 测量值，短浮点数 13
        /// </summary>
		M_ME_NC_1 = 13,
        /// <summary>
        /// 带时标的测量值，短浮点数 14
        /// </summary>
		M_ME_TC_1 = 14,
        /// <summary>
        /// 累积量 15
        /// </summary>
		M_IT_NA_1 = 15,
        /// <summary>
        /// 带时标的累积量 16
        /// </summary>
		M_IT_TA_1 = 16,
        /// <summary>
        /// 带时标的继电保护设备事件 17
        /// </summary>
		M_EP_TA_1 = 17,
        /// <summary>
        /// 带时标的继电保护成组启动事件 18
        /// </summary>
		M_EP_TB_1 = 18,
        /// <summary>
        /// 带时标的继电保护成组输出电路信息 19
        /// </summary>
		M_EP_TC_1 = 19,
        /// <summary>
        /// 具有状态变位检出的成组单点信息 20
        /// </summary>
		M_PS_NA_1 = 20,
        /// <summary>
        /// 测量值，不带品质描述的归一化值 21
        /// </summary>
		M_ME_ND_1 = 21,
        /// <summary>
        /// 带CP56Time2a时标的单点信息 30
        /// </summary>
		M_SP_TB_1 = 30,
        /// <summary>
        /// 带CP56Time2a时标的双点信息 31
        /// </summary>
		M_DP_TB_1 = 31,
        /// <summary>
        /// 带CP56Time2a时标的步位置信息 32
        /// </summary>
		M_ST_TB_1 = 32,
        /// <summary>
        /// 带CP56Time2a时标的32位串 33
        /// </summary>
		M_BO_TB_1 = 33,
        /// <summary>
        /// 带CP56Time2a时标的测量值，归一化值 34
        /// </summary>
		M_ME_TD_1 = 34,
        /// <summary>
        /// 带CP56Time2a时标的测量值，标度化值 35
        /// </summary>
		M_ME_TE_1 = 35,
        /// <summary>
        /// 带CP56Time2a时标的测量值，短浮点数 36
        /// </summary>
		M_ME_TF_1 = 36,
        /// <summary>
        /// 带CP56Time2a时标的累计值 37
        /// </summary>
		M_IT_TB_1 = 37,
        /// <summary>
        /// 带CP56Time2a时标的继电保护装置事件 38
        /// </summary>
		M_EP_TD_1 = 38,
        /// <summary>
        /// 带CP56Time2a时标的继电保护装置成组启动事件 39
        /// </summary>
		M_EP_TE_1 = 39,
        /// <summary>
        /// 带CP56Time2a时标的继电保护装置成组输出电路信息 40
        /// </summary>
		M_EP_TF_1 = 40,
        /// <summary>
        /// 故障值信息 42
        /// </summary>
        M_FT_NA_1 = 42,
        /// <summary>
        /// 单点命令 45
        /// </summary>
		C_SC_NA_1 = 45,
        /// <summary>
        /// 双点命令 46
        /// </summary>
		C_DC_NA_1 = 46,
        /// <summary>
        /// 步调节命令 47
        /// </summary>
		C_RC_NA_1 = 47,
        /// <summary>
        /// 设定值命令，归一化值 48
        /// </summary>
		C_SE_NA_1 = 48,
        /// <summary>
        /// 设定值命令，标度化值 49
        /// </summary>
		C_SE_NB_1 = 49,
        /// <summary>
        /// 设定值命令，短浮点数  50
        /// </summary>
		C_SE_NC_1 = 50,
        /// <summary>
        /// 32比特串 51
        /// </summary>
		C_BO_NA_1 = 51,
        /// <summary>
        /// 带CP56Time2a时标的单点命令
        /// </summary>
		C_SC_TA_1 = 58,
        /// <summary>
        /// 带CP56Time2a时标的双点命令
        /// </summary>
		C_DC_TA_1 = 59,
        /// <summary>
        /// 带CP56Time2a时标的步调节命令
        /// </summary>
		C_RC_TA_1 = 60,
        /// <summary>
        /// 带CP56Time2a时标的设定值命令，归一化值
        /// </summary>
		C_SE_TA_1 = 61,
        /// <summary>
        /// 带CP56Time2a时标的设定值命令，标度化值
        /// </summary>
		C_SE_TB_1 = 62,
        /// <summary>
        /// 带CP56Time2a时标的设定值命令，短浮点数
        /// </summary>
		C_SE_TC_1 = 63,
        /// <summary>
        ///带CP56Time2a时标的32比特串
        /// </summary>
		C_BO_TA_1 = 64,
        /// <summary>
        /// 初始化结束 70
        /// </summary>
		M_EI_NA_1 = 70,
        /// <summary>
        /// 总召唤 100
        /// </summary>
		C_IC_NA_1 = 100,
        /// <summary>
        /// 电能脉冲召唤命令 101
        /// </summary>
		C_CI_NA_1 = 101,
        /// <summary>
        /// 读命令 102
        /// </summary>
		C_RD_NA_1 = 102,
        /// <summary>
        /// 时钟同步命令 103
        /// </summary>
		C_CS_NA_1 = 103,
		C_TS_NA_1 = 104,
        /// <summary>
        /// 复位进程命令 105
        /// </summary>
		C_RP_NA_1 = 105,
		C_CD_NA_1 = 106,
        /// <summary>
        /// 带CP56Time2a时标的测试命令
        /// </summary>
		C_TS_TA_1 = 107,
        /// <summary>
        /// 测量值参数，归一化值 110
        /// </summary>
		P_ME_NA_1 = 110,		
        /// <summary>
        ///  测量值参数，标度化值111
        /// </summary>
        P_ME_NB_1 = 111,
        /// <summary>
        ///  测量值参数，短浮点数 112
        /// </summary>
        P_ME_NC_1 = 112,
        /// <summary>
        /// 参数激活 
        /// </summary>
        P_AC_NA_1 = 113,
      /*  
       * /// <summary>
        /// 文件已经准备好 SQ=0
        /// </summary>
        F_FR_NA_1 = 120,
        /// <summary>
        /// 节已经准备好
        /// </summary>
        F_SR_NA_1 = 121,*/
        /// <summary>
        /// 召唤目录，选择文件，召唤文件，召唤节
        /// </summary>
        F_SC_NA_1 = 122,
        /// <summary>
        /// 最后的节，最后的端，
        /// </summary>
        F_LS_NA_1 = 123,
        /// <summary>
        /// 确认文件，确认节
        /// </summary>
        F_AF_NA_1 = 124,
        /// <summary>
        /// 段
        /// </summary>
        F_SG_NA_1 = 125,
        /// <summary>
        /// 目录{空白或X，只在监视（标准）方向有效}
        /// </summary>
        F_DR_TA_1 = 126,
        /// <summary>
        /// 查询日志(QueryLog)
        /// </summary>
        F_SC_NB_1 = 127,      
        /// <summary>
        /// 切换定值区 200
        /// </summary>
        C_SR_NA_1 = 200,
        /// <summary>
        /// 读定值区号 201
        /// </summary>
        C_RR_NA_1 = 201,
        /// <summary>
        /// 读参数和定值 202
        /// </summary>
        C_RS_NA_1 = 202,
        /// <summary>
        /// 写参数和定值 203
        /// </summary>
        C_WS_NA_1 = 203,
       
        /// <summary>
        /// 累计量，短浮点数 
        /// </summary>
        M_IT_NB_1 = 206,

        /// <summary>
        /// 带CP56Time2a时标的累计量，短浮点数
        /// </summary>
        M_IT_TC_1 = 207,
        /// <summary>
        /// 文件传输 210
        /// </summary>
        F_FR_NA_1=210,
        /// <summary>
        /// 软件升级启动结束 211
        /// </summary>
        F_SR_NA_1=211,
    
	}
	
}

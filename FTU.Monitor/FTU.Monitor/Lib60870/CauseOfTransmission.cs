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
    //    原因 ＝Cause ∶＝UI6[1..6]<0..63>
    //<0> ∶＝ 未用
    //<1> ∶＝ 周期、循环 per/cyc
    //<2> ∶＝ 背景扫描 back
    //<3> ∶＝ 突发(自发) spont
    //<4> ∶＝ 初始化 init
    //<5> ∶＝ 请求或者被请求 req
    //<6> ∶＝ 激活 act
    //<7> ∶＝ 激活确认 actcon
    //<8> ∶＝ 停止激活 deact
    //<9> ∶＝ 停止激活确认 deactcon
    //<10> ∶＝ 激活终止 actterm
    //<11> ∶＝ 远方命令引起的返送信息 retrem
    //<12> ∶＝ 当地命令引起的返送信息 retloc
    //<13> ∶＝ 文件传输 file
    //<14..19>∶＝ 为配套标准兼容范围保留
    //<20> ∶＝ 响应站召唤 introgen
    //<21> ∶＝ 响应第 1 组召唤 inro1
    //<22> ∶＝ 响应第 2 组召唤 inro2
    //<29> ∶＝ 响应第 9 组召唤 inro9
    //<30> ∶＝ 响应第 10 组召唤 inro10
    //<42..43> ∶＝ 为配套标准兼容范围保留
    //<44> ∶＝ 未知的类型标识
    //<45> ∶＝ 未知的传送原因
    //<46> ∶＝ 未知的应用服务数据单元公共地址
    //<47> ∶＝ 未知的信息对象地址
    //<48..63>∶ ＝ 特殊应用能力保留(专用范围

    /// <summary>
    /// 传送原因列表,定义了传送原因代号
    /// </summary>
	public enum CauseOfTransmission {
        /// <summary>
        /// 周期,循环 per/cyc 1
        /// </summary>
		PERIODIC = 1,
        /// <summary>
        /// 背景扫描 back 2
        /// </summary>
		BACKGROUND_SCAN = 2,
        /// <summary>
        /// 突发（自发） 3
        /// </summary>
		SPONTANEOUS = 3,
        /// <summary>
        /// 初始化 init 4
        /// </summary>
		INITIALIZED = 4,
        /// <summary>
        /// 请求或者被请求 Request 5
        /// </summary>
		REQUEST = 5,
        /// <summary>
        /// 激活 act= 6
        /// </summary>
		ACTIVATION = 6,
        /// <summary>
        /// 激活确认 actcon= 7
        /// </summary>
		ACTIVATION_CON = 7,
        /// <summary>
        /// 停止激活 deact= 8
        /// </summary>
		DEACTIVATION = 8,
        /// <summary>
        /// 停止激活确认 deactcon= 9
        /// </summary>
		DEACTIVATION_CON = 9,
        /// <summary>
        /// 激活终止 ActivateTermination= 10
        /// </summary>
		ACTIVATION_TERMINATION = 10,
        /// <summary>
        /// 远方命令引起的返送信息 retrem= 11
        /// </summary>
		RETURN_INFO_REMOTE = 11,
        /// <summary>
        /// 当地命令引起的返送信息 retloc= 12
        /// </summary>
		RETURN_INFO_LOCAL = 12,
        /// <summary>
        /// 文件传输 file
        /// </summary>
		FILE_TRANSFER =	13,
		AUTHENTICATION = 14,
		MAINTENANCE_OF_AUTH_SESSION_KEY = 15,
		MAINTENANCE_OF_USER_ROLE_AND_UPDATE_KEY = 16,
        //<14..19>∶＝ 为配套标准兼容范围保留

        /// <summary>
        /// 响应站召唤 introgen
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
        /// 未知的类型标识 unknownTypeID= 44
        /// </summary>
		UNKNOWN_TYPE_ID = 44,
        /// <summary>
        /// 未知的传送原因 UnknownTypeCaseTransmission=	45
        /// </summary>
		UNKNOWN_CAUSE_OF_TRANSMISSION =	45,
        /// <summary>
        /// 未知的应用服务数据单元地址 UnknownAppDataPublicAddress= 46
        /// </summary>
		UNKNOWN_COMMON_ADDRESS_OF_ASDU = 46,
        /// <summary>
        /// 未知信息对象地址 UnknownInformationObjectAddress= 47
        /// </summary>
		UNKNOWN_INFORMATION_OBJECT_ADDRESS = 47,

        /// <summary>
        /// 切换定值区传送原因，激活终止
        /// </summary>
        CHANGE_FIXED_AREA_ACTIVATION_CON = 47,
        /// <summary>
        /// 不允许文件的写入
        /// </summary>
        NOT_ALLOWED_WRITE_FIEL = 0x2F,

        /// <summary>
        /// 遥控操作失败
        /// </summary>
        TELECONTROL_FAIL = 0x2F
	}
	
}

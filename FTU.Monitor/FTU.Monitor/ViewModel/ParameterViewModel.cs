using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using lib60870;
using FTU.Monitor.Model;
using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Threading;
using FTU.Monitor.Util;
using FTU.Monitor.DataService;
using System.Collections.Generic;
using FTU.Monitor.EncryptExportModel;
using GalaSoft.MvvmLight.Messaging;
using FTU.Monitor.Service;
using System.Windows.Threading;
using FTU.Monitor.Lib60870;
using FTU.Monitor.Dao;
using System.Text;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// ParameterViewModel 的摘要说明
    /// author: songminghao
    /// date：2017/10/12 9:48:09
    /// desc：定值参数ViewModel
    /// version: 1.0
    /// </summary>
    public class ParameterViewModel : ViewModelBase,IIEC104Handler
    {
        /// <summary>
        /// 当前定值区号
        /// </summary>
        private static int currentParameterArea;

        /// <summary>
        /// 设置和获取当前定值区号
        /// </summary>
        public int CurrentParameterArea
        {
            get
            {
                return currentParameterArea;
            }
            set
            {
                currentParameterArea = value;
            }
        }

        /// <summary>
        /// 定值0区参数加载显示数据集合
        /// </summary>
        private IList<ConstantParameter> _parameterDataZero;

        /// <summary>
        /// 设置和获取定值0区参数加载显示数据集合
        /// </summary>
        public IList<ConstantParameter> ParameterDataZero
        {
            get 
            {
                return this._parameterDataZero; 
            }
            set 
            {
                this._parameterDataZero = value; 
            }
        }

        /// <summary>
        /// 定值1区参数加载显示数据集合
        /// </summary>
        private IList<ConstantParameter> _parameterDataOne;

        /// <summary>
        /// 设置和获取定值1区参数加载显示数据集合
        /// </summary>
        public IList<ConstantParameter> ParameterDataOne
        {
            get
            {
                return this._parameterDataOne;
            }
            set
            {
                this._parameterDataOne = value;
            }
        }

        /// <summary>
        /// 定值2区参数加载显示数据集合
        /// </summary>
        private IList<ConstantParameter> _parameterDataTwo;

        /// <summary>
        /// 设置和获取定值2区参数加载显示数据集合
        /// </summary>
        public IList<ConstantParameter> ParameterDataTwo
        {
            get
            {
                return this._parameterDataTwo;
            }
            set
            {
                this._parameterDataTwo = value;
            }
        }

        /// <summary>
        /// 调试输出信息对象
        /// </summary>
        public OutputData _outputdata;

        /// <summary>
        /// 设置和获取调试输出信息对象
        /// </summary>
        public OutputData Outputdata
        {
            get
            {
                return this._outputdata;
            }
            set
            {
                this._outputdata = value;
                RaisePropertyChanged(() => Outputdata);
            }
        }

        /// <summary>
        /// 定值参数值共同体,单精度浮点数,4个字节
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct Union
        {
            /// <summary>
            /// 浮点数值
            /// </summary>
            [FieldOffset(0)]
            public float value;

            /// <summary>
            /// 第一个字节
            /// </summary>
            [FieldOffset(0)]
            public byte byte0;

            /// <summary>
            /// 第二个字节
            /// </summary>
            [FieldOffset(1)]
            public byte byte1;

            /// <summary>
            /// 第三个字节
            /// </summary>
            [FieldOffset(2)]
            public byte byte2;

            /// <summary>
            /// 第四个字节
            /// </summary>
            [FieldOffset(3)]
            public byte byte3;

        };
        Union PARAMETER;

        /// <summary>
        /// 预置报文操作子线程参数对象结构体
        /// </summary>
        public struct PresetThreadParameterStruct
        {
            /// <summary>
            /// 预置命令选中的定值参数个数
            /// </summary>
            public int presetParameterCount;

            /// <summary>
            /// 保存预置命令报文数据的临时数组
            /// </summary>
            public byte[] presetData;

        };

        /// <summary>
        /// 是否继续发送预置帧,在收到预置确认时改为TRUE
        /// </summary>
        public bool send_Continue;
        
        /// <summary>
        /// 定值参数加载显示数据集合
        /// </summary>
        public ObservableCollection<ConstantParameter> _parameterData;

        /// <summary>
        /// 设置和获取定值参数加载显示数据集合
        /// </summary>
        public ObservableCollection<ConstantParameter> ParameterData
        {
            get
            {
                return this._parameterData;
            }
            set
            {
                this._parameterData = value;
                RaisePropertyChanged(() => ParameterData);
            }
        }

        /// <summary>
        /// 定值区号
        /// </summary>
        public ObservableCollection<int> _parameterAreaData;

        /// <summary>
        /// 获取和设置定值区号
        /// </summary>
        public ObservableCollection<int> ParameterAreaData
        {
            get
            {
                return this._parameterAreaData;
            }
            set
            {
                this._parameterAreaData = value;
                RaisePropertyChanged(() => ParameterAreaData);
            }
        }

        /// <summary>
        /// 选定的定值区号索引
        /// </summary>
        private int _parameterAreaSelectedIndex;

        /// <summary>
        /// 获取和设置选定的定值区号索引
        /// </summary>
        public int ParameterAreaSelectedIndex
        {
            get
            {
                return this._parameterAreaSelectedIndex;
            }
            set
            {
                this._parameterAreaSelectedIndex = value;
                RaisePropertyChanged(() => ParameterAreaSelectedIndex);

                // 保存定值区号切换前的定值参数点表
                AssignParameterData();
                UpdateConstantPoint(value);
                this.ComboxChecked = false;
                // 设置当前定值区号
                currentParameterArea = value;

            }
        }

        /// <summary>
        /// 全选复选框选中状态值
        /// </summary>
        private bool _comboxChecked;

        /// <summary>
        /// 设置和获取全选复选框选中状态值
        /// </summary>
        public bool ComboxChecked
        {
            get
            {
                return this._comboxChecked;
            }
            set
            {
                this._comboxChecked = value;
                RaisePropertyChanged(() => ComboxChecked);
            }
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        private string _parseInformationShow;

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public string ParseInformationShow
        {
            get
            {
                return this._parseInformationShow;
            }
            set
            {
                this._parseInformationShow = value;
            }
        }

        /// <summary>
        /// 全部选择命令
        /// </summary>
        public RelayCommand SelectAllCommand 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// 全部选择命令执行操作
        /// </summary>
        public void ExecuteSelectAllCommand()
        {
            foreach (ConstantParameter p in ParameterData)
            {
                p.Selected = ComboxChecked;
            }
        }

        /// <summary>
        /// 定值参数命令
        /// </summary>
        public RelayCommand<string> ParameterCommand 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// 接受处理定值参数数据集合
        /// </summary>
        /// <param name="TI">定值类型标识</param>
        /// <param name="asdu">对应定值类型标识的ASDU</param>
        public void HandleASDUData(TypeID TI,ASDU asdu)
        {
            if (asdu.TypeId == TypeID.C_RR_NA_1)// 读定值区号 201
            {
                var rac = (ReadParameterAreaCommand)asdu.GetElement(0);

                Console.WriteLine(" sn1={0},sn2={1},sn3={2}. ", rac.SN1, rac.SN2, rac.SN3);
                ParseInformationShow = " sn1=" + rac.SN1.ToString() + "sn2=" + rac.SN2.ToString() + "sn3=" + rac.SN3.ToString() + "\n" + "\n";
                ShowMessage.ParseInformationShow(ParseInformationShow);
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                     (ThreadStart)delegate()
                     {
                         ParameterAreaData.Clear();
                         for (int i = 0; i <= (rac.SN3 - rac.SN2); i++)
                         {
                             ParameterAreaData.Add(i);
                         }

                     });
                Outputdata.CurrentArea = rac.SN1.ToString();

            }
            else if (asdu.TypeId == TypeID.C_RS_NA_1)// 读参数和定值 202
            {
                ParseInformationShow = "定值数据，个数" + asdu.NumberOfElements.ToString() + "." + DateTime.Now.ToString() + "\n";
                ShowMessage.ParseInformationShow(ParseInformationShow);

                FixedValueParameter fixedValueParameter = (FixedValueParameter)asdu.GetElement(0);
                if (fixedValueParameter.FixedValueParameterObjectList == null || fixedValueParameter.FixedValueParameterObjectList.Count == 0)
                {
                    return;
                }

                for (int i = 0; i < fixedValueParameter.FixedValueParameterObjectList.Count; i++)
                {
                    FixedValueParameterObject val = fixedValueParameter.FixedValueParameterObjectList[i];

                    ShowMessage.ParseInformationShow(ParseInformationShow);
                    try
                    {
                        DevPointDao devPointDaoObject = new DevPointDao();
                        switch (val.Tag)
                        {
                            case 38:

                                foreach (var para in ParameterData)
                                {
                                    if (Convert.ToInt32(para.ID, 16) == val.IOA)
                                    {
                                        // 返回由字节数组中指定位置的四个字节转换来的单精度浮点数
                                        para.Value = System.BitConverter.ToSingle(val.ValueBytes, 0);

                                        ParseInformationShow = "  IOA: " + val.IOA + " HEx:" + val.IOA.ToString("X") + " value: " + para.Value + "\n";
                                    }
                                }

                                break;

                            case 4:

                                foreach (var inherentParameter in InherentParameterViewModel.inherentParameterData)
                                {
                                    if (Convert.ToInt32(inherentParameter.ID, 16) == val.IOA)
                                    {
                                        // 字符串类型
                                        System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                                        inherentParameter.StringValue = Encoding.GetEncoding("GB2312").GetString(val.ValueBytes);// asciiEncoding.GetString(val.ValueBytes);

                                        ParseInformationShow = "  IOA: " + val.IOA + " HEx:" + val.IOA.ToString("X") + " value: " + inherentParameter.StringValue + "\n";
                                    }
                                }

                                break;
                        }

                    }
                    catch (Exception e)
                    {
                        CommunicationViewModel.con.DebugLog("解析读参数和定值报文错误" + e.ToString());
                        LogHelper.Error(typeof(ParameterViewModel), "解析读参数和定值报文错误" + e.Message);
                    }
                }
            }
            else if (asdu.TypeId == TypeID.C_WS_NA_1)// 写参数和定值 203
            {
                if (asdu.Cot == CauseOfTransmission.ACTIVATION_CON)
                {
                    send_Continue = true;
                }
                send_Continue = true;
            }
        }

        /// <summary>
        /// 定值参数命令执行操作
        /// </summary>
        /// <param name="arg">参数</param>
        public void ExecuteParameterCommand(string arg)
        {
            try
            {
                switch (arg)
                {
                    // 读定值区号
                    case "ParameterAreaRead":

                        if(CommunicationViewModel.IsLinkConnect())
                        {
                            CommunicationViewModel.con.SendReadParameterAreaCommand(CauseOfTransmission.ACTIVATION, 1);
                        }
                        break;

                    // 切换定值区号
                    case "ParameterAreaChange":

                        if(CommunicationViewModel.IsLinkConnect())
                        {
                            try
                            {
                                CommunicationViewModel.con.SendChangeParameterAreaCommand(CauseOfTransmission.ACTIVATION, 1, ParameterAreaData[ParameterAreaSelectedIndex]);
                            }
                            catch
                            {
                                MessageBox.Show("请选择定值区号！！！");
                            }
                        }
                        break;

                    // 读多个参数和定值
                    case "ParameterRead":

                        if(CommunicationViewModel.IsLinkConnect())
                        {
                            // 定义选中的定值参数列表
                            IList<ConstantParameter> parameterReadList = new List<ConstantParameter>();
                            // 获取选中的定值参数
                            for (int i = 0; i < ParameterData.Count; i++)
                            {
                                if (ParameterData[i].Selected == true)
                                {
                                    parameterReadList.Add(ParameterData[i]);
                                }
                            }

                            if (parameterReadList == null || parameterReadList.Count == 0)
                            {
                                MessageBox.Show("请选择要读取的定值参数！！！");
                                break;
                            }

                            readPartsParameter(ParameterAreaData[ParameterAreaSelectedIndex], parameterReadList);
                        }
                        break;

                    // 读区号内全部参数
                    case "ParameterReadAll":

                        if(CommunicationViewModel.IsLinkConnect())
                        {
                            // 定义读区号内全部参数命令发送报文字节数组
                            byte[] readAllBuffer = new byte[2];
                            try
                            {
                                // 定值区号SN
                                readAllBuffer[0] = (byte)ParameterAreaData[ParameterAreaSelectedIndex];
                                readAllBuffer[1] = (byte)(ParameterAreaData[ParameterAreaSelectedIndex] >> 8);
                            }
                            catch
                            {
                                MessageBox.Show("先选择区号！！！");
                                break;
                            }

                            CommunicationViewModel.con.SendReadParameterCommand(CauseOfTransmission.ACTIVATION, 0x01, readAllBuffer);
                        }
                        break;

                    // 写多个参数和定值的预置命令
                    case "Preset":
                        
                        if(CommunicationViewModel.IsLinkConnect())
                        {
                            send_Continue = false;

                            // 定义保存预置命令报文数据的临时数组
                            byte[] presetData = new byte[2048];
                            int presetIndex = 0;
                            int presetParameterCount = 0;

                            // 信息体地址长度
                            int sizeOfIOA = CommunicationViewModel.con.Parameters.SizeOfIOA;

                            // 定值区号SN
                            presetData[presetIndex++] = (byte)ParameterAreaData[ParameterAreaSelectedIndex];
                            presetData[presetIndex++] = (byte)(ParameterAreaData[ParameterAreaSelectedIndex] >> 8);

                            // 参数特征标识
                            presetData[presetIndex++] = 0x80;

                            for (int i = 0; i < ParameterData.Count && (presetIndex + sizeOfIOA + 5 < presetData.Length); i++)
                            {
                                if (ParameterData[i].Selected == true)
                                {
                                    PARAMETER.value = ParameterData[i].Value;

                                    // 设置信息体地址
                                    presetData[presetIndex++] = (byte)Convert.ToInt32(ParameterData[i].ID, 16);
                                    presetData[presetIndex++] = (byte)(Convert.ToInt32(ParameterData[i].ID, 16) >> 8);
                                    if (CommunicationViewModel.con.Parameters.SizeOfIOA == 3)
                                    {
                                        // 如果等于3,设置信息体地址第三个字节值为0
                                        presetData[presetIndex++] = 0;
                                    }

                                    // TAG类型:38代表单精度浮点,4个字节
                                    presetData[presetIndex++] = 38;
                                    // 数据长度
                                    presetData[presetIndex++] = 4;
                                    // 值
                                    presetData[presetIndex++] = PARAMETER.byte0;
                                    presetData[presetIndex++] = PARAMETER.byte1;
                                    presetData[presetIndex++] = PARAMETER.byte2;
                                    presetData[presetIndex++] = PARAMETER.byte3;

                                    presetParameterCount++;
                                }
                            }

                            if (presetParameterCount == 0)
                            {
                                MessageBox.Show("请选择要修改的定值！");
                            }
                            else
                            {
                                PresetThreadParameterStruct ptps;
                                ptps.presetParameterCount = presetParameterCount;
                                ptps.presetData = new byte[presetData.Length];
                                ptps.presetData = presetData;
                                Thread th = new Thread(new ParameterizedThreadStart(SendPresetMsg));
                                // th.Abort();
                                th.IsBackground = true;
                                th.Start(ptps);
                            }
                        }

                        break;

                    // 写多个参数和定值的固化命令
                    case "Solidify":

                        if(CommunicationViewModel.IsLinkConnect())
                        {
                            if (SerialPortService.parameterContinueFlag == 1)
                            {
                                MessageBox.Show("报文接收还没有结束，请等待！");
                                return;
                            }

                            SendSolidifyOrCancel(ParameterAreaData[ParameterAreaSelectedIndex], 0);
                        }
                        break;

                    // 写多个参数和定值的撤销命令
                    case "Cancel":

                        if(CommunicationViewModel.IsLinkConnect())
                        {
                            SendSolidifyOrCancel(ParameterAreaData[ParameterAreaSelectedIndex], 0x40);
                        }
                        break;

                    // 将当前区定值参数设置为默认值
                    case "SetDefaultValue":

                        foreach (ConstantParameter p in ParameterData)
                        {
                            p.Value = p.DefaultValue;
                        }

                        break;

                    // 导入定值参数数据
                    case "ImportData":

                        // 获取文件内容解密后的字符串
                        string jsonToImport = ReportUtil.GetParameterDataCiphertext();
                        // 判断加密的字符串解密是否成功
                        if (UtilHelper.IsEmpty(jsonToImport))
                        {
                            MessageBox.Show("文件内容不正确，解密失败", "提示");
                            break;
                        }

                        // 将Json格式的字符串转换成对应的数据对象
                        ConstantParameterExportModel constantParameterExportModelImport = EncryptAndDecodeUtil.JsonToObject<ConstantParameterExportModel>(jsonToImport);
                        // 获取终端序列号
                        string programVersionFromJson = constantParameterExportModelImport.DeviceSerialNumber;
                        // 判断终端序列号和连接的终端序列号是否一致
                        if (!programVersionFromJson.Equals(InherentParameterViewModel.programVersion))
                        {
                            MessageBox.Show("即将导入的点表中，终端序列号不匹配", "提示");
                            break;
                        }

                        //IList<ConstantParameter> parameterImportList = ImportParameterData("定值参数");

                        // 获取定值参数对象列表
                        IList<ContantParameterForExport> parameterImportList = constantParameterExportModelImport.ConstantParameterList;
                        if (parameterImportList != null && parameterImportList.Count > 0)
                        {
                            ParameterData.Clear();
                            foreach(var parameter in parameterImportList)
                            {
                                ConstantParameter constantParameter = new ConstantParameter();
                                constantParameter.Selected = parameter.Selected;
                                constantParameter.Number = parameter.Number;
                                constantParameter.ID = parameter.ID;
                                constantParameter.Name = parameter.Name;
                                constantParameter.StringValue = parameter.StringValue;
                                constantParameter.Unit = parameter.Unit;
                                constantParameter.Comment = parameter.Comment;
                                constantParameter.MinValue = parameter.MinValue;
                                constantParameter.MaxValue = parameter.MaxValue;
                                constantParameter.Value = parameter.Value;
                                constantParameter.DefaultValue = parameter.DefaultValue;

                                ParameterData.Add(constantParameter);
                            }
                            MessageBox.Show("导入成功", "提示");
                        }

                        break;

                    // 导出定值参数数据
                    case "ExportData":

                        // 定值参数导出对象
                        ConstantParameterExportModel constantParameterExportModel = new ConstantParameterExportModel();
                        if (ParameterData != null && ParameterData.Count > 0)
                        {
                            // 清空定值参数对象列表
                            constantParameterExportModel.ConstantParameterList.Clear();
                            for (int i = 0; i < ParameterData.Count; i++)
                            {
                                ContantParameterForExport contantParameterForExport = new ContantParameterForExport();
                                contantParameterForExport.Selected = ParameterData[i].Selected;
                                contantParameterForExport.Number = ParameterData[i].Number;
                                contantParameterForExport.ID = ParameterData[i].ID;
                                contantParameterForExport.Name = ParameterData[i].Name;
                                contantParameterForExport.StringValue = ParameterData[i].StringValue;
                                contantParameterForExport.Unit = ParameterData[i].Unit;
                                contantParameterForExport.Comment = ParameterData[i].Comment;
                                contantParameterForExport.MinValue = ParameterData[i].MinValue;
                                contantParameterForExport.MaxValue = ParameterData[i].MaxValue;
                                contantParameterForExport.Value = ParameterData[i].Value;
                                contantParameterForExport.DefaultValue = ParameterData[i].DefaultValue;

                                // 添加到定值参数对象列表
                                constantParameterExportModel.ConstantParameterList.Add(contantParameterForExport);
                            }
                        }
                        
                        // 设置终端序列号
                        constantParameterExportModel.DeviceSerialNumber = InherentParameterViewModel.programVersion;

                        // 将固有定值参数导出对象转换为Json格式字符串
                        string inherentParameterExportModelToJson = EncryptAndDecodeUtil.GetJson(constantParameterExportModel);
                        // 给转换后的Json格式字符串进行AES加密
                        string encrypt = EncryptAndDecodeUtil.AESEncrypt(inherentParameterExportModelToJson, false);
                        // 判断加密是否成功
                        if (String.Empty.Equals(encrypt))
                        {
                            MessageBox.Show("导出内容加密失败", "提示");
                            break;
                        }

                        ReportUtil.ExportParameterData("定值参数", encrypt);
                        break;

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        /// <summary>
        /// 读多个参数和定值命令
        /// </summary>
        /// <param name="sn">定值区号</param>
        /// <param name="parameterList">选中的定值参数列表</param>
        public static void readPartsParameter(int sn, IList<ConstantParameter> parameterList)
        {
            // 定义发送报文字节数组
            byte[] buffer = null;
            // 读取定值参数命令的字节数组(从定值区号SN开始到校验码CS之前)
            byte[] temp = new byte[1024];
            // 字节数组索引
            int index = 0;
            // 选中定值数量
            byte dataCount = 0;

            // 获取每个信息体地址长度
            int per_len = CommunicationViewModel.con.Parameters.SizeOfIOA;

            // 定值区号SN:两个字节
            temp[index++] = (byte)sn;
            temp[index++] = (byte)(sn >> 8);

            // 依次设置信息体地址
            for (int i = 0; i < parameterList.Count && (index + per_len - 1 < temp.Length); i++)
            {
                // 设置信息体地址
                temp[index++] = (byte)Convert.ToInt32(parameterList[i].ID, 16);
                temp[index++] = (byte)(Convert.ToInt32(parameterList[i].ID, 16) >> 8);
                // 判断信息体地址长度
                if (CommunicationViewModel.con.Parameters.SizeOfIOA == 3)
                {
                    // 如果等于3,设置信息体地址第三个字节值为0
                    temp[index++] = 0;
                }

                // 选中定值数量加1
                dataCount++;
            }

            int sendMsgCount = dataCount / 80 + (dataCount % 80 == 0 ? 0 : 1);
            if (sendMsgCount == 0)
            {
                return;
            }

            for (int i = 0; i < sendMsgCount - 1; i++)
            {
                // 定义定值区号SN和选中的定值参数总长度
                buffer = new byte[80 * per_len + 2];
                buffer[0] = temp[0];
                buffer[1] = temp[1];
                for (int j = 2; j < buffer.Length; j++)
                {
                    buffer[j] = temp[i * 80 * per_len + j];
                }

                CommunicationViewModel.con.SendReadParameterCommand(CauseOfTransmission.ACTIVATION, 0x01, buffer);
            }

            // 定义最后一条要发送的定值区号SN和选中的定值参数总长度
            int lastMsgLength = dataCount - (sendMsgCount - 1) * 80;
            buffer = new byte[lastMsgLength * per_len + 2];
            buffer[0] = temp[0];
            buffer[1] = temp[1];
            for (int i = 2; i < buffer.Length; i++)
            {
                buffer[i] = temp[(sendMsgCount - 1) * 80 * per_len + i];
            }

            CommunicationViewModel.con.SendReadParameterCommand(CauseOfTransmission.ACTIVATION, 0x01, buffer);
        }

        /// <summary>
        /// 发送写多个参数和定值的固化或撤销命令报文
        /// </summary>
        /// <param name="sn">定值区号SN</param>
        /// <param name="features">特征标识</param>
        public void SendSolidifyOrCancel(int sn, byte features)
        {
            byte[] solidifyBuffer = new byte[3];
            // 定值区号SN
            solidifyBuffer[0] = (byte)sn;
            solidifyBuffer[1] = (byte)(sn >> 8);

            // 0x00:无后续,固化;0x40;无后续,撤销
            solidifyBuffer[2] = features;

            CommunicationViewModel.con.SendSetParameterCommand((CauseOfTransmission)6, 0x01, solidifyBuffer);
        }

        /// <summary>
        /// 预置报文操作子线程
        /// </summary>
        /// <param name="presetThreadParameterStruct">预置的定值参数结构体对象</param>
        private void SendPresetMsg(object presetThreadParameterStruct)
        {
            // 获取预置报文操作子线程参数对象结构体
            PresetThreadParameterStruct ptps = (PresetThreadParameterStruct)presetThreadParameterStruct;
            // 预置命令选中的定值参数个数
            int dataCount = ptps.presetParameterCount;
            // 保存预置命令报文数据的临时数组
            byte[] data = ptps.presetData;

            // 定义发送报文字节数组
            byte[] buffer = null;

            // 是否继续发送预置帧
            send_Continue = true;
            // 分包序号(每包发送20条数据)
            int times = 0;
            // 每条数据字节长度是8还是9
            int per_len = CommunicationViewModel.con.Parameters.SizeOfIOA + 6;

            while (dataCount != 0)
            {
                if (send_Continue == true)
                {
                    if (dataCount <= 20)
                    {
                        try
                        {
                            buffer = new byte[dataCount * per_len + 3];
                            buffer[0] = data[0];
                            buffer[1] = data[1];
                            buffer[2] = 0x80;
                            for (int i = 3; i < buffer.Length; i++)
                            {
                                buffer[i] = data[i + times * 20 * per_len];
                            }

                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.ToString());
                        }

                        CommunicationViewModel.con.SendSetParameterCommand((CauseOfTransmission)6, 0x01, buffer);
                        times = 0;
                        dataCount = 0;
                        send_Continue = false;

                        break;
                    }
                    else
                    {
                        try
                        {
                            buffer = new byte[20 * per_len + 3];
                            buffer[0] = data[0];
                            buffer[1] = data[1];
                            buffer[2] = 0x81;
                            for (int i = 3; i < buffer.Length; i++)
                            {
                                buffer[i] = data[i + times * 20 * per_len];
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.ToString());
                        }

                        CommunicationViewModel.con.SendSetParameterCommand((CauseOfTransmission)6, 0x01, buffer);
                        dataCount = (byte)(dataCount - 20);
                        times++;
                        send_Continue = false;
                    }
                }
            }

        }

        /// <summary>
        /// 保存定值区号切换前的定值参数点表
        /// </summary>
        private void AssignParameterData()
        {
            // 判断当前显示额定值参数列表是否为空
            if (ParameterData == null || ParameterData.Count == 0)
            {
                return;
            }

            // 保存0区定值点表
            if (currentParameterArea == 0)
            {
                this._parameterDataZero.Clear();

                for (int i = 0; i < ParameterData.Count; i++)
                {
                    this._parameterDataZero.Add(ParameterData[i]);
                }
            }

            // 保存1区定值点表
            if (currentParameterArea == 1)
            {
                this._parameterDataOne.Clear();

                for (int i = 0; i < ParameterData.Count; i++)
                {
                    this._parameterDataOne.Add(ParameterData[i]);
                }
                return;
            }

            // 保存2区定值点表
            if (currentParameterArea == 2)
            {
                this._parameterDataTwo.Clear();

                for (int i = 0; i < ParameterData.Count; i++)
                {
                    this._parameterDataTwo.Add(ParameterData[i]);
                }
                return;
            }

        }

        /// <summary>
        /// 根据定值区号获取相应定值参数点号集合
        /// </summary>
        /// <param name="parameterArea">定值区号</param>
        /// <param name="constantParameterPointList">定值区号对应的之前保存的定值点表</param>
        public IList<ConstantParameter> GetParameterPointByParameterArea(string parameterArea, IList<ConstantParameter> constantParameterPointList)
        {
            if (constantParameterPointList != null && constantParameterPointList.Count > 0)
            {
                return constantParameterPointList;
            }
            else
            {
                // 获取定值参数点表
                ParameterManageService parameterManageService = new ParameterManageService();
                return parameterManageService.GetConstantParameter(ConfigUtil.getPointTypeID(parameterArea));
            }
        }

        /// <summary>
        /// 加载定值参数数据
        /// </summary>
        /// <param name="parameterArea">定值区号名称</param>
        /// <param name="constantParameterPointList">定值区号对应的之前保存的定值点表</param>
        public void LoadParameterData(string parameterArea, IList<ConstantParameter> constantParameterPointList)
        {
            if (constantParameterPointList != null && constantParameterPointList.Count > 0)
            {
                AssignParameterData(constantParameterPointList);
            }
            else
            {
                ParameterManageService parameterManageService = new ParameterManageService();
                // 获取定值参数点表
                IList<ConstantParameter> constantParameterList = parameterManageService.GetConstantParameter(ConfigUtil.getPointTypeID(parameterArea));
                if (constantParameterList != null && constantParameterList.Count > 0)
                {
                    ParameterData.Clear();
                    foreach (var constantParameter in constantParameterList)
                    {
                        ParameterData.Add(constantParameter);
                    }
                }
            }

        }

        /// <summary>
        /// 加载定值参数点表
        /// </summary>
        /// <param name="constantParameterPointList">要加载的定值参数点表</param>
        private void AssignParameterData(IList<ConstantParameter> constantParameterPointList)
        {
            ParameterData.Clear();

            if (constantParameterPointList != null && constantParameterPointList.Count > 0)
            {
                for (int i = 0; i < constantParameterPointList.Count; i++)
                {
                    ParameterData.Add(constantParameterPointList[i]);
                }
            }

        }

        /// <summary>
        /// 无参构造方法
        /// </summary>
        public ParameterViewModel()
        {
            IEC104.RegisterIEC104Handler(TypeID.C_RR_NA_1, this);// 读定值区号 201
            IEC104.RegisterIEC104Handler(TypeID.C_RS_NA_1, this); // 读参数和定值 202
            IEC104.RegisterIEC104Handler(TypeID.C_WS_NA_1, this);// 写参数和定值 203

            Messenger.Default.Register<object>(this, "ExportAllPoint", ExcuteExportAllPoint);
            Messenger.Default.Register<ConstantParameterExportModel>(this, "UpdateConstantPoint", ExcuteUpdateConstantPoint);
            // 注册接收更新相应页面的点表消息(点表重新导入后，需要更新相应页面显示的点表)
            Messenger.Default.Register<object>(this, "UpdateSourcePoint", ReloadParameterPoint);
            this._outputdata = new OutputData();
            send_Continue = false;
            
            ParameterCommand = new RelayCommand<string>(ExecuteParameterCommand);
            SelectAllCommand = new RelayCommand(ExecuteSelectAllCommand);

            ParameterAreaData = new ObservableCollection<int>();
            ParameterAreaData.Add(0x0000);
            ParameterAreaData.Add(0x0001);
            ParameterAreaData.Add(0x0002);
            this._parameterAreaSelectedIndex = 0;

            this._parameterDataZero = new List<ConstantParameter>();
            this._parameterDataOne = new List<ConstantParameter>();
            this._parameterDataTwo = new List<ConstantParameter>();
            this._parameterData = new ObservableCollection<ConstantParameter>();
            LoadParameterData("定值参数0区", null);

            this._comboxChecked = false;
        }

        /// <summary>
        /// 导出所有点表,包括三遥、定值0区、1区、2区点表
        /// </summary>
        private void ExcuteExportAllPoint(object obj)
        {
            // 获取定值参数0区点表
            IList<ConstantParameter> parameterDataZeroList = CurrentParameterArea == 0 ? ParameterData : GetParameterPointByParameterArea("定值参数0区", this._parameterDataZero);
            // 获取定值参数1区点表
            IList<ConstantParameter> parameterDataOneList = CurrentParameterArea == 1 ? ParameterData : GetParameterPointByParameterArea("定值参数1和2区", this._parameterDataOne);
            // 获取定值参数2区点表
            IList<ConstantParameter> parameterDataTwoList = CurrentParameterArea == 2 ? ParameterData : GetParameterPointByParameterArea("定值参数1和2区", this._parameterDataTwo);

            ManagePointTableService managePointTableService = new ManagePointTableService();
            string exportMsg = managePointTableService.ExportAllPoint(parameterDataZeroList, parameterDataOneList, parameterDataTwoList);
            if (!UtilHelper.IsEmpty(exportMsg))
            {
                MessageBox.Show(exportMsg);
            }
            
        }

        /// <summary>
        /// 重新载入定值参数点表
        /// </summary>
        /// <param name="obj"></param>
        private void ReloadParameterPoint(object obj)
        {
            // 重新加载当前定值区号点表
            if (currentParameterArea == 0)
            {
                LoadParameterData("定值参数0区", null);
            }
            else
            {
                LoadParameterData("定值参数1和2区", null);
            }

            // 清除定值0区参数加载显示数据集合
            if (this._parameterDataZero != null)
            {
                this._parameterDataZero.Clear();
            }

            // 清除定值1区参数加载显示数据集合
            if (this._parameterDataOne != null)
            {
                this._parameterDataOne.Clear();
            }

            // 清除定值2区参数加载显示数据集合
            if (this._parameterDataTwo != null)
            {
                this._parameterDataTwo.Clear();
            }

        }

        /// <summary>
        /// 导入全部点表后重新载入定值参数点表
        /// </summary>
        private void ExcuteUpdateConstantPoint(ConstantParameterExportModel constantParameterExportModel)
        {
            // 定义点表管理业务逻辑处理对象
            ManagePointTableService managePointTableService = new ManagePointTableService();
            // 导出定值参数0区点表转换为定值参数0区列表
            this._parameterDataZero = managePointTableService.TransferExportParameterToConstantParameter(constantParameterExportModel.ConstantParameterZeroList);
            // 导出定值参数1区点表转换为定值参数1区列表
            this._parameterDataOne = managePointTableService.TransferExportParameterToConstantParameter(constantParameterExportModel.ConstantParameterOneList);
            // 导出定值参数2区点表转换为定值参数2区列表
            this._parameterDataTwo = managePointTableService.TransferExportParameterToConstantParameter(constantParameterExportModel.ConstantParameterTwoList);

            // 重新加载当前定值区号点表
            UpdateConstantPoint(currentParameterArea);
            
        }

        /// <summary>
        /// 根据定值区号更新定值参数点表
        /// </summary>
        /// <param name="parameterArea">定值参数区号</param>
        private void UpdateConstantPoint(int parameterArea)
        {
            if (parameterArea == 0)
            {
                LoadParameterData("定值参数0区", this._parameterDataZero);
                return;
            }
            
            if (parameterArea == 1)
            {
                LoadParameterData("定值参数1和2区", this._parameterDataOne);
                return;
            }

            if (parameterArea == 2)
            {
                LoadParameterData("定值参数1和2区", this._parameterDataTwo);
                return;
            }
        }

    }
}

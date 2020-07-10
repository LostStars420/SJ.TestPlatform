using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using lib60870;
using FTU.Monitor.Model;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Threading;
using FTU.Monitor.Service;
using GalaSoft.MvvmLight.Messaging;
using FTU.Monitor.Util;

namespace FTU.Monitor.ViewModel
{
    /// <summary>
    /// CoefficientViewModel 的摘要说明
    /// author: songminghao
    /// date：2017/10/23 9:48:09
    /// desc：系数校准ViewModel
    /// version: 1.0
    /// </summary>
    public class CoefficientViewModel : ViewModelBase, IDisposable
    {
        /// <summary>
        /// 共同体
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
            /// 第0个字节(下标从0开始)
            /// </summary>
            [FieldOffset(0)]
            public byte byte0;

            /// <summary>
            /// 第1个字节(下标从0开始)
            /// </summary>
            [FieldOffset(1)]
            public byte byte1;

            /// <summary>
            /// 第2个字节(下标从0开始)
            /// </summary>
            [FieldOffset(2)]
            public byte byte2;

            /// <summary>
            /// 第3个字节(下标从0开始)
            /// </summary>
            [FieldOffset(3)]
            public byte byte3;
        };
        Union PARAMETER;

        /// <summary>
        /// 次数
        /// </summary>
        public static int number = 0;

        /// <summary>
        /// 定时器
        /// </summary>
        System.Timers.Timer t;

        /// <summary>
        /// 表格数据集合
        /// </summary>
        public static ObservableCollection<CoefficientBase> userData;

        /// <summary>
        /// 获取和设置表格数据集合
        /// </summary>
        public ObservableCollection<CoefficientBase> UserData
        {
            get
            {
                return userData;
            }
            set
            {
                userData = value;
                RaisePropertyChanged(() => UserData);
            }
        }

        /// <summary>
        /// 全选框，bool值
        /// </summary>
        private bool _comboxChecked;

        /// <summary>
        /// 获取和设置全选框值
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
        /// 全选框操作
        /// </summary>
        public RelayCommand SelectAllCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// 全选框操作执行方法
        /// </summary>
        void ExecuteSelectAllCommand()
        {
            foreach (CoefficientBase cb in UserData)
            {
                cb.Selected = ComboxChecked;
            }
        }

        /// <summary>
        /// 达到定时器时间执行事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">包含事件数据的 System.Timers.ElapsedEventArgs 对象</param>
        public void t_Elapsed(object sender, EventArgs e)
        {
            number++;
            if (number >= 10)
            {
                number = 0;
                t.Stop();
            }
            else
            {
                CommunicationViewModel.con.SendInterrogationCommand(CauseOfTransmission.ACTIVATION, 0x01, 20);
            }
        }

        /// <summary>
        /// 校准时间间隔
        /// </summary>
        private int _updateTime;

        /// <summary>
        /// 获取和设置校准时间间隔
        /// </summary>
        public int UpdateTime
        {
            get
            {
                return this._updateTime;
            }
            set
            {
                this._updateTime = value;
                RaisePropertyChanged(() => UpdateTime);
            }
        }
        
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public CoefficientViewModel()
        {
            // 注册接收更新相应页面的点表消息(点表重新导入后，需要更新相应页面显示的点表)
            Messenger.Default.Register<object>(this, "UpdateSourcePoint", ReloadCoefficientBasePoint);

            this.t = new System.Timers.Timer();
            this._updateTime = 1000;
            SelectAllCommand = new RelayCommand(ExecuteSelectAllCommand);
            CalibrationCommand = new RelayCommand<string>(ExecuteCalibrationCommand);
            t.Interval = UpdateTime;
            t.Elapsed += t_Elapsed;

            userData = new ObservableCollection<CoefficientBase>();
            //重新载入点表
            ReloadCoefficientBasePoint(null);
        }

        /// <summary>
        /// 重新载入系数校准点表
        /// </summary>
        /// <param name="obj"></param>
        private void ReloadCoefficientBasePoint(object obj)
        {
            // 获取使用的所有系数校准点表
            CoefficientManageService coefficientManageService = new CoefficientManageService();
            IList<CoefficientBase> coefficientBaseList = coefficientManageService.GetCoefficientPoint(ConfigUtil.getPointTypeID("系数校准"));
            if (coefficientBaseList != null && coefficientBaseList.Count > 0)
            {
                userData.Clear();
                foreach (var coefficientBase in coefficientBaseList)
                {
                    userData.Add(coefficientBase);
                }
            }

        }

        /// <summary>
        /// 校准操作
        /// </summary>
        public RelayCommand<string> CalibrationCommand 
        {
            get; 
            private set;
        }

        /// <summary>
        /// 校准操作执行方法
        /// </summary>
        /// <param name="arg">参数</param>
        public void ExecuteCalibrationCommand(string arg)
        {
            if(!CommunicationViewModel.IsLinkConnect())
            {
                return;
            }
            switch (arg)
            {
                // 开始校准
                case "Start":
                    // t.Start();

                    byte[]  temp = new byte[1024];
                    int len = 0;
                    byte dataCount = 0;
                    temp[len++] = 0;//定值区号 SN
                    temp[len++] = 0;
                    temp[len++] = 0x80;

                    for (int i = 0; i < UserData.Count; i++)
                    {
                        if (UserData[i].Selected == true)
                        {
                            PARAMETER.value = UserData[i].Value;

                            temp[len++] = (byte)UserData[i].ID;
                            temp[len++] = (byte)(UserData[i].ID >> 8);
                            if (CommunicationViewModel.con.Parameters.SizeOfIOA == 3)
                            {
                                temp[len++] = 0;
                            }
                            temp[len++] = 38;//TAG
                            temp[len++] = 4;
                            temp[len++] = PARAMETER.byte0;
                            temp[len++] = PARAMETER.byte1;
                            temp[len++] = PARAMETER.byte2;
                            temp[len++] = PARAMETER.byte3;
                           dataCount++;
                        }
                    }

                    byte[] buffer = new byte[len];
                    for (int i = 0; i < len; i++)
                    {
                        buffer[i] = temp[i];
                    }

                    CommunicationViewModel.con.SendSetParameterCommand((CauseOfTransmission)6, 0x01, buffer);                    
                    break;

                // 校准确认
                case "Confirm":
                    buffer = new byte[3];
                    //定值区号 SN
                    buffer[0] = 0;
                    buffer[1] = 0;
                    //无后续 固化
                    buffer[2] = 0;
                    CommunicationViewModel.con.SendSetParameterCommand((CauseOfTransmission)6, 0x01, buffer);
                    break;

                // 停止校准
                case "Stop":
                    t.Stop();
                    break;

                // 导入
                case "Import":
                    break;

                // 导出
                case "Export":
                    break;
            }
        }

        #region 实现IDisposable接口，释放资源

        /// <summary>
        /// 释放标志位
        /// </summary>
        private int _disposed;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">释放资源布尔值</param>
        protected virtual void Dispose(bool disposing)
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) != 0)
            {
                return;
            }

            if (disposing)
            {
                Destroy();
            }

        }

        /// <summary>
        /// 释放内存资源
        /// </summary>
        virtual protected void Destroy()
        {
            if (t != null)
            {
                t.Close();
                t.Dispose();
            }

        }

        #endregion 实现IDisposable接口，释放资源

    }
}

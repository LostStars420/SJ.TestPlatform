using System;
using lib60870;
using FTU.Monitor.Model;
using FTU.Monitor.ViewModel;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using System.Threading;
using FTU.Monitor.DataService;
using GalaSoft.MvvmLight.Messaging;
using FTU.Monitor.Util;
using FTU.Monitor.View;

namespace FTU.Monitor.lib60870
{
    /// <summary>
    /// FileService 的摘要说明
    /// author: songminghao
    /// date：2017/12/12 11:42:40
    /// desc：文件服务操作类
    /// version: 1.0
    /// </summary>
    public class FileService : InformationObject
    {
        /// <summary>
        /// 重写父类GetEncodedSize方法,获取信息对象长度(不包括信息对象地址)
        /// </summary>
        /// <returns></returns>
        override public int GetEncodedSize()
        {
            return 2;
        }

        /// <summary>
        /// 重写父类的Type方法，获取类型标识TypeID
        /// </summary>
        override public TypeID Type
        {
            get
            {
                return TypeID.F_FR_NA_1;
            }
        }

        /// <summary>
        /// 支持顺序(可变结构限定词中的SQ位是否可以为1).返回true,代表顺序,SQ为1;返回false,代表单个,SQ为0
        /// </summary>
        override public bool SupportsSequence
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 操作标识
        /// </summary>
        private byte operationLabel;

        /// <summary>
        /// 设置和获取操作标识
        /// </summary>
        public byte OperationLabel
        {
            get
            {
                return this.operationLabel;
            }
            set
            {
                this.operationLabel = value;
            }
        }

        /// <summary>
        /// 结果描述字
        /// </summary>
        private byte resoultByte;

        /// <summary>
        /// 设置和获取结果描述字
        /// </summary>
        public byte ResoultByte
        {
            get
            {
                return this.resoultByte;
            }
            set
            {
                this.resoultByte = value;
            }
        }

        /// <summary>
        /// 目录标识
        /// </summary>
        private int directoryID;

        /// <summary>
        /// 设置和获取目录标识
        /// </summary>
        public int DirectoryID
        {
            get
            {
                return this.directoryID;
            }
            set
            {
                this.directoryID = value;
            }
        }

        /// <summary>
        /// 后续标志
        /// </summary>
        private int continuationByte;

        /// <summary>
        /// 设置和获取后续标志
        /// </summary>
        public int ContinuationByte
        {
            get
            {
                return this.continuationByte;
            }
            set
            {
                this.continuationByte = value;
            }
        }

        /// <summary>
        /// 文件数量
        /// </summary>
        private byte fileNumber;

        /// <summary>
        /// 设置和获取文件数量
        /// </summary>
        public byte FileNumber
        {
            get
            { 
                return this.fileNumber;
            }
            set
            { 
                this.fileNumber = value;
            }
        }

        /// <summary>
        /// 文件名长度
        /// </summary>
        private byte filenameSize;

        /// <summary>
        /// 设置和获取文件名长度
        /// </summary>
        public byte FilenameSize
        {
            get
            {
                return this.filenameSize;
            }
            set
            { 
                this.filenameSize = value;
            }
        }

        /// <summary>
        /// 文件名
        /// </summary>
        private string filename;

        /// <summary>
        /// 设置和获取文件名
        /// </summary>
        public string Filename
        {
            get
            {
                return this.filename;
            }
            set
            {
                this.filename = value;
            }
        }

        /// <summary>
        /// 属性
        /// </summary>
        private byte fileProperty;

        /// <summary>
        /// 设置和获取属性
        /// </summary>
        public byte FileProperty
        {
            get
            {
                return this.fileProperty;
            }
            set
            {
                this.fileProperty = value;
            }
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        private int fileSize;

        /// <summary>
        /// 设置和获取文件大小
        /// </summary>
        public int FileSize
        {
            get
            {
                return this.fileSize;
            }
            set
            {
                this.fileSize = value;
            }
        }

        /// <summary>
        /// 文件时间
        /// </summary>
        private CP56Time2a fileTime;

        /// <summary>
        /// 设置和获取文件时间
        /// </summary>
        public CP56Time2a FileTime
        {
            get
            {
                return this.fileTime;
            }
            set
            {
                this.fileTime = value;
            }
        }

        /// <summary>
        /// 文件ID
        /// </summary>
        private int fileID;

        /// <summary>
        /// 设置和获取文件ID
        /// </summary>
        public int FileID
        {
            get
            {
                return this.fileID;
            }
            set
            {
                this.fileID = value;
            }
        }

        /// <summary>
        /// 数据段号
        /// </summary>
        private int segmentNumber;

        /// <summary>
        /// 设置和获取数据段号
        /// </summary>
        public int SegmentNumber
        {
            get
            {
                return this.segmentNumber;
            }
            set
            {
                this.segmentNumber = value;
            }
        }

        /// <summary>
        /// 文件数量
        /// </summary>
        private byte sum;

        /// <summary>
        /// 设置和获取文件数量
        /// </summary>
        public byte Sum
        {
            get
            {
                return this.sum;
            }
            set
            {
                this.sum = value;
            }
        }

        /// <summary>
        /// FileStream对象,读写文件操作
        /// </summary>
        static FileStream fs;

        /// <summary>
        /// 当前读取的文件名称
        /// </summary>
        private static string fileName;

        /// <summary>
        /// 有参构造方法
        /// </summary>
        /// <param name="parameters">连接配置参数</param>
        /// <param name="msg">信息对象数组</param>
        /// <param name="startIndex">信息数据起始索引</param>
        /// <param name="isSequence">是否是序列</param>
        internal FileService(ConnectionParameters parameters, byte[] msg, int startIndex, bool isSequence)
            : base(parameters, msg, startIndex, isSequence)
        {
            // startIndex += parameters.SizeOfIOA; /* skip IOA */

            // 跳过附加数据包类型字节
            startIndex++;

            // 获取操作标识,1个字节
            OperationLabel = msg[startIndex++];
            
            // 判断操作标识
            switch (OperationLabel)
            {
                #region 读目录

                //读目录
                case 0x02:

                    try
                    {
                        // 结果描述字,1个字节:0,成功;1,失败
                        ResoultByte = msg[startIndex++];
                        //接收成功
                        if (ResoultByte == 0)
                        {
                            // 目录ID,目录标识,低字节在前,4个字节
                            DirectoryID = msg[startIndex++] + (msg[startIndex++] << 8) + (msg[startIndex++] << 16) + (msg[startIndex++] << 24);
                            // 后续标志,1个字节:0,无后续,1,有后续
                            ContinuationByte = msg[startIndex++];

                            // 文件数量,1个字节:本帧文件数量
                            FileNumber = msg[startIndex++];
                            // 解析所有文件信息
                            for (int i = 0; i < FileNumber; i++)
                            {
                                // 文件名称长度,1个字节:表示文件名称所占用的x个字节
                                FilenameSize = msg[startIndex++];

                                // 定义文件基本信息对象
                                FileModel f = new FileModel();

                                // 文件名称:x个字节,与文件名称长度大小对应
                                for (int j = 0; j < FilenameSize; j++)
                                {
                                    f.Name += (char)msg[startIndex++];
                                }

                                // 文件属性:1个字节,备用
                                f.Remark = msg[startIndex++].ToString();

                                // 文件大小,4个字节:文件内容的字节数，便于传输结束后的简单校验，低字节在前
                                f.Size = msg[startIndex++] + (msg[startIndex++] << 8) + (msg[startIndex++] << 16) + (msg[startIndex++] << 24);

                                // CP56Time2a时标,七个字节
                                CP56Time2a t = new CP56Time2a(msg, startIndex);

                                // 时间
                                f.Time = t.ToStringDateTime();
                                // 跳过CP56Time2a时标所占的七个字节
                                startIndex += 7;

                                // 设置文件目录
                                f.IDOfDirectory = DirectoryID;
                                
                                // 判定当前文件是否是最新文件
                                if(FileServiceViewModel.existFileName == null || FileServiceViewModel.existFileName.Count == 0)
                                {
                                    f.Selected = true;
                                    FileServiceViewModel.existFileName.Add(f.Name);
                                }
                                else
                                {
                                    if(FileServiceViewModel.existFileName.Contains(f.Name))
                                    {
                                        // 如果该文件名已经存在
                                        f.Selected = false;
                                    }
                                    else
                                    {
                                        f.Selected = true;
                                        FileServiceViewModel.existFileName.Add(f.Name);
                                    }
                                }

                                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                   (ThreadStart)delegate()
                                   {
                                       // f.Number = FileServiceViewModel.fileData.Count;
                                       // 设置文件序号
                                       f.Number = FileServiceViewModel.fileData.Count;
                                       FileServiceViewModel.fileData.Add(f);
                                       FileServiceViewModel.fileSelectedIndex = FileServiceViewModel.fileData.Count - 1;
                                   });
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                    
                    break;

                #endregion 读目录

                #region 读文件激活确认
                case 0x04://读文件激活确认
                    ResoultByte = msg[startIndex++];
                    if (ResoultByte == 0)//接收成功
                    {
                        FilenameSize = msg[startIndex++];
                        FileModel f = new FileModel();
                        for (int j = 0; j < FilenameSize; j++)//去除路径 注意“/”和“\”
                        {
                            f.Name += (char)msg[startIndex++];
                        }

                        fileName = f.Name;
                        FileID = msg[startIndex++] + (msg[startIndex++] << 8) + (msg[startIndex++] << 16) + (msg[startIndex++] << 24);
                        f.Size = msg[startIndex++] + (msg[startIndex++] << 8) + (msg[startIndex++] << 16) + (msg[startIndex++] << 24);

                        string path = System.IO.Path.GetDirectoryName(f.Name);
                        string newPath = System.IO.Path.GetFileName(f.Name);

                        // 判断文件是否为录波文件
                        int FileNameIndex = f.Name.LastIndexOf("/") + 1;
                        string subString = f.Name.Substring(FileNameIndex, 3);
                        if (subString == "BAY")
                        {
                            //创建文件
                            fs = new FileStream(@".\Comtrade\" + newPath, FileMode.Create);
                        }
                        else
                        {
                            //创建文件
                            fs = new FileStream(@".\InteractiveFile\" + newPath, FileMode.Create);
                        }
                    }
                    else
                    {

                    }
                    break;
                #endregion

                #region 读文件数据传输
                case 0x05:

                    //读文件数据传输
                    FileID = msg[startIndex++] + (msg[startIndex++] << 8) + (msg[startIndex++] << 16) + (msg[startIndex++] << 24);
                    SegmentNumber = msg[startIndex++] + (msg[startIndex++] << 8) + (msg[startIndex++] << 16) + (msg[startIndex++] << 24);
                    ContinuationByte = msg[startIndex++];

                    Sum = msg[msg.Length - 1];
                    int sum_temp = 0;

                    for (int i = startIndex; i < (msg.Length - 1); i++)
                    {
                        sum_temp += msg[i];
                    }
                    if ((sum_temp % 256) == Sum)//接收到正确数据
                    {
                        if (fs.CanWrite == true)
                        {
                            fs.Seek(segmentNumber, SeekOrigin.Begin);
                            fs.Write(msg, startIndex, msg.Length - 12 - parameters.SizeOfIOA);
                        }
                        else
                        {
                            MessageBox.Show("当前文件不能写入！！");
                            MainViewModel.isParseUploadFile = false;
                            MainViewModel.waitUnbrokenFile.Set();
                        }
                    }

                    if (ContinuationByte == 0)//无后续，发送完成
                    {

                        byte[] buf = new byte[255];
                        int len = 0;
                        buf[len++] = 0;
                        buf[len++] = 0;
                        buf[len++] = 0x02;
                        buf[len++] = 0x06;//读文件数据响应

                        buf[len++] = (byte)FileID;//ID
                        buf[len++] = (byte)(FileID >> 8);
                        buf[len++] = (byte)(FileID >> 16);
                        buf[len++] = (byte)(FileID >> 24);

                        buf[len++] = (byte)SegmentNumber;//数据段号
                        buf[len++] = (byte)(SegmentNumber >> 8);
                        buf[len++] = (byte)(SegmentNumber >> 16);
                        buf[len++] = (byte)(SegmentNumber >> 24);
                        buf[len++] = 0;

                        byte[] buffer = new byte[len];
                        for (int j = 0; j < len; j++)
                        {
                            buffer[j] = buf[j];
                        }
                        CommunicationViewModel.con.SendFileServiceCommand(CauseOfTransmission.REQUEST, 1, buffer);
                        fs.Close();

                        if (fileName != null && fileName.Length != 0)
                        {
                            int fileNameIndex = fileName.LastIndexOf("/") + 1;
                            string subString = fileName.Substring(fileNameIndex);

                            if (subString == "ConfigurationSet.cfg" || subString == "HardwareInterfaceSet.cfg")
                            {
                                MainViewModel.isParseUploadFile = true;
                                MainViewModel.waitUnbrokenFile.Set();
                            }
                            else
                            {
                                string foreStr = subString.Substring(0,2);
                                if (foreStr == "BA" || foreStr == "co" || foreStr =="so" || foreStr == "fe")
                                {
                                    MainViewModel.isParseUploadFile = false;
                                    MainViewModel.waitUnbrokenFile.Set();
                                }
                            }
                        }
                        //FileServiceViewModel.m_dlgWaiting.TaskEnd(null);
                    }
                    break;
                #endregion

                #region 写文件激活确认
                case 0x08://写文件激活确认
                    ResoultByte = msg[startIndex++];
                    if (resoultByte == 0)//成功
                    {
                        FileServiceViewModel.sendfileContinue = true;
                        FileServiceViewModel.SendFileDataRunning = true;
                        FileServiceViewModel.pos = 0;
                        Thread t = new Thread(new ThreadStart(sendFileData));
                        t.IsBackground = true;
                        t.Start();
                    }
                    break;
                #endregion

                #region 写文件数据传输确认
                case 0x0a://写文件数据传输确认
                    FileServiceViewModel.SendFileDataRunning = false;
                    switch (msg[12])//结果描述字
                    {
                        case 0://成功
                            //ShowMessage.ShowFunction("写文件成功。");                         
                            MainViewModel.outputdata.ParseInformation += "写文件成功!\n";
                            break;
                        case 1://未知错误
                            //ShowMessage.ShowFunction("未知错误。");
                            MainViewModel.outputdata.ParseInformation += "未知错误!\n";
                            break;
                        case 2://校验和错误
                            //ShowMessage.ShowFunction("校验和错误。");
                            MainViewModel.outputdata.ParseInformation += "校验和错误!\n";
                            break;
                        case 3://文件长度不对
                            //ShowMessage.ShowFunction("文件长度不对。");
                            MainViewModel.outputdata.ParseInformation += "文件长度不对。\n";
                            break;
                        case 4://文件ID与激活ID不一致
                            //ShowMessage.ShowFunction("文件ID与激活ID不一致。");
                            MainViewModel.outputdata.ParseInformation += "文件ID与激活ID不一致!\n";
                            break;
                    }
                    break;
                #endregion
            }
        }

        internal override void Encode(Frame frame, ConnectionParameters parameters, bool isSequence)
        {
            base.Encode(frame, parameters, isSequence);

            //frame.AppendBytes (scaledValue.GetEncodedValue ());
        }

        /// <summary>
        /// 发送文件数据
        /// </summary>
        public void sendFileData()
        {
            byte[] buf = new byte[255];

            while (FileServiceViewModel.SendFileDataRunning == true)
            {
                if (Connection.running || (SerialPortService.serialPort.IsOpen && FileServiceViewModel.sendfileContinue))
                {
                    fileSize = FileServiceViewModel.fileSize;
                    sum = 0;
                    int len = 0;
                    buf[len++] = 0;
                    buf[len++] = 0;
                    if (CommunicationViewModel.con.Parameters.SizeOfIOA == 3)
                    {
                        buf[len++] = 0;
                    }
                    buf[len++] = 0x02;
                    buf[len++] = 0x09;

                    buf[len++] = 0;//ID
                    buf[len++] = 0;
                    buf[len++] = 0;
                    buf[len++] = 0;

                    buf[len++] = (byte)FileServiceViewModel.pos;//数据段号
                    buf[len++] = (byte)(FileServiceViewModel.pos >> 8);
                    buf[len++] = (byte)(FileServiceViewModel.pos >> 16);
                    buf[len++] = (byte)(FileServiceViewModel.pos >> 24);

                    if (FileServiceViewModel.fileSize - FileServiceViewModel.pos > 200)
                    {
                        buf[len++] = 1;//有后续
                        for (int i = 0; i < 200; i++)
                        {
                            buf[len++] = FileServiceViewModel.filebuf[i + FileServiceViewModel.pos];
                            sum += FileServiceViewModel.filebuf[i + FileServiceViewModel.pos];
                        }
                        FileServiceViewModel.pos += 200;
                        buf[len++] = sum;
                    }
                    else
                    {
                        buf[len++] = 0;//无后续
                        for (int i = 0; i < (FileServiceViewModel.fileSize - FileServiceViewModel.pos); i++)
                        {
                            buf[len++] = FileServiceViewModel.filebuf[i + FileServiceViewModel.pos];
                            sum += FileServiceViewModel.filebuf[i + FileServiceViewModel.pos];
                        }
                        FileServiceViewModel.pos += FileServiceViewModel.fileSize;
                        buf[len++] = sum;
                        FileServiceViewModel.SendFileDataRunning = false;
                    }


                    byte[] buffer = new byte[len];
                    for (int j = 0; j < len; j++)
                    {
                        buffer[j] = buf[j];
                    }

                    if (Connection.running)
                    {
                        Thread.Sleep(10);
                        while (Connection.IsSentBufferFull())
                        {
                            Thread.Sleep(100);
                        }
                        
                    }

                    CommunicationViewModel.con.SendFileServiceCommand(CauseOfTransmission.ACTIVATION, 1, buffer);
                    
                    if (SerialPortService.serialPort.IsOpen)
                    {
                        FileServiceViewModel.sendfileContinue = false;
                    }

                    int progressBarCurrentValue = (FileServiceViewModel.pos * 100 )/ FileServiceViewModel.fileSize;
                    Messenger.Default.Send<int>(progressBarCurrentValue, "progressBarValue");
                }
                //Thread.Sleep(1);

            }

        }
    }

}

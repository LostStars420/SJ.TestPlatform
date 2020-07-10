using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using FTU.Monitor.DataService;
using FTU.Monitor.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;


namespace FTU.Monitor.ViewModel
{
    public class SafetyViewModel : ViewModelBase
    {
        //public static int ConnectHandle;//连接句柄
        //public static byte[] randomBuffer = new byte[8];//主站随机数
        //public static byte[] certificate;
        //public byte[] Certificate
        //{
        //    get { return certificate; }
        //    set
        //    {
        //        certificate = value;

        //        RaisePropertyChanged("Certificate");
        //    }
        //}

        //public SafetyService s = new SafetyService();
        //public RelayCommand<string> SafetyCommand { get; private set; }
        //public SafetyViewModel()
        //{
        //    SafetyCommand = new RelayCommand<string>(ExecuteSafetyCommand);
        //}
        //public void ExecuteSafetyCommand(string arg)
        //{
        //    try
        //    {
        //        switch (arg)
        //        {

        //            case "Connect"://连接加密机
        //                ConnectHandle = CPPDLL.ConnectHSM("192.168.1.102", 2000, 100);
        //                Console.WriteLine("ConnectHandle={0}", ConnectHandle.ToString());
        //                break;
        //            case "Stop"://关闭加密机
        //                int result = CPPDLL.DisConnect(ConnectHandle);
        //                Console.WriteLine(result.ToString());
        //                break;
        //            case "ReadRandom"://取随机数

        //                result = CPPDLL.GenRand(ConnectHandle, 8, randomBuffer);
        //                Console.Write("随机数等于：");
        //                foreach (byte b in randomBuffer)
        //                {
        //                    Console.Write(b.ToString("X2"));
        //                }
        //                Console.WriteLine();
        //                byte[] buffer = s.PrepareFrame(0x50, null);
        //                foreach (byte b in buffer)
        //                {
        //                    Console.Write(b.ToString("X2"));
        //                }
        //                Console.WriteLine();
        //                break;
        //            case "ReadCertificate"://读终端证书
        //                //OpenFileDialog fileDialog = new OpenFileDialog();
        //                //fileDialog.Multiselect = true;
        //                //fileDialog.Title = "请选择文件";
        //                //fileDialog.Filter = "所有文件(*.*)|*.*";
        //                //if (fileDialog.ShowDialog()==true)
        //                //{
        //                //    string file = fileDialog.FileName;
        //                //    FileStream f = new FileStream(fileDialog.FileName,FileMode.Open);
        //                //    certificate=new byte[f.Length];
        //                //    f.Read(certificate, 0, 100);                         
        //                //    //MessageBox.Show("已选择文件:" + file);
        //                //}
        //                FileStream f = new FileStream("SOJOF3001.cer", FileMode.Open);
        //                byte[] temp = new byte[f.Length];
        //                f.Read(temp, 0, (int)f.Length);
        //                for (int i = 0; i < (int)f.Length; i++)
        //                {
        //                    if (temp[i] >= 0x41 && temp[i] <= 0x46)
        //                    {
        //                        temp[i] -= 0x37;
        //                    }
        //                    else
        //                    {
        //                        temp[i] -= 0x30;
        //                    }
        //                }
        //                //foreach (byte b in temp)
        //                //{
        //                //    Console.Write(b.ToString() + " ");
        //                //}
        //                certificate = new byte[f.Length / 2];
        //                for (int i = 0; i < f.Length / 2; i++)
        //                {
        //                    certificate[i] = (byte)((temp[i * 2] << 4) + temp[i * 2 + 1]);
        //                }
        //                foreach (byte b in certificate)
        //                {
        //                    Console.Write(b.ToString("X2") + " ");
        //                }
        //                //MessageBox.Show(certificate.Length.ToString());
        //                break;
        //            case "VerSign"://验证签名
        //                byte[] buf ={0x3D,0xBA,0x16,0x10,0x45,0x6E,0x5E,0x70,
        //                         0xEE,0x61,0x63,0xAA,0xD1,0x59,0x78,0xE1};
        //                byte[] buf1 ={
        //                         0x38,0x56,0xE5,0x40,0x78,0x0A,0x06,0x11
        //                        ,0x03,0xA5,0x24,0xEE,0x79,0x8A,0x08,0xED
        //                        ,0xC8,0xB3,0xDB,0x88,0x85,0x31,0xA1,0xDE
        //                        ,0x10,0x67,0x30,0x71,0x9E,0x47,0xA2,0x3B
        //                        ,0x9B,0xE9,0xFD,0x34,0x15,0x8B,0x0B,0x8C
        //                        ,0x12,0xF1,0x70,0xD9,0x2E,0xE1,0xD8,0x28
        //                        ,0x3D,0x41,0xDB,0xE2,0x2B,0xFD,0x39,0x2B
        //                        ,0xC7,0xAB,0x19,0x2E,0x8C,0x9E,0x73,0x17 
        //                        };
        //                result = CPPDLL.VerSign(ConnectHandle, certificate.Length,
        //                   certificate, 16, buf, 64, buf1);
        //                Console.WriteLine(result.ToString());
        //                break;
        //            case "InternalSign"://对终端随机数签名
        //                byte[] tem = new byte[64];
        //                byte[] buf2 = { 0xEE, 0x61, 0x63, 0xAA, 0xD1, 0x59, 0x78, 0xE1 };
        //                int len = 0;
        //                result = CPPDLL.InternalSign(ConnectHandle, 1, 8, buf2, tem, ref len);
        //                Console.WriteLine(result.ToString());
        //                Console.WriteLine(len.ToString());
        //                foreach (byte b in tem)
        //                {
        //                    Console.Write(b.ToString("X2") + " ");
        //                }
        //                break;
        //            case "ReadSerialNumber"://读终端芯片序列号
        //                break;
        //        }
        //    }
        //    catch(Exception e)
        //    {
        //        MainViewModel.outputdata.Debug += e.ToString();
        //    }
        //}
    }
}

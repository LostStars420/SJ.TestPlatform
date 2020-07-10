using FTU.Monitor.Dao;
using FTU.Monitor.EncryptExportModel;
using FTU.Monitor.Model;
using FTU.Monitor.UpLoadConfigurePointTable;
using FTU.Monitor.Util;
using FTU.Monitor.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FTU.Monitor.Service
{
    /// <summary>
    /// ManagePointTableService 的摘要说明
    /// author: liyan
    /// date：2018/6/21 11:15:21
    /// desc：点表管理业务逻辑处理(service)类
    /// version: 1.0
    /// </summary>
    public class ManagePointTableService
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public ManagePointTableService()
        {

        }

        /// <summary>
        /// 导入所有点号(密文)
        /// </summary>
        /// <param name="programVersion">程序版本号</param>
        /// <param name="promptMsg">提示信息</param>
        /// <param name="constantParameterExportModel">定值参数导出模型类</param>
        /// <returns>成功(true)或失败(false)标志</returns>
        public bool ImportAllPoint(string programVersion, ref string promptMsg, ref ConstantParameterExportModel constantParameterExportModel)
        {
            // 获取文件内容解密后的字符串
            string jsonToImport = ReportUtil.GetParameterDataCiphertext();
            // 判断加密的字符串解密是否成功
            if (UtilHelper.IsEmpty(jsonToImport))
            {
                promptMsg = "文件内容不正确，解密失败";
                return false;
            }

            // 将Json格式的字符串转换成对应的数据对象
            DevPointExportModel devPointExportModelImport = EncryptAndDecodeUtil.JsonToObject<DevPointExportModel>(jsonToImport);
            // 获取终端序列号
            // string programVersionFromJson = devPointExportModelImport.DeviceSerialNumber.Trim().Substring(0, 12);
            string programVersionFromJson = UtilHelper.GetTerminalSeriaNumber(devPointExportModelImport.DeviceSerialNumber.Trim());
            // 判断终端序列号和连接的终端序列号是否一致
            if (!programVersionFromJson.Equals(UtilHelper.GetTerminalSeriaNumber(programVersion.Trim())))
            {
                promptMsg = "即将导入的点表中，终端序列号不匹配";
                return false;
            }

            // 获取所有点表列表并更新到数据库
            DevPointDao devPointDaoImport = new DevPointDao();
            devPointDaoImport.batchInsert(devPointExportModelImport.DevPointList);
            constantParameterExportModel = devPointExportModelImport.ConstantParameterExportModel;

            promptMsg = "成功导入所有点表";
            return true;

        }

        /// <summary>
        /// 导出所有点号(密文)
        /// </summary>
        /// <returns>成功或失败提示信息</returns>
        public string ExportAllPoint(IList<ConstantParameter> parameterDataZero, IList<ConstantParameter> parameterDataOne, IList<ConstantParameter> parameterDataTwo)
        {
            // 提示信息
            string promptMsg = "";

            //创建点表Dao对象
            DevPointDao allDevPointDao = new DevPointDao();
            List<DevPoint> devPointList = new List<DevPoint>();

            //查询数据库获取要导出的点表列表
            for (int i = 0; i < ConfigUtil.pointTypeConfigList.Count; i++)
            {
                // 获取当前点号类型的所有点号
                devPointList.AddRange((List<DevPoint>)allDevPointDao.queryAllByPointTypeId(ConfigUtil.pointTypeConfigList[i].ID));
            }
            // 所有点表导出对象
            DevPointExportModel devPointExportModel = new DevPointExportModel();
            // 设置所有点号列表
            devPointExportModel.DevPointList = devPointList;
            // 设置包含值字段的定值参数点号列表
            devPointExportModel.ConstantParameterExportModel.ConstantParameterZeroList = TransferConstantParameterToExportParameter(parameterDataZero);
            devPointExportModel.ConstantParameterExportModel.ConstantParameterOneList = TransferConstantParameterToExportParameter(parameterDataOne);
            devPointExportModel.ConstantParameterExportModel.ConstantParameterTwoList = TransferConstantParameterToExportParameter(parameterDataTwo);
            // 设置终端序列号
            devPointExportModel.DeviceSerialNumber = InherentParameterViewModel.programVersion;
            // 将所有点号导出对象转换为Json格式字符串
            string devPointExportModelToJson = EncryptAndDecodeUtil.GetJson(devPointExportModel);
            // 给转换后的Json格式字符串进行AES加密
            string encrypt = EncryptAndDecodeUtil.AESEncrypt(devPointExportModelToJson, false);

            if (String.Empty.Equals(encrypt))
            {
                promptMsg = "导出内容加密失败";
                return promptMsg;
            }

            return ReportUtil.ExportParameterData("所有点表", encrypt);
        }

        /// <summary>
        /// 定值参数列表转换为导出定值参数列表
        /// </summary>
        /// <returns></returns>
        public IList<ContantParameterForExport> TransferConstantParameterToExportParameter(IList<ConstantParameter> parameterData)
        {
            if (parameterData == null || parameterData.Count == 0)
            {
                return null;
            }

            // 定值参数导出对象
            IList<ContantParameterForExport> constantParameterForExportList = new List<ContantParameterForExport>();
            // 清空定值参数对象列表
            constantParameterForExportList.Clear();
            for (int i = 0; i < parameterData.Count; i++)
            {
                ContantParameterForExport contantParameterForExport = new ContantParameterForExport();
                contantParameterForExport.Selected = parameterData[i].Selected;
                contantParameterForExport.Number = parameterData[i].Number;
                contantParameterForExport.ID = parameterData[i].ID;
                contantParameterForExport.Name = parameterData[i].Name;
                contantParameterForExport.StringValue = parameterData[i].StringValue;
                contantParameterForExport.Unit = parameterData[i].Unit;
                contantParameterForExport.Comment = parameterData[i].Comment;
                contantParameterForExport.MinValue = parameterData[i].MinValue;
                contantParameterForExport.MaxValue = parameterData[i].MaxValue;
                contantParameterForExport.Value = parameterData[i].Value;
                contantParameterForExport.Enable = parameterData[i].Enable;

                // 添加到定值参数对象列表
                constantParameterForExportList.Add(contantParameterForExport);
            }
            return constantParameterForExportList;

        }

        /// <summary>
        /// 导出定值参数点表转换为定值参数列表
        /// </summary>
        /// <param name="constantParameterListForExportList">导出定值参数点表集合</param>
        /// <returns></returns>
        public IList<ConstantParameter> TransferExportParameterToConstantParameter(IList<ContantParameterForExport> constantParameterListForExportList)
        {
            if (constantParameterListForExportList == null || constantParameterListForExportList.Count == 0)
            {
                return null;
            }
            IList<ConstantParameter> constantParameterList = new List<ConstantParameter>();
            foreach (var item in constantParameterListForExportList)
            {
                ConstantParameter constantParameter = new ConstantParameter();
                constantParameter.Selected = item.Selected;
                constantParameter.Number = item.Number;
                constantParameter.ID = item.ID;
                constantParameter.Name = item.Name;
                constantParameter.StringValue = item.StringValue;
                constantParameter.Unit = item.Unit;
                constantParameter.Comment = item.Comment;
                constantParameter.MinValue = item.MinValue;
                constantParameter.MaxValue = item.MaxValue;
                constantParameter.Value = item.Value;
                constantParameter.Enable = item.Enable;

                constantParameterList.Add(constantParameter);
            }
            return constantParameterList;
        }

        /// <summary>
        /// 批量操作,将点表导入到数据库
        /// </summary>
        /// <param name="pointTypeInfos">点号类型配置类列表</param>
        /// <param name="alldevPointImportList">点表集合</param>
        /// <returns>成功或失败提示信息</returns>
        public string BatchInsertToDB(IList<PointTypeInfo> pointTypeInfos, IList<DevPoint> alldevPointImportList)
        {
            try
            {
                DevPointDao devPointDaoImport = new DevPointDao();
                IList<DevPoint> devPointImportList = new List<DevPoint>();
                for (int i = 0; i < pointTypeInfos.Count; ++i)
                {
                    // 遍历所有点表，并找到对应的点表集合
                    foreach (DevPoint devPointItem in alldevPointImportList)
                    {
                        if (devPointItem.PointTypeId.Equals(pointTypeInfos[i].ID))
                        {
                            devPointImportList.Add(devPointItem);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    // 批量导入对应的点表
                    devPointDaoImport.batchInsert(pointTypeInfos[i].ID, devPointImportList);
                    // 清空点表存储空间
                    devPointImportList.Clear();
                }
                return "";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 解析终端上传点表的json格式数据并更新至数据库中
        /// </summary>
        /// <param name="pointNumberCollection"></param>
        public void AnalysisPointNumberCollectionAndInsertIntoDatabase(PointNumberCollection pointNumberCollection)
        {
            // 所有点号集合
            IList<DevPoint> allPoint = new List<DevPoint>();

            string errorMsg = "";

            #region 解析终端序列号
            string terminalSerialNumber = pointNumberCollection.productSerialNumber;
            if (terminalSerialNumber != null)
            {
                Messenger.Default.Send<string>(terminalSerialNumber, "UpdateProductSerialNumber");
            }

            #endregion 解析终端序列号

            #region 解析遥测点号

            IList<DevPoint> telemeteringDevPointList = null;
            if (pointNumberCollection.TelemeteringForUpLoad == null || pointNumberCollection.TelemeteringForUpLoad.Count == 0)
            {
                errorMsg += "遥测点号为空;";
            }
            else
            {
                IList<TelemeteringForUpLoad> telemeteringForUpLoad = pointNumberCollection.TelemeteringForUpLoad;
                telemeteringDevPointList = new List<DevPoint>();
                // 获取遥测点号类型编号
                int telemeterinPointTypeID = ConfigUtil.getPointTypeID("遥测");
                // 添加一个空点号（点号为0，名称也为0）
                DevPoint nullPoint = new DevPoint();
                nullPoint.PointTypeId = telemeterinPointTypeID;
                nullPoint.ID = "0";
                nullPoint.Name = "0";
                telemeteringDevPointList.Add(nullPoint);
                allPoint.Add(nullPoint);

                // 遥测点号起始地址前址
                int telemeterinPointIDStartIndex = 0x4000;
                foreach (var telemeteringForUpLoadPoint in telemeteringForUpLoad)
                {
                    DevPoint telemeteringDevPoint = new DevPoint();
                    telemeteringDevPoint.PointTypeId = telemeterinPointTypeID;
                    telemeteringDevPoint.ID = (++telemeterinPointIDStartIndex).ToString("x4").ToUpper();
                    telemeteringDevPoint.Name = telemeteringForUpLoadPoint.pNameUp;
                    telemeteringDevPoint.Value = 0;
                    telemeteringDevPoint.Unit = telemeteringForUpLoadPoint.pUnit;
                    telemeteringDevPoint.Comment = "";
                    telemeteringDevPoint.Rate = 1;
                    telemeteringDevPoint.Flag = 0;
                    telemeteringDevPoint.MinValue = 0;
                    telemeteringDevPoint.MaxValue = 0;
                    telemeteringDevPointList.Add(telemeteringDevPoint);
                    allPoint.Add(telemeteringDevPoint);
                }
            }

            #endregion 解析遥测点号

            #region 解析遥信点号

            IList<DevPoint> telesignalDevPointList = null;
            if (pointNumberCollection.TelesignalForUpLoad == null || pointNumberCollection.TelesignalForUpLoad.Count == 0)
            {
                errorMsg += "遥信点号为空;";
            }
            else
            {
                IList<TelesignalForUpLoad> telesignalForUpLoad = pointNumberCollection.TelesignalForUpLoad;
                telesignalDevPointList = new List<DevPoint>();
                // 获取遥信点号类型编号
                int telesignalPointTypeID = ConfigUtil.getPointTypeID("遥信");
                // 添加一个空点号（点号为0，名称也为0）
                DevPoint nullPoint = new DevPoint();
                nullPoint.PointTypeId = telesignalPointTypeID;
                nullPoint.ID = "0";
                nullPoint.Name = "0";
                telesignalDevPointList.Add(nullPoint);
                allPoint.Add(nullPoint);

                // 遥信点号起始地址前址
                int telesignalPointIDStartIndex = 0x0000;
                foreach (var telesignalForUpLoadPoint in telesignalForUpLoad)
                {
                    DevPoint telesignalDevPoint = new DevPoint();
                    telesignalDevPoint.PointTypeId = telesignalPointTypeID;
                    telesignalDevPoint.ID = (++telesignalPointIDStartIndex).ToString("x4").ToUpper();
                    telesignalDevPoint.Name = telesignalForUpLoadPoint.pName;
                    telesignalDevPoint.Value = (telesignalForUpLoadPoint.pVal == null || telesignalForUpLoadPoint.pVal.Count == 0) ? 0 : telesignalForUpLoadPoint.pVal[0];
                    telesignalDevPoint.Unit = "";

                    string comment = "";
                    if (telesignalForUpLoadPoint.pContentYx != null && telesignalForUpLoadPoint.pContentYx.Count > 0)
                    {
                        for (int i = 0; i < telesignalForUpLoadPoint.pContentYx.Count; i++)
                        {
                            comment += i + "." + telesignalForUpLoadPoint.pContentYx[i] + " ";
                        }
                    }
                    telesignalDevPoint.Comment = comment;

                    telesignalDevPoint.Rate = 0;
                    telesignalDevPoint.Flag = 0;
                    telesignalDevPoint.MinValue = 0;
                    telesignalDevPoint.MaxValue = 0;
                    telesignalDevPointList.Add(telesignalDevPoint);
                    allPoint.Add(telesignalDevPoint);
                }
            }

            #endregion 解析遥信点号

            #region 解析遥控点号

            IList<DevPoint> telecontrolDevPointList = null;
            if (pointNumberCollection.TelecontrolForUpLoad == null || pointNumberCollection.TelecontrolForUpLoad.Count == 0)
            {
                errorMsg += "遥控点号为空;操作记录点号为空;";
            }
            else
            {
                IList<TelecontrolForUpLoad> telecontrolForUpLoad = pointNumberCollection.TelecontrolForUpLoad;
                telecontrolDevPointList = new List<DevPoint>();
                // 获取遥控点号类型编号
                int telecontrolPointTypeID = ConfigUtil.getPointTypeID("遥控");
                // 添加一个空点号（点号为0，名称也为0）
                DevPoint nullPoint = new DevPoint();
                nullPoint.PointTypeId = telecontrolPointTypeID;
                nullPoint.ID = "0";
                nullPoint.Name = "0";
                // telecontrolDevPointList.Add(nullPoint);
                allPoint.Add(nullPoint);

                // 遥控点号起始地址前址
                int telecontrolPointIDStartIndex = 0x6000;
                foreach (var telecontrolForUpLoadPoint in telecontrolForUpLoad)
                {
                    DevPoint telecontrolDevPoint = new DevPoint();
                    telecontrolDevPoint.PointTypeId = telecontrolPointTypeID;
                    telecontrolDevPoint.ID = (++telecontrolPointIDStartIndex).ToString("x4").ToUpper();
                    telecontrolDevPoint.Name = telecontrolForUpLoadPoint.pName;
                    telecontrolDevPoint.Value = 0;
                    telecontrolDevPoint.Unit = "";
                    telecontrolDevPoint.Comment = "";
                    telecontrolDevPoint.Rate = 0;
                    telecontrolDevPoint.Flag = 0;
                    telecontrolDevPoint.MinValue = 0;
                    telecontrolDevPoint.MaxValue = 0;
                    telecontrolDevPointList.Add(telecontrolDevPoint);
                    allPoint.Add(telecontrolDevPoint);
                }
            }

            #endregion 解析遥控点号

            #region 解析定值参数0区点号

            IList<DevPoint> fixedValueAreaZeroDevPointList = null;
            if (pointNumberCollection.FixedValueAreaZero == null || pointNumberCollection.FixedValueAreaZero.Count == 0)
            {
                errorMsg += "定值参数0区点号为空;";
            }
            else
            {
                IList<FixedParameter> fixedValueAreaZero = pointNumberCollection.FixedValueAreaZero;
                fixedValueAreaZeroDevPointList = new List<DevPoint>();
                // 获取定值参数0区点号类型编号
                int fixedValueAreaZeroPointTypeID = ConfigUtil.getPointTypeID("定值参数0区");
                // 定值参数0区点号起始地址前址
                int fixedValueAreaZeroPointIDStartIndex = 0x8100;
                foreach (var fixedValueAreaZeroPoint in fixedValueAreaZero)
                {
                    DevPoint fixedValueAreaZeroDevPoint = new DevPoint();
                    fixedValueAreaZeroDevPoint.Enable = fixedValueAreaZeroPoint.enable;
                    fixedValueAreaZeroDevPoint.PointTypeId = fixedValueAreaZeroPointTypeID;
                    fixedValueAreaZeroDevPoint.ID = (++fixedValueAreaZeroPointIDStartIndex).ToString("x4").ToUpper();
                    fixedValueAreaZeroDevPoint.Name = fixedValueAreaZeroPoint.pName;
                    fixedValueAreaZeroDevPoint.Value = 0;
                    fixedValueAreaZeroDevPoint.Unit = fixedValueAreaZeroPoint.pUnit;
                    fixedValueAreaZeroDevPoint.Rate = 0;
                    fixedValueAreaZeroDevPoint.Flag = 0;
                    fixedValueAreaZeroDevPoint.MinValue = fixedValueAreaZeroPoint.valMin;
                    fixedValueAreaZeroDevPoint.MaxValue = fixedValueAreaZeroPoint.valMax;
                    fixedValueAreaZeroDevPoint.DefaultValue = fixedValueAreaZeroPoint.defaultVal;

                    string comment = "";
                    if (fixedValueAreaZeroPoint.pContent != null && fixedValueAreaZeroPoint.pContent.Count > 0)
                    {
                        int begin = (int)fixedValueAreaZeroPoint.valMin;
                        foreach (var str in fixedValueAreaZeroPoint.pContent)
                        {
                            comment += (begin++) + "." + str + " ";
                        }
                    }
                    if (fixedValueAreaZeroPoint.pNote != null)
                    {
                        comment += "  " + fixedValueAreaZeroPoint.pNote; 
                    }

                    fixedValueAreaZeroDevPoint.Comment = comment;

                    fixedValueAreaZeroDevPointList.Add(fixedValueAreaZeroDevPoint);
                    allPoint.Add(fixedValueAreaZeroDevPoint);
                }
            }

            #endregion 解析定值参数0区点号

            #region 解析定值参数1和2区点号

            IList<DevPoint> fixedValueAreaOneDevPointList = null;
            if (pointNumberCollection.FixedValueAreaOne == null || pointNumberCollection.FixedValueAreaOne.Count == 0)
            {
                errorMsg += "定值参数1和2区点号为空;";
            }
            else
            {
                IList<FixedParameter> fixedValueAreaOne = pointNumberCollection.FixedValueAreaOne;
                fixedValueAreaOneDevPointList = new List<DevPoint>();
                // 获取定值参数1和2区点号类型编号
                int fixedValueAreaOnePointTypeID = ConfigUtil.getPointTypeID("定值参数1和2区");
                // 定值参数1和2区点号起始地址前址
                int fixedValueAreaOnePointIDStartIndex = 0x8300;
                foreach (var fixedValueAreaOnePoint in fixedValueAreaOne)
                {
                    DevPoint fixedValueAreaOneDevPoint = new DevPoint();
                    fixedValueAreaOneDevPoint.Enable = fixedValueAreaOnePoint.enable;
                    fixedValueAreaOneDevPoint.PointTypeId = fixedValueAreaOnePointTypeID;
                    fixedValueAreaOneDevPoint.ID = (++fixedValueAreaOnePointIDStartIndex).ToString("x4").ToUpper();
                    fixedValueAreaOneDevPoint.Name = fixedValueAreaOnePoint.pName;
                    fixedValueAreaOneDevPoint.Value = 0;
                    fixedValueAreaOneDevPoint.Unit = fixedValueAreaOnePoint.pUnit;
                    fixedValueAreaOneDevPoint.Rate = 0;
                    fixedValueAreaOneDevPoint.Flag = 0;
                    fixedValueAreaOneDevPoint.MinValue = fixedValueAreaOnePoint.valMin;
                    fixedValueAreaOneDevPoint.MaxValue = fixedValueAreaOnePoint.valMax;
                    fixedValueAreaOneDevPoint.DefaultValue = fixedValueAreaOnePoint.defaultVal;

                    string comment = "";
                    if (fixedValueAreaOnePoint.pContent != null && fixedValueAreaOnePoint.pContent.Count > 0)
                    {
                        int begin = (int)fixedValueAreaOnePoint.valMin;
                        foreach (var str in fixedValueAreaOnePoint.pContent)
                        {
                            comment += (begin++) + "." + str + " ";
                        }
                    }
                    if (fixedValueAreaOnePoint.pNote != "")
                    {
                        comment += "  " + fixedValueAreaOnePoint.pNote;
                    }

                    fixedValueAreaOneDevPoint.Comment = comment;

                    fixedValueAreaOneDevPointList.Add(fixedValueAreaOneDevPoint);
                    allPoint.Add(fixedValueAreaOneDevPoint);
                }
            }

            #endregion 解析定值参数1和2区点号

            #region 解析固有参数点号

            IList<DevPoint> inherentParameterDevPointList = null;
            if (pointNumberCollection.InherentParameter == null || pointNumberCollection.InherentParameter.Count == 0)
            {
                errorMsg += "固有定值参数点号为空;";
            }
            else
            {
                IList<FixedParameter> inherentParameter = pointNumberCollection.InherentParameter;
                inherentParameterDevPointList = new List<DevPoint>();
                // 获取固有定值参数点号类型编号
                int inherentParameterPointTypeID = ConfigUtil.getPointTypeID("固有定值参数");
                // 固有定值参数点号起始地址前址
                int inherentParameterPointIDStartIndex = 0x8000;
                foreach (var inherentParameterPoint in inherentParameter)
                {
                    DevPoint inherentParameterDevPoint = new DevPoint();
                    inherentParameterDevPoint.PointTypeId = inherentParameterPointTypeID;
                    inherentParameterDevPoint.ID = (++inherentParameterPointIDStartIndex).ToString("x4").ToUpper();
                    inherentParameterDevPoint.Name = inherentParameterPoint.pName;
                    inherentParameterDevPoint.Value = 0;
                    inherentParameterDevPoint.Unit = "";
                    inherentParameterDevPoint.Comment = "";
                    inherentParameterDevPoint.Rate = 0;
                    inherentParameterDevPoint.Flag = 0;
                    inherentParameterDevPoint.MinValue = 0;
                    inherentParameterDevPoint.MaxValue = 0;
                    inherentParameterDevPointList.Add(inherentParameterDevPoint);
                    allPoint.Add(inherentParameterDevPoint);
                }
            }

            #endregion 解析固有参数点号

            #region 解析系数校准点号

            IList<DevPoint> calibrateCoefficientDevPointList = null;
            if (pointNumberCollection.CalibrateCoefficient == null || pointNumberCollection.CalibrateCoefficient.Count == 0)
            {
                errorMsg += "系数校准点号为空;";
            }
            else
            {
                IList<CalibrateCoefficient> calibrateCoefficient = pointNumberCollection.CalibrateCoefficient;
                calibrateCoefficientDevPointList = new List<DevPoint>();
                // 获取系数校准点号类型编号
                int calibrateCoefficientPointTypeID = ConfigUtil.getPointTypeID("系数校准");
                // 系数校准点号起始地址前址
                int calibrateCoefficientPointIDStartIndex = 0x823F;
                foreach (var calibrateCoefficientPoint in calibrateCoefficient)
                {
                    DevPoint calibrateCoefficientDevPoint = new DevPoint();
                    calibrateCoefficientDevPoint.PointTypeId = calibrateCoefficientPointTypeID;
                    calibrateCoefficientDevPoint.ID = (++calibrateCoefficientPointIDStartIndex).ToString("x4").ToUpper();
                    calibrateCoefficientDevPoint.Name = calibrateCoefficientPoint.pName;
                    calibrateCoefficientDevPoint.Value = 0;
                    calibrateCoefficientDevPoint.Unit = "";
                    calibrateCoefficientDevPoint.Comment = "";
                    calibrateCoefficientDevPoint.Rate = 0;
                    calibrateCoefficientDevPoint.Flag = 0;
                    calibrateCoefficientDevPoint.MinValue = 0;
                    calibrateCoefficientDevPoint.MaxValue = 0;
                    calibrateCoefficientDevPointList.Add(calibrateCoefficientDevPoint);
                    allPoint.Add(calibrateCoefficientDevPoint);
                }
            }

            #endregion 解析系数校准点号

            #region 解析操作记录点号

            IList<DevPoint> operationRecordDevPointList = null;
            if (telecontrolDevPointList != null && telecontrolDevPointList.Count > 0)
            {
                operationRecordDevPointList = new List<DevPoint>();
                // 获取操作记录点号类型编号
                int operationRecordPointTypeID = ConfigUtil.getPointTypeID("操作记录");
                foreach (var telecontrolDevPoint in telecontrolDevPointList)
                {
                    DevPoint operationRecordPoint = new DevPoint();
                    operationRecordPoint.PointTypeId = operationRecordPointTypeID;
                    operationRecordPoint.ID = telecontrolDevPoint.ID;
                    operationRecordPoint.Name = telecontrolDevPoint.Name;
                    operationRecordPoint.Value = telecontrolDevPoint.Value;
                    operationRecordPoint.Unit = telecontrolDevPoint.Unit;
                    operationRecordPoint.Comment = telecontrolDevPoint.Comment;
                    operationRecordPoint.Rate = telecontrolDevPoint.Rate;
                    operationRecordPoint.Flag = telecontrolDevPoint.Flag;
                    operationRecordPoint.MinValue = telecontrolDevPoint.MinValue;
                    operationRecordPoint.MaxValue = telecontrolDevPoint.MaxValue;
                    operationRecordDevPointList.Add(operationRecordPoint);
                    allPoint.Add(operationRecordPoint);
                }
            }

            #endregion 解析操作记录点号

            #region 批量导入到数据库，并更新相应页面的点表

            DevPointDao devPointDao = new DevPointDao();
            devPointDao.batchInsert(allPoint);

            #endregion 批量导入到数据库，并更新相应页面的点表

        }

        /// <summary>
        /// 获取数据库中程序版本号
        /// </summary>
        /// <returns>程序版本号类</returns>
        public ProgramVersionModel GetProgramVersion()
        {
            // 程序版本号数据库操作类
            ProgramVersionDao programVersionDao = new ProgramVersionDao();
            IList<ProgramVersionModel> programVersionModelList = programVersionDao.query();
            if (programVersionModelList != null && programVersionModelList.Count > 0)
            {
                return programVersionModelList[0];
            }
            return null;
        }

    }
}

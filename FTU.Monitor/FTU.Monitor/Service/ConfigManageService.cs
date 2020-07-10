using FTU.Monitor.Dao;
using FTU.Monitor.Model;
using System;
using System.Collections.Generic;

namespace FTU.Monitor.Service
{
    /// <summary>
    /// ConfigManageService 的摘要说明
    /// author: liyan
    /// date：2018/7/2 9:26:18
    /// desc：配置管理业务逻辑处理(service)类
    /// version: 1.0
    /// </summary>
    public class ConfigManageService
    {
        /// <summary>
        /// 将使用的点表保存到数据库
        /// </summary>
        /// <param name="TelemeteringPoint">要下发的遥测点号</param>
        /// <param name="TelesignalisationPoint">要下发的遥信点号</param>
        /// <param name="TelecontrolPoint">要下发的遥控点号</param>
        public void SaveUsedPointToDB(IList<Telemetering> TelemeteringPoint, IList<Telesignalisation> TelesignalisationPoint, IList<Telecontrol> TelecontrolPoint)
        {
            try
            {
                // 定义所有使用的点表集合
                IList<PointUsed> allPointUsed = new List<PointUsed>();
                // 定义DAO对象,将使用的点表插入到数据库中
                PointUsedDao pointUsedDao = new PointUsedDao();

                #region 获取使用的遥测点表

                IList<PointUsed> telemeteringViewPointUsedList = new List<PointUsed>();
                for (int i = 0; i < TelemeteringPoint.Count; i++)
                {
                    PointUsed telemeteringViewPointUsed = new PointUsed();
                    telemeteringViewPointUsed.NO = i + 1;
                    telemeteringViewPointUsed.Devpid = TelemeteringPoint[i].Devpid;
                    telemeteringViewPointUsed.UsedRate = TelemeteringPoint[i].Rate;

                    telemeteringViewPointUsedList.Add(telemeteringViewPointUsed);
                    allPointUsed.Add(telemeteringViewPointUsed);
                }

                #endregion 获取使用的遥测点表

                #region 获取使用的遥信点表

                IList<PointUsed> telesignalisationPointUsedList = new List<PointUsed>();
                for (int i = 0; i < TelesignalisationPoint.Count; i++)
                {
                    PointUsed telesignalisationPointUsed = new PointUsed();
                    telesignalisationPointUsed.NO = i + 1;
                    telesignalisationPointUsed.Devpid = TelesignalisationPoint[i].Devpid;
                    telesignalisationPointUsed.IsChanged = TelesignalisationPoint[i].IsChanged ? 1 : 0;
                    telesignalisationPointUsed.IsSOE = TelesignalisationPoint[i].IsSOE ? 1 : 0;
                    telesignalisationPointUsed.IsNegated = TelesignalisationPoint[i].IsNegated ? 1 : 0;
                    telesignalisationPointUsed.DoublePoint = TelesignalisationPoint[i].DoublePoint ? 1 : 0;

                    telesignalisationPointUsedList.Add(telesignalisationPointUsed);
                    allPointUsed.Add(telesignalisationPointUsed);
                }

                #endregion 获取使用的遥信点表

                #region 获取使用的遥控点表

                IList<PointUsed> telecontrolPointUsedList = new List<PointUsed>();
                for (int i = 0; i < TelecontrolPoint.Count; i++)
                {
                    PointUsed telecontrolPointUsed = new PointUsed();
                    telecontrolPointUsed.NO = i + 1;
                    telecontrolPointUsed.Devpid = TelecontrolPoint[i].Devpid;
                    telecontrolPointUsed.IsNegated = TelecontrolPoint[i].IsNegated ? 1 : 0;

                    telecontrolPointUsedList.Add(telecontrolPointUsed);
                    allPointUsed.Add(telecontrolPointUsed);
                }

                #endregion 获取使用的遥控点表

                // 将使用的所有终端点表导入到数据库
                pointUsedDao.BatchInsert(allPointUsed);
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}

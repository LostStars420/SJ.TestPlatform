using FTU.Monitor.Dao;
using FTU.Monitor.Model;
using System;
using System.Collections.Generic;

namespace FTU.Monitor.Service
{
    /// <summary>
    /// CoefficientManageService 的摘要说明
    /// author: liyan
    /// date：2018/6/26 10:32:21
    /// desc：校准系数业务逻辑处理(service)类
    /// version: 1.0
    /// </summary>
    public class CoefficientManageService
    {
        /// <summary>
        /// 根据点号类型编号获取系数校准点表
        /// </summary>
        /// <param name="pointTypeId">点号类型编号</param>
        /// <returns>系数校准点表</returns>
        public IList<CoefficientBase> GetCoefficientPoint(int pointTypeId)
        {
            // 获取使用的所有系数校准点表
            DevPointDao devPointDao = new DevPointDao();
            IList<DevPoint> devPointList = devPointDao.queryByPointTypeId(pointTypeId);

            if (devPointList == null || devPointList.Count == 0)
            {
                return null;
            }

            IList<CoefficientBase> coefficientBaseList = new List<CoefficientBase>();
            for (int i = 0; i < devPointList.Count; i++)
            {
                CoefficientBase coefficientBase = new CoefficientBase();
                coefficientBase.Number = i + 1;
                coefficientBase.ID = Convert.ToInt32(devPointList[i].ID, 16);
                coefficientBase.Name = devPointList[i].Name;
                coefficientBase.Value = (float)devPointList[i].Value;

                coefficientBaseList.Add(coefficientBase);
            }
            return coefficientBaseList;
        }

    }
}

using FTU.Monitor.Dao;
using FTU.Monitor.Model;
using System.Collections.Generic;

namespace FTU.Monitor.Service
{
    /// <summary>
    /// TelemeteringManageService 的摘要说明
    /// author: liyan
    /// date：2018/6/26 15:39:09
    /// desc：遥测业务逻辑处理(service)类
    /// version: 1.0
    /// </summary>
    public class TelemeteringManageService
    {

        /// <summary>
        /// 根据点号类型编号获取使用的所有遥测点表
        /// </summary>
        /// <param name="pointTypeId">点号类型编号</param>
        /// <returns>使用的所有遥测点表</returns>
        public IList<Telemetering> GetTelemeteringPoint(int pointTypeId)
        {
            //获取使用的所有遥测点表
            PointUsedDao pointUsedDao = new PointUsedDao();
            IList<PointUsed> pointUsedList = pointUsedDao.queryByPointTypeId(pointTypeId);
            if (pointUsedList == null || pointUsedList.Count == 0)
            {
                return null;
            }

            IList<Telemetering> telemeteringList = new List<Telemetering>();
            for (int i = 0; i < pointUsedList.Count; i++)
            {
                Telemetering telemetering = new Telemetering();
                telemetering.Number = i + 1;
                //telemetering.ID = Convert.ToInt32(listID[i].InnerText, 16);
                telemetering.ID = i + 16385;
                telemetering.Name = pointUsedList[i].Name;
                telemetering.Value = 0;
                telemetering.Unit = pointUsedList[i].Unit;
                telemetering.Comment = pointUsedList[i].Comment;
                telemetering.Rate = (float)pointUsedList[i].UsedRate;

                telemeteringList.Add(telemetering);
            }
            return telemeteringList;

        }

    }
}

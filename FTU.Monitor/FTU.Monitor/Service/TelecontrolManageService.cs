using FTU.Monitor.Dao;
using FTU.Monitor.Model;
using System;
using System.Collections.Generic;

namespace FTU.Monitor.Service
{
    /// <summary>
    /// TelecontrolManageService 的摘要说明
    /// author: liyan
    /// date：2018/6/26 16:22:19
    /// desc：遥控业务逻辑处理(service)类
    /// version: 1.0
    /// </summary>
    public class TelecontrolManageService
    {
        /// <summary>
        /// 根据点号类型编号获取使用的所有遥控点表
        /// </summary>
        /// <param name="pointTypeId">点号类型编号</param>
        /// <returns>使用的所有遥控点表</returns>
        public IList<Telecontrol> GetTelecontrolPoint(int pointTypeId)
        {
            //获取使用的所有遥控点表
            PointUsedDao pointUsedDao = new PointUsedDao();
            IList<PointUsed> pointUsedList = pointUsedDao.queryByPointTypeId(pointTypeId);
            if (pointUsedList == null || pointUsedList.Count == 0)
            {
                return null;
            }

            IList<Telecontrol> telecontrolList = new List<Telecontrol>();
            for (int i = 0; i < pointUsedList.Count; i++)
            {
                Telecontrol telecontrol = new Telecontrol();
                telecontrol.Number = Convert.ToInt32(pointUsedList[i].NO);
                //telecontrol.YKID = Convert.ToInt32(listID[i].InnerText, 16);
                telecontrol.YKID = i + 24577;
                telecontrol.YKName = pointUsedList[i].Name;
                //telecontrol.YKResoult = listResoult[i].InnerText;
                telecontrol.YKRemark = pointUsedList[i].Comment;
                telecontrol.Selected = false;

                telecontrolList.Add(telecontrol);
            }
            return telecontrolList;

        }
    }
}

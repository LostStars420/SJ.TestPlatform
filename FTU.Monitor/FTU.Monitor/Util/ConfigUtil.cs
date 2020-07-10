using FTU.Monitor.Dao;
using FTU.Monitor.Model;
using System.Collections.Generic;

namespace FTU.Monitor.Util
{
    /// <summary>
    /// ConfigUtil 的摘要说明
    /// author: songminghao
    /// date：2017/12/1 16:00:35
    /// desc：一些业务层的配置信息
    /// version: 1.0
    /// </summary>
    public class ConfigUtil
    {
        /// <summary>
        /// 静态构造方法，执行一次，获取点表类型列表和空点表（点号为0的点表信息）
        /// </summary>
        static ConfigUtil()
        {
            PointTypeDao pointTypeDao = new PointTypeDao();
            pointTypeConfigList = pointTypeDao.query();

            DevPointDao devPointDao = new DevPointDao();
            nullDevPointList = devPointDao.queryByPoint("0");

        }

        /// <summary>
        /// 点表类型列表
        /// </summary>
        public static IList<PointTypeInfo> pointTypeConfigList;

        /// <summary>
        /// 空点表列表（点号为0的点表信息）
        /// </summary>
        public static IList<DevPoint> nullDevPointList;

        /// <summary>
        /// 获取空点号集合
        /// </summary>
        public static void GetNullDevPointList()
        {
            DevPointDao devPointDao = new DevPointDao();
            nullDevPointList = devPointDao.queryByPoint("0");
        }

        /// <summary>
        /// 根据点号类型获取其相应的点号类型编号
        /// </summary>
        /// <param name="pointType">点号类型</param>
        /// <returns>点号类型编号，如果未找到，则返回-1</returns>
        public static int getPointTypeID(string pointType)
        {
            int pointTypeId = -1;

            if (pointTypeConfigList != null && pointTypeConfigList.Count > 0)
            {
                for (int i = 0; i < pointTypeConfigList.Count; i++)
                {
                    if (pointType.Equals(pointTypeConfigList[i].PointType)) 
                    {
                        pointTypeId = pointTypeConfigList[i].ID;
                        break;
                    }
                }
            }

            return pointTypeId;
            
        }

        /// <summary>
        /// 根据点号类型获取其相应的空点号所在的设备点表编号
        /// </summary>
        /// <param name="pointType">点号类型</param>
        /// <returns>设备点表编号，如果未找到，则返回-1</returns>
        public static int getDevPointID(string pointType)
        {
            int devpid = -1;

            if (nullDevPointList != null && nullDevPointList.Count > 0)
            {
                for (int i = 0; i < nullDevPointList.Count; i++)
                {
                    if (pointType.Equals(nullDevPointList[i].PointType))
                    {
                        devpid = nullDevPointList[i].Devpid;
                        break;
                    }
                }
            }
            return devpid;
        }

    }
}

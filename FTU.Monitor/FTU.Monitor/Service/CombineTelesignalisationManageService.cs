using FTU.Monitor.Dao;
using FTU.Monitor.Model;
using FTU.Monitor.Util;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FTU.Monitor.Service
{
    /// <summary>
    /// CombineTelesignalisationManageService 的摘要说明
    /// author: liyan
    /// date：2018/7/2 15:40:08
    /// desc：组合遥信管理业务逻辑处理(service)类
    /// version: 1.0
    /// </summary>
    public class CombineTelesignalisationManageService
    {
        /// <summary>
        /// 组合遥信内容字符串验证正则表达式
        /// </summary>
        private static string validCombine = "^((!)?([0]){2}([0-9a-fA-F]){2}([(]){0,1}([\u4e00-\u9fa5_a-zA-Z0-9]){0,}([)]){0,1}([|]|[&]))*((!)?([0]){2}([0-9a-fA-F]){2}([(]){0,1}([\u4e00-\u9fa5_a-zA-Z0-9]){0,}([)]){0,1})$";
        
        /// <summary>
        /// 判断组合遥信内容是否合法
        /// </summary>
        /// <param name="combineTelesignalisationContent">组合遥信内容</param>
        /// <returns>提示信息,返回null值表示合法</returns>
        public string CheckCombineTelesignalisationName(string combineTelesignalisationContent)
        {
            if (combineTelesignalisationContent == null || "".Equals(combineTelesignalisationContent.Trim()))
            {
                return "组合遥信内容不能为空";
            }

            //if (combineTelesignalisationContent.Trim().Length > 30)
            //{
            //    return "组合遥信内容长度不能超过30";
            //}

            // 去掉前后中间空白字符
            string combineContent = Regex.Replace(combineTelesignalisationContent.Trim(), @"\s", "");
            if (!ValidCombineContent(combineContent))
            {
                return "组合遥信内容不合法";
            }

            // 获取原始遥信点表的点号列表
            IList<int> telesignalisationSourcePointIDList = GetTelesignalisationPointIDByFlag(0);

            combineContent = combineContent.Replace("!", "");
            // 判断组合要信包含的遥信原始点表是否存在
            string[] idInfos = combineContent.Split(new char[2] { '|', '&'});
            string[] ids = new string[idInfos.Length];
            for (int i = 0; i < idInfos.Length; i++)
            {
                if (idInfos[i].IndexOf('(') != -1)
                {
                    ids[i] = idInfos[i].Substring(0, idInfos[i].IndexOf('('));
                }
                else
                {
                    ids[i] = idInfos[i];
                }
            }

            // 判断原始遥信点表的点号列表是否为空
            if (telesignalisationSourcePointIDList == null || telesignalisationSourcePointIDList.Count == 0)
            {
                return "原始遥信点表为空";
            }

            // 原始遥信点表的点号列表不为空
            // 错误信息提示
            string errorMsg = "";
            foreach (var id in ids)
            {
                if (telesignalisationSourcePointIDList.IndexOf(Convert.ToInt32(id, 16)) == -1)
                {
                    errorMsg += id + ",";
                }
            }
            if (errorMsg != null && !"".Equals(errorMsg.Trim()))
            {
                return "组合遥信包含的点号" + errorMsg.Substring(0, errorMsg.Length - 1) + "不存在";
            }

            return null;
        }

        /// <summary>
        /// 保存组合遥信,并获取此组合遥信点号
        /// </summary>
        /// <param name="combineTelesignalisationPoint">组合遥信点号</param>
        /// <returns>组合遥信点号,null代表未找到</returns>
        public Telesignalisation SaveCombineTelesignalisation(DevPoint combineTelesignalisationPoint)
        {
            DevPointDao devPointDao = new DevPointDao();
            int success = devPointDao.insert(combineTelesignalisationPoint);
            if (success > 0)
            {
                // 获取该组合遥信点号
                DevPoint devPoint = devPointDao.queryByPointTypeIdAndPoint(ConfigUtil.getPointTypeID("遥信"), combineTelesignalisationPoint.ID);

                Telesignalisation telesignalisationCombine = new Telesignalisation();
                telesignalisationCombine.Selected = false;
                telesignalisationCombine.Devpid = devPoint.Devpid;
                telesignalisationCombine.Name = devPoint.Name;
                telesignalisationCombine.IsChanged = true;
                telesignalisationCombine.IsSOE = true;
                telesignalisationCombine.ID = Convert.ToInt32(devPoint.ID, 16);
                telesignalisationCombine.Comment = devPoint.Comment;
                telesignalisationCombine.Flag = devPoint.Flag;
                return telesignalisationCombine;
            }

            return null;
        }

        /// <summary>
        /// 根据点号流水号删除点号
        /// </summary>
        /// <param name="devpid"></param>
        /// <returns></returns>
        public int DeleteByDevpid(int devpid)
        {
            DevPointDao devPointDao = new DevPointDao();
            return devPointDao.DeleteByDevpid(devpid);
        }

        /// <summary>
        /// 根据点号类型编号获取该点号类型编号下的点表个数
        /// </summary>
        /// <param name="pointTypeId"></param>
        /// <returns></returns>
        public int GetCountByPointTypeId(int pointTypeId)
        {
            DevPointDao devPointDao = new DevPointDao();
            return devPointDao.GetCountByPointTypeId(pointTypeId);
        }

        /// <summary>
        /// 根据点号类型编号和组合遥信标志位查询此类型下的点表集合
        /// </summary>
        /// <param name="flag">组合遥信标志位(1代表是组合遥信,0代表原始遥信点号)</param>
        /// <returns>遥信点表集合</returns>
        public IList<Telesignalisation> GetTelesignalisationPointByFlag(int flag)
        {
            // 定义点表DAO对象
            DevPointDao devPointDao = new DevPointDao();
            // 根据点号类型编号和组合遥信标志位查询此类型下的点号列表
            IList<DevPoint> telesignalisationPointList = devPointDao.queryByPointTypeIdAndFlag(ConfigUtil.getPointTypeID("遥信"), flag);

            if (telesignalisationPointList != null && telesignalisationPointList.Count > 0)
            {
                IList<Telesignalisation> telesignalisationList = new List<Telesignalisation>();

                for (int i = 0; i < telesignalisationPointList.Count; i++)
                {
                    Telesignalisation telesignalisation = new Telesignalisation();
                    telesignalisation.Selected = false;
                    telesignalisation.Number = i + 1;
                    telesignalisation.Devpid = telesignalisationPointList[i].Devpid;
                    telesignalisation.Name = telesignalisationPointList[i].Name;
                    telesignalisation.ID = Convert.ToInt32(telesignalisationPointList[i].ID, 16);
                    telesignalisation.Comment = telesignalisationPointList[i].Comment;
                    telesignalisation.Flag = telesignalisationPointList[i].Flag;

                    telesignalisationList.Add(telesignalisation);
                }

                return telesignalisationList;
            }

            return null;
        }

        /// <summary>
        /// 根据点号类型编号和组合遥信标志位查询此类型下的点表的点号列表
        /// </summary>
        /// <param name="flag">组合遥信标志位(1代表是组合遥信,0代表原始遥信点号)</param>
        /// <returns>遥信点表点号列表</returns>
        public IList<int> GetTelesignalisationPointIDByFlag(int flag)
        {
            // 定义点表DAO对象
            DevPointDao devPointDao = new DevPointDao();
            // 根据点号类型编号和组合遥信标志位查询此类型下的点号列表
            IList<DevPoint> telesignalisationPointList = devPointDao.queryByPointTypeIdAndFlag(ConfigUtil.getPointTypeID("遥信"), flag);

            if (telesignalisationPointList != null && telesignalisationPointList.Count > 0)
            {
                IList<int> telesignalisationPointIDList = new List<int>();

                for (int i = 0; i < telesignalisationPointList.Count; i++)
                {
                    telesignalisationPointIDList.Add(Convert.ToInt32(telesignalisationPointList[i].ID, 16));
                }

                return telesignalisationPointIDList;
            }

            return null;
        }

        /// <summary>
        /// 组合遥信内容字符串验证方法
        /// </summary>
        /// <param name="combineContent">组合遥信内容字符串</param>
        /// <returns></returns>
        public static bool ValidCombineContent(string combineContent)
        {
            if (UtilHelper.IsEmpty(combineContent))
            {
                return false;
            }
            return new Regex(validCombine).IsMatch(combineContent);
        }

        /// <summary>
        /// 获取遥信点表中点号的最大值
        /// </summary>
        public static int GetMaxTelesignalisationPointID()
        {
            DevPointDao devPointDao = new DevPointDao();
            IList<DevPoint> telesignalisationPointList = devPointDao.queryByPointTypeId(ConfigUtil.getPointTypeID("遥信"));

            int maxID = 0;
            if (telesignalisationPointList != null && telesignalisationPointList.Count > 0)
            {
                for (int i = 0; i < telesignalisationPointList.Count; i++)
                {
                    if (maxID < Convert.ToInt32(telesignalisationPointList[i].ID, 16))
                    {
                        maxID = Convert.ToInt32(telesignalisationPointList[i].ID, 16);
                    }
                }
            }
            return maxID + 1;
        }

    }
}

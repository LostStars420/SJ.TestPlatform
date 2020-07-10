using FTU.Monitor.Dao;
using FTU.Monitor.Model;
using System.Collections.Generic;

namespace FTU.Monitor.Service
{
    /// <summary>
    /// TelesignalisationManageService 的摘要说明
    /// author: liyan
    /// date：2018/6/26 16:05:57
    /// desc：遥信业务逻辑处理(service)类
    /// version: 1.0
    /// </summary>
    public class TelesignalisationManageService
    {
        /// <summary>
        /// 根据点号类型编号获取使用的所有遥信点表
        /// </summary>
        /// <param name="pointTypeId">点号类型编号</param>
        /// <returns>使用的所有遥信点表</returns>
        public IList<Telesignalisation> GetTelesignalisationPoint(int pointTypeId)
        {
            //获取使用的所有遥信点表
            PointUsedDao pointUsedDao = new PointUsedDao();
            IList<PointUsed> pointUsedList = pointUsedDao.queryByPointTypeId(pointTypeId);
            if (pointUsedList == null || pointUsedList.Count == 0)
            {
                return null;
            }

            IList<Telesignalisation> telesignalisationList = new List<Telesignalisation>();
            for (int i = 0; i < pointUsedList.Count; i++)
            {
                Telesignalisation telesignalisation = new Telesignalisation();
                telesignalisation.Number = i + 1;
                //telesignalisation.ID = Convert.ToInt32(listID[i].InnerText, 16);
                telesignalisation.ID = i + 1;
                telesignalisation.Name = pointUsedList[i].Name;
                string comment = pointUsedList[i].Comment;
                if(pointUsedList[i].DoublePoint == 1)
                {
                    if(comment != null)
                    {
                        //双点遥信
                        int firstDotIndex = comment.IndexOf('.');
                        if (firstDotIndex > -1)
                        {
                            int secondDotIndex = comment.IndexOf('.', firstDotIndex);
                            if (secondDotIndex > -1)
                            {
                                // 0->1 1->2
                                comment = comment.Replace("1.", "2.");
                                comment = comment.Replace("0.", "1.");
                            }
                        }
                    }                
                }
                if(pointUsedList[i].IsNegated == 1)
                {
                    //遥信取反
                    int firstDotIndex = comment.IndexOf('.');
                    if (firstDotIndex > 0)
                    {
                        char firstDot = comment[firstDotIndex - 1];
                        int secondDotIndex = comment.IndexOf('.', firstDotIndex + 1);
                        if (secondDotIndex > 0)
                        {
                            char secondDot = comment[secondDotIndex - 1];
                            //交换显示
                            string firstString = comment.Substring(0, 2);                              
                            string secondString = comment.Substring(firstDotIndex + 1,secondDotIndex - 1 - firstDotIndex -1);
                            string thirdString = comment.Substring(secondDotIndex - 1, 2); 
                            string fourthString = comment.Substring(secondDotIndex + 1);
                            comment = thirdString + secondString + firstString + fourthString;
                        }
                        else
                        {

                        }
                    }
                    else
                    {

                    }
                }
                telesignalisation.Comment = comment;
               
                telesignalisationList.Add(telesignalisation);
            }
            return telesignalisationList;
        }

    }
}

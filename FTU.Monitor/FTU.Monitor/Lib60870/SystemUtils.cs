

using System;

namespace lib60870
{
    /// <summary>
    /// SystemUtils 的摘要说明
    /// author: zhengshuiqing
    /// date：2017/12/15 08:54:32
    /// desc：系统工具类
    /// version: 1.0
    /// </summary>
    public static class SystemUtils
    {
        /// <summary>
        /// 1970年1月1日当天时间
        /// </summary>
        private static DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 从1970年1月1日到当前日期总毫秒数
        /// </summary>
        /// <returns></returns>
        public static long currentTimeMillis()
        {
            return (long)((DateTime.UtcNow - Jan1st1970).TotalMilliseconds);
        }
    }

}

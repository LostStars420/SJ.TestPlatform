

using System;

namespace lib60870
{
    /// <summary>
    /// SystemUtils ��ժҪ˵��
    /// author: zhengshuiqing
    /// date��2017/12/15 08:54:32
    /// desc��ϵͳ������
    /// version: 1.0
    /// </summary>
    public static class SystemUtils
    {
        /// <summary>
        /// 1970��1��1�յ���ʱ��
        /// </summary>
        private static DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// ��1970��1��1�յ���ǰ�����ܺ�����
        /// </summary>
        /// <returns></returns>
        public static long currentTimeMillis()
        {
            return (long)((DateTime.UtcNow - Jan1st1970).TotalMilliseconds);
        }
    }

}

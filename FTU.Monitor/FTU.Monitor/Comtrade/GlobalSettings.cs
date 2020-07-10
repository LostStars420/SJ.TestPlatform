/*
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace FTU.Monitor.Comtrade
{
    /// <summary>
    /// GlobalSettings 的摘要说明
    /// author: songminghao
    /// date：2017/12/08 10:39:09
    /// desc：录波文件处理全局配置类
    /// version: 1.0
    /// </summary>
    public static class GlobalSettings
	{
        /// <summary>
        /// 逗号
        /// </summary>
		internal const char commaDelimiter=',';

        /// <summary>
        /// 空白字符
        /// </summary>
		internal const char whiteSpace=' ';

        /// <summary>
        /// 换行
        /// </summary>
		internal const string newLine="\r\n";

		/// <summary>
        /// 录波文件后缀名
		/// </summary>
		public const string extentionsForFileDialogFilter="(*.cfg;*.dat;*.cff)|*.cfg;*.dat;*.cff";

        /// <summary>
        /// .cfg后缀名
        /// </summary>
		internal const string extentionCFG=".cfg";

        /// <summary>
        /// .dat后缀名
        /// </summary>
		internal const string extentionDAT=".dat";

        /// <summary>
        /// .cff后缀名
        /// </summary>
		internal const string extentionCFF=".cff";
	}
}

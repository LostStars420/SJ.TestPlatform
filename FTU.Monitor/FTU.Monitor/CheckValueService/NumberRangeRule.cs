using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FTU.Monitor.CheckValueService
{
    /// <summary>
    /// NumberRangeRule 的摘要说明
    /// author: liyan
    /// date：2018/7/12 10:31:15
    /// desc：校验用户输入的整型数据是否合法
    /// version: 1.0
    /// </summary>
    public class NumberRangeRule:ValidationRule
    {
        /// <summary>
        /// 最小值
        /// </summary>
        private int _min;

        /// <summary>
        /// 设置和获取最小值
        /// </summary>
        public int Min
        {
            get
            {
                return this._min;
            }
            set
            {
                this._min = value;
            }
        }

        /// <summary>
        /// 最大值
        /// </summary>
        private int _max;

        /// <summary>
        /// 设置和获取最大值
        /// </summary>
        public int Max
        {
            get
            {
               return this._max;
            }
            set
            {
                this._max = value;
            }
        }

        /// <summary>
        /// 重写Validate方法
        /// </summary>
        /// <param name="value">被校验的值</param>
        /// <param name="cultureInfo">特定区域性的信息</param>
        /// <returns>校验结果</returns>
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int number = 0;
            try
            {
                if(((string)value).Length > 0)
                {
                    number = Int32.Parse((string)value);
                }
            }
            catch (Exception ex)
            {
                //return new ValidationResult(false, "Illegal character or" + ex.Message);
                return new ValidationResult(false, "非法输入！");
            }
            if(number < Min || number > Max)
            {
                //return new ValidationResult(false, "Please enter an number in range:" + Min + "-" + Max);
                return new ValidationResult(false, "输入范围:" + Min + "-" + Max + "！");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }
}

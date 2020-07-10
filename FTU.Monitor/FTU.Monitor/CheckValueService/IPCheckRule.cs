using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FTU.Monitor.CheckValueService
{
    /// <summary>
    /// IPCheckRule 的摘要说明
    /// author: liyan
    /// date：2018/7/12 14:39:02
    /// desc：校验用户输入的IP地址的格式
    /// version: 1.0
    /// </summary>
    public class IPCheckRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string IP = value.ToString();
            string validIpAddressRegex = "^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5]).){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";
            bool result = Regex.IsMatch(IP,validIpAddressRegex);
            if(!result)
            {
                return new ValidationResult(false,"请输入正确的IP地址");
            }
            return new ValidationResult(true, null); 
        }
    }
}

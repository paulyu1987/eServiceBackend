using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Core.Helper
{
    /// <summary>
    /// Validation Helper
    /// </summary>
    public class ValidateHelper
    {
        //Email Regex 
        private static Regex _emailregex = new Regex(@"^([0-9a-zA-Z/_-]+[-/._+'&])*[0-9a-zA-Z/_-]+@([-0-9a-zA-Z_-]+[.])+[a-zA-Z_-]{2,6}$", RegexOptions.IgnoreCase);
        //Mobile number Regex
        private static Regex _mobileregex = new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");
        //IP Regex
        private static Regex _ipregex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
        //Date regex
        private static Regex _dateregex = new Regex(@"(\d{1,2})/(\d{1,2})/(\d{4})");
        //float regex
        private static Regex _numericregex = new Regex(@"^[-]?[0-9]+(\.[0-9]+)?$");
        //zip code regex
        private static Regex _zipcoderegex = new Regex(@"^\d{6}$");

        /// <summary>
        /// Check if it is an email address
        /// </summary>
        public static bool IsEmail(string s)
        {
            if (string.IsNullOrEmpty(s))
                return true;
            return _emailregex.IsMatch(s);
        }

        /// <summary>
        /// Check if it is a moblie number
        /// </summary>
        public static bool IsMobile(string s)
        {
            if (string.IsNullOrEmpty(s))
                return true;
            return _mobileregex.IsMatch(s);
        }
        
        /// <summary>
        /// Check if given string is an IP
        /// </summary>
        public static bool IsIP(string s)
        {
            return _ipregex.IsMatch(s);
        }
        
        /// <summary>
        /// Check if given string is a date
        /// </summary>
        public static bool IsDate(string s)
        {
            return _dateregex.IsMatch(s);
        }

        /// <summary>
        /// Check if given string is a float
        /// </summary>
        public static bool IsNumeric(string numericStr)
        {
            return _numericregex.IsMatch(numericStr);
        }

        /// <summary>
        /// Check if given string is a zipcode
        /// </summary>
        public static bool IsZipCode(string s)
        {
            if (string.IsNullOrEmpty(s))
                return true;
            return _zipcoderegex.IsMatch(s);
        }

        /// <summary>
        /// Check if given file name is a image file name
        /// </summary>
        /// <returns> </returns>
        public static bool IsImgFileName(string fileName)
        {
            if (fileName.IndexOf(".") == -1)
                return false;

            string tempFileName = fileName.Trim().ToLower();
            string extension = tempFileName.Substring(tempFileName.LastIndexOf("."));
            return extension == ".png" || extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".gif";
        }

        /// <summary>
        /// Check if given ip is in target IPs
        /// </summary>
        /// <param name="sourceIP">IP</param>
        /// <param name="targetIP">IP address, support *, such as 192.168.1.*</param>
        /// <returns></returns>
        public static bool InIP(string sourceIP, string targetIP)
        {
            if (string.IsNullOrEmpty(sourceIP) || string.IsNullOrEmpty(targetIP))
                return false;

            string[] sourceIPBlockList = StringHelper.SplitString(sourceIP, @".");
            string[] targetIPBlockList = StringHelper.SplitString(targetIP, @".");

            int sourceIPBlockListLength = sourceIPBlockList.Length;

            for (int i = 0; i < sourceIPBlockListLength; i++)
            {
                if (targetIPBlockList[i] == "*")
                    return true;

                if (sourceIPBlockList[i] != targetIPBlockList[i])
                {
                    return false;
                }
                else
                {
                    if (i == 3)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if given ip is in target IPs
        /// </summary>
        /// <param name="sourceIP">ip</param>
        /// <param name="targetIPList">ip list</param>
        /// <returns></returns>
        public static bool InIPList(string sourceIP, string[] targetIPList)
        {
            if (targetIPList != null && targetIPList.Length > 0)
            {
                foreach (string targetIP in targetIPList)
                {
                    if (InIP(sourceIP, targetIP))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if current datetime is in a preiod time list
        /// </summary>
        /// <param name="periodList">Preiod list</param>
        /// <param name="liePeriod">In time preiod</param>
        /// <returns></returns>
        public static bool BetweenPeriod(string[] periodList, out string liePeriod)
        {
            if (periodList != null && periodList.Length > 0)
            {
                DateTime startTime;
                DateTime endTime;
                DateTime nowTime = DateTime.Now;
                DateTime nowDate = nowTime.Date;

                foreach (string period in periodList)
                {
                    int index = period.IndexOf("-");
                    startTime = TypeHelper.StringToDateTime(period.Substring(0, index));
                    endTime = TypeHelper.StringToDateTime(period.Substring(index + 1));

                    if (startTime < endTime)
                    {
                        if (nowTime > startTime && nowTime < endTime)
                        {
                            liePeriod = period;
                            return true;
                        }
                    }
                    else
                    {
                        if ((nowTime > startTime && nowTime < nowDate.AddDays(1)) || (nowTime < endTime))
                        {
                            liePeriod = period;
                            return true;
                        }
                    }
                }
            }
            liePeriod = string.Empty;
            return false;
        }

        /// <summary>
        /// Check if current datetime is in a preiod time list
        /// </summary>
        /// <param name="periodStr">datetime preiod list</param>
        /// <param name="liePeriod">In time preiod</param>
        /// <returns></returns>
        public static bool BetweenPeriod(string periodStr, out string liePeriod)
        {
            string[] periodList = StringHelper.SplitString(periodStr, "\n");
            return BetweenPeriod(periodList, out liePeriod);
        }

        /// <summary>
        /// Check if current datetime is in a preiod time list
        /// </summary>
        /// <param name="periodList">datetime preiod</param>
        /// <returns></returns>
        public static bool BetweenPeriod(string periodList)
        {
            string liePeriod = string.Empty;
            return BetweenPeriod(periodList, out liePeriod);
        }

        /// <summary>
        /// Check if given stirng list are all float
        /// </summary>
        public static bool IsNumericArray(string[] numericStrList)
        {
            if (numericStrList != null && numericStrList.Length > 0)
            {
                foreach (string numberStr in numericStrList)
                {
                    if (!IsNumeric(numberStr))
                        return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if give string is a float
        /// </summary>
        public static bool IsNumericRule(string numericRuleStr, string splitChar)
        {
            return IsNumericArray(StringHelper.SplitString(numericRuleStr, splitChar));
        }

        /// <summary>
        /// Check if give string is a float
        /// </summary>
        public static bool IsNumericRule(string numericRuleStr)
        {
            return IsNumericRule(numericRuleStr, ",");
        }
    }
}

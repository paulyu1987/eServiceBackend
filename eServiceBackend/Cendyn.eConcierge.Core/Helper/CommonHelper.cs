using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Cendyn.eConcierge.Core.Helper
{
    public class CommonHelper
    {
        //weekday
        private static string[] _weekdays = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
        //Space, Enter, NewLine, Tab regex
        private static Regex _tbbrRegex = new Regex(@"\s*|\t|\r|\n", RegexOptions.IgnoreCase);

        #region Datetime Functions

        /// <summary>
        /// Get current datetime in format ""yyyy-MM-dd HH:mm:ss:fffffff""
        /// </summary>
        public static string GetDateTimeMS()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffffff");
        }

        /// <summary>
        /// Get current datetime in format ""Monday, May 2, 2016 12:43:43 AM""
        /// </summary>
        public static string GetDateTimeU()
        {
            return string.Format("{0:U}", DateTime.Now);
        }

        /// <summary>
        /// Get current datetime in format ""yyyy-MM-dd HH:mm:ss""
        /// </summary>
        public static string GetDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// Get current datetime in format ""yyyy-MM-dd HH:mm:ss""
        /// </summary>
        public static string GetDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }
        
        /// <summary>
        /// Get current time(not include date)
        /// </summary>
        public static string GetTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

        /// <summary>
        /// Get current hour
        /// </summary>
        public static string GetHour()
        {
            return DateTime.Now.Hour.ToString("00");
        }

        /// <summary>
        /// Get current day
        /// </summary>
        public static string GetDay()
        {
            return DateTime.Now.Day.ToString("00");
        }

        /// <summary>
        /// Get current month
        /// </summary>
        public static string GetMonth()
        {
            return DateTime.Now.Month.ToString("00");
        }

        /// <summary>
        /// Get current year
        /// </summary>
        public static string GetYear()
        {
            return DateTime.Now.Year.ToString();
        }

        /// <summary>
        /// Get current weekday in number
        /// </summary>
        public static string GetDayOfWeek()
        {
            return ((int)DateTime.Now.DayOfWeek).ToString();
        }

        #endregion

        #region Array Functions

        /// <summary>
        /// Get substring position from orignal string 
        /// </summary>
        public static int GetIndexInArray(string s, string[] array, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(s) || array == null || array.Length == 0)
                return -1;

            int index = 0;
            string temp = null;

            if (ignoreCase)
                s = s.ToLower();

            foreach (string item in array)
            {
                if (ignoreCase)
                    temp = item.ToLower();
                else
                    temp = item;

                if (s == temp)
                    return index;
                else
                    index++;
            }

            return -1;
        }

        /// <summary>
        /// Get substring position from orignal string array
        /// </summary>
        public static int GetIndexInArray(string s, string[] array)
        {
            return GetIndexInArray(s, array, false);
        }

        /// <summary>
        /// Check if a substring in a given string array
        /// </summary>
        public static bool IsInArray(string s, string[] array, bool ignoreCase)
        {
            return GetIndexInArray(s, array, ignoreCase) > -1;
        }

        /// <summary>
        /// Check if a substring in a given string array
        /// </summary>
        public static bool IsInArray(string s, string[] array)
        {
            return IsInArray(s, array, false);
        }

        /// <summary>
        /// Check if a substring in a given string
        /// </summary>
        public static bool IsInArray(string s, string array, string splitStr, bool ignoreCase)
        {
            return IsInArray(s, StringHelper.SplitString(array, splitStr), ignoreCase);
        }

        /// <summary>
        /// Check if a substring in a given string
        /// </summary>
        public static bool IsInArray(string s, string array, string splitStr)
        {
            return IsInArray(s, StringHelper.SplitString(array, splitStr), false);
        }

        /// <summary>
        /// Check if a substring in a given string, , use "," as split string
        /// </summary>
        public static bool IsInArray(string s, string array)
        {
            return IsInArray(s, StringHelper.SplitString(array, ","), false);
        }



        /// <summary>
        /// Combine given array as a string
        /// </summary>
        public static string ObjectArrayToString(object[] array, string splitStr)
        {
            if (array == null || array.Length == 0)
                return "";

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
                result.AppendFormat("{0}{1}", array[i], splitStr);

            return result.Remove(result.Length - splitStr.Length, splitStr.Length).ToString();
        }

        /// <summary>
        /// Combine given array as a string, , use "," as split string
        /// </summary>
        public static string ObjectArrayToString(object[] array)
        {
            return ObjectArrayToString(array, ",");
        }

        /// <summary>
        /// Combine given string array as a string
        /// </summary>
        public static string StringArrayToString(string[] array, string splitStr)
        {
            return ObjectArrayToString(array, splitStr);
        }

        /// <summary>
        /// Combine given string array as a string, use "," as split string
        /// </summary>
        public static string StringArrayToString(string[] array)
        {
            return StringArrayToString(array, ",");
        }

        /// <summary>
        /// Combine given int array as a string
        /// </summary>
        public static string IntArrayToString(int[] array, string splitStr)
        {
            if (array == null || array.Length == 0)
                return "";

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
                result.AppendFormat("{0}{1}", array[i], splitStr);

            return result.Remove(result.Length - splitStr.Length, splitStr.Length).ToString();
        }

        /// <summary>
        /// Combine given int array as a string, use "," as split string
        /// </summary>
        public static string IntArrayToString(int[] array)
        {
            return IntArrayToString(array, ",");
        }



        /// <summary>
        /// Remove given item from array list
        /// </summary>
        /// <param name="array">Original List</param>
        /// <param name="removeItem">Item need to be removed</param>
        /// <param name="removeBackspace">If need remove backspace</param>
        /// <param name="ignoreCase">If ignore case</param>
        /// <returns></returns>
        public static string[] RemoveArrayItem(string[] array, string removeItem, bool removeBackspace, bool ignoreCase)
        {
            if (array != null && array.Length > 0)
            {
                StringBuilder arrayStr = new StringBuilder();
                if (ignoreCase)
                    removeItem = removeItem.ToLower();
                string temp = "";
                foreach (string item in array)
                {
                    if (ignoreCase)
                        temp = item.ToLower();
                    else
                        temp = item;

                    if (temp != removeItem)
                        arrayStr.AppendFormat("{0}_", removeBackspace ? item.Trim() : item);
                }

                return StringHelper.SplitString(arrayStr.Remove(arrayStr.Length - 1, 1).ToString(), "_");
            }

            return array;
        }

        /// <summary>
        /// Remove empty string from string array list
        /// </summary>
        /// <param name="array">String List</param>
        /// <returns></returns>
        public static string[] RemoveArrayItem(string[] array)
        {
            return RemoveArrayItem(array, "", true, false);
        }

        /// <summary>
        /// Remove substring from given string
        /// </summary>
        /// <param name="s">String need to be remove</param>
        /// <param name="splitStr">Split String</param>
        /// <returns></returns>
        public static string[] RemoveStringItem(string s, string splitStr)
        {
            return RemoveArrayItem(StringHelper.SplitString(s, splitStr), "", true, false);
        }

        /// <summary>
        /// Remove substring from given string
        /// </summary>
        /// <param name="s">String need to be remove</param>
        /// <returns></returns>
        public static string[] RemoveStringItem(string s)
        {
            return RemoveArrayItem(StringHelper.SplitString(s), "", true, false);
        }



        /// <summary>
        /// Remove duplicate item from array
        /// </summary>
        /// <returns></returns>
        public static int[] RemoveRepeaterItem(int[] array)
        {
            if (array == null || array.Length < 2)
                return array;

            Array.Sort(array);

            int length = 1;
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] != array[i - 1])
                    length++;
            }

            int[] uniqueArray = new int[length];
            uniqueArray[0] = array[0];
            int j = 1;
            for (int i = 1; i < array.Length; i++)
                if (array[i] != array[i - 1])
                    uniqueArray[j++] = array[i];

            return uniqueArray;
        }

        /// <summary>
        /// Remove duplicate item from array
        /// </summary>
        /// <returns></returns>
        public static string[] RemoveRepeaterItem(string[] array)
        {
            if (array == null || array.Length < 2)
                return array;

            Array.Sort(array);

            int length = 1;
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] != array[i - 1])
                    length++;
            }

            string[] uniqueArray = new string[length];
            uniqueArray[0] = array[0];
            int j = 1;
            for (int i = 1; i < array.Length; i++)
                if (array[i] != array[i - 1])
                    uniqueArray[j++] = array[i];

            return uniqueArray;
        }

        /// <summary>
        /// Remove duplicate char from a string
        /// </summary>
        /// <returns></returns>
        public static string GetUniqueString(string s)
        {
            return GetUniqueString(s, ",");
        }

        /// <summary>
        /// Remove duplicate char from a string
        /// </summary>
        /// <returns></returns>
        public static string GetUniqueString(string s, string splitStr)
        {
            return ObjectArrayToString(RemoveRepeaterItem(StringHelper.SplitString(s, splitStr)), splitStr);
        }

        #endregion

        /// <summary>
        /// Trim backspace,enter,newline,tab from a string 
        /// </summary>
        public static string TBBRTrim(string str)
        {
            if (!string.IsNullOrEmpty(str))
                return str.Trim().Trim('\r').Trim('\n').Trim('\t');
            return string.Empty;
        }

        /// <summary>
        /// Remove backspace,enter,newline,tab from a string 
        /// </summary>
        public static string ClearTBBR(string str)
        {
            if (!string.IsNullOrEmpty(str))
                return _tbbrRegex.Replace(str, "");

            return string.Empty;
        }

        /// <summary>
        /// Remove null or space row from a string
        /// </summary>
        /// <returns></returns>
        public static string DeleteNullOrSpaceRow(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "";

            string[] tempArray = StringHelper.SplitString(s, "\r\n");
            StringBuilder result = new StringBuilder();
            foreach (string item in tempArray)
            {
                if (!string.IsNullOrWhiteSpace(item))
                    result.AppendFormat("{0}\r\n", item);
            }
            if (result.Length > 0)
                result.Remove(result.Length - 2, 2);
            return result.ToString();
        }

        /// <summary>
        /// Get Html Space
        /// </summary>
        /// <returns></returns>
        public static string GetHtmlBS(int count)
        {
            if (count == 1)
                return "&nbsp;";
            else if (count == 2)
                return "&nbsp;&nbsp;";
            else if (count == 3)
                return "&nbsp;&nbsp;&nbsp;";
            else
            {
                StringBuilder result = new StringBuilder();

                for (int i = 0; i < count; i++)
                    result.Append("&nbsp;");

                return result.ToString();
            }
        }

        /// <summary>
        /// Get domain from an email address
        /// </summary>
        /// <param name="email">Emaill address</param>
        /// <returns></returns>
        public static string GetEmailProvider(string email)
        {
            int index = email.LastIndexOf('@');
            if (index > 0)
                return email.Substring(index + 1);
            return string.Empty;
        }

        /// <summary>
        /// Escape Regex Expression
        /// </summary>
        public static string EscapeRegex(string s)
        {
            string[] oList = { "\\", ".", "+", "*", "?", "{", "}", "[", "^", "]", "$", "(", ")", "=", "!", "<", ">", "|", ":" };
            string[] eList = { "\\\\", "\\.", "\\+", "\\*", "\\?", "\\{", "\\}", "\\[", "\\^", "\\]", "\\$", "\\(", "\\)", "\\=", "\\!", "\\<", "\\>", "\\|", "\\:" };
            for (int i = 0; i < oList.Length; i++)
                s = s.Replace(oList[i], eList[i]);
            return s;
        }

        /// <summary>
        /// Convert a IP address to a long varaible 
        /// </summary>
        /// <param name="ip">ip</param>
        /// <returns></returns>
        public static long ConvertIPToLong(string ip)
        {
            string[] ips = ip.Split('.');
            long number = 16777216L * long.Parse(ips[0]) + 65536L * long.Parse(ips[1]) + 256 * long.Parse(ips[2]) + long.Parse(ips[3]);
            return number;
        }

        /// <summary>
        /// replace email address with "*"
        /// </summary>
        public static string HideEmail(string email)
        {
            int index = email.LastIndexOf('@');

            if (index == 1)
                return "*" + email.Substring(index);
            if (index == 2)
                return email[0] + "*" + email.Substring(index);

            StringBuilder sb = new StringBuilder();
            sb.Append(email.Substring(0, 2));
            int count = index - 2;
            while (count > 0)
            {
                sb.Append("*");
                count--;
            }
            sb.Append(email.Substring(index));
            return sb.ToString();
        }

        /// <summary>
        /// Hide part of the phone number
        /// </summary>
        public static string HideMobile(string mobile)
        {
            if (mobile != null && mobile.Length > 10)
                return mobile.Substring(0, 3) + "*****" + mobile.Substring(8);
            return string.Empty;
        }

        /// <summary>
        /// Convert a array to a list
        /// </summary>
        /// <param name="array">array</param>
        /// <returns></returns>
        public static List<T> ArrayToList<T>(T[] array)
        {
            List<T> list = new List<T>(array.Length);
            foreach (T item in array)
                list.Add(item);
            return list;
        }

        /// <summary> 
        /// Convert datatable to a list
        /// </summary> 
        /// <param name="dt">DataTable</param> 
        /// <returns></returns> 
        public static List<Dictionary<string, object>> DataTableToList(DataTable dt)
        {
            int columnCount = dt.Columns.Count;
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>(dt.Rows.Count);
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> item = new Dictionary<string, object>(columnCount);
                for (int i = 0; i < columnCount; i++)
                {
                    item.Add(dt.Columns[i].ColumnName, dr[i]);
                }
                list.Add(item);
            }
            return list;
        }

        public static DateTime? ConvertToDateTime(string date, string format)
        {
            DateTime retDateTime;

            if (DateTime.TryParseExact(date, format,
                    new System.Globalization.CultureInfo("en-US", false),
                    System.Globalization.DateTimeStyles.AllowWhiteSpaces, out retDateTime))
            {
                return retDateTime;
            }
            else
            {
                return null;
            }
        }

    }
}

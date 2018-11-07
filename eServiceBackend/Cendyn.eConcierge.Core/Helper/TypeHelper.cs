using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Core.Helper
{
    /// <summary>
    /// Type Convert Helper
    /// </summary>
    public class TypeHelper
    {
        #region Convert Int

        /// <summary>
        /// Convert string to int
        /// </summary>
        /// <param name="s">String</param>
        /// <param name="defaultValue">default value if cannot convert</param>
        /// <returns></returns>
        public static int StringToInt(string s, int defaultValue)
        {
            if (!string.IsNullOrWhiteSpace(s))
            {
                int result;
                if (int.TryParse(s, out result))
                    return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// Convert string to int
        /// </summary>
        /// <param name="s">string</param>
        /// <returns>If cannot convert, then return 0</returns>
        public static int StringToInt(string s)
        {
            return StringToInt(s, 0);
        }

        /// <summary>
        /// Convert object to int
        /// </summary>
        /// <param name="s">Object</param>
        /// <param name="defaultValue">Defult value if cannot convert</param>
        /// <returns></returns>
        public static int ObjectToInt(object o, int defaultValue)
        {
            if (o != null)
                return StringToInt(o.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// Convert object to int
        /// </summary>
        /// <param name="s">Object</param>
        /// <returns>If cannot convert, then return 0</returns>
        public static int ObjectToInt(object o)
        {
            return ObjectToInt(o, 0);
        }

        #endregion

        #region Convert Bool

        /// <summary>
        /// Convert string to bool
        /// </summary>
        /// <param name="s">String</param>
        /// <param name="defaultValue">Default Value if cannot convert</param>
        /// <returns></returns>
        public static bool StringToBool(string s, bool defaultValue)
        {
            if (s == "false")
                return false;
            else if (s == "true")
                return true;

            return defaultValue;
        }

        /// <summary>
        /// Convert string to bool
        /// </summary>
        /// <param name="s">string</param>
        /// <returns>If cannot convert, return false</returns>
        public static bool ToBool(string s)
        {
            return StringToBool(s, false);
        }

        /// <summary>
        /// Convert object to bool
        /// </summary>
        /// <param name="s">Object</param>
        /// <param name="defaultValue">default value</param>
        /// <returns></returns>
        public static bool ObjectToBool(object o, bool defaultValue)
        {
            if (o != null)
                return StringToBool(o.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// Convert object to bool
        /// </summary>
        /// <param name="s">Object</param>
        /// <returns>If cannot convert, return false</returns>
        public static bool ObjectToBool(object o)
        {
            return ObjectToBool(o, false);
        }

        #endregion

        #region Convert Datetime

        /// <summary>
        /// Convert string to datetime
        /// </summary>
        /// <param name="s">string</param>
        /// <param name="defaultValue">default value</param>
        /// <returns></returns>
        public static DateTime StringToDateTime(string s, DateTime defaultValue)
        {
            if (!string.IsNullOrWhiteSpace(s))
            {
                DateTime result;
                if (DateTime.TryParse(s, out result))
                    return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// Convert string to datetime
        /// </summary>
        /// <param name="s">string</param>
        /// <returns>If cannot convert, return Datetime.Now</returns>
        public static DateTime StringToDateTime(string s)
        {
            return StringToDateTime(s, DateTime.Now);
        }

        /// <summary>
        /// Convert object to datetime
        /// </summary>
        /// <param name="s">Object</param>
        /// <param name="defaultValue">default value</param>
        /// <returns></returns>
        public static DateTime ObjectToDateTime(object o, DateTime defaultValue)
        {
            if (o != null)
                return StringToDateTime(o.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// Convert object to datetime
        /// </summary>
        /// <param name="s">Object</param>
        /// <returns>If cannot convert, return Datetime.Now</returns>
        public static DateTime ObjectToDateTime(object o)
        {
            return ObjectToDateTime(o, DateTime.Now);
        }

        #endregion

        #region Convert Decimal

        /// <summary>
        /// Convert string to decimal
        /// </summary>
        /// <param name="s">string</param>
        /// <param name="defaultValue">default value</param>
        /// <returns></returns>
        public static decimal StringToDecimal(string s, decimal defaultValue)
        {
            if (!string.IsNullOrWhiteSpace(s))
            {
                decimal result;
                if (decimal.TryParse(s, out result))
                    return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// Convert string to decimal
        /// </summary>
        /// <param name="s">string</param>
        /// <returns>If cannot convert, return 0m</returns>
        public static decimal StringToDecimal(string s)
        {
            return StringToDecimal(s, 0m);
        }

        /// <summary>
        /// Convert object to decimal
        /// </summary>
        /// <param name="s">Object</param>
        /// <param name="defaultValue">default value</param>
        /// <returns></returns>
        public static decimal ObjectToDecimal(object o, decimal defaultValue)
        {
            if (o != null)
                return StringToDecimal(o.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// Convert object to decimal
        /// </summary>
        /// <param name="s">Object</param>
        /// <returns>If cannot convert, return 0m</returns>
        public static decimal ObjectToDecimal(object o)
        {
            return ObjectToDecimal(o, 0m);
        }

        #endregion
    }
}

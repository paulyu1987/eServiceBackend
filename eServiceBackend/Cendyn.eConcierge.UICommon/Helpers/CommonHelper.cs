using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.UICommon.Helpers
{
    public class CommonHelper
    {
        public static string CreateValidKey(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                string result = key.Replace(" ", "_");

                // Remove invalid characters
                Regex regex = new Regex(@"[^\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Nd}\p{Nl}\p{Mn}\p{Mc}\p{Cf}\p{Pc}\p{Lm}]");
                result = regex.Replace(result, "");

                // Class name doesn't begin with a letter, insert an underscore
                if (!char.IsLetter(result, 0))
                {
                    result = result.Insert(0, "_");
                }

                return result;
            }
            return String.Empty;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Core.Helper
{
    public class SimpleTextEncodeHelper
    {
        public static string EncodeText(string mValue)
        {
            int i = 0;
            string sBuild = "";
            try
            {
                var arrayChars = mValue.ToCharArray();

                for (i = 0; i < mValue.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        sBuild = sBuild + ((int)(arrayChars[i]) - 1).ToString("000");
                    }
                    else
                    {
                        sBuild = sBuild + ((int)(arrayChars[i]) + 1).ToString("000");

                    }
                }
            }
            catch
            {
                sBuild = "";
            }
            return sBuild;
        }

        public static string DecodeText(string mValue)
        {
            int i = 0;
            string sBuild = "";
            try
            {
                for (i = 0; i < mValue.Length; i += 3)
                {
                    if (i % 2 == 0)
                    {
                        sBuild = sBuild + Convert.ToChar(Convert.ToInt32(mValue.Substring(i, 3)) + 1);
                    }
                    else
                    {
                        sBuild = sBuild + Convert.ToChar(Convert.ToInt32(mValue.Substring(i, 3)) -1);
                    }
                }
            }
            catch
            {
                sBuild = "";
            }
            return sBuild;
        }

    }
}

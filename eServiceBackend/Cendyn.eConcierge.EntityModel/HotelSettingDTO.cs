using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public class HotelSettingDTO
    {
        public int LanguageID { get; set; }
        public string CultureID { get; set; }
        public string SettingKey { get; set; }
        public string SettingValue { get; set; }
    }
}

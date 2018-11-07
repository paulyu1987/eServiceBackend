﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public class RateTypeSearchResultDTO
    {
        public int id { get; set; }
        public string HotelCode { get; set; }
        public string RateTypeCode { get; set; }
        public string RateTypeCodeDescription { get; set; }
        public DateTime? InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsActive { get; set; }
        public string DateFormat { get; set; }
    }
}

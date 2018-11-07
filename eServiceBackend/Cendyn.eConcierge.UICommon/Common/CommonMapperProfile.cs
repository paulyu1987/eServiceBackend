using System;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cendyn.eConcierge.EntityModel;
using Cendyn.eConcierge.Service.ExcelModel;

namespace Cendyn.eConcierge.UICommon.Common
{
    public class CommonMapperProfile : Profile
    {
        public override string ProfileName
        {
            get { return "CommonMapping"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<Hotel, HotelDTO>();
        }
    }
}

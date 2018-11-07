using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using Cendyn.eConcierge.EntityModel;
using Cendyn.eConcierge.Service.Interface;

namespace Cendyn.eConcierge.Service.Implement
{
    public class HotelSocialMediaService : ServiceBase, IHotelSocialMediaService
    {
        public IHotelSocialMediaRepository hotelSocialMediaRepo { get; set; }

        public IList<HotelSocialMediaDTO> GetHotelSocialMedia(string hotelCode)
        {
            var hotelSocialMediaQueryable = hotelSocialMediaRepo.GetAll();
            var query = (from hsm in hotelSocialMediaQueryable
                         select new
                         {
                             hsm
                         }).Where(p => p.hsm.Hotel_Code == hotelCode && p.hsm.Active == true);
            var list = query.Select(x => new HotelSocialMediaDTO()
            {
                SocialMediaUrl = x.hsm.SocialMediaUrl,
                IconUrl = x.hsm.IconUrl,
                DisplayOrder = x.hsm.DisplayOrder
            }).OrderBy(x => x.DisplayOrder).ToList();

            return list;

        }
    }
}

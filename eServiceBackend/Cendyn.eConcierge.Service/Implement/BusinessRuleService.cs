using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using Cendyn.eConcierge.Core.Helper;
using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using Cendyn.eConcierge.EntityModel;
using Cendyn.eConcierge.Service.Interface;

namespace Cendyn.eConcierge.Service.Implement
{
    class BusinessRuleService : ServiceBase, IBusinessRuleService
    {
        //TODO: create a planner event repo
        public IConciergeHotelAccessMappingRepository conHotelAccessMappingRepo { get; set; }
        public IPlannerEventRepository plannerEventRepo { get; set; }
        public IHotelRepository hotelRepo { get; set; }
        PlannerEvent plannerEvent = null;

        //public IList<ListItemDTO> GetRoomTypeBooked(string HotelCode)
        //{
        //    string eventCategory = "RoomUpgrade";
        //    plannerEvent.Hotel_Code = HotelCode;
        //    plannerEvent.EventCategory = eventCategory;
        //    // get planner event IDs from hotel code
        //    List<string> pid = new List<string>();
        //var list = (from planner_event in plannerEventRepo.GetRequestDesc_New(HotelCode, eventCategory, pid)
        //            join hotel in hotelRepo.GetAll()
        //            on planner_event.Hotel_Code equals hotel.Hotel_Code
        //            where hotel.Hotel_Code == HotelCode
        //            select new ListItemDTO()
        //            {
        //                DisplayName = planner_event.RoomTypeCodeBooked,
        //                Value = planner_event.RoomTypeCodeUpgrade
        //            }).ToList();

        //    return list;
        //}

        public bool AddRoomUpgrade(string roomTypeBooked, string upgradePricesBy, DateTime startDate, DateTime endDate, string weekpart, string roomTypeUpgrade, decimal upgradeCost)
        {
            //TODO: add the new room upgrade defined by admin
            plannerEvent.RoomTypeCodeBooked = roomTypeBooked;
            plannerEvent.UpgradePricedBy = upgradePricesBy;
            plannerEvent.DateEnd = endDate;
            plannerEvent.DateStart = startDate;
            plannerEvent.UpgradeWeekDayWeekEnd = weekpart;
            plannerEvent.RoomTypeCodeUpgrade = roomTypeUpgrade;
            plannerEvent.USDPrice = upgradeCost;
            //plannerEventRepo.Add(plannerEvent);
            //unitOfWork.Commit();
            return true;
        }

    }
}

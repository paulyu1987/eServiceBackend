using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cendyn.eConcierge.Service.Interface;
using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using Cendyn.eConcierge.EntityModel;
using Cendyn.eConcierge.Core;
using Castle.Core;

namespace Cendyn.eConcierge.Service.Implement
{
    public class RequestAvailableRoomService : IRequestAvailableRoomService
    {
        public IFGuestRepository FGuestRepo { get; set; }
        public IGuestPlannerRepository GuestPlannerRepo { get; set; }
        public IPlannerEventRepository PlannerEventRepo { get; set; }
        public IRoomType_CodeRepository RoomType_CodeRepo { get; set; }

        public int requestID { get; set; }

        string subject = string.Empty;
        GuestPlanner gp = null;

        public IList<AvailableRoomDTO> GetGuestInfo(int requestID)
        {
            var list = (from r in FGuestRepo.GetAll()
                        join gpr in GuestPlannerRepo.GetAll() on r.LoginConfirmationNum equals gpr.LoginConfirmationNum
                        join pe in PlannerEventRepo.GetAll() on  gpr.DescID equals pe.ID.ToString()
                        join rc in RoomType_CodeRepo.GetAll() on new {A=pe.Hotel_Code, B=pe.RoomTypeCodeUpgrade} equals new {A=rc.Hotel_Code,B=rc.RoomCode}
                         where gpr.Event.Contains("RoomUpgrade") && gpr.ID == requestID 
                        select new AvailableRoomDTO()
                            {
                                ID= gpr.ID,
                                LName=r.LName,
                                FName=r.FName,
                                RoomType=r.RoomType,
                                RateType=r.RateType,
                                ArrivalDate=r.ArrivalDate.ToString(),
                                DepartureDate=r.DepartureDate.ToString(),                              
                                RoomTypeCodeUpgrade=pe.RoomTypeCodeUpgrade,
                                NightsOfStay = r.NightsOfStay.ToString(), 
                                USDPrice=gpr.USDPrice,
                                HotelCode=pe.Hotel_Code,
                                LoginConfirmationNum = gpr.LoginConfirmationNum,
                                TotalAmount = (rc.PerNightCharge == "Y" ? gpr.USDPrice * (r.NightsOfStay == null ? 0 : r.NightsOfStay) : gpr.USDPrice)
                               
                            }).ToList();
            return list;
        }              
    }
}

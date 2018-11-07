using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cendyn.eConcierge.EntityModel;

namespace Cendyn.eConcierge.Service.Interface
{
    public interface IBusinessRuleService
    {
        //bool CheckUserHasPermissionByHotelCode(string userName, string HotelCode);

        // get the available room upgrade options of the selected hotel
        //IList<ListItemDTO> GetRoomTypeBooked(string HotelCode);
        //SELECT pe.Hotel_Code, pe.DateStart, pe.DateEnd, pe.UpgradePricedBy, pe.RoomTypeCodeBooked, pe.RoomTypeCodeUpgrade, pe.RoomTypeCodeUpgradeDescription, pe.USDPrice
        //FROM PlannerEvent pe, Hotels h
        //WHERE pe.Hotel_Code=h.Hotel_Code and h.Hotel_Name='Loews Chicago Hotel'

        // add the new room upgrade into the PlannerEvent table
        bool AddRoomUpgrade(string roomTypeBooked, string upgradePricesBy, DateTime startDate, DateTime endDate, string weekpart, string roomTypeUpgrade, decimal upgradeCost);
        //INSERT INTO PlannerEvent
        //(Hotel_Code, DateStart, DateEnd, UpgradePricedBy, RoomTypeCodeBooked, RoomTypeCodeUpgrade, RoomTypeCodeUpgradeDescription, USDPrice)
        //VALUES
        //('LOCHI', '2016-01-01 00:00:00', '2017-01-01 00:00:00', 'Arrival', 'SUPK1', 'PREK1', 'Premium King', 10.00)
    }
}

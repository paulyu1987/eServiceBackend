using Cendyn.eConcierge.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cendyn.eConcierge.Service.CenResModel;

namespace Cendyn.eConcierge.Service.Interface
{
    public interface IPlannerEventService
    {
        RoomType_Code GetRoomTypeCode(string hotelCode, string roomCode);
        RoomType_Code GetRoomTypeCode(string hotelCode, string roomCode, int languageID);
        List<eUpgradeOptionDTO> GetEventDetail(string hotelCode, string loginComfirmationNum, string roomtype, string ratetype, DateTime? guestArrivaldate, DateTime? guestDeparturedate, int languageId, bool multiLanguageEnabled);
        List<RoomInfo> GetRoomEventDetail(string hotelCode, string loginComfirmationNum, string roomtype, string ratetype, DateTime? guestArrivaldate, DateTime? guestDeparturedate);
        bool SaveRoomOptions(string loginConfirmationNum, List<string> requestIDs, string hotelCode, int languageID, bool multiLanguageEnabled,  ref bool roomUnavailable, string langForSave, string currencyForSave);
        int GetBussinessRuleGroupid(NewBusinessRuleSaveDTO data,int type);
        bool UpdateByBusinessRule(NewBusinessRuleSaveDTO data);
        bool UpdateUpgradeRuleByGroupID(int? groupidbefore, int newgroupid, string ratetypebooked, int type);
        bool IsExistingBussinessRule(NewBusinessRuleSaveDTO data);
        bool IsBussinessRuleValid(NewBusinessRuleSaveDTO data);
        bool IsBussinessRuleValid(PlannerEvent data);
        IList<PriceItem> GetOptionPrices(int parentID);
    }
}

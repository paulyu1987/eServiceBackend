using Cendyn.eConcierge.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Service.Interface
{
    public interface IGuestPlannerService
    {
        int AddNewEUpgradeRequest(eUpgradeRequest eUpgradeRequest);
        string GetPackageCode(string RoomTypeCodeUpgrade,string hotelCode);


        int AddNewGuestPlanner(GuestPlanner guestPlanner);
        GuestPlanner GetGuestPlannerByID(int ID);
        bool UpdatRequestIDs(List<int> guestPlannerIDList);
        GuestPlanner GetGuestPlannerByLookupID(Guid lookUpID);

        int AddGuestPlannerLog(GuestPlanner_Log log);
        GuestPlanner_Log GetGuestPlannerLogByID(int ID);

        IList<RequestSearchResultDTO> GetRequestsBySearchCriteria(RequestSearchCriteriaDTO searchCriteria, PagingInformation pageInfo);
        IList<BusinessRuleSearchResultDTO> GetBusinessRuleBySearchCriteria(BusinessRuleSearchCriteriaDTO searchCriteria, PagingInformation pageInfo);

        /// <summary>
        /// Grant or Decline requst
        /// </summary>
        /// <param name="requestID">Request ID</param>
        /// <param name="userName">current logged user</param>
        /// <param name="Operation">1 -- Grant, 0 -- Decline</param>
        /// <returns></returns>
        string ConfirmUpgradeByRequestID(int requestID, string userName, int Operation);

        IList<GrantedRequestEmailGenerateDTO> GetGrantedRequestForEmail(string loginConfirmationNum);
        IList<DeniedRequestEmailGenerateDTO> GetDeniedRequestForEmail(string loginConfirmationNum);

        bool SendRequestConfirmEmail(RequestConfirmEmailSendDTO data);
        bool SendGlobalEmail(RequestConfirmEmailSendDTO data);
        void SendRequestNotificationEmail(int id);

        string UpdateActiveYNByBusinessRuleID(int businessRuleID, string userName, int operation);

        byte[] GenerateRequestExcelBySearchCriteria(RequestSearchCriteriaDTO searchCriteria, PagingInformation pageInfo);
        byte[] GenerateBusinessRuleExcelBySearchCriteria(BusinessRuleSearchCriteriaDTO searchCriteria, PagingInformation pageInfo);
        int GetRequestCommitted();
        bool CheckAllRequestsProcessed(int reqId, out GlobalEmailGuestDTO guestInfo);
        GuestPlanner GetUnconfirmedRequest(string confirmNum, string reqId);
        GuestPlanner_Log GetUnconfirmedRequestLog(string confirmNum, string reqId);
        void updateUnconfirmedRequest(GuestPlanner unconfirmedRequest);
        void updateUnconfirmedRequestLog(GuestPlanner_Log unconfirmedRequestLog);
        int CheckNewRequest(int interval);
        //void UpdateRoomTypeCodeInPlannerEvent(string hotelCode, string oldRoomTypeCode, string newRoomTypeCode);

        string GetRequestID(List<int> guestPlannerIDList);
        bool UpdatRequestIDsOfeUpgradeRequest(List<int> eUpgradeRequestIDList, string requestID);
    }
}

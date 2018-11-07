using Cendyn.eConcierge.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Service.Interface
{
    public interface IUserAccountService
    {
        bool Login(string userName, string password);
        bool LoginUseToken(string token, out LoginTokenDTO loginToken);
        IList<ListItemDTO> GetUserMappedHotels(string username);
        IList<ListItemDTO> GetCommandColumnSetting(string username);
        IList<string> GetUserMappedHotelCodes(string username);
        IList<ListItemDTO> GetUserMappedHotelsCurrencyList(string username);
        bool CheckUserHasPermissionByHotelCode(string userName, string HotelCode);
        ConciergeLogin GetUserByConciergeID(string email);
        string GeneratePasswordResetToken(string email);
        string DecryptPasswordResetToken(string token);
        bool ResetUserPassword(string email, string password);
        bool RedirectUseToken(string username, string password, string timestamp, out string conciergeID);
        string GenerateRedirectUrlToken(string p1, string p2, DateTime dateTime);
        string StringTripleDESEncrypt(string input);
        string GetreservationNum(string hotelCode, string loginConfirmationNum);
    }
}

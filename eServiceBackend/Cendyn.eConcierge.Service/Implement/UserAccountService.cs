
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using Cendyn.eConcierge.Service.Interface;
using Cendyn.eConcierge.Core.Helper;
using Cendyn.eConcierge.EntityModel;
using EContact.Infrastructure.Encryption;

namespace Cendyn.eConcierge.Service.Implement
{
    public class UserAccountService : ServiceBase, IUserAccountService
    {
        public IConciergeLoginRepository conciergeLoginloginRepo { get; set; }

        public IConciergeHotelAccessMappingRepository conHotelAccessMappingRepo { get; set; }
        public ICurrencyConvertRepository currencyConvertRepo { get; set; }

        public IHotelRepository hotelRepo { get; set; }
        public IFGuestRepository fgRepo { get; set; }
        public IAppConfig appConfig { get; set; }

        public bool Login(string userName, string password)
        {
            //Encode the password user input
            var encodedPassword = SimpleTextEncodeHelper.EncodeText(password);

            //Retrieve user information from concierge login table
            var user = conciergeLoginloginRepo.Get(x => x.ConciergeID == userName && x.password == encodedPassword && x.Active == true);
            if (user != null)
            {
                var accesslist = (from x in conHotelAccessMappingRepo.GetAll()
                                  where (x.ConciergeID == userName && x.ActiveYN == true)
                                  select x).ToList();
                if (accesslist.Any())
                    return true;
                else
                    return false;
            }

            return false;

        }

        public bool LoginUseToken(string token, out LoginTokenDTO loginToken)
        {
            loginToken = new LoginTokenDTO();

            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            var decryptedString = string.Empty;
            using (var decrypter = new TripleDESEncryption(appConfig.TripleDESEncryptionKey))
            {
                decryptedString = decrypter.Decrypt(token);
            }

            var paramsFromUrl = StringHelper.SplitString(decryptedString, "&")
                .Select(p => { var m = p.Split('='); return new { key = m[0], value = m[1] }; })
                .ToDictionary(x => x.key, x => x.value);

            //Encode the password user input
            var encodedPassword = SimpleTextEncodeHelper.EncodeText(paramsFromUrl["pwd"]);
            var username = paramsFromUrl["uid"];
            //Retrieve user information from concierge login table
            var user = conciergeLoginloginRepo.Get(x => x.ConciergeID == username && x.password == encodedPassword);

            if (user != null)
            {
                
                string arrivalDate, status;
                paramsFromUrl.TryGetValue("arrivaldate", out arrivalDate);
                paramsFromUrl.TryGetValue("status", out status);

                //Set Info
                loginToken.UserName = user.ConciergeID;
                loginToken.ArrivalDate = arrivalDate;
                loginToken.Status = status;

                return true;
            }

            return false;

        }

        public IList<ListItemDTO> GetUserMappedHotels(string username)
        {
            var list = (from mapping in conHotelAccessMappingRepo.GetAll()
                        join hotel in hotelRepo.GetAll() on mapping.HotelCode equals hotel.Hotel_Code
                        where mapping.ConciergeID == username && mapping.ActiveYN == true
                        orderby hotel.Hotel_Code
                        select new ListItemDTO()
                        {
                            DisplayName = hotel.Hotel_Name,
                            Value = hotel.Hotel_Code+ "_"+ hotel.DateFormat
                        }).ToList();

            return list;

        }

        public IList<ListItemDTO> GetCommandColumnSetting(string username)
        {
            var list = (from mapping in conHotelAccessMappingRepo.GetAll()
                        join hotel in hotelRepo.GetAll() on mapping.HotelCode equals hotel.Hotel_Code
                        where mapping.ConciergeID == username && mapping.ActiveYN == true
                        orderby hotel.Hotel_Code
                        select new ListItemDTO()
                        {
                            DisplayName = hotel.Hotel_Code,
                            Value = (hotel.CenResServiceInterface != null && hotel.CenResServiceInterface != "" && hotel.CenResServiceUrl != null && hotel.CenResServiceUrl !="" && hotel.AutoRequestProcessYN) ? "false" : "true",
                        }).ToList();

            return list;

        }

        public IList<string> GetUserMappedHotelCodes(string username)
        {
            var list = (from mapping in conHotelAccessMappingRepo.GetAll()
                        join hotel in hotelRepo.GetAll() on mapping.HotelCode equals hotel.Hotel_Code
                        where mapping.ConciergeID == username && mapping.ActiveYN == true
                        orderby hotel.Hotel_Code
                        select hotel.Hotel_Code).ToList();

            return list;

        }

        public IList<ListItemDTO> GetUserMappedHotelsCurrencyList(string username)
        {
            var list = (from mapping in conHotelAccessMappingRepo.GetAll()
                        join hotel in hotelRepo.GetAll() on mapping.HotelCode equals hotel.Hotel_Code
                        join cc in currencyConvertRepo.GetAll() on new { A = hotel.Hotel_Code, B = hotel.LocalCurrencyCode } equals new { A = cc.Hotel_Code, B = cc.Code }
                        where mapping.ConciergeID == username && mapping.ActiveYN == true
                        orderby hotel.Hotel_Code
                        select new ListItemDTO()
                        {
                            DisplayName = cc.Symbol,
                            Value = hotel.Hotel_Code + "_" + cc.Code
                        }).ToList();

            return list;

        }

        public bool CheckUserHasPermissionByHotelCode(string userName, string HotelCode)
        {
            return conHotelAccessMappingRepo.GetAll().Any(x => x.ConciergeID == userName 
                && x.HotelCode == HotelCode && x.ActiveYN == true);
        }

        public ConciergeLogin GetUserByConciergeID(string email)
        {
            var user = conciergeLoginloginRepo.Get(x => x.ConciergeID == email);
            return user;
        }

        public string GeneratePasswordResetToken(string token)
        {
            using (var encrypter = new TripleDESEncryption(appConfig.TripleDESEncryptionKey))
            {
                token = System.Web.HttpUtility.UrlEncode(encrypter.Encrypt(token));
            }
            //var linkurl = "{0}/Account/ResetPassword?token={1}";
            //string domainName = System.Web.HttpContext.Current.Request.Url.Host;
            //return string.Format(linkurl, domainName, token);
            return token;
        }

        public string DecryptPasswordResetToken(string token)
        {
            using (var decrypter = new TripleDESEncryption(appConfig.TripleDESEncryptionKey))
            {
                token = System.Web.HttpUtility.UrlDecode(decrypter.Decrypt(token));
            }
            return token;
        }

        public bool ResetUserPassword(string email, string password)
        {
            bool succeed = false;
            var user = conciergeLoginloginRepo.Get(x => x.ConciergeID == email);
            if (user == null)
            {
                return succeed;
            }
            user.password1 = password;
            user.password = SimpleTextEncodeHelper.EncodeText(password);
            conciergeLoginloginRepo.Update(user);
            unitOfWork.Commit();
            succeed = true;
            return succeed;
        }

        //public byte[] GenerateUserExcelBySearchCriteria(UserSearchCriteriaDTO searchCriteria, PagingInformation pageInfo)
        //{

        //    //For excel export, we need to export all records. 
        //    //So set the pageindex and pagesize to -1
        //    pageInfo.StartIndex = -1;
        //    pageInfo.PageSize = -1;

        //    var list = GetUserBySearchCriteria(searchCriteria, pageInfo);

        //    //Get excel export list
        //    var excelList = mapper.Map<IList<BusinessRuleExportExcelModel>>(list);

        //    return excelList.ToExcelContent();

        //}

        public bool RedirectUseToken(string username, string password, string timestamp, out string conciergeID)
        {
            conciergeID = string.Empty;
            if (string.IsNullOrEmpty(username)
                || string.IsNullOrEmpty(password)
                || string.IsNullOrEmpty(timestamp))
            {
                return false;
            }

            var decrypted_username = string.Empty;
            var decrypted_password = string.Empty;
            var decrypted_timestamp = string.Empty;
            using (var decrypter = new TripleDESEncryption(appConfig.TripleDESEncryptionKey))
            {
                try
                {
                    decrypted_username = decrypter.Decrypt(username);
                    decrypted_password = decrypter.Decrypt(password);
                    decrypted_timestamp = decrypter.Decrypt(timestamp);
                }
                catch (FormatException fe)
                {
                    return false;
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            DateTime datetime = DateTime.Parse(decrypted_timestamp);
            if (DateTime.Compare(datetime.AddDays(20), DateTime.Now) < 0)
            {
                return false;
            }
            //Encode the password user input
            var encodedPassword = SimpleTextEncodeHelper.EncodeText(decrypted_password);
            var userid = decrypted_username;
            //Retrieve user information from concierge login table
            var user = conciergeLoginloginRepo.Get(x => x.ConciergeID == userid && x.password == encodedPassword);

            if (user != null)
            {
                conciergeID = user.ConciergeID;
                return true;
            }

            return false;

        }

        public string GenerateRedirectUrlToken(string username, string password, DateTime dt)
        {
            string encrypted_username = string.Empty;
            string encrypted_password = string.Empty;
            string encrypted_timestamp = string.Empty;

            using (var encrypter = new TripleDESEncryption(appConfig.TripleDESEncryptionKey))
            {
                encrypted_username = System.Web.HttpUtility.UrlEncode(encrypter.Encrypt(username));
                encrypted_password = System.Web.HttpUtility.UrlEncode(encrypter.Encrypt(password));
                encrypted_timestamp = System.Web.HttpUtility.UrlEncode(encrypter.Encrypt(dt.ToString()));
            }
            var linkurl = "{0}/Account/SSORedirectUrl?username={1}&password={2}&timestamp={3}";
            string domainName = System.Web.HttpContext.Current.Request.Url.Host;
            return string.Format(linkurl, domainName, encrypted_username, encrypted_password, encrypted_timestamp);
            //return token;
        }

        public string StringTripleDESEncrypt(string input)
        {
            var encrypted = string.Empty;
            using (var encrypter = new TripleDESEncryption(appConfig.TripleDESEncryptionKey))
            {
                try
                {
                    encrypted = encrypter.Encrypt(input);
                }
                catch (FormatException fe)
                {
                    encrypted = string.Empty;
                }
                catch (Exception e)
                {
                    encrypted = string.Empty;
                }
            }
            return encrypted;
        }

        public string GetreservationNum(string hotelCode, string loginConfirmationNum)
        {
            string reservationNum = string.Empty;

            bool CentralResNumYN = hotelRepo.Get(p => p.Hotel_Code == hotelCode && p.Live == true).CentralResNumYN;
            FGuest guest = fgRepo.Get(x => x.LoginConfirmationNum == loginConfirmationNum && x.HOTEL_CODE == hotelCode);

            if (CentralResNumYN)
            {
                if (guest.CentralResNum != "" && guest.CentralResNum != null && guest.CentralResNum != "''")
                {
                    reservationNum = guest.CentralResNum;
                }
                else
                {
                    reservationNum = guest.ConfirmationNum;
                }
            }
            else
            {
                reservationNum = guest.ConfirmationNum;
            }
            return reservationNum;

        }
    }
}

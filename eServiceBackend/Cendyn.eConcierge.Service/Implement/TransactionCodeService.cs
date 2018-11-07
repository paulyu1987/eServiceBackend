using AutoMapper;
using Cendyn.eConcierge.Core.Extensions;
using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using Cendyn.eConcierge.EntityModel;
using Cendyn.eConcierge.Service.Interface;
using Cendyn.eConcierge.Service.ExcelModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using NPOI.Extension;

namespace Cendyn.eConcierge.Service.Implement
{
    public class TransactionCodeService : ServiceBase, ITransactionCodeService
    {
        public IEUpgradeTransactionCodeRepository transactionCodeRepo { get; set; }
        public IHotelRepository hotelRepo { get; set; }
        public IHotels_LanguagesRepository hotellanguageRepo { get; set; }
        public ILanguageRepository languageRepo { get; set; }

        public IHotelService hotelService { get; set; }

        public IList<ListItemDTO> GeteUpgradeTransactionCodeListByHotelCode(string hotelCode)
        {
            IList<ListItemDTO> returnList = new List<ListItemDTO>();
            IList<ListItemDTO> removeList = new List<ListItemDTO>();

            if (!string.IsNullOrWhiteSpace(hotelCode))
            {
                var list = (from t in transactionCodeRepo.GetAll()
                            join hotel in hotelRepo.GetAll() on t.HotelCode equals hotel.Hotel_Code
                            where t.HotelCode == hotelCode && !t.UpgradeType.Contains("adj")
                            orderby t.HotelCode, t.UpgradeType descending
                            select new ListItemDTO()
                            {
                                DisplayName = t.UpgradeTypeDisplayName,
                                Value = t.HotelCode + "," + t.UpgradeType
                            }).Distinct().OrderByDescending(x => x.Value).ToList();
                returnList = list;
            }
            else
            {
                var list = (from t in transactionCodeRepo.GetAll()
                            join hotel in hotelRepo.GetAll() on t.HotelCode equals hotel.Hotel_Code
                            where !t.UpgradeType.Contains("adj")
                            orderby t.HotelCode, t.UpgradeType descending
                            select new ListItemDTO()
                            {
                                DisplayName = t.UpgradeTypeDisplayName,
                                Value = t.HotelCode + "," + t.UpgradeType
                            }).Distinct().OrderByDescending(x => x.Value).ToList();
                returnList = list;
            }
            
            IList<string> twoWaysHotels= hotelService.Get2WaysHotels();
            if (null != twoWaysHotels && twoWaysHotels.Count > 0)
            {

                if (returnList != null && returnList.Count > 0)
                {
                    foreach (var it in returnList)
                    {
                        if (twoWaysHotels.Contains(it.Value.Split(',')[0]) && !it.DisplayName.Contains("Room"))
                        {
                            if (!it.DisplayName.Equals("Add-On"))
                                removeList.Add(it);
                        }
                    }
                }
                returnList =  returnList.Except(removeList).ToList();
            }
            
            return returnList;
        }

        public IList<HotelLanguageItem> GetLanguageListByHotelCode(string hotelCode)
        {
            IList<HotelLanguageItem> returnList = new List<HotelLanguageItem>();

            if (!string.IsNullOrWhiteSpace(hotelCode))
            {
                var list = (from l in hotellanguageRepo.GetAll()
                            join hotel in hotelRepo.GetAll() on l.Hotel_Code equals hotel.Hotel_Code
                            join langs in languageRepo.GetAll() on l.LanguageID equals langs.LanguageID
                            where l.Hotel_Code == hotelCode && l.Active == true && langs.Active == true
                            orderby l.Hotel_Code descending
                            select new HotelLanguageItem()
                            {
                                ID = l.ID,
                                Hotel_Code = l.Hotel_Code,
                                LanguageID = l.LanguageID,
                                Active = l.Active,
                                CultureID = l.CultureID,
                                DateFormat = l.DateFormat,
                                CurrencyCode = l.CurrencyCode,
                                LanguageName = langs.Name
                            }).Distinct().OrderByDescending(x => x.ID).ToList();
                returnList = list;
            }
            else
            {
                var list = (from l in hotellanguageRepo.GetAll()
                            join hotel in hotelRepo.GetAll() on l.Hotel_Code equals hotel.Hotel_Code
                            join langs in languageRepo.GetAll() on l.LanguageID equals langs.LanguageID
                            where l.Active == true && langs.Active == true
                            orderby l.Hotel_Code descending
                            select new HotelLanguageItem()
                            {
                                ID = l.ID,
                                Hotel_Code = l.Hotel_Code,
                                LanguageID = l.LanguageID,
                                Active = l.Active,
                                CultureID = l.CultureID,
                                DateFormat = l.DateFormat,
                                CurrencyCode = l.CurrencyCode,
                                LanguageName = langs.Name
                            }).Distinct().OrderByDescending(x => x.ID).ToList();
                returnList = list;
            }

            return returnList;
        }
    }
}

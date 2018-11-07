using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using Cendyn.eConcierge.EntityModel;
using Cendyn.eConcierge.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Service.Implement
{
    public class SettingService: ServiceBase, ISettingService
    {
        public ISettingRepository settingRepo { get; set; }
        public ILanguageRepository languageRepo { get; set; }
        public IL_DateFormatRepository dateFormatRepo { get; set; }
        public IHotels_CurrencyRepository hotelsCurrencyRepo { get; set; }
        public IL_CurrencyRepository lCurrencyRepo { get; set; }
        public IHotels_LanguagesRepository hotelLangRepo { get; set; }
        public IPlannerEventRepository plannerEventRepo { get; set; }

        /// <summary>
        /// Based on setting key to get back setting value.
        /// </summary>
        /// <param name="key">Setting Key</param>
        /// <returns>Setting Value</returns>
        public string GetSettingValueByKey(string key)
        {
            var settingRecord = settingRepo.Get(x => x.settingKey == key);

            return settingRecord == null ? string.Empty : (settingRecord.settingValue ?? string.Empty);
        }

        public IList<ListItemDTO> GetAllLanguages()
        {
            IList<ListItemDTO> resultList = new List<ListItemDTO>();

            var langList = languageRepo.GetAll().Where(p => p.Active == true).ToList();
            if(langList != null && langList.Count > 0)
            {
                foreach(var lang in langList)
                {
                    resultList.Add(new ListItemDTO
                    {
                        DisplayName = lang.Name,
                        Value = lang.LanguageID.ToString(),
                    });
                };
            }

            return resultList;
        }

        public Language GetLanguageByCode(string langID)
        {
            int languageID = 1;
            Int32.TryParse(langID, out languageID);

            return languageRepo.GetAll().Where(p => p.Active == true && p.LanguageID == languageID).FirstOrDefault();
        }

        public IList<ListItemDTO> GetAllDateFormat()
        {
            IList<ListItemDTO> resultList = new List<ListItemDTO>();

            var dateFormatList = dateFormatRepo.GetAll().Where(p => p.Active == true).ToList();
            if (dateFormatList != null && dateFormatList.Count > 0)
            {
                foreach (var dateFormat in dateFormatList)
                {
                    resultList.Add(new ListItemDTO
                    {
                        DisplayName = dateFormat.DateFormat,
                        Value = dateFormat.ID.ToString(),
                    });
                };
            }

            return resultList;
        }

        public IList<ListItemDTO> GetAllCurrency()
        {
            List<ListItemDTO> currency = new List<ListItemDTO>();
            try
            {
                currency = (from lc in lCurrencyRepo.GetAll()
                            select new ListItemDTO()
                            {
                                DisplayName = lc.Name + " - " + lc.Code + " - " + lc.EncodedSymbol,
                                Value = lc.Code
                            }
                            ).ToList();
            }
            catch (Exception ex)
            {
                var message = ex.ToString();
            }
            return currency;
        }

        public IList<ListItemDTO> GetAllCurrencyByHotelCode(string hotelCode)
        {
            List<ListItemDTO> currency = new List<ListItemDTO>();
            try
            {
                currency = (from hc in hotelsCurrencyRepo.GetAll()
                            join lc in lCurrencyRepo.GetAll() on hc.Code equals lc.Code
                            where hc.Hotel_Code == hotelCode && hc.Active == true
                            select new ListItemDTO()
                            {
                                DisplayName = lc.Name + " - " + lc.Code + " - " + lc.EncodedSymbol,
                                Value = lc.Code
                            }
                            ).ToList();
            }
            catch (Exception ex)
            {
                var message = ex.ToString();
            }
            return currency;
        }

        public IList<HotelCurrencyItem> GetAllHotelActiveCurrency(string hotelCode)
        {
            IList<HotelCurrencyItem> currencies = plannerEventRepo.GetHotelActiveCurrency(hotelCode);
            return currencies;
        }

        public bool SaveHotelsLanguages(List<Hotels_Languages> lstHotelLanguage)
        {
            if (lstHotelLanguage == null || lstHotelLanguage.Count <= 0) return true;
            string hotelCode = lstHotelLanguage[0].Hotel_Code;
            List<Hotels_Languages> tempLstLanguages = hotelLangRepo.GetAll().Where(p => p.Hotel_Code == hotelCode).ToList();

            try
            {
                foreach (var hotelLang in lstHotelLanguage)
                {
                    int dateFormatID = 1;
                    Int32.TryParse(hotelLang.DateFormat, out dateFormatID);
                    hotelLang.DateFormat = dateFormatRepo.GetById(dateFormatID).DateFormat;
                    Hotels_Languages tempLang = tempLstLanguages.Where(p => p.LanguageID == hotelLang.LanguageID).FirstOrDefault();

                    if (tempLang != null && tempLang.ID != 0)
                    {
                        tempLang.Active = true;
                        tempLang.DateFormat = hotelLang.DateFormat;
                        tempLang.CurrencyCode = hotelLang.CurrencyCode;
                        tempLang.UpdateBy = hotelLang.UpdateBy;
                        tempLang.UpdateDate = hotelLang.UpdateDate;
                        tempLang.SortOrder = hotelLang.SortOrder;
                        hotelLangRepo.Update(tempLang);
                    }
                    else
                    {
                        hotelLang.InsertBy = hotelLang.UpdateBy;
                        hotelLang.InsertDate = DateTime.Now;
                        hotelLangRepo.Add(hotelLang);
                    }
                    unitOfWork.Commit();
                }

                foreach(var lang in tempLstLanguages)
                {
                    if (lstHotelLanguage.Where(p=>p.LanguageID == lang.LanguageID).Count() <= 0)
                    {
                        lang.Active = false;
                        hotelLangRepo.Update(lang);
                        unitOfWork.Commit();
                    }
                }
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool SaveHotelsCurrency(List<Hotels_Currency> lsthotelCurrency)
        {
            if (lsthotelCurrency == null || lsthotelCurrency.Count <= 0) return true;
            string hotelCode = lsthotelCurrency[0].Hotel_Code;
            List<Hotels_Currency> tempLstCurrency = hotelsCurrencyRepo.GetAll().Where(p => p.Hotel_Code == hotelCode).ToList();

            try
            {
                foreach (var hotelCurrency in lsthotelCurrency)
                {
                    Hotels_Currency tempCurrency = tempLstCurrency.Where(p => p.Code == hotelCurrency.Code).FirstOrDefault();

                    if (tempCurrency != null && tempCurrency.ID != 0)
                    {
                        tempCurrency.Active = true;

                        tempCurrency.USD_to_Rate = hotelCurrency.USD_to_Rate;
                        tempCurrency.SymbolPosition = hotelCurrency.SymbolPosition;
                        tempCurrency.UpdateBy = hotelCurrency.UpdateBy;
                        tempCurrency.UpdateDate = hotelCurrency.UpdateDate;

                        hotelsCurrencyRepo.Update(tempCurrency);
                    }
                    else
                    {
                        hotelCurrency.InsertBy = hotelCurrency.UpdateBy;
                        hotelCurrency.InsertDate = DateTime.Now;
                        hotelsCurrencyRepo.Add(hotelCurrency);
                    }
                    unitOfWork.Commit();
                }

                foreach (var currency in tempLstCurrency)
                {
                    if (lsthotelCurrency.Where(p => p.Code == currency.Code).Count() <= 0)
                    {
                        currency.Active = false;
                        hotelsCurrencyRepo.Update(currency);
                        unitOfWork.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public IList<ListItemDTO> GetAllHotelLangInfo()
        {
            List<ListItemDTO> result = new List<ListItemDTO>();
            try
            {
                result = (from hl in hotelLangRepo.GetAll()
                 join lc in lCurrencyRepo.GetAll() on hl.CurrencyCode equals lc.Code
                 join df in dateFormatRepo.GetAll() on hl.DateFormat equals df.DateFormat
                 where hl.Active == true
                 select new ListItemDTO()
                 {
                     DisplayName = hl.LanguageID + "_" + df.ID + "_" + lc.Code,
                     Value = hl.Hotel_Code
                 }).ToList();
            }
            catch (Exception ex)
            {
                var message = ex.ToString();
            }
            return result;
        }
    }
}

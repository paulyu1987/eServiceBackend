using AutoMapper;
using Cendyn.eConcierge.Core.Extensions;
using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using Cendyn.eConcierge.EntityModel;
using Cendyn.eConcierge.Service.Interface;
using Cendyn.eConcierge.Service.ExcelModel;
using NPOI.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Service.Implement
{
    public class RateTypeService : ServiceBase, IRateTypeService
    {
        public IConciergeHotelAccessMappingRepository conAccMapRepo { get; set; }
        public IHotelRepository hotelRepo { get; set; }
        public IPlannerEventRepository plannerEventRepo { get; set; }
        public IRateTypeRepository ratetypeRepo { get; set; }
        public IRateTypeLogRepository ratetypeLogRepo { get; set; }
        public IUserAccountService userAccountService { get; set; }
        public IBusinessRuleService businessRuleService { get; set; }
        public IMapper mapper { get; set; }

        public int AddNewRateTypeCode(RateType rateType)
        {
            try
            {
                ratetypeRepo.Add(rateType);
                unitOfWork.Commit();

                return rateType.ID;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public RateType GetRateTypeCodeByID(int ID)
        {
            return ratetypeRepo.GetById(ID);
        }

        public IList<ListItemDTO> GetRateTypeListByHotelCode(string hotel_code)
        {
            //var query = from rt in ratetypeRepo.GetAll()
            //            select new { rt };

            //if (!string.IsNullOrWhiteSpace(hotel_code))
            //{
            //    query = query.Where(p => p.rt.HotelCode == hotel_code);
            //}

            if (!string.IsNullOrWhiteSpace(hotel_code))
            {
                var list = (from rt in ratetypeRepo.GetAll()
                            where rt.ActiveYN == true && rt.HotelCode == hotel_code 
                            join hotel in hotelRepo.GetAll() on rt.HotelCode equals hotel.Hotel_Code
                            select new ListItemDTO()
                            {
                                DisplayName = rt.RateTypeCode,
                                Value = rt.HotelCode + "," + rt.RateTypeCode
                            }).ToList();
                return list;
            }
            else
            {
                var list1 = (from rt in ratetypeRepo.GetAll()
                             where rt.ActiveYN == true
                             join hotel in hotelRepo.GetAll() on rt.HotelCode equals hotel.Hotel_Code
                             select new ListItemDTO()
                             {
                                 DisplayName = rt.RateTypeCode,
                                 Value = rt.HotelCode + "," + rt.RateTypeCode
                             }).ToList();
                return list1;
            }


            
        }

        //public IList<RateTypeSearchResultDTO> getRateTypeBySearchCriteria(RateTypeSearchCriteriaDTO searchCriteria, PagingInformation pageInfo)
        //{
        //    IList<string> hotelCodeList = userAccountService.GetUserMappedHotelCodes(searchCriteria.UserName);
        //    if (hotelCodeList != null && hotelCodeList.Any())
        //    {
        //        List<RateType> rateTypeList = (from rt in ratetypeRepo.GetAll()
        //                                       where hotelCodeList.Contains(rt.HotelCode)
        //                                       select rt).ToList();

        //        //Hotel Name
        //        if (!string.IsNullOrWhiteSpace(searchCriteria.HotelName))
        //        {
        //            rateTypeList = rateTypeList.Where(p => p.HotelCode == searchCriteria.HotelName).ToList();
        //        }

        //        pageInfo.RecordsTotal = rateTypeList.Count();

        //        if (!string.IsNullOrWhiteSpace(pageInfo.OrderColumn))
        //        {
        //            string orderString = pageInfo.OrderColumn.TrimEnd(',');
        //            rateTypeList = rateTypeList.AsQueryable().OrderBy(orderString).ToList();
        //        }
        //        else
        //        {
        //            rateTypeList = rateTypeList.AsQueryable().OrderByDescending(p => p.ID).ToList();
        //        }

        //        //Setup paging
        //        if (pageInfo.PageSize == -1 && pageInfo.StartIndex == -1)
        //        {

        //        }
        //        else
        //        {
        //            rateTypeList = rateTypeList.AsQueryable().Skip(pageInfo.StartIndex).Take(pageInfo.PageSize).ToList();
        //        }
        //        //Get column that we need
        //        var list = rateTypeList.Select(x => new RateTypeSearchResultDTO()
        //        {
        //            id = x.ID,
        //            HotelCode = x.HotelCode,
        //            RateTypeCode = x.RateTypeCode,
        //            RateTypeCodeDescription = x.RateTypeCodeDescription,
        //            InsertDate = x.InsertDate,
        //            UpdateDate = x.UpdateDate,
        //            IsActive = x.ActiveYN

        //        }).ToList();

        //        return list;
        //    }
        //    else
        //        return null;
        //}

        public IList<RateTypeSearchResultDTO> getRateTypeBySearchCriteria(RateTypeSearchCriteriaDTO searchCriteria, PagingInformation pageInfo)
        {
                IList<RateTypeSearchResultDTO> list = ratetypeRepo.GetRateTypeBySearchCriteria(searchCriteria, pageInfo);
                var query = list.AsEnumerable();

                //Get all rateTypes
                pageInfo.RecordsTotal = query.Count();

                if (!string.IsNullOrWhiteSpace(pageInfo.OrderColumn))
                {
                    var orderString = TransformOrder(pageInfo.OrderColumn.TrimEnd(','));
                    query = query.OrderBy(orderString);
                }
                else
                {
                    //query = query.OrderByDescending(p => p.id);
                    query = query.OrderBy(p => p.HotelCode);
                }

                //Setup paging
                if (pageInfo.PageSize == -1 && pageInfo.StartIndex == -1)
                {

                }
                else
                {
                    query = query.Skip(pageInfo.StartIndex).Take(pageInfo.PageSize);
                }

                return query.ToList();
        }

        private string TransformOrder(string p)
        {
            //Get each order
            var Orders = p.Split(',');
            //var orderIndex = 0;
            var orderString = string.Empty;

            foreach (var order in Orders)
            {
                if (!string.IsNullOrWhiteSpace(order))
                {
                    //Get column name and order type, array[0] is column name , array[1] is order type
                    var array = order.Split(' ');
                    switch (array[0])
                    {
                        case "ID":
                            orderString += string.Format("ID {0},", array[1]);
                            break;
                        case "Hotel_Code":
                            orderString += string.Format("HotelCode {0},", array[1]);
                            break;
                        case "RateTypeCode":
                            orderString += string.Format("RateTypeCode {0},", array[1]);
                            break;
                        case "RateTypeCodeDescription":
                            orderString += string.Format("RateTypeCodeDescription {0},", array[1]);
                            break;
                        case "InsertDate":
                            orderString += string.Format("InsertDate {0},", array[1]);
                            break;
                        case "UpdateDate":
                            orderString += string.Format("UpdateDate {0},", array[1]);
                            break;
                        case "IsActive":
                            orderString += string.Format("IsActive {0},", array[1]);
                            break;
                        default:
                            break;
                    }
                }
            }

            return orderString.TrimEnd(',');
        }

        public string UpdateActiveYNByRateTypeID(int RateTypeID, string userName, int operation)
        {
            //Check if current logged in usser has permission to do this action
            //Based on Hotel code
            //Get current request
            var request = ratetypeRepo.GetById(RateTypeID);

            if (request == null)
            {
                return "404";
            }

            //Check if the hotelcode current user's access mapping
            if (!userAccountService.CheckUserHasPermissionByHotelCode(userName, request.HotelCode))
            {
                //Current user doesn't have permission
                return "403";
            }

            //Update Request Table
            request.UpdateDate = DateTime.Now;
            request.ActiveYN = operation == 1 ? true : false;

            ratetypeRepo.Update(request);

            //Update PlannerEvent table
            var active_pe_list = plannerEventRepo.GetAllActive().Where(p => p.RateTypeBooked == request.RateTypeCode).ToList();

            foreach(var pe in active_pe_list)
            {
                pe.ActiveYN = operation == 1;
                plannerEventRepo.Update(pe);
            }

            unitOfWork.Commit();

            return string.Empty;

        }

        //UpdateByToomType
        public bool UpdateByRateType(NewRateTypeSaveDTO data)
        {
            bool succeed = false;

            RateType rateType = null;
            //RateTypeLog rateTypeLog = null;

            //If id is 0, it means add a new Rate type
            if (data.ID == 0)
            {
                rateType = new RateType();
                rateType.ActiveYN = true;   //New item should be activate automatic
                rateType.HotelCode = data.HotelCode;
                rateType.RateTypeCode = data.RateTypeCode;
                rateType.InsertDate = DateTime.Now;
                rateType.UpdateDate = DateTime.Now;
                // rate type log data
                //rateTypeLog = new RateTypeLog();
                //rateTypeLog.ConciergeID = data.UserName;
                //rateTypeLog.RateTypeCode = data.RateTypeCode;
                //rateTypeLog.InsertDate = DateTime.Now;
            }
            else
            {
                rateType = ratetypeRepo.GetById(data.ID);
                rateType.UpdateDate = DateTime.Now;
            }

            #region insert to table RateType_Code

            //Set value from Ui
            rateType.RateTypeCodeDescription = data.RateTypeCodeDescription;
            try
            {
                if (data.ID == 0)
                {
                    //if id is 0, then add new
                    ratetypeRepo.Add(rateType);
                    //ratetypeLogRepo.Add(rateTypeLog);
                }
                else
                {
                    //else update
                    ratetypeRepo.Update(rateType);
                }
                unitOfWork.Commit();

                succeed = true;
            }
            catch (Exception e)
            {
                logger.Error("Exception: " + e.ToString());
                throw;
            }
            #endregion

            return succeed;
        }


        public byte[] GenerateRateTypeExcelBySearchCriteria(RateTypeSearchCriteriaDTO searchCriteria, PagingInformation pageInfo)
        {
            //For excel export, we need to export all records. 
            //So set the pageindex and pagesize to -1
            pageInfo.StartIndex = -1;
            pageInfo.PageSize = -1;

            var list = getRateTypeBySearchCriteria(searchCriteria, pageInfo);
            //if (searchCriteria.HotelName == null)
            //{
            //    //Hotel List
            //    var hotelList = userAccountService.GetUserMappedHotels(searchCriteria.UserName);

            //    if (hotelList.Count > 1)
            //    {
            //        var secondHotelValue = hotelList[0].Value;
            //        int index = secondHotelValue.IndexOf('_');
            //        var secondDateFormat = secondHotelValue.Substring(index + 1);
            //        secondDateFormat = secondDateFormat.ToLower().Replace("m", "M");
            //        for (var i = 0; i < list.Count; i++)
            //        {
            //            list[i].DateFormat = secondDateFormat;
            //        }
            //    }
            //}
            //Get excel export list
            var excelList = mapper.Map<IList<RateTypeExportExcelModel>>(list);

            return excelList.ToExcelContent();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using Cendyn.eConcierge.EntityModel;
using Cendyn.eConcierge.Service.Interface;
using System.Linq.Dynamic;
using AutoMapper;
using Cendyn.eConcierge.Service.ExcelModel;
using NPOI.Extension;

namespace Cendyn.eConcierge.Service.Implement
{
    public class GetReportInfoService : ServiceBase, IReportInfoService
    {
        public IReportInfoService reportInfoService { get; set; }

        public IFGuestRepository fguestRepo { get; set; }
        public IRoomType_CodeRepository roomTypeRepo { get; set; }

        public IEUpgradeRequestRepository eUpgradeRepo { get; set; }

        public IHotelRepository hotelRepo { get; set; }

        public IUserAccountService userAccountService { get; set; }

        public IMapper mapper { get; set; }

        /// <summary>
        /// Get all requests based on search criteria
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public IList<ReportSearchResultDTO> GetReportBySearchCriteria(ReportSearchCriteriaDTO searchCriteria, PagingInformation pageInfo)//, string DateFormatForALL)
        {
            IList<ReportSearchResultDTO> list = eUpgradeRepo.GetReportBySearchCriteria(searchCriteria, pageInfo);
            pageInfo.RecordsTotal = list.Count();

            //Setup Order, the order that passed in like "columnname asc, columnname desc"
            var query = list.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(pageInfo.OrderColumn))
            {
                var orderString = TransformOrder(pageInfo.OrderColumn);
                query = query.OrderBy(orderString);
            }
            else
            {
                //query = query.OrderByDescending(p => p.ConfirmedDate);
                query = query.OrderByDescending(p => p.ConfirmedDate).OrderBy(p=>p.HotelCode);
            }

            //Setup paging
            query = query.Skip(pageInfo.StartIndex).Take(pageInfo.PageSize);

            list = query.ToList();
            //if (string.IsNullOrEmpty(searchCriteria.HotelName) && !string.IsNullOrEmpty(DateFormatForALL))
            //{
            //    foreach (var it in list)
            //    {
            //        it.DateFormat = DateFormatForALL.ToLower().Replace("m", "M");
            //    }
            //}
            return list;

        }
        /// <summary>
        /// Transform the order string to column name
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
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
                        case "HotelCode":
                            orderString += string.Format("HotelCode {0},", array[1]);
                            break;
                        case "GuestName":
                            orderString += string.Format("GuestName {0},", array[1]);
                            break;
                        case "Email":
                            orderString += string.Format("Email {0},", array[1]);
                            break;
                        case "ArrivalDate":
                            orderString += string.Format("ArrivalDate {0},", array[1]);
                            break;
                        case "ConfirmedDate":
                            orderString += string.Format("ConfirmedDate {0},", array[1]);
                            break;
                        case "BookedRoomType":
                            orderString += string.Format("BookedRoomType {0},", array[1]);
                            break;
                        case "UpgradeRoomType":
                            orderString += string.Format("UpgradeRoomType {0},", array[1]);
                            break;
                        case "UpgradeCost":
                            orderString += string.Format("UpgradeCost {0},", array[1]);
                            break;
                        case "UpgradeStatus":
                            orderString += string.Format("UpgradeStatus {0},", array[1]);
                            break;
                        case "NightsOfStay":
                            orderString += string.Format("NightsOfStay {0},", array[1]);
                            break;
                        case "ReservationID":
                            orderString += string.Format("ReservationID {0},", array[1]);
                            break;
                        case "IncrementalTotalAmountForStay":
                            orderString += string.Format("IncTotalAmountForStay {0},", array[1]);
                            break;
                        default:
                            break;
                    }
                }
            }

            return orderString.TrimEnd(',');
        }

        public byte[] GenerateReportExcelBySearchCriteria(ReportSearchCriteriaDTO searchCriteria, PagingInformation pageInfo)//, string DateFormatForALL)
        {
            pageInfo.StartIndex = 0;
            pageInfo.PageSize =Int16.MaxValue;
            var list = GetReportBySearchCriteria(searchCriteria, pageInfo);//, DateFormatForALL);
            //Get excel export list
            var excelList = mapper.Map<IList<ReportExportExcelModel>>(list);

            return excelList.ToExcelContent();
        }
    }
}

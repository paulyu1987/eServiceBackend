using AutoMapper;
using Cendyn.eConcierge.Core.Extensions;
using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using Cendyn.eConcierge.EntityModel;
using Cendyn.eConcierge.Service.Interface;
using Cendyn.eConcierge.Service.ExcelModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using NPOI.Extension;

namespace Cendyn.eConcierge.Service.Implement
{
    public class UserManagementService : ServiceBase, IUserManagementService
    {
        public IConciergeHotelAccessMappingRepository conAccMapRepo { get; set; }
        public IHotelRepository hotelRepo { get; set; }
        public IConciergeLoginRepository conciergeLoginRepo { get; set; }
        public IUserAccountService userAccountService { get; set; }
        public IMapper mapper { get; set; }

        public IList<ListItemDTO> GetUserListByHotelCode(string hotel_code)
        {
            var query = from cl in conciergeLoginRepo.GetAll()
                        select new { cl };

            if (!string.IsNullOrWhiteSpace(hotel_code))
            {
                query = query.Where(p => p.cl.HOTEL_CODE == hotel_code);
            }

            var list = (from cl in conciergeLoginRepo.GetAll()
                        join hotel in hotelRepo.GetAll() on cl.HOTEL_CODE equals hotel.Hotel_Code
                        select new ListItemDTO()
                        {
                            DisplayName = cl.ConciergeID,
                            Value = cl.FName + "," + cl.LName
                        }).ToList();

            return list;
        }

        public IList<UserSearchResultDTO> getUserBySearchCriteria(UserSearchCriteriaDTO searchCriteria, PagingInformation pageInfo)
        {
            var query = from cl in conciergeLoginRepo.GetAll()
                        join h in hotelRepo.GetAll() on cl.HOTEL_CODE equals h.Hotel_Code
                        //select new { cl };
                        select new UserSearchResultDTO()
                        {
                            id = cl.ID,
                            HotelCode = cl.HOTEL_CODE,
                            ConciergeID = cl.ConciergeID,
                            FName = cl.FName,
                            LName = cl.LName,
                            Phone = cl.Phone,
                            IsActive = cl.Active,
                            AccessLevel = cl.AccessLevel,
                            DateFormat = h.DateFormat,
                        };

            //Hotel Name
            if (!string.IsNullOrWhiteSpace(searchCriteria.HotelName))
            {
                //query = query.Where(p => p.cl.HOTEL_CODE == searchCriteria.HotelName);
                query = query.Where(p => p.HotelCode == searchCriteria.HotelName);
            }

            pageInfo.RecordsTotal = query.Count();

            //Setup Order, the order that passed in like "columnname asc, columnname desc"
            if (!string.IsNullOrWhiteSpace(pageInfo.OrderColumn))
            {
                var orderString = TransformOrder(pageInfo.OrderColumn.TrimEnd(','));
                query = query.OrderBy(orderString);
            }
            else
            {
                //query = query.OrderByDescending(p => p.cl.ID);
                query = query.OrderByDescending(p => p.id);
            }

            //Setup paging
            if (pageInfo.PageSize == -1 && pageInfo.StartIndex == -1)
            {

            }
            else
            {
                query = query.Skip(pageInfo.StartIndex).Take(pageInfo.PageSize);
            }
            //Get column that we need
            //var list = query.Select(x => new UserSearchResultDTO()
            //{
            //    id = x.cl.ID,
            //    HotelCode = x.cl.HOTEL_CODE,
            //    ConciergeID = x.cl.ConciergeID,
            //    FName = x.cl.FName,
            //    LName = x.cl.LName,
            //    Phone = x.cl.Phone,
            //    IsActive = x.cl.Active,
            //    AccessLevel = x.cl.AccessLevel

            //}).ToList();



            //return list;
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
                            orderString += string.Format("id {0},", array[1]);
                            break;
                        case "Hotel_Code":
                            orderString += string.Format("HotelCode {0},", array[1]);
                            break;
                        case "ConciergeID":
                            orderString += string.Format("ConciergeID {0},", array[1]);
                            break;
                        case "FName":
                            orderString += string.Format("FName {0},", array[1]);
                            break;
                        case "LName":
                            orderString += string.Format("LName {0},", array[1]);
                            break;
                        case "Phone":
                            orderString += string.Format("Phone {0},", array[1]);
                            break;
                        case "IsActive":
                            orderString += string.Format("IsActive {0},", array[1]);
                            break;
                        case "AccessLevel":
                            orderString += string.Format("AccessLevel {0},", array[1]);
                            break;
                        default:
                            break;
                    }
                }
            }

            return orderString.TrimEnd(',');
        }
        
        public byte[] GenerateUserExcelBySearchCriteria(UserSearchCriteriaDTO searchCriteria, PagingInformation pageInfo)
        {
            //For excel export, we need to export all records. 
            //So set the pageindex and pagesize to -1
            pageInfo.StartIndex = -1;
            pageInfo.PageSize = -1;

            var list = getUserBySearchCriteria(searchCriteria, pageInfo);

            //Get excel export list
            var excelList = mapper.Map<IList<UserExportExcelModel>>(list);

            return excelList.ToExcelContent();
        }
    }
}

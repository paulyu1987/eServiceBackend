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
    public class RoomTypeService : ServiceBase, IRoomTypeService
    {
        public IConciergeHotelAccessMappingRepository conAccMapRepo { get; set; }
        public IHotelRepository hotelRepo { get; set; }
        public IHotels_LanguagesRepository hotelLangRepo { get; set; }
        public ILanguageRepository langRepo { get; set; }
        public IRoomType_CodeRepository roomtypeRepo { get; set; }
        public IUserAccountService userAccountService { get; set; }
        //public IGuestPlannerService guestPlannerService { get; set; }

        public IMapper mapper { get; set; }

        public int AddNewRoomTypeCode(RoomType_Code RoomTypeCode)
        {
            try
            {
                roomtypeRepo.Add(RoomTypeCode);
                unitOfWork.Commit();

                return RoomTypeCode.id;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public RoomType_Code GetRoomTypeCodeByID(int ID)
        {
            return roomtypeRepo.GetById(ID);
        }

        public IList<ListItemDTO> GetRoomTypeListByHotelCode(string hotel_code)
        {
            if (!string.IsNullOrWhiteSpace(hotel_code))
            {
                var list = (from grtc in roomtypeRepo.GetAll()
                            join hotel in hotelRepo.GetAll() on grtc.Hotel_Code equals hotel.Hotel_Code
                            where grtc.IsActive == true && grtc.Hotel_Code == hotel_code && grtc.LanguageId == 1

                            select new ListItemDTO()
                            {
                                DisplayName = !string.IsNullOrEmpty(grtc.RoomDescription) ? grtc.RoomDescription : grtc.RoomCode,
                                Value = grtc.Hotel_Code + "," + grtc.RoomCode
                            }).ToList();

                return list;

            }
            else
            {
                var list = (from grtc in roomtypeRepo.GetAll()
                            join hotel in hotelRepo.GetAll() on grtc.Hotel_Code equals hotel.Hotel_Code
                            where grtc.IsActive == true && grtc.LanguageId == 1

                            select new ListItemDTO()
                            {
                                //DisplayName = grtc.RoomDescription,
                                DisplayName = !string.IsNullOrEmpty(grtc.RoomDescription) ? grtc.RoomDescription : grtc.RoomCode,
                                Value = grtc.Hotel_Code + "," + grtc.RoomCode
                            }).ToList();
                return list;
            }
        }

        public IList<ListItemDTO> GetAllRoomTypeByHotelCode(string hotel_code)
        {
            var list = (from grtc in roomtypeRepo.GetAll()
                        join hotel in hotelRepo.GetAll() on grtc.Hotel_Code equals hotel.Hotel_Code
                        where grtc.IsActive == true && grtc.Hotel_Code == hotel_code && grtc.LanguageId == 1

                        select new ListItemDTO()
                        {
                            DisplayName = !string.IsNullOrEmpty(grtc.RoomDescription) ? grtc.RoomDescription : grtc.RoomCode,
                            Value = grtc.Hotel_Code + "," + grtc.RoomCode
                        }).ToList();

            return list;
        }

        public IList<ListItemDTO> GetRoomTypeListByRoomCode(string hotel_code, string roomcode)
        {
            var list = (from grtc in roomtypeRepo.GetAll()
                        join hotel in hotelRepo.GetAll() on grtc.Hotel_Code equals hotel.Hotel_Code
                        where grtc.IsActive == true && grtc.Hotel_Code == hotel_code && grtc.RoomCode == roomcode

                        select new ListItemDTO()
                        {
                            DisplayName = !string.IsNullOrEmpty(grtc.RoomDescription) ? grtc.RoomDescription : grtc.RoomCode,
                            Value = grtc.id + "," + grtc.RoomCode
                        }).ToList();

            return list;
        }

        public IList<RoomType_Code> GetAllRoomTypeListByRoomCode(string hotel_code, string roomcode)
        {
            var list = (from grtc in roomtypeRepo.GetAll()
                        join hotel in hotelRepo.GetAll() on grtc.Hotel_Code equals hotel.Hotel_Code
                        where grtc.Hotel_Code == hotel_code && grtc.RoomCode == roomcode
                        select grtc).ToList();

            return list;
        }



        //public IList<RoomTypeSearchResultDTO> getRoomTypeBySearchCriteria(RoomTypeSearchCriteriaDTO searchCriteria, PagingInformation pageInfo)
        //{
        //    IList<string> hotelCodeList = userAccountService.GetUserMappedHotelCodes(searchCriteria.UserName);
        //    if (hotelCodeList != null && hotelCodeList.Any())
        //    {
        //        List<RoomType_Code> roomtypeList = (from grtc in roomtypeRepo.GetAll()
        //                                            where hotelCodeList.Contains(grtc.Hotel_Code)
        //                                            select grtc).ToList();

        //        //Hotel Name
        //        if (!string.IsNullOrWhiteSpace(searchCriteria.HotelName))
        //        {
        //            roomtypeList = roomtypeList.Where(p => p.Hotel_Code == searchCriteria.HotelName).ToList();
        //        }

        //        pageInfo.RecordsTotal = roomtypeList.Count();

        //        //Setup Order, the order that passed in like "columnname asc, columnname desc"
        //        //if (pageInfo.OrderColumn.Contains("UpgradeType"))
        //        //{
        //        //    pageInfo.OrderColumn = pageInfo.OrderColumn.Replace("UpgradeType", "AddOnYn");
        //        //}

        //        if (!string.IsNullOrWhiteSpace(pageInfo.OrderColumn))
        //        {
        //            string orderString = pageInfo.OrderColumn.TrimEnd(',');
        //            roomtypeList = roomtypeList.AsQueryable().OrderBy(orderString).ToList();
        //        }
        //        else
        //        {
        //            roomtypeList = roomtypeList.AsQueryable().OrderByDescending(p => p.id).ToList();
        //        }
        //        //var ordercolumns = query.OrderBy(p)

        //        //Setup paging
        //        if (pageInfo.PageSize == -1 && pageInfo.StartIndex == -1)
        //        {

        //        }
        //        else
        //        {
        //            roomtypeList = roomtypeList.AsQueryable().Skip(pageInfo.StartIndex).Take(pageInfo.PageSize).ToList();
        //        }
        //        //Get column that we need
        //        var list = roomtypeList.AsQueryable().Select(x => new RoomTypeSearchResultDTO()
        //        {
        //            id = x.id,
        //            Hotel_Code = x.Hotel_Code,
        //            RoomCode = x.RoomCode,
        //            RoomDescription = x.RoomDescription,
        //            RoomLongDescription = x.RoomLongDescription,
        //            PriceDesc = x.PriceDesc,
        //            PerNightCharge = x.PerNightCharge,
        //            IsActive = x.IsActive,
        //            AddOnYN = x.AddOnYN,
        //            ImageYN = x.ImageYN,
        //            UpgradeType = x.AddOnYN=="Y"? "Add-on": "Room",
        //        }).ToList();

        //        return list;
        //    }
        //    else
        //        return null;
        //}

        public IList<RoomTypeSearchResultDTO> getRoomTypeBySearchCriteria(RoomTypeSearchCriteriaDTO searchCriteria, PagingInformation pageInfo)
        {
            IList<RoomTypeSearchResultDTO> list = roomtypeRepo.GetRateTypeBySearchCriteria(searchCriteria, pageInfo);

            var query = list.AsEnumerable();

            //Get all rateTypes
            pageInfo.RecordsTotal = query.Count();

            if (!string.IsNullOrWhiteSpace(pageInfo.OrderColumn))
            {
                var orderString = pageInfo.OrderColumn.TrimEnd(',');
                query = query.OrderBy(orderString);
            }
            else
            {
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
                        case "id":
                            orderString += string.Format("grtc.id {0},", array[1]);
                            break;
                        case "Hotel_Code":
                            orderString += string.Format("grtc.Hotel_Code {0},", array[1]);
                            break;
                        case "RoomCode":
                            orderString += string.Format("grtc.RoomCode {0},", array[1]);
                            break;
                        case "RoomDescription":
                            orderString += string.Format("grtc.RoomDescription {0},", array[1]);
                            break;
                        case "RoomLongDescription":
                            orderString += string.Format("grtc.RoomLongDescription {0},", array[1]);
                            break;
                        case "PriceDesc":
                            orderString += string.Format("grtc.PriceDesc {0},", array[1]);
                            break;
                        case "Threshold":
                            orderString += string.Format("grtc.Threshold {0},", array[1]);
                            break;
                        case "ImageYN":
                            orderString += string.Format("grtc.ImageYN {0},", array[1]);
                            break;
                        case "AddOnYN":
                            orderString += string.Format("grtc.AddOnYN {0},", array[1]);
                            break;
                        case "TotalRoom":
                            orderString += string.Format("grtc.TotalRoom {0},", array[1]);
                            break;
                        default:
                            break;
                    }
                }
            }

            return orderString.TrimEnd(',');
        }



        public string UpdateActiveYNByRoomTypeID(int RoomTypeID, string userName, int operation)
        {
            //Check if current logged in usser has permission to do this action
            //Based on Hotel code
            //Get current request
            var request = roomtypeRepo.GetById(RoomTypeID);

            if (request == null)
            {
                return "404";
            }

            //Check if the hotelcode current user's access mapping
            if (!userAccountService.CheckUserHasPermissionByHotelCode(userName, request.Hotel_Code))
            {
                //Current user doesn't have permission
                return "403";
            }

            //Update Request Table
            request.Updatedate = DateTime.Now;
            request.IsActive = operation == 1 ? true : false;

            roomtypeRepo.Update(request);

            unitOfWork.Commit();

            //Enable/Disable other language rooms
            var lstAllRoomTypes = GetAllRoomTypeListByRoomCode(request.Hotel_Code, request.RoomCode);
            if(lstAllRoomTypes != null && lstAllRoomTypes.Count > 0)
            {
                foreach(var room in lstAllRoomTypes)
                {
                    if (room.id == request.id) continue;

                    room.Updatedate = DateTime.Now;
                    room.IsActive = operation == 1 ? true : false;

                    roomtypeRepo.Update(room);
                    unitOfWork.Commit();
                }
            }

            return string.Empty;

        }

        //UpdateByToomType
        public bool UpdateByRoomType(NewRoomTypeSaveDTO data)
        {
            bool succeed = false;

            RoomType_Code roomTypeCode = null;

            //string oldRoomTypeCode = string.Empty;

            //If id is 0, it means add a new room type
            if (data.id == 0)
            {
                roomTypeCode = new RoomType_Code();
                roomTypeCode.IsActive = true;   //New item should be activate automatic

            }
            else
            {
                roomTypeCode = roomtypeRepo.GetById(data.id);
                //oldRoomTypeCode = roomTypeCode.RoomCode;

            }

            #region insert to table RoomType_Code

            //Set value from Ui
            roomTypeCode.Hotel_Code = data.Hotel_Code;
            roomTypeCode.RoomCode = data.RoomCode;
            roomTypeCode.RoomDescription = data.RoomDescription;
            roomTypeCode.RoomLongDescription = data.RoomLongDescription;
            roomTypeCode.LanguageId = Convert.ToInt32(data.languageid);
            roomTypeCode.PerNightCharge = data.PerNightCharge ? "Y" : "N";
            roomTypeCode.PriceDesc = data.PerNightCharge ?  "Additional per night" : "";
            roomTypeCode.ImageYN = data.ImageYN ? "Y" : "N";
            roomTypeCode.AddOnYN = data.AddOnYN;
            roomTypeCode.Threshold = data.Threshold;
            roomTypeCode.UpgradeType = data.UpgradeType;
            
            try
            {
                if (data.id == 0)
                {
                    //if id is 0, then add new
                    roomtypeRepo.Add(roomTypeCode);
                }
                else
                {
                    //else update
                    roomtypeRepo.Update(roomTypeCode);
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


        public byte[] GenerateRoomTypeExcelBySearchCriteria(RoomTypeSearchCriteriaDTO searchCriteria, PagingInformation pageInfo)
        {
            //For excel export, we need to export all records. 
            //So set the pageindex and pagesize to -1
            pageInfo.StartIndex = -1;
            pageInfo.PageSize = -1;

            var list = getRoomTypeBySearchCriteria(searchCriteria, pageInfo);

            //Get excel export list
            var excelList = mapper.Map<IList<RoomTypeExportExcelModel>>(list);

            return excelList.ToExcelContent();
        }

        public IList<String> GetRoomType(string hotel_code)
        {
            if (!string.IsNullOrWhiteSpace(hotel_code))
            {
                var list = (from grtc in roomtypeRepo.GetAll()
                            join hotel in hotelRepo.GetAll() on grtc.Hotel_Code equals hotel.Hotel_Code
                            where grtc.IsActive == true && grtc.Hotel_Code == hotel_code
                            select grtc.RoomCode).ToList();
                return list;
            }
            else
            {

                var list = (from grtc in roomtypeRepo.GetAll()
                            join hotel in hotelRepo.GetAll() on grtc.Hotel_Code equals hotel.Hotel_Code
                            where grtc.IsActive == true
                            select grtc.RoomCode).ToList();
                return list;
            }
        }

        public List<RoomTypeDisplayItem> GetRoomTypeDispaly(string roomCode, string hotelCode)
        {
            List<RoomTypeDisplayItem> items = new List<RoomTypeDisplayItem>();
            try
            {
                List<int> languageIDs = hotelLangRepo.GetAll().Where(x => x.Hotel_Code == hotelCode).Select(x => x.LanguageID).ToList();
                if (!languageIDs.Any()) languageIDs.Add(1);
                items = (from x in roomtypeRepo.GetAll()
                         join l in langRepo.GetAll() on x.LanguageId equals l.LanguageID
                         where x.RoomCode == roomCode && languageIDs.Contains(x.LanguageId) && x.Hotel_Code == hotelCode
                         select new RoomTypeDisplayItem()
                    {
                        RoomTypeCodeID = x.id,
                        CultureID = l.DicLanguageId,
                        LanguageID = l.LanguageID,
                        RoomDescription = x.RoomDescription,
                        RoomLongDescription = x.RoomLongDescription,
                        PriceDesc = x.PriceDesc
                    }).ToList();
            }
            catch (Exception ex)
            {
                var message = ex.ToString();
            }

            return items;
        }

    }
}

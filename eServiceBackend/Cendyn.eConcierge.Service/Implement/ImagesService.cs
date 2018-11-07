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
    public class ImagesService : ServiceBase, IImagesService
    {
        public IRoomImageRepository imagesRepo { get; set; }

        public IHotelRepository hotelRepo { get; set; }
        public IRoomType_CodeRepository roomtypeRepo { get; set; }

        public IUserAccountService userAccountService { get; set; }

        public RoomImage GetRoomImageById(int ID)
        {
            return imagesRepo.GetById(ID);
        }

        public IList<RoomImage> GetRoomImages(string hotelCode, string roomCode)
        {
            return imagesRepo.GetAll().Where(x => x.HotelCode == hotelCode && x.RoomCode == roomCode).OrderBy(x=>x.SortOrder).ToList();
        }

        public IList<ImagesSearchResultDTO> getUpGradeTypeImageBySearchCriteria(ImagesSearchCriteriaDTO searchCriteria, PagingInformation pageInfo)
        {
            IList<string> hotelCodeList = userAccountService.GetUserMappedHotelCodes(searchCriteria.UserName);
            if (hotelCodeList != null && hotelCodeList.Any())
            {
                List<RoomType_Code> roomtypeList = (from grtc in roomtypeRepo.GetAll()
                                                    where hotelCodeList.Contains(grtc.Hotel_Code) && grtc.IsActive == true && grtc.ImageYN == "Y" && grtc.LanguageId == 1
                                                    select grtc).ToList();

                //filter Hotel Name
                if (!string.IsNullOrWhiteSpace(searchCriteria.HotelName))
                {
                    roomtypeList = roomtypeList.Where(p => p.Hotel_Code == searchCriteria.HotelName).ToList();
                }

                //pageInfo.RecordsTotal = roomtypeList.Count();

                List<RoomImage> roomImages = imagesRepo.GetAll().ToList();

                //Get column that we need
                var list = roomtypeList.AsQueryable().Select(x => new ImagesSearchResultDTO()
                {
                    id = x.id,
                    Hotel_Code = x.Hotel_Code,
                    RoomCode = x.RoomCode,
                    RoomDescription = x.RoomDescription,
                    RoomLongDescription = x.RoomLongDescription,
                    PriceDesc = x.PriceDesc,
                    IsActive = x.IsActive,
                    AddOnYN = x.AddOnYN,
                    ImageYN = x.ImageYN,
                    UpgradeType = x.AddOnYN == "Y" ? "Add-on" : "Room Upgrade",
                    ImagesCount=roomImages.Where(y=>y.HotelCode==x.Hotel_Code&&y.RoomCode==x.RoomCode).ToList().Count,
                    LanguageId=x.LanguageId,
                    Images = roomImages.Where(y => y.HotelCode == x.Hotel_Code && y.RoomCode == x.RoomCode).OrderBy(p=>p.SortOrder).ToList()
                }).ToList();


                pageInfo.RecordsTotal = list.Count();

                if (!string.IsNullOrWhiteSpace(pageInfo.OrderColumn))
                {
                    string orderString = pageInfo.OrderColumn.TrimEnd(',');
                    list = list.AsQueryable().OrderBy(orderString).ToList();
                }
                else
                {
                    list = list.AsQueryable().OrderByDescending(p => p.id).ToList();
                }

                //Setup paging
                if (pageInfo.PageSize == -1 && pageInfo.StartIndex == -1)
                {

                }
                else
                {
                    list = list.AsQueryable().Skip(pageInfo.StartIndex).Take(pageInfo.PageSize).ToList();
                }

                return list;
            }
            else
                return null;
        }


        public bool SaveNewImages(IList<RoomImageDTO> images)
        {
            bool succeed = false;
            IList<RoomImage> savedImages = new List<RoomImage>();
            if (images.Count != 0)
            {
                IList<RoomImage> allImages = imagesRepo.GetAll().ToList();
                if(allImages.Count>0)
                    allImages=allImages.Where(x => x.HotelCode == images[0].HotelCode && x.RoomCode == images[0].RoomCode).ToList();
                foreach (RoomImageDTO imageDto in images)
                {
                    RoomImage image;
                    bool update = false;
                    if (allImages.Any(x => x.FileName == imageDto.FileName))
                    {
                        image = allImages.Where(x => x.FileName == imageDto.FileName).Single();
                        update = true;
                    }
                    else
                    {
                        image = new RoomImage();
                        image.InsertDate = DateTime.Now;
                    }
                    image.HotelCode = imageDto.HotelCode;
                    image.RoomCode = imageDto.RoomCode;
                    image.FileName = imageDto.FileName;
                    image.ImageCaption = imageDto.ImageCaption;
                    image.ImageUrl = imageDto.ImageUrl;
                    image.LanguageID = imageDto.LanguageID;
                    image.UpdateDate = DateTime.Now;
                    if (update == true)
                        imagesRepo.Update(image);
                    else
                        savedImages.Add(image);
                }
            }
            try
            {

                imagesRepo.Add(savedImages);
                unitOfWork.Commit();
                succeed = true;
            }
            catch (Exception e)
            {
                logger.Error("Exception: " + e.ToString());
                throw;
            }
            return succeed;
        }


        public bool UpdateRoomTypeImages(List<ImageUpdateItemDTO> Images)
        {
            bool succeed = false;
            IList<ImageUpdateItemDTO> updateImages = new List<ImageUpdateItemDTO>();
            IList<RoomImage> deleteImages = new List<RoomImage>();
            IList<RoomImage> allImages = imagesRepo.GetAll().ToList();
            if (Images.Count != 0)
            {
                if (allImages.Count > 0)
                    allImages = allImages.Where(x => x.HotelCode == Images[0].HotelCode && x.RoomCode == Images[0].RoomCode).ToList();
                deleteImages = allImages.Where(i1 => !Images.Any(i2 => i2.FileName == i1.FileName)).ToList();
                updateImages = Images.Where(i1 => allImages.Any(i2 => i2.FileName == i1.FileName)).ToList();
                foreach (ImageUpdateItemDTO imageDto in updateImages)
                {
                    RoomImage image = allImages.Where(x => x.FileName == imageDto.FileName).First();
                    image.ImageCaption = imageDto.ImageCaption;
                    if (imageDto.FileName != imageDto.ChangedFileName)
                    {
                        image.ImageUrl = image.ImageUrl.Substring(0,image.ImageUrl.LastIndexOf('/')) + "/" + imageDto.ChangedFileName;
                        image.FileName = imageDto.ChangedFileName;
                    }
                    image.LanguageID = imageDto.LanguageID;
                    image.UpdateDate = DateTime.Now;
                    image.SortOrder = imageDto.SortOrder;
                    imagesRepo.Update(image);
                }
                foreach (RoomImage deleteimage in deleteImages)
                {
                    imagesRepo.Delete(deleteimage);
                }
            }
            try
            {
                unitOfWork.Commit();
                succeed = true;
            }
            catch (Exception e)
            {
                logger.Error("Exception: " + e.ToString());
                throw;
            }
            return succeed;
        }

        public void DeleteAllImagesForRoomType(string hotelCode,string roomCode)
        {
            IList<RoomImage> allImages = GetRoomImages(hotelCode, roomCode);
            foreach (var image in allImages)
            {
                imagesRepo.Delete(image);
            }
            try
            {
                unitOfWork.Commit();
            }
            catch (Exception e)
            {
                logger.Error("Exception: " + e.ToString());
                throw;
            }
        }

    }
}

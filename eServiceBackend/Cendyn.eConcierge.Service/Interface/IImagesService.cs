using Cendyn.eConcierge.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Service.Interface
{
    public interface IImagesService
    {
        RoomImage GetRoomImageById(int ID);
        IList<ImagesSearchResultDTO> getUpGradeTypeImageBySearchCriteria(ImagesSearchCriteriaDTO searchCriteria, PagingInformation pageInfo);
        bool SaveNewImages(IList<RoomImageDTO> images);
        IList<RoomImage> GetRoomImages(string hotelCode, string roomCode);
        bool UpdateRoomTypeImages(List<ImageUpdateItemDTO> Images);
        void DeleteAllImagesForRoomType(string hotelCode, string roomCode);
    }
}

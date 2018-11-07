using Cendyn.eConcierge.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Service.Interface
{
    public interface IRoomTypeService
    {
        int AddNewRoomTypeCode(RoomType_Code RoomTypeCode);
        RoomType_Code GetRoomTypeCodeByID(int ID);
        IList<RoomTypeSearchResultDTO> getRoomTypeBySearchCriteria(RoomTypeSearchCriteriaDTO searchCriteria, PagingInformation pageInfo);
        string UpdateActiveYNByRoomTypeID(int RoomTypeID, string userName, int operation);
        IList<ListItemDTO> GetAllRoomTypeByHotelCode(string hotelCode);
        IList<ListItemDTO> GetRoomTypeListByHotelCode(string hotelCode);
        IList<ListItemDTO> GetRoomTypeListByRoomCode(string hotel_code, string roomcode);
        IList<RoomType_Code> GetAllRoomTypeListByRoomCode(string hotel_code, string roomcode);
        bool UpdateByRoomType(NewRoomTypeSaveDTO data);

        byte[] GenerateRoomTypeExcelBySearchCriteria(RoomTypeSearchCriteriaDTO searchCriteria, PagingInformation pageInfo);

        IList<String> GetRoomType(string hotel_code);
        List<RoomTypeDisplayItem> GetRoomTypeDispaly(string roomCode, string hotelCode);
    }
}

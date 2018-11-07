using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using Cendyn.eConcierge.Service.Interface;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cendyn.eConcierge.EntityModel;
using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using ExcelHelperCommon;
using Cendyn.eConcierge.Service;

namespace Cendyn.eConcierge.Service.Implement
{
    public class ImportRoomDetailsService : ServiceBase , IImportRoomDetailsService
    {
        public ITransactionCodeService transactionCodeService { get; set; }
        public IRoomTypeService roomTypeService { get; set; }
        public IRoomType_CodeRepository roomtypeRepo { get; set; }

        public List<int> Import(ISheet Sheet, IList<ListItemDTO> hotellist)
        {
            List<int> returnlist = new List<int>();
            int successnum = 0;
            RoomType_Code roomTypeCode = new RoomType_Code();
            var upgradeTypeList = transactionCodeService.GeteUpgradeTransactionCodeListByHotelCode("");
            List<string> hotelcodeList = hotellist.Select(p => p.Value.Split('_')[0]).ToList();
            List<string> upgradeTypeDisplayNameList = upgradeTypeList.Select(p => p.DisplayName).ToList();
            string HotelCode = "";
            string UpgradeType = "";
            string RoomCode = "";
            string RoomDescription = "";
            string Threshold = "";
            string RoomLongDescription = "";
            string ImageYN = "";
            string PerNightCharge = "";
            Int16? ThresholdNull = null;

            #region validate
            for (var i = 5; i <= Sheet.LastRowNum; i++)
            {
                var row = Sheet.GetRow(i);
                HotelCode = ExcelHelper.FormatCellValue(row.GetCell(1));
                UpgradeType = ExcelHelper.FormatCellValue(row.GetCell(2));
                RoomCode = ExcelHelper.FormatCellValue(row.GetCell(3));
                RoomDescription = ExcelHelper.FormatCellValue(row.GetCell(4));
                Threshold = ExcelHelper.FormatCellValue(row.GetCell(5));
                RoomLongDescription = ExcelHelper.FormatCellValue(row.GetCell(6));
                ImageYN = ExcelHelper.FormatCellValue(row.GetCell(7));
                PerNightCharge = ExcelHelper.FormatCellValue(row.GetCell(8));

                if(string.IsNullOrEmpty(HotelCode) && string.IsNullOrEmpty(UpgradeType) && string.IsNullOrEmpty(RoomCode)
                    && string.IsNullOrEmpty(RoomDescription) && string.IsNullOrEmpty(Threshold) && string.IsNullOrEmpty(RoomLongDescription)
                    && string.IsNullOrEmpty(ImageYN) && string.IsNullOrEmpty(PerNightCharge))   // check row null
                {
                    continue;
                }

                if(string.IsNullOrEmpty(HotelCode) || string.IsNullOrEmpty(UpgradeType) || string.IsNullOrEmpty(RoomCode)
                        || string.IsNullOrEmpty(RoomDescription) || string.IsNullOrEmpty(RoomLongDescription)) //check require field
                {
                    returnlist.Add(-1);
                }

                if (!hotelcodeList.Exists(p => p == HotelCode))
                {
                    returnlist.Add(-2);
                }
                if (!upgradeTypeDisplayNameList.Exists(p => p == UpgradeType))
                {
                    if(UpgradeType.IndexOf("Room") < 0)
                    {
                        returnlist.Add(-3);
                    }
                    returnlist.Add(-2);
                }
                if (RoomCode.Length > 30)
                {
                    returnlist.Add(-2);
                }
                if (RoomDescription.Length > 500)
                {
                    returnlist.Add(-2);
                }
                if(UpgradeType.IndexOf("Room") >= 0 && !string.IsNullOrEmpty(Threshold))
                {
                    try
                    {
                        Convert.ToInt16(Threshold);

                    }
                    catch(Exception ex)
                    {

                        returnlist.Add(-2);
                    }
                }
                if (RoomLongDescription.Length > 1000)
                {
                    returnlist.Add(-2);
                }
                if (ImageYN.ToUpper() != "Y" && ImageYN.ToUpper() != "N" && !string.IsNullOrEmpty(ImageYN))
                {
                    returnlist.Add(-2);
                }
                if (PerNightCharge.ToUpper() != "Y" && PerNightCharge.ToUpper() != "N" && !string.IsNullOrEmpty(ImageYN))
                {
                    returnlist.Add(-2);
                }
               
            }
            #endregion

            if (returnlist.Count > 0)
            {
                return returnlist;
            }

            #region dataaccess
            for (var i = 5; i <= Sheet.LastRowNum; i++)
            {
                var row = Sheet.GetRow(i);
                HotelCode = ExcelHelper.FormatCellValue(row.GetCell(1));
                UpgradeType = ExcelHelper.FormatCellValue(row.GetCell(2));
                RoomCode = ExcelHelper.FormatCellValue(row.GetCell(3));
                RoomDescription = ExcelHelper.FormatCellValue(row.GetCell(4));
                Threshold = ExcelHelper.FormatCellValue(row.GetCell(5));
                RoomLongDescription = ExcelHelper.FormatCellValue(row.GetCell(6));
                ImageYN = ExcelHelper.FormatCellValue(row.GetCell(7));
                PerNightCharge = ExcelHelper.FormatCellValue(row.GetCell(8));


                try
                {
                    if(!string.IsNullOrEmpty(HotelCode))
                    {
                        var upgradevaluelist = upgradeTypeList.Where(p => p.DisplayName == UpgradeType);
                        UpgradeType = upgradevaluelist.Select(p => p.Value.Split(',')[1]).ToList()[0].ToString();

                        var roomTypeList = roomTypeService.GetRoomTypeListByRoomCode(HotelCode, RoomCode);
                        List<string> roomCodeList = roomTypeList.Select(p => p.Value.Split(',')[1]).ToList();
                        if (roomCodeList.Exists(p => p == RoomCode))
                        {
                            string roomCodeID = roomTypeList.Select(p => p.Value.Split(',')[0]).ToList().First();
                            roomTypeCode = roomTypeService.GetRoomTypeCodeByID(Int32.Parse(roomCodeID));
                            roomTypeCode.Hotel_Code = HotelCode;
                            roomTypeCode.RoomCode = RoomCode;
                            roomTypeCode.RoomDescription = RoomDescription;
                            roomTypeCode.RoomLongDescription = RoomLongDescription;
                            roomTypeCode.PerNightCharge = PerNightCharge.ToUpper() == "Y" ? "Y" : "N";
                            roomTypeCode.PriceDesc = PerNightCharge.ToUpper() == "Y" ? "Additional per night" : "";
                            roomTypeCode.ImageYN = ImageYN.ToUpper() == "Y" ? "Y" : "N";
                            roomTypeCode.AddOnYN = UpgradeType.IndexOf("Room") >= 0 ? "N" : "Y";
                            roomTypeCode.Threshold = !string.IsNullOrEmpty(Threshold) && UpgradeType.IndexOf("Room") >= 0 ? Convert.ToInt16(Threshold) : ThresholdNull;
                            roomTypeCode.UpgradeType = UpgradeType;
                            roomTypeCode.LanguageId = 1;
                            roomTypeCode.Updatedate = DateTime.Now;
                            roomtypeRepo.Update(roomTypeCode);
                        }
                        else
                        {
                            roomTypeCode.IsActive = true;
                            roomTypeCode.Hotel_Code = HotelCode;
                            roomTypeCode.RoomCode = RoomCode;
                            roomTypeCode.RoomDescription = RoomDescription;
                            roomTypeCode.RoomLongDescription = RoomLongDescription;
                            roomTypeCode.PerNightCharge = PerNightCharge.ToUpper() == "Y" ? "Y" : "N";
                            roomTypeCode.PriceDesc = PerNightCharge.ToUpper() == "Y" ? "Additional per night" : "";
                            roomTypeCode.ImageYN = ImageYN.ToUpper() == "Y" ? "Y" : "N";
                            roomTypeCode.AddOnYN = UpgradeType.IndexOf("Room") >= 0 ? "N" : "Y";
                            roomTypeCode.Threshold = !string.IsNullOrEmpty(Threshold) && UpgradeType.IndexOf("Room") >= 0 ? Convert.ToInt16(Threshold) : ThresholdNull;
                            roomTypeCode.UpgradeType = UpgradeType;
                            roomTypeCode.LanguageId = 1;
                            roomTypeCode.InsertDate = DateTime.Now;
                            roomtypeRepo.Add(roomTypeCode);
                        }
                        unitOfWork.Commit();
                        successnum += 1;
                    }
                }
                catch (Exception dataaccessex)
                {
                    logger.Error("Exception: " + dataaccessex.ToString());
                    returnlist.Add(-2);
                }

            }
            #endregion
            returnlist.Add(successnum);
            return returnlist;
        }
    }
}

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
using System.Collections;
using System.Text.RegularExpressions;

namespace Cendyn.eConcierge.Service.Implement
{
    public class ImportRuleDetailsService : ServiceBase , IImportRuleDetailsService
    {
        public IPlannerEventService plannerEventService { get; set; }
        public IPlannerEventRepository plannerEventRepo { get; set; }
        public IRoomTypeService roomTypeService { get; set; }
        public IRateTypeService rateTypeService { get; set; }

        public ITransactionCodeService transactionCodeService { get; set; }
        //public IRoomTypeService roomTypeService { get; set; }
        //public IRoomType_CodeRepository roomtypeRepo { get; set; }


        public List<ListItemInt> Import(ISheet Sheet, IList<ListItemDTO> hotellist)
        {

            List<ListItemInt> returnlist = new List<ListItemInt>();

            int successnum = 0;
            //var upgradeTypeList = transactionCodeService.GeteUpgradeTransactionCodeListByHotelCode("");
            List<string> hotelcodeList = hotellist.Select(p => p.Value.Split('_')[0].ToUpper()).ToList();
            //List<string> upgradeTypeDisplayNameList = upgradeTypeList.Select(p => p.DisplayName).ToList();
            string HotelCode = "";
            string BookedRoom = "";
            string UpgradesPricedBy = "";
            string StartDate = "";
            string EndDate = "";
            string UpgradeOption = "";
            string RateTypes = "";
            string UpgradeCost = "";
            //Int16? ThresholdNull = null;

            bool FormatIncorrect = false;

            #region validate
            for (var i = 4; i <= Sheet.LastRowNum; i++)
            {
                List<string> listRatetypeValidate = new List<string>();

                var row = Sheet.GetRow(i);
                HotelCode = ExcelHelper.FormatCellValue(row.GetCell(1));
                BookedRoom = ExcelHelper.FormatCellValue(row.GetCell(2));
                UpgradesPricedBy = ExcelHelper.FormatCellValue(row.GetCell(3));
                StartDate = ExcelHelper.FormatCellValue(row.GetCell(4));
                EndDate = ExcelHelper.FormatCellValue(row.GetCell(5));
                UpgradeOption = ExcelHelper.FormatCellValue(row.GetCell(6));
                RateTypes = ExcelHelper.FormatCellValue(row.GetCell(7));
                UpgradeCost = ExcelHelper.FormatCellValue(row.GetCell(8));

                if (string.IsNullOrEmpty(HotelCode) && string.IsNullOrEmpty(BookedRoom) && string.IsNullOrEmpty(UpgradeOption)
                    && string.IsNullOrEmpty(UpgradesPricedBy) && string.IsNullOrEmpty(StartDate) && string.IsNullOrEmpty(EndDate)
                    && string.IsNullOrEmpty(UpgradeCost) && string.IsNullOrEmpty(RateTypes))   // check row null
                {
                    continue;
                }

                if (string.IsNullOrEmpty(HotelCode) || string.IsNullOrEmpty(BookedRoom)
                    || string.IsNullOrEmpty(UpgradeOption) || string.IsNullOrEmpty(UpgradeCost)
                    || string.IsNullOrEmpty(UpgradesPricedBy) || string.IsNullOrEmpty(RateTypes)) //check require field
                {
                    ListItemInt listItemEm = new ListItemInt();
                    listItemEm.Key = -1;
                    listItemEm.Value = 0;

                    returnlist.Add(listItemEm);

                    FormatIncorrect = true;
                }

                if (!hotelcodeList.Exists(p => p == (HotelCode.ToUpper())))
                {
                    ListItemInt listItemHotelCode = new ListItemInt();
                    listItemHotelCode.Key = -2;
                    listItemHotelCode.Value = 0;

                    returnlist.Add(listItemHotelCode);

                    FormatIncorrect = true;
                }

                //if HotelCode exsits，need judge BookedRoom，UpgradeOption，RateTypes whether belongs to HotelCode.
                if (hotelcodeList.Exists(p => p == (HotelCode.ToUpper())))
                {
                    IList<ListItemDTO> roomTypeList = roomTypeService.GetRoomTypeListByHotelCode(HotelCode);
                    IList<ListItemDTO> rateTypeList = rateTypeService.GetRateTypeListByHotelCode(HotelCode);

                    List<string> roomTypeDisplayNameList = roomTypeList.Select(p => p.Value.Split(',')[1].ToUpper()).ToList();
                    List<string> rateTypeDisplayNameList = rateTypeList.Select(p => p.DisplayName.ToUpper()).ToList();

                    if (!roomTypeDisplayNameList.Exists(p => p == (BookedRoom.ToUpper())))
                    {
                        ListItemInt listItemBookedRoom = new ListItemInt();
                        listItemBookedRoom.Key = -2;
                        listItemBookedRoom.Value = 0;

                        returnlist.Add(listItemBookedRoom);

                        FormatIncorrect = true;
                    }
                    if (!roomTypeDisplayNameList.Exists(p => p == (UpgradeOption.ToUpper())))
                    {
                        ListItemInt listItemUpgradeOption = new ListItemInt();
                        listItemUpgradeOption.Key = -2;
                        listItemUpgradeOption.Value = 0;

                        returnlist.Add(listItemUpgradeOption);

                        FormatIncorrect = true;
                    }

                    if (!string.IsNullOrEmpty(RateTypes))
                    {
                        //RateTypes is not selected all
                        if (RateTypes.ToUpper() != "ALL")
                        {
                            //RateTypes contain one more ratetype(split by ;)
                            listRatetypeValidate = RateTypes.Split(';').ToList();

                            foreach (var ratetype in listRatetypeValidate)
                            {
                                if (!rateTypeDisplayNameList.Exists(p => p == (ratetype.ToUpper())))
                                {
                                    ListItemInt listItemRateTypes = new ListItemInt();
                                    listItemRateTypes.Key = -2;
                                    listItemRateTypes.Value = 0;

                                    returnlist.Add(listItemRateTypes);

                                    FormatIncorrect = true;
                                }
                            }

                            //one line whether exist the same ratetypes
                            for (int ratetypeouter = 0; ratetypeouter < listRatetypeValidate.Count; ratetypeouter++)  //outer loop
                            {
                                for (int ratetypeinner = ratetypeouter + 1; ratetypeinner < listRatetypeValidate.Count; ratetypeinner++)  //inner loop
                                {

                                    if (listRatetypeValidate[ratetypeouter] == listRatetypeValidate[ratetypeinner])
                                    {
                                        ListItemInt listItemRateTypes = new ListItemInt();
                                        listItemRateTypes.Key = -2;
                                        listItemRateTypes.Value = 0;

                                        returnlist.Add(listItemRateTypes);

                                        FormatIncorrect = true;
                                    }

                                }
                            }
                        }
                        else
                        {
                            listRatetypeValidate.Add(null);
                        }
                    }

                }

                if (UpgradesPricedBy != "Arrival Date" && UpgradesPricedBy != "All")
                {
                    ListItemInt listItemUpgradesPricedBy = new ListItemInt();
                    listItemUpgradesPricedBy.Key = -2;
                    listItemUpgradesPricedBy.Value = 0;

                    returnlist.Add(listItemUpgradesPricedBy);

                    FormatIncorrect = true;
                }

                if (!IsNumber(UpgradeCost))
                {
                    ListItemInt listItemUpgradeCost = new ListItemInt();
                    listItemUpgradeCost.Key = -2;
                    listItemUpgradeCost.Value = 0;

                    returnlist.Add(listItemUpgradeCost);

                    FormatIncorrect = true;
                }


                if (UpgradesPricedBy == "Arrival Date")
                {
                    if (!string.IsNullOrEmpty(StartDate) || !string.IsNullOrEmpty(EndDate))
                    {
                        //StartDate，EndDate is the correct datetime format
                        DateTime dtStartDate;
                        DateTime dtEndDate;
                        if (!DateTime.TryParse(StartDate, out dtStartDate))
                        {
                            ListItemInt listItemStartDate = new ListItemInt();
                            listItemStartDate.Key = -2;
                            listItemStartDate.Value = 0;

                            returnlist.Add(listItemStartDate);

                            FormatIncorrect = true;
                        }
                        if (!DateTime.TryParse(EndDate, out dtEndDate))
                        {
                            ListItemInt listItemEndDate = new ListItemInt();
                            listItemEndDate.Key = -2;
                            listItemEndDate.Value = 0;

                            returnlist.Add(listItemEndDate);

                            FormatIncorrect = true;
                        }

                        if (dtStartDate > dtEndDate)
                        {
                            ListItemInt listItemDate = new ListItemInt();
                            listItemDate.Key = -2;
                            listItemDate.Value = 0;

                            returnlist.Add(listItemDate);

                            FormatIncorrect = true;
                        }
                    }

                }
            }

            for (var i = 4; i <= Sheet.LastRowNum; i++)
            {
                List<string> listRateTypeRuleValidate = new List<string>();

                var row = Sheet.GetRow(i);
                HotelCode = ExcelHelper.FormatCellValue(row.GetCell(1));
                BookedRoom = ExcelHelper.FormatCellValue(row.GetCell(2));
                UpgradesPricedBy = ExcelHelper.FormatCellValue(row.GetCell(3));
                StartDate = ExcelHelper.FormatCellValue(row.GetCell(4));
                EndDate = ExcelHelper.FormatCellValue(row.GetCell(5));
                UpgradeOption = ExcelHelper.FormatCellValue(row.GetCell(6));
                RateTypes = ExcelHelper.FormatCellValue(row.GetCell(7));
                UpgradeCost = ExcelHelper.FormatCellValue(row.GetCell(8));

                if (RateTypes.ToUpper() != "ALL")
                {
                    //RateTypes contain one more ratetype(split by ;)
                    listRateTypeRuleValidate = RateTypes.Split(';').ToList();

                }
                else
                {
                    listRateTypeRuleValidate.Add(null);
                }

                //excel's data is all correct
                if (!FormatIncorrect)
                {
                    //RuleValid and ExistValid
                    if (!string.IsNullOrEmpty(HotelCode))
                    {
                        #region  --foreach (var ratetype in listRateTypeRuleValidate)--
                        foreach (var ratetype in listRateTypeRuleValidate)
                        {

                            NewBusinessRuleSaveDTO model = new NewBusinessRuleSaveDTO();

                            //get the value by displayName
                            IList<ListItemDTO> roomTypeList = roomTypeService.GetRoomTypeListByHotelCode(HotelCode);

                            //string bookedRoomValue = roomTypeList.Where(p => p.DisplayName.ToUpper() == BookedRoom.ToUpper()).Select(p => p.Value.Split(',')[1]).ToList().FirstOrDefault();
                            string bookedRoomValue = roomTypeList.Where(p => p.Value.Split(',')[1].ToUpper() == BookedRoom.ToUpper()).Select(p => p.Value.Split(',')[1]).ToList().FirstOrDefault();
                            //string UpgradeOptionValue = roomTypeList.Where(p => p.DisplayName.ToUpper() == UpgradeOption.ToUpper()).Select(p => p.Value.Split(',')[1]).ToList().FirstOrDefault();
                            string UpgradeOptionValue = roomTypeList.Where(p => p.Value.Split(',')[1].ToUpper() == UpgradeOption.ToUpper()).Select(p => p.Value.Split(',')[1]).ToList().FirstOrDefault();

                            //get correct hotelcode
                            string getHotelCode = hotellist.Where(p => p.Value.Split('_')[0].ToUpper() == HotelCode.ToUpper()).Select(p => p.Value.Split('_')[0]).ToList().FirstOrDefault();

                            model.BookedRoomTypeCode = bookedRoomValue;
                            model.BusinessRuleID = 0;
                            if (!string.IsNullOrEmpty(EndDate))
                            {
                                model.EndDate = DateTime.Parse(EndDate);

                            }
                            model.HotelCode = getHotelCode;

                            model.RateTypeBooked = ratetype;
                            if (!string.IsNullOrEmpty(StartDate))
                            {
                                model.StartDate = DateTime.Parse(StartDate);
                            }
                            model.UpgradeCost = UpgradeCost;
                            model.UpgradePricedBy = UpgradesPricedBy;
                            model.UpgradeRoomTypeCode = UpgradeOptionValue;
                            model.UpgradeWeekDayWeekEnd = null;
                            //model.UserName = User.Identity.Name;

                            if (!plannerEventService.IsBussinessRuleValid(model))
                            {
                                //return Json(new
                                //{
                                //    StatusCode = "500",
                                //    Result = "failure",
                                //    Message = "The new business rule was not saved due to conflicts with other rules."
                                //})
                                ListItemInt listItemIsBussinessRuleValid = new ListItemInt();
                                listItemIsBussinessRuleValid.Key = -3;
                                listItemIsBussinessRuleValid.Value = i - 3;

                                returnlist.Add(listItemIsBussinessRuleValid);
                            }
                            if (plannerEventService.IsExistingBussinessRule(model))
                            {
                                //return Json(new
                                //{
                                //    StatusCode = "500",
                                //    Result = "failure",
                                //    Message = "This rule already exists."
                                //});
                                ListItemInt listItemIsExistingBussinessRule = new ListItemInt();
                                listItemIsExistingBussinessRule.Key = -4;
                                listItemIsExistingBussinessRule.Value = i - 3;

                                returnlist.Add(listItemIsExistingBussinessRule);
                            }


                            //excel's data compare with its own data (data whether conflict)
                            bool isNoteForBussinessRule = false; //for current conflict of BussinessRule is whether noting
                            bool isNoteForExistingRule = false; //for current conflict of ExistingRule is whether noting

                            #region  --for (var j = i + 1; j <= Sheet.LastRowNum; j++)--
                            for (var j = i + 1; j <= Sheet.LastRowNum; j++)
                            {

                                var rowCompareLine = Sheet.GetRow(j);
                                string HotelCodeCompareLine = ExcelHelper.FormatCellValue(rowCompareLine.GetCell(1));
                                string BookedRoomCompareLine = ExcelHelper.FormatCellValue(rowCompareLine.GetCell(2));
                                string UpgradesPricedByCompareLine = ExcelHelper.FormatCellValue(rowCompareLine.GetCell(3));
                                string StartDateCompareLine = ExcelHelper.FormatCellValue(rowCompareLine.GetCell(4));
                                string EndDateCompareLine = ExcelHelper.FormatCellValue(rowCompareLine.GetCell(5));
                                string UpgradeOptionCompareLine = ExcelHelper.FormatCellValue(rowCompareLine.GetCell(6));
                                string RateTypesCompareLine = ExcelHelper.FormatCellValue(rowCompareLine.GetCell(7));
                                string UpgradeCostCompareLine = ExcelHelper.FormatCellValue(rowCompareLine.GetCell(8));

                                //get the value by displayName
                                IList<ListItemDTO> roomTypeListCompareLine = roomTypeService.GetRoomTypeListByHotelCode(HotelCodeCompareLine);

                                //string bookedRoomValueCompareLine = roomTypeList.Where(p => p.DisplayName.ToUpper() == BookedRoomCompareLine.ToUpper()).Select(p => p.Value.Split(',')[1]).ToList().FirstOrDefault();
                                string bookedRoomValueCompareLine = roomTypeList.Where(p => p.Value.Split(',')[1].ToUpper() == BookedRoomCompareLine.ToUpper()).Select(p => p.Value.Split(',')[1]).ToList().FirstOrDefault();
                                //string UpgradeOptionValueCompareLine = roomTypeList.Where(p => p.DisplayName.ToUpper() == UpgradeOptionCompareLine.ToUpper()).Select(p => p.Value.Split(',')[1]).ToList().FirstOrDefault();
                                string UpgradeOptionValueCompareLine = roomTypeList.Where(p => p.Value.Split(',')[1].ToUpper() == UpgradeOptionCompareLine.ToUpper()).Select(p => p.Value.Split(',')[1]).ToList().FirstOrDefault();

                                if (!string.IsNullOrEmpty(HotelCodeCompareLine))
                                {
                                    // datetime convert to format of DateTime?
                                    DateTime? dtStartDate;
                                    if (string.IsNullOrEmpty(StartDate))
                                    {
                                        dtStartDate = null;
                                    }
                                    else
                                    {
                                        dtStartDate = Convert.ToDateTime(StartDate);
                                    }

                                    DateTime? dtEndDate;
                                    if (string.IsNullOrEmpty(EndDate))
                                    {
                                        dtEndDate = null;
                                    }
                                    else
                                    {
                                        dtEndDate = Convert.ToDateTime(EndDate);
                                    }

                                    DateTime? dtStartDateCompareLine;
                                    if (string.IsNullOrEmpty(StartDateCompareLine))
                                    {
                                        dtStartDateCompareLine = null;
                                    }
                                    else
                                    {
                                        dtStartDateCompareLine = Convert.ToDateTime(StartDateCompareLine);
                                    }

                                    DateTime? dtEndDateCompareLine;
                                    if (string.IsNullOrEmpty(EndDateCompareLine))
                                    {
                                        dtEndDateCompareLine = null;
                                    }
                                    else
                                    {
                                        dtEndDateCompareLine = Convert.ToDateTime(EndDateCompareLine);
                                    }


                                    List<string> RatetypeCompareLine = new List<string>();
                                    if (RateTypesCompareLine.ToUpper() != "ALL")
                                    {
                                        RatetypeCompareLine = RateTypesCompareLine.Split(';').ToList();
                                    }
                                    else
                                    {
                                        RatetypeCompareLine.Add(null);
                                    }

                                    #region  --foreach (var ratetypeCompareLine in RatetypeCompareLine)--
                                    foreach (var ratetypeCompareLine in RatetypeCompareLine)
                                    {
                                        //IsBussinessRuleValid
                                        if (UpgradesPricedByCompareLine != "All")
                                        {

                                            if ((HotelCode == HotelCodeCompareLine) && (bookedRoomValue == bookedRoomValueCompareLine)
                                          && (UpgradeOptionValue == UpgradeOptionValueCompareLine) && (UpgradesPricedByCompareLine == "Arrival Date")
                                                && (ratetype == ratetypeCompareLine) && (dtEndDate >= dtStartDateCompareLine && dtEndDateCompareLine >= dtStartDate))
                                            {
                                                if (!isNoteForBussinessRule)
                                                {
                                                    ListItemInt listItemIsBussinessRuleValidisNote = new ListItemInt();
                                                    listItemIsBussinessRuleValidisNote.Key = -5;
                                                    listItemIsBussinessRuleValidisNote.Value = i - 3;

                                                    returnlist.Add(listItemIsBussinessRuleValidisNote);

                                                    isNoteForBussinessRule = true;
                                                }

                                                //IsBussinessRuleValid for excel
                                                ListItemInt listItemIsBussinessRuleValidForExcel = new ListItemInt();
                                                listItemIsBussinessRuleValidForExcel.Key = -5;
                                                listItemIsBussinessRuleValidForExcel.Value = j - 4;

                                                returnlist.Add(listItemIsBussinessRuleValidForExcel);

                                            }

                                        }

                                        //IsExistingBussinessRule
                                        bool isDateAll = UpgradesPricedByCompareLine == "All";
                                        decimal cost = decimal.Parse(UpgradeCostCompareLine);
                                        DateTime start = (dtStartDateCompareLine ?? DateTime.Now).Date;
                                        DateTime end = (dtEndDateCompareLine ?? DateTime.Now).Date;
                                        if ((HotelCode == HotelCodeCompareLine) && (bookedRoomValue == bookedRoomValueCompareLine)
                                         && (UpgradeOptionValue == UpgradeOptionValueCompareLine) && (UpgradesPricedBy == UpgradesPricedByCompareLine)
                                         && (UpgradeCost == UpgradeCostCompareLine)
                                               && (ratetype == ratetypeCompareLine)
                                               && (isDateAll || (DateTime.Compare(dtStartDate ?? DateTime.Now, start) == 0 && DateTime.Compare(dtEndDate ?? DateTime.Now, end) == 0)))
                                        {
                                            if (!isNoteForExistingRule)
                                            {
                                                ListItemInt listItemIsExistingRuleValidisNote = new ListItemInt();
                                                listItemIsExistingRuleValidisNote.Key = -6;
                                                listItemIsExistingRuleValidisNote.Value = i - 3;

                                                returnlist.Add(listItemIsExistingRuleValidisNote);

                                                isNoteForExistingRule = true;
                                            }

                                            //IsExistingRuleValid for excel
                                            ListItemInt listItemIsExistingRuleForExcel = new ListItemInt();
                                            listItemIsExistingRuleForExcel.Key = -6;
                                            listItemIsExistingRuleForExcel.Value = j - 4;

                                            returnlist.Add(listItemIsExistingRuleForExcel);
                                        }
                                    }
                                    #endregion

                                }
                            }
                            #endregion
                        }

                        #endregion

                    }
                }

            }

            if (returnlist.Count > 0)
            {
                return returnlist;
            }

            #endregion


            #region dataaccess
            for (var i = 4; i <= Sheet.LastRowNum; i++)
            {

                var row = Sheet.GetRow(i);
                HotelCode = ExcelHelper.FormatCellValue(row.GetCell(1));
                BookedRoom = ExcelHelper.FormatCellValue(row.GetCell(2));
                UpgradesPricedBy = ExcelHelper.FormatCellValue(row.GetCell(3));
                StartDate = ExcelHelper.FormatCellValue(row.GetCell(4));
                EndDate = ExcelHelper.FormatCellValue(row.GetCell(5));
                UpgradeOption = ExcelHelper.FormatCellValue(row.GetCell(6));
                RateTypes = ExcelHelper.FormatCellValue(row.GetCell(7));
                UpgradeCost = ExcelHelper.FormatCellValue(row.GetCell(8));


                try
                {
                    if (!string.IsNullOrEmpty(HotelCode))
                    {
                        //get the value by displayName
                        IList<ListItemDTO> roomTypeList = roomTypeService.GetRoomTypeListByHotelCode(HotelCode);

                        //string bookedRoomValue = roomTypeList.Where(p => p.DisplayName.ToUpper() == BookedRoom.ToUpper()).Select(p => p.Value.Split(',')[1]).ToList().FirstOrDefault();
                        string bookedRoomValue = roomTypeList.Where(p => p.Value.Split(',')[1].ToUpper() == BookedRoom.ToUpper()).Select(p => p.Value.Split(',')[1]).ToList().FirstOrDefault();
                        //string UpgradeOptionValue = roomTypeList.Where(p => p.DisplayName.ToUpper() == UpgradeOption.ToUpper()).Select(p => p.Value.Split(',')[1]).ToList().FirstOrDefault();
                        string UpgradeOptionValue = roomTypeList.Where(p => p.Value.Split(',')[1].ToUpper() == UpgradeOption.ToUpper()).Select(p => p.Value.Split(',')[1]).ToList().FirstOrDefault();

                        //get correct hotelcode
                        string getHotelCode = hotellist.Where(p => p.Value.Split('_')[0].ToUpper() == HotelCode.ToUpper()).Select(p => p.Value.Split('_')[0]).ToList().FirstOrDefault();

                        //judge database whether exist selectedall ratetypes
                        NewBusinessRuleSaveDTO model = new NewBusinessRuleSaveDTO();
                        model.BookedRoomTypeCode = bookedRoomValue;
                        model.BusinessRuleID = 0;
                        if (!string.IsNullOrEmpty(EndDate))
                        {
                            model.EndDate = DateTime.Parse(EndDate);
                        }
                        model.HotelCode = getHotelCode;
                        model.RateTypeBooked = null;

                        if (!string.IsNullOrEmpty(StartDate))
                        {
                            model.StartDate = DateTime.Parse(StartDate);
                        }
                        model.UpgradeCost = UpgradeCost;
                        model.UpgradePricedBy = UpgradesPricedBy;
                        model.UpgradeRoomTypeCode = UpgradeOptionValue;
                        model.UpgradeWeekDayWeekEnd = null;
                        //model.UserName = User.Identity.Name;

                        if (!plannerEventService.IsExistingBussinessRule(model)) //exist no selectedall ratetypes,need to add to database
                        {
                            List<string> RatetypeAdd = new List<string>();


                            if (RateTypes.ToUpper() != "ALL")
                            {
                                RatetypeAdd = RateTypes.Split(';').ToList();
                            }
                            else
                            {
                                RatetypeAdd.Add(null);
                            }

                            //--------------------------------------------------
                            foreach (var ratetypeAdd in RatetypeAdd)
                            {
                                model.RateTypeBooked = ratetypeAdd;

                                PlannerEvent plannerEvent = NewBusinessRuleSaveDTOToPlannerEvent(model);

                                plannerEventRepo.Add(plannerEvent);
                                unitOfWork.Commit();

                                // add new, need to sync parent id with id
                                UpdateBusinessRuleParentID(plannerEvent.Hotel_Code, plannerEvent.RoomTypeCodeBooked, plannerEvent.RoomTypeCodeUpgrade);

                            }

                            successnum += 1;
                        }


                    }
                }
                catch (Exception dataaccessex)
                {
                    logger.Error("Exception: " + dataaccessex.ToString());

                    ListItemInt listItemExc = new ListItemInt();
                    listItemExc.Key = -2;
                    listItemExc.Value = 0;

                    returnlist.Add(listItemExc);

                    return returnlist;
                }

            }
            #endregion
            ListItemInt listItemSucc = new ListItemInt();
            listItemSucc.Key = 1;
            listItemSucc.Value = successnum;

            returnlist.Add(listItemSucc);
            return returnlist;
        }

        public PlannerEvent NewBusinessRuleSaveDTOToPlannerEvent(NewBusinessRuleSaveDTO data)
        {
            //get ratetype
            string ratetype = null;
            if (data.RateTypeBooked != null)
            {
                IList<ListItemDTO> rateTypeList = rateTypeService.GetRateTypeListByHotelCode(data.HotelCode);
                List<string> rateTypeDisplayNameList = rateTypeList.Select(p => p.DisplayName.ToUpper()).ToList();
                ratetype = rateTypeList.Where(p => p.DisplayName.ToUpper() == data.RateTypeBooked.ToUpper()).Select(p => p.DisplayName).ToList().FirstOrDefault();
            }

            PlannerEvent plannerEvent = new PlannerEvent();
            plannerEvent.ID = data.BusinessRuleID;
            plannerEvent.ParentID = plannerEvent.ID;
            plannerEvent.Hotel_Code = data.HotelCode;
            plannerEvent.RoomTypeCodeBooked = data.BookedRoomTypeCode;
            plannerEvent.RoomTypeCodeUpgrade = data.UpgradeRoomTypeCode;
            plannerEvent.RateTypeBooked = ratetype;
            plannerEvent.EventCategory = "RoomUpgrade";
            // Price and price display
            plannerEvent.USDPrice = Decimal.Parse(data.UpgradeCost);
            plannerEvent.Price = Decimal.Parse(data.UpgradeCost);
            plannerEvent.PriceDesc = Decimal.Parse(data.UpgradeCost).ToString();
            plannerEvent.DisplayPriceYN = true;
            // Time and Date
            plannerEvent.TimeStart = "0:00";
            plannerEvent.TimeEnd = "24:00";
            plannerEvent.TimeInterval = 30;
            plannerEvent.TimeDisplayType = 1;
            plannerEvent.InsertDate = DateTime.Now;
            plannerEvent.UpdateDate = DateTime.Now;

            plannerEvent.UpgradePricedBy = data.UpgradePricedBy;
            if (data.UpgradePricedBy == "Arrival Date")
            {
                plannerEvent.DateStart = data.StartDate;
                plannerEvent.DateEnd = data.EndDate;
            }

            plannerEvent.UpgradeWeekDayWeekEnd = data.UpgradeWeekDayWeekEnd;

            // non-nullable columns
            plannerEvent.languageID = 1;
            // TODO: EventDetailDesc needs to be defined
            plannerEvent.EventDetailDesc = "updated or new event";
            plannerEvent.ShowOnYN = true;
            plannerEvent.ActiveYN = true;
            plannerEvent.Seasonal = true;
            plannerEvent.reservationYN = true;
            plannerEvent.ContentYN = true;
            plannerEvent.DropDownYN = true;
            plannerEvent.SortOrder = 0;
            plannerEvent.SortOrder2 = 0;
            plannerEvent.RevenuePP = false;

            //groupid
            int groupid = plannerEventService.GetBussinessRuleGroupid(data, 0);
            if (groupid != 0)
            {
                if (data.RateTypeBooked == null)
                {
                    List<PlannerEvent> plannerEventlist = plannerEventRepo.GetAll().Where(p => p.Groupid == groupid).ToList();
                    foreach (var plannerEventitem in plannerEventlist)
                    {
                        plannerEventitem.Groupid = null;
                        plannerEventitem.ActiveYN = false;
                        plannerEventRepo.Update(plannerEventitem);
                        unitOfWork.Commit();
                        //succeed = true;
                    }
                    plannerEvent.Groupid = groupid;
                }
                else
                {
                    plannerEvent.Groupid = groupid;
                }
            }
            else
            {
                int maxgroupid = plannerEventService.GetBussinessRuleGroupid(data, 1);
                plannerEvent.Groupid = maxgroupid + 1;
            }

            return plannerEvent;
        }

        public void UpdateBusinessRuleParentID(string hotel_code, string room_code, string room_upgrade_code)
        {
            PlannerEvent plannerEvent = plannerEventRepo.Get(p => p.Hotel_Code.Equals(hotel_code)
                                                && p.RoomTypeCodeBooked.Equals(room_code)
                                                && p.RoomTypeCodeUpgrade.Equals(room_upgrade_code)
                                                && p.ParentID == 0);
            plannerEvent.ParentID = plannerEvent.ID;

            try
            {
                plannerEventRepo.Update(plannerEvent);
                unitOfWork.Commit();
            }
            catch (Exception e)
            {
                logger.Error("Exception: " + e.StackTrace);
            }
        }

        public bool IsNumber(string stringNumber)
        {
            try
            {
                Convert.ToDecimal(stringNumber);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

    }
}

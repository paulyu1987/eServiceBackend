//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cendyn.eConcierge.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class RoomType_Code
    {
        public int id { get; set; }
        public string Hotel_Code { get; set; }
        public string RoomCode { get; set; }
        public string RoomDescription { get; set; }
        public string RoomType { get; set; }
        public int LanguageId { get; set; }
        public string Image { get; set; }
        public Nullable<System.DateTime> InsertDate { get; set; }
        public Nullable<System.DateTime> Updatedate { get; set; }
        public string RoomLongDescription { get; set; }
        public string PriceDesc { get; set; }
        public string PerNightCharge { get; set; }
        public Nullable<short> TotalRoom { get; set; }
        public Nullable<short> Threshold { get; set; }
        public bool IsActive { get; set; }
        public string AddOnYN { get; set; }
        public string ImageYN { get; set; }
        public string UpgradeType { get; set; }
        public string PackageCode { get; set; }
    }
}
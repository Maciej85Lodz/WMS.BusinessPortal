using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Models.Invent
{
    public class ItemType: INetcoreBasic
    {
        public ItemType()
        {
            this.CreatedAt = DateTime.UtcNow;
            this.ItemTypeId = Guid.NewGuid().ToString();
        }

        [StringLength(38)]
        [Display(Name = "Item Type Id")]
        public string ItemTypeId { get; set; }

        [StringLength(50)]
        [Display(Name = "Item Code")]
        [Required]
        public string ItemCode { get; set; }

        [StringLength(50)]
        [Display(Name = "Item Type Name")]
        [Required]
        public string ItemTypeName { get; set; }

        [StringLength(50)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [StringLength(50)]
        [Display(Name = "Barcode")]
        public string Barcode { get; set; }

        [StringLength(50)]
        [Display(Name = "Manufacturer's Number")]
        public string ManufacturersNumber { get; set; }

        
        [Display(Name = "ItemType Type")]
        public ItemTypeType ItemTypeType { get; set; }

       
        [Display(Name = "Unit of Measure (UOM)")]
        public UOM UOM { get; set; }
    }
}

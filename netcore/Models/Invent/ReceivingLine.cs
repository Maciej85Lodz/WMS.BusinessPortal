using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Models.Invent
{
    public class ReceivingLine : INetcoreBasic
    {
        public ReceivingLine()
        {
            this.CreatedAt = DateTime.UtcNow;
            this.ReceivingLineId = Guid.NewGuid().ToString(); 
        }

        [StringLength(38)]
        [Display(Name = "Receiving Line Id")]
        public string ReceivingLineId { get; set; }

        [StringLength(38)]
        [Display(Name = "Receiving Id")]
        public string ReceivingId { get; set; }

        [Display(Name = "Receiving")]
        public Receiving Receiving { get; set; }

        [StringLength(38)]
        [Display(Name = "Branch Id")]
        public string BranchId { get; set; }

        [Display(Name = "Branch")]
        public Branch Branch { get; set; }

        [StringLength(38)]
        [Display(Name = "Warehouse Id")]
        public string WarehouseId { get; set; }

        [Display(Name = "Warehouse")]
        public Warehouse Warehouse { get; set; }

        [StringLength(38)]
        [Display(Name = "ItemType Id")]
        public string ItemTypeId { get; set; }

        [Display(Name = "ItemType")]
        public ItemType ItemType { get; set; }

        [Display(Name = "Qty Order")]
        public float QtyOrder { get; set; }

        [Display(Name = "Qty Receive")]
        public float QtyReceive { get; set; }

        [Display(Name = "Qty Inventory")]
        public float QtyInventory { get; set; }
    }
}

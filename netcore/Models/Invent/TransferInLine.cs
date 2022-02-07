using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Models.Invent
{
    public class TransferInLine : INetcoreBasic
    {
        public TransferInLine()
        {
            this.createdAt = DateTime.UtcNow;
            this.TransferInLineId = Guid.NewGuid().ToString();
        }

        [StringLength(38)]
        [Display(Name = "Transfer In Line Id")]
        public string TransferInLineId { get; set; }

        [StringLength(38)]
        [Display(Name = "Goods Receive Id")]
        public string TransferInId { get; set; }

        [Display(Name = "Goods Receive")]
        public TransferIn TransferIn { get; set; }

        [StringLength(38)]
        [Display(Name = "ItemType Id")]
        public string ItemTypeId { get; set; }

        [Display(Name = "ItemType")]
        public ItemType ItemType { get; set; }

        [Display(Name = "Qty")]
        public float Qty { get; set; }

        [Display(Name = "Qty Inventory")]
        public float QtyInventory { get; set; }
    }
}

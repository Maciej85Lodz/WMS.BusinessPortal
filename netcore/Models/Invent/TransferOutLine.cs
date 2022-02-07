using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Models.Invent
{
    public class TransferOutLine : INetcoreBasic
    {
        public TransferOutLine()
        {
            this.createdAt = DateTime.UtcNow;
            this.TransferOutLineId = Guid.NewGuid().ToString();
        }

        [StringLength(38)]
        [Display(Name = "Transfer Out Line Id")]
        public string TransferOutLineId { get; set; }

        [StringLength(38)]
        [Display(Name = "Goods Issue Id")]
        public string TransferOutId { get; set; }

        [Display(Name = "Goods Issue")]
        public TransferOut TransferOut { get; set; }

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

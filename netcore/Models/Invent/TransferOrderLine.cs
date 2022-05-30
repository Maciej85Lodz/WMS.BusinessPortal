using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Models.Invent
{
    public class TransferOrderLine : INetcoreBasic
    {
        public TransferOrderLine()
        {
            this.CreatedAt = DateTime.UtcNow;
            this.TransferOrderLineId = Guid.NewGuid().ToString();
        }

        [StringLength(38)]
        [Display(Name = "Transfer Order Line Id")]
        public string TransferOrderLineId { get; set; }

        [StringLength(38)]
        [Display(Name = "Transfer Order Id")]
        public string TransferOrderId { get; set; }

        [Display(Name = "Transfer Order")]
        public TransferOrder TransferOrder { get; set; }

        [StringLength(38)]
        [Display(Name = "ItemType Id")]
        public string ItemTypeId { get; set; }

        [Display(Name = "ItemType")]
        public ItemType ItemType { get; set; }

        [Display(Name = "Qty")]
        public float Qty { get; set; }
    }
}

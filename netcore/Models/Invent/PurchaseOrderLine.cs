using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Models.Invent
{
    public class PurchaseOrderLine : INetcoreBasic
    {
        public PurchaseOrderLine()
        {
            this.CreatedAt = DateTime.UtcNow;
            this.DiscountAmount = 0m;
            this.TotalAmount = 0m;
            this.PurchaseOrderLineId = Guid.NewGuid().ToString();
        }

        [StringLength(38)]
        [Display(Name = "Purchase Order Line Id")]
        public string PurchaseOrderLineId { get; set; }

        [StringLength(38)]
        [Display(Name = "Purchase Order Id")]
        public string PurchaseOrderId { get; set; }

        [Display(Name = "Purchase Order")]
        public PurchaseOrder PurchaseOrder { get; set; }

        [StringLength(38)]
        [Display(Name = "Item Type Id")]
        public string ItemTypeId { get; set; }

        [Display(Name = "ItemType")]
        public ItemType ItemType { get; set; }

        [Display(Name = "Quantity")]
        public float Quantity { get; set; }

        [Display(Name = "Item Price")]
        public decimal ItemPrice { get; set; }

        [Display(Name = "Discount Amount")]
        public decimal DiscountAmount { get; set; }

        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }
    }
}

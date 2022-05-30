using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Models.Invent
{
    public class SalesOrderLine : INetcoreBasic
    {
        public SalesOrderLine()
        {
            this.CreatedAt = DateTime.UtcNow;
            this.DiscountAmount = 0m;
            this.TotalAmount = 0m;
            this.SalesOrderLineId = Guid.NewGuid().ToString();
        }

        [StringLength(38)]
        [Display(Name = "Sales Order Line Id")]
        public string SalesOrderLineId { get; set; }

        [StringLength(38)]
        [Display(Name = "Sales Order Id")]
        public string SalesOrderId { get; set; }

        [Display(Name = "Sales Order")]
        public SalesOrder SalesOrder { get; set; }

        [StringLength(38)]
        [Display(Name = "ItemType Id")]
        public string ItemTypeId { get; set; }

        [Display(Name = "ItemType")]
        public ItemType ItemType { get; set; }

        [Display(Name = "Qty")]
        public float Qty { get; set; }

        [Display(Name = "Item Price")]
        public decimal Price { get; set; }

        [Display(Name = "Discount Amount")]
        public decimal DiscountAmount { get; set; }

        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }
    }
}

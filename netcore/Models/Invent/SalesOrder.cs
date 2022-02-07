using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Models.Invent
{
    public class SalesOrder : INetcoreMasterChild
    {
        public SalesOrder()
        {
            this.createdAt = DateTime.UtcNow;
            this.SalesOrderNumber = DateTime.UtcNow.Date.ToString("yyyyMMdd") + Guid.NewGuid().ToString().Substring(0, 5).ToUpper() + "#SO";
            this.SalesOrderDate = DateTime.UtcNow.Date;
            this.DeliveryDate = this.SalesOrderDate.AddDays(5);
            this.SalesOrderStatus = SalesOrderStatus.Draft;
            this.TotalDiscountAmount = 0m;
            this.TotalOrderAmount = 0m;
            this.SalesOrderId = Guid.NewGuid().ToString();
        }

        [StringLength(38)]
        [Display(Name = "Sales Order Id")]
        public string SalesOrderId { get; set; }

        [StringLength(20)]
        [Required]
        [Display(Name = "SO Number")]
        public string SalesOrderNumber { get; set; }

        [Display(Name = "Terms of Payment (TOP)")]
        public TOP Top { get; set; }

        [Display(Name = "SO Date")]
        public DateTime SalesOrderDate { get; set; }

        [Display(Name = "Delivery Date")]
        public DateTime DeliveryDate { get; set; }

        [StringLength(50)]
        [Display(Name = "Delivery Address")]
        public string DeliveryAddress { get; set; }

        [StringLength(30)]
        [Display(Name = "Reference Number (Internal)")]
        public string ReferenceNumberInternal { get; set; }

        [StringLength(30)]
        [Display(Name = "Reference Number (External)")]
        public string ReferenceNumberExternal { get; set; }

        [StringLength(100)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [StringLength(38)]
        [Required]
        [Display(Name = "Branch Id")]
        public string BranchId { get; set; }

        [Display(Name = "Branch")]
        public Branch Branch { get; set; }

        [StringLength(38)]
        [Required]
        [Display(Name = "Customer Id")]
        public string CustomerId { get; set; }

        [Display(Name = "Customer")]
        public Customer Customer { get; set; }

        [StringLength(30)]
        [Required]
        [Display(Name = "PIC Internal")]
        public string PicInternal { get; set; }

        [StringLength(30)]
        [Required]
        [Display(Name = "PIC Customer")]
        public string PicCustomer { get; set; }

        [Display(Name = "SO Status")]
        public SalesOrderStatus SalesOrderStatus { get; set; }

        [Display(Name = "Total Discount")]
        public decimal TotalDiscountAmount { get; set; }

        [Display(Name = "Total Order")]
        public decimal TotalOrderAmount { get; set; }

        [Display(Name = "Sales Shipment Number")]
        public string SalesShipmentNumber { get; set; }

        [Display(Name = "Sales Order Lines")]
        public List<SalesOrderLine> SalesOrderLine { get; set; } = new List<SalesOrderLine>();
    }
}

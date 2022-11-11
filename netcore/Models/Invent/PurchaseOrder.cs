using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Models.Invent
{
    public class PurchaseOrder : INetcoreMasterChild
    {
        public PurchaseOrder()
        {
            this.CreatedAt = DateTime.UtcNow.Date;
            this.PurchaseOrderNumber = DateTime.UtcNow.Date.ToString("dd/mm/yyyy") + Guid.NewGuid().ToString().Substring(0,6).ToUpper() + "#PO";
            this.PurchaseOrderDate = DateTime.UtcNow.Date;
            this.DeliveryDate = this.PurchaseOrderDate.Date;
            this.PurchaseOrderStatus = PurchaseOrderStatus.Draft;
            this.TotalDiscountAmount = 0m;
            this.TotalOrderAmount = 0m;
            this.PurchaseOrderId = Guid.NewGuid().ToString(); 
        }

        [StringLength(38)]
        [Display(Name = "Purchase Order Id")]
        public string PurchaseOrderId { get; set; }

        [StringLength(20)]
        [Required]
        [Display(Name = "Purchase Order Number")]
        public string PurchaseOrderNumber { get; set; }
        
        [Display(Name = "Terms of Payment (TOP)")]
        public TOP Top { get; set; }

        [Display(Name = "Purchase Order Date")]
        public DateTime PurchaseOrderDate { get; set; }

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
        [Display(Name = "Vendor Id")]
        public string VendorId { get; set; }

        [Display(Name = "Vendor")]
        public Vendor Vendor { get; set; }

        [StringLength(30)]
        [Required]
        [Display(Name = "PIC Internal")]
        public string PicInternal { get; set; }

        [StringLength(30)]
        [Required]
        [Display(Name = "PIC Vendor")]
        public string PicVendor { get; set; }

        [Display(Name = "PO Status")]
        public PurchaseOrderStatus PurchaseOrderStatus { get; set; }

        [Display(Name = "Total Discount")]
        public decimal TotalDiscountAmount { get; set; }

        [Display(Name = "Total Order")]
        public decimal TotalOrderAmount { get; set; }

        [Display(Name = "Purchase Receive Number")]
        public string PurchaseReceiveNumber { get; set; }

        [Display(Name = "Purchase Order Lines")]
        public List<PurchaseOrderLine> PurchaseOrderLine { get; set; } = new List<PurchaseOrderLine>();
    }
}

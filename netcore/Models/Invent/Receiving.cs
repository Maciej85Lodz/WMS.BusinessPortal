using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Models.Invent
{
    public class Receiving : INetcoreMasterChild
    {
        public Receiving()
        {
            this.CreatedAt = DateTime.UtcNow;
            this.ReceivingNumber = DateTime.UtcNow.Date.ToString("yyyyMMdd") + Guid.NewGuid().ToString().Substring(0, 5).ToUpper() + "#GSRN";
            this.ReceivingDate = DateTime.UtcNow;
            this.ReceivingId = Guid.NewGuid().ToString();
        }

        [StringLength(38)]
        [Display(Name = "Receiving Id")]
        public string ReceivingId { get; set; }

        [StringLength(38)]
        [Required]
        [Display(Name = "Purchase Order Id")]
        public string PurchaseOrderId { get; set; }

        [Display(Name = "Purchase Order")]
        public PurchaseOrder PurchaseOrder { get; set; }

        [StringLength(20)]
        [Required]
        [Display(Name = "GSRN Number")]
        public string ReceivingNumber { get; set; }
        
        [Required]
        [Display(Name = "Receving Date")]
        public DateTime ReceivingDate { get; set; }

        [StringLength(38)]
        [Display(Name = "Vendor Id")]
        public string VendorId { get; set; }

        [Display(Name = "Vendor")]
        public Vendor Vendor { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Vendor DO Number")]
        public string VendorDONumber { get; set; }

        [StringLength(50)]
        [Display(Name = "Vendor Invoice Number")]
        public string VendorInvoice { get; set; }

        [StringLength(38)]
        [Display(Name = "Branch Id")]
        public string BranchId { get; set; }

        [Display(Name = "Branch")]
        public Branch Branch { get; set; }

        [StringLength(38)]
        [Required]
        [Display(Name = "Warehouse Id")]
        public string WarehouseId { get; set; }

        [Display(Name = "Warehouse")]
        public Warehouse Warehouse { get; set; }

        [Display(Name = "Receiving Lines")]
        public List<ReceivingLine> ReceivingLine { get; set; } = new List<ReceivingLine>();
    }
}

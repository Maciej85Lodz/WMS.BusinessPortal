using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Models.Invent
{
    public class Shipment : INetcoreMasterChild
    {
        public Shipment()
        {
            this.createdAt = DateTime.UtcNow;
            this.ShipmentNumber = DateTime.UtcNow.Date.ToString("yyyyMMdd") + Guid.NewGuid().ToString().Substring(0, 5).ToUpper() + "#DO";
            this.ShipmentDate = DateTime.UtcNow;
            this.ExpeditionType = ExpeditionType.Internal;
            this.ExpeditionMode = ExpeditionMode.Land;
            this.ShipmentId = Guid.NewGuid().ToString();
        }

        [StringLength(38)]
        [Display(Name = "Shipment Id")]
        public string ShipmentId { get; set; }

        [StringLength(38)]
        [Required]
        [Display(Name = "Sales Order Id")]
        public string SalesOrderId { get; set; }

        [Display(Name = "Sales Order")]
        public SalesOrder SalesOrder { get; set; }

        [StringLength(20)]
        [Required]
        [Display(Name = "DO Number")]
        public string ShipmentNumber { get; set; }

        [Required]
        [Display(Name = "DO Date")]
        public DateTime ShipmentDate { get; set; }

        [StringLength(38)]
        [Display(Name = "Customer Id")]
        public string CustomerId { get; set; }

        [Display(Name = "Customer")]
        public Customer Customer { get; set; }

        [StringLength(50)]
        [Display(Name = "Customer PO Number")]
        public string CustomerPurchaseOrder { get; set; }

        [StringLength(50)]
        [Display(Name = "Invoice Number")]
        public string InvoiceNumber { get; set; }

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

        [Display(Name = "Expedition Type")]
        public ExpeditionType ExpeditionType { get; set; }

        [Display(Name = "Expedition Mode")]
        public ExpeditionMode ExpeditionMode { get; set; }

        [Display(Name = "Shipment Lines")]
        public List<ShipmentLine> ShipmentLine { get; set; } = new List<ShipmentLine>();
    }
}

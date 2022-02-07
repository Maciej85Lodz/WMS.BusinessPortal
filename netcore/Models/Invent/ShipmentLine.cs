using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Models.Invent
{
    public class ShipmentLine : INetcoreBasic
    {
        public ShipmentLine()
        {
            this.createdAt = DateTime.UtcNow;
            this.ShipmentLineId = Guid.NewGuid().ToString();
        }

        [StringLength(38)]
        [Display(Name = "Shipment Line Id")]
        public string ShipmentLineId { get; set; }

        [StringLength(38)]
        [Display(Name = "Shipment Id")]
        public string ShipmentId { get; set; }

        [Display(Name = "Shipment")]
        public Shipment Shipment { get; set; }

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

        [Display(Name = "Qty")]
        public float Qty { get; set; }

        [Display(Name = "Qty Shipment")]
        public float QtyShipment { get; set; }

        [Display(Name = "Qty Inventory")]
        public float QtyInventory { get; set; }
    }
}

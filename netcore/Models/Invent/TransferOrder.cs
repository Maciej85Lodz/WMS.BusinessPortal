using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Models.Invent
{
    public class TransferOrder : INetcoreMasterChild
    {
        public TransferOrder()
        {
            this.CreatedAt = DateTime.UtcNow;
            this.TransferOrderNumber = DateTime.UtcNow.Date.ToString("yyyyMMdd") + Guid.NewGuid().ToString().Substring(0, 5).ToUpper() + "#TO";
            this.TransferOrderDate = DateTime.UtcNow;
            this.TransferOrderStatus = TransferOrderStatus.Draft;
            this.IsIssued = false;
            this.IsReceived = false;
            this.TransferOrderId = Guid.NewGuid().ToString();
        }

        [StringLength(38)]
        [Display(Name = "Transfer Id")]
        public string TransferOrderId { get; set; }

        [StringLength(20)]
        [Required]
        [Display(Name = "Transfer Number")]
        public string TransferOrderNumber { get; set; }

        [Required]
        [Display(Name = "Transfer Date")]
        public DateTime TransferOrderDate { get; set; }

        [StringLength(100)]
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "PIC Name")]
        public string PicName { get; set; }

        [StringLength(38)]
        [Display(Name = "From Branch")]
        public string BranchIdFrom { get; set; }

        [Display(Name = "From Branch")]
        public Branch BranchFrom { get; set; }

        [StringLength(38)]
        [Display(Name = "From Warehouse")]
        public string WarehouseIdFrom { get; set; }

        [Display(Name = "From Warehouse")]
        public Warehouse WarehouseFrom { get; set; }

        [StringLength(38)]
        [Display(Name = "To Branch")]
        public string BranchIdTo { get; set; }

        [Display(Name = "To Branch")]
        public Branch BranchTo { get; set; }

        [StringLength(38)]
        [Display(Name = "To Warehouse")]
        public string WarehouseIdTo { get; set; }

        [Display(Name = "To Warehouse")]
        public Warehouse WarehouseTo { get; set; }

        [Display(Name = "Transfer Order Status")]
        public TransferOrderStatus TransferOrderStatus { get; set; }

        [Display(Name = "Is Issued")]
        public bool IsIssued { get; set; }

        [Display(Name = "Is Received")]
        public bool IsReceived { get; set; }

        [Display(Name = "Transfer Order Lines")]
        public List<TransferOrderLine> TransferOrderLine { get; set; } = new List<TransferOrderLine>();
    }
}

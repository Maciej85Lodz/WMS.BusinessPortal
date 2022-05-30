using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Models.Invent
{
    public class TransferOut : INetcoreMasterChild
    {
        public TransferOut()
        {
            this.CreatedAt = DateTime.UtcNow;
            this.TransferOutNumber = DateTime.UtcNow.Date.ToString("yyyyMMdd") + Guid.NewGuid().ToString().Substring(0, 5).ToUpper() + "#OUT";
            this.TransferOutDate = DateTime.UtcNow;
            this.TransferOutId = Guid.NewGuid().ToString();
        }

        [StringLength(38)]
        [Display(Name = "Transfer Out Id")]
        public string TransferOutId { get; set; }

        [StringLength(38)]
        [Required]
        [Display(Name = "Transfer Order Id")]
        public string TransferOrderId { get; set; }

        [Display(Name = "Transfer Order")]
        public TransferOrder TransferOrder { get; set; }

        [StringLength(20)]
        [Required]
        [Display(Name = "Goods Issue Number")]
        public string TransferOutNumber { get; set; }

        [Required]
        [Display(Name = "Goods Issue Date")]
        public DateTime TransferOutDate { get; set; }

        [StringLength(100)]
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        

        [StringLength(38)]
        [Display(Name = "From Branch Id")]
        public string BranchIdFrom { get; set; }

        [Display(Name = "From Branch")]
        public Branch BranchFrom { get; set; }

        [StringLength(38)]
        [Display(Name = "From Warehouse Id")]
        public string WarehouseIdFrom { get; set; }

        [Display(Name = "From Warehouse")]
        public Warehouse WarehouseFrom { get; set; }

        [StringLength(38)]
        [Display(Name = "To Branch Id")]
        public string BranchIdTo { get; set; }

        [Display(Name = "To Branch")]
        public Branch BranchTo { get; set; }

        [StringLength(38)]
        [Display(Name = "To Warehouse Id")]
        public string WarehouseIdTo { get; set; }

        [Display(Name = "To Warehouse")]
        public Warehouse WarehouseTo { get; set; }

        [Display(Name = "Transfer Out Lines")]
        public List<TransferOutLine> TransferOutLine { get; set; } = new List<TransferOutLine>();
    }
}

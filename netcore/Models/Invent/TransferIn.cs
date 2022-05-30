using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WMS.Models.Invent
{
    public class TransferIn : INetcoreMasterChild
    {
        public TransferIn()
        {
            this.CreatedAt = DateTime.UtcNow;
            this.TransferInNumber = DateTime.UtcNow.Date.ToString("yyyyMMdd") + Guid.NewGuid().ToString().Substring(0, 5).ToUpper() + "#IN";
            this.TransferInDate = DateTime.UtcNow;
            this.TansferInId = Guid.NewGuid().ToString();
        }

        [StringLength(38)]
        [Display(Name = "Transfer In Id")]
        [Key]
        public string TansferInId { get; set; }

        [StringLength(38)]
        [Required]
        [Display(Name = "Transfer Order Id")]
        public string TransferOrderId { get; set; }

        [Display(Name = "Transfer Order")]
        public TransferOrder TransferOrder { get; set; }

        [StringLength(20)]
        [Required]
        [Display(Name = "Goods Receive Number")]
        public string TransferInNumber { get; set; }

        [Required]
        [Display(Name = "Goods Receive Date")]
        public DateTime TransferInDate { get; set; }

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

        [Display(Name = "Transfer In Lines")]
        public List<TransferInLine> TransferInLine { get; set; } = new List<TransferInLine>();
    }
}

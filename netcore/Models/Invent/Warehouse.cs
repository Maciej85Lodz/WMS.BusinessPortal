using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Models.Invent
{
    public class Warehouse: INetcoreBasic, IBaseAddress
    {
        public Warehouse()
        {
            this.CreatedAt = DateTime.UtcNow;
            this.WarehouseId = Guid.NewGuid().ToString();
        }

        [StringLength(38)]
        [Display(Name = "Warehouse Id")]
        public string WarehouseId { get; set; }

        [StringLength(50)]
        [Display(Name = "Warehouse Name")]
        [Required]
        public string WarehouseName { get; set; }

        [StringLength(50)]
        [Display(Name = "Warehouse Description")]
        public string Description { get; set; }

        [StringLength(38)]
        [Display(Name = "Branch Id")]
        public string BranchId { get; set; }
        
        [Display(Name = "Branch")]
        public Branch Branch { get; set; }

        //IBaseAddress
        [Display(Name = "Street Address 1")]
        [Required]
        [StringLength(50)]
        public string Street1 { get; set; }

        [Display(Name = "Street Address 2")]
        [StringLength(50)]
        public string Street2 { get; set; }

        [Display(Name = "City")]
        [StringLength(30)]
        public string City { get; set; }

        [Display(Name = "Province")]
        [StringLength(30)]
        public string Province { get; set; }

        [Display(Name = "Country")]
        [StringLength(30)]
        public string Country { get; set; }
        //IBaseAddress
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Models.Invent
{
    public class Branch: INetcoreBasic, IBaseAddress
    {
        public Branch()
        {
            this.CreatedAt = DateTime.UtcNow;
            this.IsDefaultBranch = false;
            this.BranchId = Guid.NewGuid().ToString();
        }

        [StringLength(38)]
        [Display(Name = "Branch Id")]
        public string BranchId { get; set; }

        [StringLength(50)]
        [Display(Name = "Branch Name")]
        [Required]
        public string BranchName { get; set; }

        [StringLength(50)]
        [Display(Name = "Branch Description")]
        public string Description { get; set; }

        [Display(Name = "Is Default Branch ?")]
        public bool IsDefaultBranch { get; set; } = false;

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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Models.Invent
{
    public class Vendor : INetcoreMasterChild, IBaseAddress
    {
        public Vendor()
        {
            this.CreatedAt = DateTime.UtcNow;
            this.VendorId = Guid.NewGuid().ToString();
        }

        [StringLength(38)]
        [Display(Name = "Vendor Id")]
        public string VendorId { get; set; }

        [StringLength(50)]
        [Display(Name = "Vendor Name")]
        [Required]
        public string VendorName { get; set; }

        [StringLength(50)]
        [Display(Name = "Description")]
        public string Description { get; set; }
        
        [Display(Name = "Business Size")]
        public BusinessSize Size { get; set; }

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

        [Display(Name = "Vendor Contacts")]
        public List<VendorLine> VendorLine { get; set; } = new List<VendorLine>();
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Models.Invent
{
    public interface IBaseAddress
    {
        [Display(Name = "Street Address 1")]
        [Required]
        [StringLength(50)]
        string Street1 { get; set; }

        [Display(Name = "Street Address 2")]
        [StringLength(50)]
        string Street2 { get; set; }

        [Display(Name = "City")]
        [StringLength(30)]
        string City { get; set; }

        [Display(Name = "Province")]
        [StringLength(30)]
        string Province { get; set; }

        [Display(Name = "Country")]
        [StringLength(30)]
        string Country { get; set; }
    }
}

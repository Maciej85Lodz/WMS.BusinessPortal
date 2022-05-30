using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Models.Invent
{
    public interface IBaseCommunication
    {
        [Display(Name = "Mobile Phone")]
        [StringLength(20)]
        string MobilePhone { get; set; }

        [Display(Name = "Office Phone")]
        [StringLength(20)]
        string OfficePhone { get; set; }

        [Display(Name = "Fax")]
        [StringLength(20)]
        string Fax { get; set; }

        [Display(Name = "Personal Email")]
        [StringLength(50)]
        string PersonalEmail { get; set; }

        [Display(Name = "Work Email")]
        [StringLength(50)]
        string WorkEmail { get; set; }

    }
}

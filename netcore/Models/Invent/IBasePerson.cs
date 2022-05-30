using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Models.Invent
{
    public interface IBasePerson
    {
        [Display(Name = "First Name")]
        [Required]
        [StringLength(20)]
        string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        [StringLength(20)]
        string LastName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(20)]
        string MiddleName { get; set; }

        [Display(Name = "Nick Name")]
        [StringLength(20)]
        string NickName { get; set; }

        [Display(Name = "Gender")]
        Gender Gender { get; set; }

        [Display(Name = "Salutation")]
        Salutation Salutation { get; set; }
    }


}

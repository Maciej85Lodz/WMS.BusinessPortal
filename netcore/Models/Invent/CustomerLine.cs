using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Models.Invent
{
    public class CustomerLine : INetcoreBasic, IBasePerson, IBaseCommunication
    {
        public CustomerLine()
        {
            this.CreatedAt = DateTime.UtcNow;
            this.CustomerLineId = Guid.NewGuid().ToString();
            this.Salutation = Salutation.Mr;
            this.Gender = Gender.Female;
        }

        [StringLength(38)]
        [Display(Name = "Customer Line Id")]
        public string CustomerLineId { get; set; }

        [StringLength(20)]
        [Display(Name = "Job Title")]
        [Required]
        public string JobTitle { get; set; }

        [StringLength(38)]
        [Display(Name = "Customer Id")]
        public string CustomerId { get; set; }

        [Display(Name = "Customer")]
        public Customer Customer { get; set; }

        //IBasePerson
        [Display(Name = "First Name")]
        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        [StringLength(20)]
        public string LastName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(20)]
        public string MiddleName { get; set; }

        [Display(Name = "Nick Name")]
        [StringLength(20)]
        public string NickName { get; set; }

        [Display(Name = "Gender")]
        public Gender Gender { get; set; }

        [Display(Name = "Salutation")]
        public Salutation Salutation { get; set; }
        //IBasePerson

        //IBaseCommunication
        [Display(Name = "Mobile Phone")]
        [StringLength(20)]
        public string MobilePhone { get; set; }

        [Display(Name = "Office Phone")]
        [StringLength(20)]
        public string OfficePhone { get; set; }

        [Display(Name = "Fax")]
        [StringLength(20)]
        public string Fax { get; set; }

        [Display(Name = "Personal Email")]
        [StringLength(50)]
        public string PersonalEmail { get; set; }

        [Display(Name = "Work Email")]
        [StringLength(50)]
        public string WorkEmail { get; set; }
        //IBaseCommunication
    }
}

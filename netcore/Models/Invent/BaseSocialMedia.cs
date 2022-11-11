using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Models.Invent
{
    public class BaseSocialMedia
    {
        [Display(Name = "Blog")]
        [StringLength(100)]
        public string Blog { get; set; }

        [Display(Name = "Website")]
        [StringLength(100)]
        public string Website { get; set; }

        [Display(Name = "Linkedin")]
        [StringLength(100)]
        public string Linkedin { get; set; }
    }
}

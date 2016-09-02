using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButiqueShops.ViewModels
{
    public class SizesViewModel
    {

        public int Id { get; set; }
        [Required]
        [DisplayName("Full Name")]
        public string FullName { get; set; }
        [Required]
        [DisplayName("Short Name")]
        public string ShortName { get; set; }
    }
}

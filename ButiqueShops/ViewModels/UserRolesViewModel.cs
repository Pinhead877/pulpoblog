using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButiqueShops.ViewModels
{
    public class UserRolesViewModel
    {
        [Display(Name = "User Name")]
        [Required]
        [MinLength(1)]
        public string UserId { get; set; }
        [Display(Name ="Role Name")]
        [Required]
        [MinLength(1)]
        public string RoleId { get; set; }
    }
}

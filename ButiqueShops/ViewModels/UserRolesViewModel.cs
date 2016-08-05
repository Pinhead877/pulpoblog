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
        public string UserId { get; set; }
        [Display(Name ="Role Name")]
        public string RoleId { get; set; }
    }
}

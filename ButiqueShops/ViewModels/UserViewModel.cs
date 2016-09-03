using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButiqueShops.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [DisplayName("e-Mail")]
        public string Email { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime Birthday { get; set; }
        public Nullable<int> AddressId { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        [DisplayName("User Name")]
        public string UserName { get; set; }
        public string Gender { get; set; }

        public virtual List<RolesViewModel> Roles { get; set; }
    }
}

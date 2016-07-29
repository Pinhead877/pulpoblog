using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButiqueShops.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public System.DateTime Birthday { get; set; }
        public Nullable<int> AddressId { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }

        public virtual List<RolesViewModel> Roles { get; set; }
    }
}

using AutoMapper;
using ButiqueShops.Models;
using ButiqueShops.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButiqueShops
{
    /// <summary>
    /// Class of the third party packge - auto mapper
    /// for more info:
    /// https://github.com/AutoMapper/AutoMapper/wiki/Getting-started
    /// </summary>
    public static class AutoMapperConfig
    {
        public static void Config()
        {
            Mapper.Initialize(mapper => {

                mapper.CreateMap<Shops, ShopViewModel>();
                mapper.CreateMap<ShopViewModel, Shops>();

                mapper.CreateMap<AspNetRoles, RolesViewModel>();
                mapper.CreateMap<RolesViewModel, AspNetRoles> ();

                mapper.CreateMap<AspNetUsers, UserViewModel>().ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.AspNetRoles));
                mapper.CreateMap<UserViewModel, AspNetUsers>().ForMember(dest => dest.AspNetRoles, opt => opt.MapFrom(src => src.Roles));

                mapper.CreateMap<Items, ItemsViewModel>();
                mapper.CreateMap<ItemsViewModel, Items>();

                mapper.CreateMap<ItemsToSubmit, ItemsToSubmitViewModel>();
                mapper.CreateMap<ItemsToSubmitViewModel, ItemsToSubmit>();

                mapper.CreateMap<ItemsToSubmit, Items>().ForMember(dest => dest.DateAdded, opt => opt.UseValue(DateTime.Now));

                mapper.CreateMap<Sizes, SizesViewModel>();
                mapper.CreateMap<SizesViewModel, Sizes>();
            });
        }
    }
}

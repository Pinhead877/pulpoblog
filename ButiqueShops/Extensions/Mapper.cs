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
    public static class AutoMapperConfig
    {
        public static void Config()
        {
            Mapper.Initialize(mapper => {

                mapper.CreateMap<Shops, ShopViewModel>();
                mapper.CreateMap<ShopViewModel, Shops>();

                mapper.CreateMap<AspNetRoles, RolesViewModel>();
                mapper.CreateMap<RolesViewModel, AspNetRoles> ();

                mapper.CreateMap<AspNetUsers, UserViewModel>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.AspNetRoles));
                mapper.CreateMap<UserViewModel, AspNetUsers>()
                .ForMember(dest => dest.AspNetRoles, opt => opt.MapFrom(src => src.Roles));


            });
            
        }
    }
}

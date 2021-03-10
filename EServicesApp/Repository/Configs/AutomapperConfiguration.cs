using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Data.Models;
using Repository.DTOModel;

namespace Repository.Configs
{
    public class AutomapperConfiguration
    {
        public static void IntializeAutomapper()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<User, UserDto>();
                config.CreateMap<Role,RoleDto>();
                config.CreateMap<UserRole,UserRoleDto>();
                config.CreateMap<AddressDto, AddressDto>();
            });
        }
    }
}

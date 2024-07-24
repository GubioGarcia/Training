using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.ViewModels;
using Training.Domain.Entities;

namespace Training.Application.AutoMapper
{
    public class AutoMapperSetup : Profile
    {
        public AutoMapperSetup() 
        {
            #region ViewModelToDomain

            CreateMap<UsersTypeViewModel, UsersType>();

            #endregion

            #region DomainToViewModel

            CreateMap<UsersType, UsersTypeViewModel>();

            #endregion
        }
    }
}

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
            CreateMap<ProfessionalTypeViewModel, ProfessionalType>();
            CreateMap<ProfessionalViewModel, Professional>();

            #endregion

            #region DomainToViewModel

            CreateMap<UsersType, UsersTypeViewModel>();
            CreateMap<ProfessionalType, ProfessionalTypeViewModel>();
            CreateMap<Professional, ProfessionalViewModel>();

            #endregion
        }
    }
}

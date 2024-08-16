using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.ViewModels;
using Training.Application.ViewModels.ClientViewModels;
using Training.Application.ViewModels.ProfessionalViewModels;
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
            CreateMap<ClientMinimalFieldViewModel, Client>();
            CreateMap<ClientResponseViewModel, Client>();

            #endregion

            #region DomainToViewModel

            CreateMap<UsersType, UsersTypeViewModel>();
            CreateMap<ProfessionalType, ProfessionalTypeViewModel>();
            CreateMap<Professional, ProfessionalViewModel>();
            CreateMap<Client, ClientMinimalFieldViewModel>();
            CreateMap<Client, ClientResponseViewModel>();

            #endregion
        }
    }
}

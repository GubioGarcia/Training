using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.ViewModels;
using Training.Application.ViewModels.ClientProfessionalViewModels;
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

            CreateMap<ProfessionalMinimalFieldViewModel, Professional>();
            CreateMap<ProfessionalResponseViewModel, Professional>();
            CreateMap<ProfessionalRequestViewModel, Professional>();
            CreateMap<ProfessionalRequestUpdateViewModel, Professional>();
            
            CreateMap<ClientMinimalFieldViewModel, Client>();
            CreateMap<ClientResponseViewModel, Client>();
            CreateMap<ClientRequestViewModel, Client>();
            CreateMap<ClientRequestUpdateViewModel, Client>();

            CreateMap<ClientProfessionalViewModel, ClientProfessional>();
            CreateMap<ClientProfessionalRequestViewModel, ClientProfessional>();
            CreateMap<ClientProfessionalRequestUpdateViewModel, ClientProfessional>();

            #endregion

            #region DomainToViewModel

            CreateMap<UsersType, UsersTypeViewModel>();
            CreateMap<ProfessionalType, ProfessionalTypeViewModel>();

            CreateMap<Professional, ProfessionalMinimalFieldViewModel>();
            CreateMap<Professional, ProfessionalResponseViewModel>();
            CreateMap<Professional, ProfessionalRequestViewModel>();
            CreateMap<Professional, ProfessionalRequestUpdateViewModel>();
            
            CreateMap<Client, ClientMinimalFieldViewModel>();
            CreateMap<Client, ClientResponseViewModel>();
            CreateMap<Client, ClientRequestViewModel>();
            CreateMap<Client, ClientRequestUpdateViewModel>();

            CreateMap<ClientProfessional, ClientProfessionalViewModel>();
            CreateMap<ClientProfessional, ClientProfessionalRequestViewModel>();
            CreateMap<ClientProfessional, ClientProfessionalRequestUpdateViewModel>();

            #endregion
        }
    }
}

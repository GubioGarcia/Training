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
            CreateMap<ClientRequestViewModel, Client>();
            CreateMap<ClientUpdateRequestViewModel, Client>()
                    .ForMember(dest => dest.Cpf, opt => opt.Condition(src => src.Cpf != null))
                    .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
                    .ForMember(dest => dest.Password, opt => opt.Condition(src => src.Password != null))
                    .ForMember(dest => dest.Fone, opt => opt.Condition(src => src.Fone != null))
                    .ForMember(dest => dest.DateBirth, opt => opt.Condition(src => src.DateBirth.HasValue))
                    .ForMember(dest => dest.InitialObjective, opt => opt.Condition(src => src.InitialObjective != null))
                    .ForMember(dest => dest.Height, opt => opt.Condition(src => src.Height.HasValue))
                    .ForMember(dest => dest.StartingWeight, opt => opt.Condition(src => src.StartingWeight.HasValue))
                    .ForMember(dest => dest.CurrentWeight, opt => opt.Condition(src => src.CurrentWeight.HasValue))
                    .ForMember(dest => dest.UrlProfilePhoto, opt => opt.Condition(src => src.UrlProfilePhoto != null))
                    .ForMember(dest => dest.IsActive, opt => opt.Condition(src => src.IsActive.HasValue));


            #endregion

            #region DomainToViewModel

            CreateMap<UsersType, UsersTypeViewModel>();
            CreateMap<ProfessionalType, ProfessionalTypeViewModel>();
            CreateMap<Professional, ProfessionalViewModel>();
            CreateMap<Client, ClientMinimalFieldViewModel>();
            CreateMap<Client, ClientResponseViewModel>();
            CreateMap<Client, ClientRequestViewModel>();
            CreateMap<Client, ClientUpdateRequestViewModel>();

            #endregion
        }
    }
}

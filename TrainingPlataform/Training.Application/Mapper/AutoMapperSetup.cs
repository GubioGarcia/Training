using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Application.ViewModels;
using Training.Application.ViewModels.ClientProfessionalViewModels;
using Training.Application.ViewModels.ClientViewModels;
using Training.Application.ViewModels.ExerciseViewModels;
using Training.Application.ViewModels.MuscleGroupViewModels;
using Training.Application.ViewModels.ProfessionalViewModels;
using Training.Application.ViewModels.WorkoutCategoryViewModels;
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
            CreateMap<MuscleGroupViewModel, MuscleGroup>();
            CreateMap<WorkoutCategoryViewModel, WorkoutCategory>();
            CreateMap<ExerciseViewModel, Exercise>();
            CreateMap<TrainingViewModel, Domain.Entities.Training>();
            CreateMap<PeriodizationViewModel, Periodization>();
            CreateMap<PeriodizationTrainingViewModel, PeriodizationTraining>();

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
            CreateMap<MuscleGroup, MuscleGroupViewModel>();
            CreateMap<WorkoutCategory, WorkoutCategoryViewModel>();
            CreateMap<Exercise, ExerciseViewModel>();
            CreateMap<Domain.Entities.Training, TrainingViewModel>();
            CreateMap<Periodization, PeriodizationViewModel>();
            CreateMap<PeriodizationTraining, PeriodizationTrainingViewModel>();

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

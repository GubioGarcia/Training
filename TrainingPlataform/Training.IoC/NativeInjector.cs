using Microsoft.Extensions.DependencyInjection;
using Training.Application.Interfaces;
using Training.Application.Services;
using Training.Data.Repositories;
using Training.Domain.Entities;
using Training.Domain.Interfaces;
using Training.Domain.Models;
using Training.Security;

namespace Training.IoC
{
    public static class NativeInjector
    {
        public static void RegisterServices(IServiceCollection services)
        {
            #region Services

            services.AddScoped<IUsersTypeService, UsersTypeService>();
            services.AddScoped<IProfessionalTypeService, ProfessionalTypeService>();
            services.AddScoped<IProfessionalService, ProfessionalService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IChecker, Checker>();
            services.AddScoped<IClientProfessionalService, ClientProfessinalService>();
            services.AddScoped<IMuscleGroupService, MuscleGroupService>();
            services.AddScoped<IWorkoutCategoryService, WorkoutCategoryService>();
            services.AddScoped<IExerciseService, ExerciseService>();
            services.AddScoped<ITrainingService, TrainingService>();
            services.AddScoped<IPeriodizationTrainingService, PeriodizationTrainingService>();

            services.AddScoped(typeof(IUserServiceBase<>), typeof(UserServiceBase<>));
            services.AddScoped<UserServiceBase<Professional>>();
            services.AddScoped<UserServiceBase<Client>>();

            #endregion

            #region Repositories

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IUsersTypeRepository, UsersTypeRepository>();
            services.AddScoped<IProfessionalTypeRepository, ProfessionalTypeRepository>();
            services.AddScoped<IProfessionalRepository, ProfessionalRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IClientProfessionalRepository, ClienteProfessionalRepository>();
            services.AddScoped<IMuscleGroupRepository, MuscleGroupRepository>();
            services.AddScoped<IWorkoutCategoryRepository, WorkoutCategoryRepository>();
            services.AddScoped<IExerciseRepository, ExerciseRepository>();
            services.AddScoped<ITrainingRepository, TrainingRepository>();
            services.AddScoped<IPeriodizationTrainingRepository, PeriodizationTrainingRepository>();

            #endregion

            services.AddScoped<IPasswordHasher, PasswordHasher>();
        }
    }
}

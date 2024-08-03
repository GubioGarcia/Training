using Microsoft.Extensions.DependencyInjection;
using Training.Application.Interfaces;
using Training.Application.Services;
using Training.Data.Repositories;
using Training.Domain.Entities;
using Training.Domain.Interfaces;
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

            #endregion

            #region Repositories

            services.AddScoped<IUsersTypeRepository, UsersTypeRepository>();
            services.AddScoped<IProfessionalTypeRepository, ProfessionalTypeRepository>();
            services.AddScoped<IProfessionalRepository, ProfessionalRepository>();

            #endregion

            services.AddScoped<IPasswordHasher, PasswordHasher>();
        }
    }
}

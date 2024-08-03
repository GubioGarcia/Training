using Microsoft.Extensions.DependencyInjection;
using Training.Application.Interfaces;
using Training.Application.Services;
using Training.Data.Repositories;
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

            #endregion

            #region Repositories

            services.AddScoped<IUsersTypeRepository, UsersTypeRepository>();
            services.AddScoped<IProfessionalTypeRepository, ProfessionalTypeRepository>();

            #endregion

            services.AddScoped<IPasswordHasher, PasswordHasher>();
        }
    }
}

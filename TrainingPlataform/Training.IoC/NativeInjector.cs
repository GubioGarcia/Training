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

            services.AddScoped<IUserTypeService, UserTypeService>();
            services.AddScoped<IProfessionalTypeService, ProfessionalTypeService>();
            services.AddScoped<IProfessionalService, ProfessionalService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IChecker, Checker>();

            #endregion

            #region Repositories

            services.AddScoped<IUserTypeRepository, UserTypeRepository>();
            services.AddScoped<IProfessionalTypeRepository, ProfessionalTypeRepository>();
            services.AddScoped<IProfessionalRepository, ProfessionalRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();

            #endregion

            services.AddScoped<IPasswordHasher, PasswordHasher>();
        }
    }
}

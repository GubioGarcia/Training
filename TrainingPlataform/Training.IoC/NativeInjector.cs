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
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IChecker, Checker>();
            services.AddScoped<IClientProfessionalService, ClientProfessinalService>();

            #endregion

            #region Repositories

            services.AddScoped<IUsersTypeRepository, UsersTypeRepository>();
            services.AddScoped<IProfessionalTypeRepository, ProfessionalTypeRepository>();
            services.AddScoped<IProfessionalRepository, ProfessionalRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IClientProfessionalRepository, ClienteProfessionalRepository>();

            #endregion

            services.AddScoped<IPasswordHasher, PasswordHasher>();
        }
    }
}

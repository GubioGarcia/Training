using Microsoft.Extensions.DependencyInjection;
using Training.Application.Interfaces;
using Training.Application.Services;
using Training.Data.Repositories;
using Training.Domain.Interfaces;

namespace Training.IoC
{
    public static class NativeInjector
    {
        public static void RegisterServices(IServiceCollection services)
        {
            #region Services

            services.AddScoped<IUsersTypeService, UsersTypeService>();

            #endregion

            #region Repositories

            services.AddScoped<IUsersTypeRepository, UsersTypeRepository>();

            #endregion
        }
    }
}

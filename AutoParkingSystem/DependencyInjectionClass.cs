using APSDataAccessLibrary.Context;
using APSDataAccessLibrary.DAL;
using APSDataAccessLibrary.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoParkingSystem.BusinessLayer.Core;
using AutoParkingSystem.BusinessLayer.Domain;

namespace AutoParkingSystem
{
    public static class DependencyInjectionClass
    {
        public static IServiceCollection LoadRepos(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IParkingLotsRepository, ParkingLotsRepository>();
            services.AddTransient<IVehicleRepository, VehicleRepository>();
            services.AddTransient<IBillsRepository, BillsRepository>();
            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            //also loads the dbcontext
            services.AddDbContext<ParkingContext>(options => options.UseSqlServer(config.GetConnectionString("Default")));

            return services;
        }

        public static IServiceCollection LoadServices(this IServiceCollection services)
        {
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IBillingService, BillingService>();
            services.AddTransient<IParkingService, ParkingService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IValidationService, ValidationService>();
            services.AddTransient<IVehicleService, VehicleService>();

            return services;
        }
    }
}

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ShareCar.Logic.RideRequest_Logic;
using ShareCar.Logic.User_Logic;
using ShareCar.Logic.Ride_Logic;
using ShareCar.Db.Repositories;
using ShareCar.Logic.Address_Logic;
using ShareCar.Logic.Route_Logic;
using ShareCar.Logic.Passenger_Logic;
using ShareCar.Db.Repositories.Address_Repository;
using ShareCar.Db.Repositories.RideRequest_Repository;
using ShareCar.Db.Repositories.Passenger_Repository;
using ShareCar.Db.Repositories.Route_Repository;
using ShareCar.Db.Repositories.Ride_Repository;
using ShareCar.Db.Repositories.User_Repository;
using ShareCar.Logic.Identity_Logic;
using ShareCar.Db.Repositories.Notes_Repository;
using ShareCar.Logic.Note_Logic;

namespace ShareCar.Logic.DI
{
    public class Bootstrapper
    {
        public static IContainer AddRegistrationsToDIContainer(IServiceCollection services)
        {
            RegisterLogic(services);
            RegisterRepositories(services);

            var builder = new ContainerBuilder();
            builder.Populate(services);

            return builder.Build();
        }

        private static void RegisterLogic(IServiceCollection services)
        {
            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddScoped<IFacebookIdentity, FacebookIdentity>();
            services.AddScoped<IGoogleIdentity, GoogleIdentity>();
            services.AddScoped<IRideLogic, RideLogic>();
            services.AddScoped<IRouteLogic, RouteLogic>();
            services.AddScoped<IRideRequestLogic, RideRequestLogic>();
            services.AddScoped<IUserLogic, UserLogic>();
            services.AddScoped<IAddressLogic, AddressLogic>();
            services.AddScoped<IPassengerLogic, PassengerLogic>();
            services.AddScoped<ICognizantIdentity, CognizantIdentity>();
            services.AddScoped<IDriverNoteLogic, DriverNoteLogic>();
            services.AddScoped<IRideRequestNoteLogic, RideRequestNoteLogic>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IRideRequestRepository, RideRequestRepository>();
            services.AddScoped<IPassengerRepository, PassengerRepository>();
            services.AddScoped<IRouteRepository, RouteRepository>();
            services.AddScoped<IRideRepository, RideRepository>();
            services.AddScoped<IPassengerRepository, PassengerRepository>();
            services.AddScoped<IDriverNoteRepository, DriverNoteRepository>();
            services.AddScoped<IDriverSeenNoteRepository, DriverSeenNoteRepository>();
            services.AddScoped<IRideRequestNoteRepository, RideRequestNoteRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }


    }
}
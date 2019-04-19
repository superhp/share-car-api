using AutoMapper;
using ShareCar.Dto;
using ShareCar.Db.Entities;
using ShareCar.Dto.Identity;

namespace ShareCar.Logic.ObjectMapping
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Ride, RideDto>();
            CreateMap<RideDto, Ride>();
            CreateMap<RideRequest, RideRequestDto>();
            CreateMap<RideRequestDto, RideRequest>();
            CreateMap<Passenger, PassengerDto>();
            CreateMap<PassengerDto, Passenger>();
            CreateMap<Address, AddressDto>();
            CreateMap<AddressDto, Address>();
            CreateMap<UserDto, User>();
            CreateMap<Route, RouteDto>();
            CreateMap<RouteDto, Route>();
            CreateMap<UnauthorizedUser, UnauthorizedUserDto>();
            CreateMap<UnauthorizedUserDto, UnauthorizedUser>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<DriverNote, DriverNoteDto>();
            CreateMap<DriverNoteDto, DriverNote>();
            CreateMap<RideRequestNote, RideRequestNoteDto>();
            CreateMap<RideRequestNoteDto, RideRequestNote>();
        }
    }
}

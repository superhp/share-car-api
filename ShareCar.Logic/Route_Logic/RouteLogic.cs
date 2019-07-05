using AutoMapper;
using ShareCar.Db.Entities;
using ShareCar.Db.Repositories.Route_Repository;
using ShareCar.Dto;
using ShareCar.Logic.Address_Logic;
using ShareCar.Logic.User_Logic;
using System.Collections.Generic;
using System.Linq;

namespace ShareCar.Logic.Route_Logic
{
    public class RouteLogic : IRouteLogic
    {
        private readonly IMapper _mapper;
        private readonly IUserLogic _userLogic;
        private readonly IRouteRepository _routeRepository;
        private readonly IAddressLogic _addressLogic;

        public RouteLogic(IRouteRepository routeRepository, IMapper mapper, IAddressLogic addressLogic, IUserLogic userLogic)
        {
            _routeRepository = routeRepository;
            _mapper = mapper;
            _addressLogic = addressLogic;
            _userLogic = userLogic;
        }

        public int GetRouteId(string geometry)
        {
            int routeId = _routeRepository.GetRouteId(geometry);
            return routeId;
        }

        public RouteDto GetRouteById(int id)
        {
            Route route = _routeRepository.GetRouteById(id);
            if (route == null)
            {
                return null;
            }

            RouteDto routeDto = _mapper.Map<Route, RouteDto>(route);

            return routeDto;
        }

        // Returns routes by passengers criteria
        public List<RouteDto> GetRoutes(RouteDto routeDto, string email)
        {
            var address = _mapper.Map<AddressDto, Address>(routeDto.ToAddress);
            Address secondAddress = null;

            var routeType = RouteType.ToOffice;

            if (routeDto.FromAddress != null)
            {
                if (routeDto.ToAddress != null)
                {
                    address = _mapper.Map<AddressDto, Address>(routeDto.FromAddress);
                    secondAddress = _mapper.Map<AddressDto, Address>(routeDto.ToAddress);
                    routeType = RouteType.OfficeToOffice;
                }
                else
                {
                    address = _mapper.Map<AddressDto, Address>(routeDto.FromAddress);
                    routeType = RouteType.FromOffice;
                }
            }

            var entityRoutes = _routeRepository.GetRoutes(routeType, address, secondAddress).ToList();

            List<RouteDto> dtoRoutes = new List<RouteDto>();
            foreach (var route in entityRoutes)
            {
                RouteDto mappedRoute = _mapper.Map<Route, RouteDto>(route);
                mappedRoute.FromId = route.FromId;
                mappedRoute.ToId = route.ToId;
                var driverEmails = route.Rides.Select(x => x.DriverEmail).Distinct().ToList();
                var driverNames = new List<string>();

                foreach(var driver in driverEmails)
                {
                    if (driver != email)
                    {
                        var user = _userLogic.GetUserByEmail(EmailType.LOGIN, driver);
                        mappedRoute.DriverFirstName = user.FirstName;
                        mappedRoute.DriverLastName = user.LastName;
                        mappedRoute.DriverEmail = user.Email;
                        dtoRoutes.Add(_mapper.Map<RouteDto, RouteDto>(mappedRoute));
                    }
                }
            }
            return dtoRoutes;
        }

        public void AddRoute(RouteDto route)
        {
            Route entityRoute = new Route
            {
                FromId = route.FromId,
                ToId = route.ToId,
                Geometry = route.Geometry,
                FromAddress = _mapper.Map<AddressDto, Address>(route.FromAddress),
                ToAddress = _mapper.Map<AddressDto, Address>(route.ToAddress)
            };
            _routeRepository.AddRoute(entityRoute);
        }

        public RouteDto GetRouteByRequest(int requestId)
        {
            var entity = _routeRepository.GetRouteByRequest(requestId);

            var dto = _mapper.Map<Route, RouteDto>(entity);

            dto.ToAddress = _mapper.Map<Address, AddressDto>(entity.ToAddress);
            dto.FromAddress = _mapper.Map<Address, AddressDto>(entity.FromAddress);

            return dto;
        }
    }
}

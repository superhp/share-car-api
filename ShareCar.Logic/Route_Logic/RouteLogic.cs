using System;
using AutoMapper;
using ShareCar.Db.Entities;
using ShareCar.Db.Repositories.Route_Repository;
using ShareCar.Dto;
using ShareCar.Logic.Address_Logic;
using System.Collections.Generic;
using System.Linq;

namespace ShareCar.Logic.Route_Logic
{
    public class RouteLogic : IRouteLogic
    {
        private readonly IMapper _mapper;
        private readonly IRouteRepository _routeRepository;
        private readonly IAddressLogic _addressLogic;

        public RouteLogic(IRouteRepository routeRepository, IMapper mapper, IAddressLogic addressLogic)
        {
            _routeRepository = routeRepository;
            _mapper = mapper;
            _addressLogic = addressLogic;
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
            Address address = _mapper.Map<AddressDto, Address>(routeDto.ToAddress);
            bool isFromOffice = false;

            if (routeDto.FromAddress != null)
            {
                address = _mapper.Map<AddressDto, Address>(routeDto.FromAddress);
                isFromOffice = true;
            }

            var entityRoutes = _routeRepository.GetRoutes(isFromOffice, address).ToList();

            List<RouteDto> dtoRoutes = new List<RouteDto>();

            foreach (var route in entityRoutes)
            {
                var drivers = route.Rides.Where(x => x.RideDateTime > DateTime.Now).Select(x => x.DriverEmail).Distinct().ToList();
                if (drivers.Count() == 1 && drivers.SingleOrDefault(x => x == email) != null)
                {
                    continue;
                }
                    
                RouteDto mappedRoute = _mapper.Map<Route, RouteDto>(route);
                    mappedRoute.FromId = route.FromId;
                    mappedRoute.ToId = route.ToId;
                    mappedRoute.Drivers = drivers;
                    dtoRoutes.Add(mappedRoute);
                
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

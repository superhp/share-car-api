using AutoMapper;
using ShareCar.Db.Entities;
using ShareCar.Db.Repositories;
using ShareCar.Db.Repositories.Route_Repository;
using ShareCar.Dto;
using ShareCar.Logic.Address_Logic;
using ShareCar.Logic.RideRequest_Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShareCar.Logic.Route_Logic
{
    public class RouteLogic: IRouteLogic
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
            Address address = _mapper.Map<AddressDto,Address>(routeDto.ToAddress);
            bool isFromOffice = false;

            if (routeDto.FromAddress != null)
            {
                // If user needs a ride to office, he recieves routes independently from his location
                address = _mapper.Map<AddressDto, Address>(routeDto.FromAddress);
                isFromOffice = true;                
            }

            IEnumerable<Route> entityRoutes = _routeRepository.GetRoutes(isFromOffice, address);
            List<RouteDto> dtoRoutes = new List<RouteDto>();
            foreach(var route in entityRoutes)
            {
                RouteDto mappedRoute = new RouteDto();
                mappedRoute.Rides = new List<RideDto>();
                List<Ride> rides = new List<Ride>();
                foreach(var ride in route.Rides)
                {
                    if(ride.DriverEmail != email && ride.RideDateTime >= routeDto.FromTime && ride.isActive)
                    {
                        ride.Route = null;
                        rides.Add(ride);
                    }
                }
                route.Rides = rides;
                if (route.Rides.Count != 0)
                {
                    foreach (var ride in route.Rides)
                    {
                        if (ride.NumberOfSeats > 0 && ride.RideDateTime >= DateTime.Now)
                        {
                            mappedRoute.Rides.Add(_mapper.Map<Ride, RideDto>(ride));
                        }
                    }
                    if (mappedRoute.Rides.Count > 0)
                    {
                        mappedRoute.FromAddress = _mapper.Map<Address, AddressDto>(route.FromAddress);
                        mappedRoute.ToAddress = _mapper.Map<Address, AddressDto>(route.ToAddress);
                        mappedRoute.FromId = route.FromId;
                        mappedRoute.Geometry = route.Geometry;

                        dtoRoutes.Add(mappedRoute);
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
                FromAddress = _mapper.Map<AddressDto,Address>(route.FromAddress),
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

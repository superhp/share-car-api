using Microsoft.EntityFrameworkCore;
using ShareCar.Db.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShareCar.Db.Repositories.Route_Repository
{
    public class RouteRepository : IRouteRepository
    {
        private readonly ApplicationDbContext _databaseContext;
        public RouteRepository(ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public int GetRouteId(string geometry)
        {
            try
            {
                return _databaseContext.Routes.Single(x => x.Geometry == geometry).RouteId;
            }
            catch
            {
                return -1; // Address doesnt exist
            }
        }
        public Route GetRouteById(int id)
        {
            return _databaseContext.Routes.Include(x => x.FromAddress).Include(x => x.ToAddress).Single(x => x.RouteId == id);
        }
        public void AddRoute(Route route)
        {
            _databaseContext.Routes.Add(route);
            _databaseContext.SaveChanges();
        }
        public void UpdateRoute(Route route)
        {
            Route routeToUpdate = GetRouteById(route.RouteId);
            if (routeToUpdate.Rides == null)
            {
                routeToUpdate.Rides = new List<Ride>();
            }
            foreach (var ride in route.Rides)
            {
                routeToUpdate.Rides.Add(ride);
            }

            _databaseContext.Routes.Update(routeToUpdate);
            _databaseContext.SaveChanges();
        }

        public IEnumerable<Route> GetRoutes(RouteType routeType, Address address, Address secondAddress = null)
        {
            if (routeType == RouteType.FromOffice)
            {
                return _databaseContext.Routes
                    .Include(x => x.Rides)
                    .Include(x => x.FromAddress)
                    .Include(x => x.ToAddress)
                    .Where(x => x.FromAddress.City == address.City &&
                    x.FromAddress.Street == address.Street &&
                    x.FromAddress.Number == address.Number &&
                    (x.Rides.Where(y => y.isActive && y.RideDateTime > DateTime.Now && y.NumberOfSeats > 0).Any()));
            }
            else if (routeType == RouteType.ToOffice)
            {
                return _databaseContext.Routes
                    .Include(x => x.Rides)
                    .Include(x => x.FromAddress)
                    .Include(x => x.ToAddress)
                    .Where(x => x.ToAddress.City == address.City &&
                    x.ToAddress.Street == address.Street &&
                    x.ToAddress.Number == address.Number &&
                    (x.Rides.Where(y => y.isActive && y.RideDateTime > DateTime.Now && y.NumberOfSeats > 0).Any()));
            }
            else
            {
                if(secondAddress == null)
                {
                    throw new ArgumentException("Second address is not provided in office to office route search");
                }

             return _databaseContext.Routes
                 .Include(x => x.Rides)
                 .Include(x => x.FromAddress)
                 .Include(x => x.ToAddress)
                 .Where(x => ((x.FromAddress.City == address.City &&
                 x.FromAddress.Street == address.Street &&
                 x.FromAddress.Number == address.Number) ||
                 (x.ToAddress.City == secondAddress.City &&
                 x.ToAddress.Street == secondAddress.Street &&
                 x.ToAddress.Number == secondAddress.Number)
                 ) &&
                (x.Rides.Where(y => y.isActive && y.RideDateTime > DateTime.Now && y.NumberOfSeats > 0).Any()));
            }
        }

        public Route GetRouteByRequest(int requestId)
        {

            var result = from route in _databaseContext.Routes.Include(x => x.ToAddress).Include(x => x.FromAddress)
                         from rides in route.Rides
                         from requests in rides.Requests
                         where requests.RideRequestId == requestId
                         select route;

            return result.ToList()[0];
        }
    }
}

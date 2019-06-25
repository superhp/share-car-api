using System.Collections.Generic;
using ShareCar.Db.Entities;
using ShareCar.Dto;

namespace ShareCar.Db.Repositories.Route_Repository
{
    public interface IRouteRepository
    {
        int GetRouteId(string geometry);
        Route GetRouteById(int id);
        void AddRoute(Route route);
        void UpdateRoute(Route route);
        IEnumerable<Route> GetRoutes(RouteType routeType, Address address, Address secondAddress = null);
        Route GetRouteByRequest(int requestId);
    }
}

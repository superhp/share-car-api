using System.Collections.Generic;
using ShareCar.Db.Entities;

namespace ShareCar.Db.Repositories.Route_Repository
{
    public interface IRouteRepository
    {
        int GetRouteId(string geometry);
        Route GetRouteById(int id);
        void AddRoute(Route route);
        void UpdateRoute(Route route);
        IEnumerable<Route> GetRoutes(bool isOfficeAddress, Address address);
        Route GetRouteByRequest(int requestId);
    }
}

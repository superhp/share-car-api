using System.Collections.Generic;
using ShareCar.Dto;

namespace ShareCar.Logic.Route_Logic
{
    public interface IRouteLogic
    {
        int GetRouteId(string geometry);
        RouteDto GetRouteById(int id);
        void AddRoute(RouteDto route);
        List<RouteDto> GetRoutes(RouteDto routeDto, string email);
        RouteDto GetRouteByRequest(int requestId);
    }
}
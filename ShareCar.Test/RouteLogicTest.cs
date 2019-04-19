using AutoMapper;
using Moq;
using NUnit.Framework;
using ShareCar.Db.Entities;
using ShareCar.Db.Repositories.Route_Repository;
using ShareCar.Dto;
using ShareCar.Logic.Address_Logic;
using ShareCar.Logic.Route_Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareCar.Test
{
    public class RouteLogicTest
    {
        Mock<IMapper> mapper = new Mock<IMapper>();
        Mock<IRouteRepository> routeRepository = new Mock<IRouteRepository>();
        Mock<IAddressLogic> addressLogic = new Mock<IAddressLogic>();

        [Test]
        public void GetRoutes_RideToOffice_SingleRouteContainingSingleRide()
        {
            DateTime baseDate = new DateTime(2000, 1, 1, 12, 0, 0);
            RouteDto routeParam = new RouteDto { FromAddress = new AddressDto(), ToAddress = new AddressDto(), FromTime = baseDate};
            string loggedInUser = "user";
            string otherUser1 = "user1";
            string otherUser2 = "user2";
            string otherUser3 = "user3";

            List<Ride> rideList = new List<Ride>
            {
                new Ride
                {
                    DriverEmail = loggedInUser,
                    RideDateTime = new DateTime(baseDate.Ticks).AddDays(1),
                    isActive = true
                },
                new Ride
                {
                    DriverEmail = otherUser1,
                    RideDateTime = new DateTime(baseDate.Ticks).AddDays(1),
                    isActive = false
                },     
                new Ride
                {
                    DriverEmail = otherUser2,
                    RideDateTime = new DateTime(baseDate.Ticks).AddDays(-1),
                    isActive = true
                },
                new Ride
                {
                    DriverEmail = otherUser3,
                    RideDateTime = new DateTime(baseDate.Ticks).AddDays(1),
                    isActive = true
                }
            };

            Route routes = new Route
            {
                FromAddress = new Address(),
                ToAddress = new Address(),
                Rides = rideList,
                Geometry = ""
            };

            routeRepository.Setup(x => x.GetRoutes(true, It.IsAny<Address>())).Returns(new List<Route> { routes });
            routeRepository.Setup(x => x.GetRoutes(false, It.IsAny<Address>())).Throws(new ArgumentException("fromOffice property was expected to be set to TRUE"));

            mapper.Setup(x => x.Map<Address, AddressDto>(It.IsAny<Address>())).Returns(new AddressDto());
            mapper.Setup(x => x.Map<AddressDto, Address>(It.IsAny<AddressDto>())).Returns(new Address());

            mapper.Setup(x => x.Map<Ride, RideDto>(rideList[0])).Returns(new RideDto { DriverEmail = rideList[0].DriverEmail });
            mapper.Setup(x => x.Map<Ride, RideDto>(rideList[1])).Returns(new RideDto { DriverEmail = rideList[1].DriverEmail });
            mapper.Setup(x => x.Map<Ride, RideDto>(rideList[2])).Returns(new RideDto { DriverEmail = rideList[2].DriverEmail });
            mapper.Setup(x => x.Map<Ride, RideDto>(rideList[3])).Returns(new RideDto { DriverEmail = rideList[3].DriverEmail });

            var routeLogic = new RouteLogic(routeRepository.Object, mapper.Object, addressLogic.Object);

            var result = routeLogic.GetRoutes(routeParam, loggedInUser);

            Assert.DoesNotThrow(() => routeLogic.GetRoutes(routeParam, loggedInUser));
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(result[0].Rides.Count, 1);
            Assert.AreEqual(result[0].Rides[0].DriverEmail, rideList[3].DriverEmail);

        }

    }
}

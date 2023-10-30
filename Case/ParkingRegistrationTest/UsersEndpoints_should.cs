using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ParkingRegistration.EventFeed;
using CarTypeService.Services;
using ParkingRegistration.Parking;
using Polly;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using ParkingRegistrationTest.Mocks;
using Xunit;

namespace ParkingRegistrationTest
{
    public class UsersEndpoints_should
    {
        private readonly IMotorApiService motorApiService;
        private readonly IEventStore eventStore;
        private readonly IParkingStore parkingStore;

        public UsersEndpoints_should()
        {
            var services = new ServiceCollection();
            services.AddScoped<IMotorApiService, MotorApiServiceMock>();
            services.AddScoped<IEventStore, EventStore>();
            services.AddScoped<IParkingStore, ParkingStore>();
            var serviceProvider = services.BuildServiceProvider();
            motorApiService = serviceProvider.GetService<IMotorApiService>();
            eventStore = serviceProvider.GetService<IEventStore>();
            parkingStore = serviceProvider.GetService<IParkingStore>();

        }

        [Fact]
        public async void ValidateParking()
        {
            //arrange
            parkingStore.RegisterParking(new Parking { LicensePlate = "AB12345", Email = "test@test.dk" });
            var productController = new ParkingController(parkingStore, motorApiService, eventStore);

            //act
            Parking parking = productController.GetActive("AB12345");

            //assert
            Assert.Equal("test@test.dk", parking.Email);
            Assert.Equal("AB12345", parking.LicensePlate);
        }
    }


}
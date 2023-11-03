using CarTypeService.Services;
using Microsoft.AspNetCore.Hosting;
using Polly;
using ParkingRegistration.Parking;
using ParkingRegistration.EventFeed;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using System.Data.Common;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddSingleton<IEventStoreConnection>(sp =>
{
    var settings = ConnectionSettings
        .Create()
        .KeepReconnecting()
        .Build();
    var connection = EventStoreConnection.Create(
        "ConnectTo=tcp://eventstore:1113; UseSslConnection=false;"
    );
    //connection = EventStoreConnection.Create(
    //new IPEndPoint(
    //    IPAddress.Parse("127.0.0.1"),
    //    1113
    //));

    connection.ConnectAsync().Wait(); // Wait for the connection to be established
    return connection;
});

builder.Services.AddHttpClient<IMotorApiService, MotorApiService>()
    .AddTransientHttpErrorPolicy(
    p => p.WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(1000 * Math.Pow(2, attempt))));

builder.Services.Scan(selector => selector
    .FromAssemblyOf<IParkingStore>()
    .AddClasses()
    .AsImplementedInterfaces());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

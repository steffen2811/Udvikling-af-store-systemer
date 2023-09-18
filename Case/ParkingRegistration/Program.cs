using CarTypeService.Services;
using Microsoft.AspNetCore.Hosting;
using Polly;
using ParkingRegistration.Parking;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
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

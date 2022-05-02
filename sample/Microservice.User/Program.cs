using Microservice.Service;
using Microservice.Consul;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var mvcBuilder = builder.Services.AddControllers();

mvcBuilder.ConfigureApplicationPartManager(manager =>
    {
        manager.FeatureProviders.Add(new AutoControllerFeatureProvider());
    })
    .AddControllersAsServices();
mvcBuilder.Services.Configure<MvcOptions>(options =>
{
    options.Conventions.Add(new AutoModelConvention());
    options.Filters.Add<JsonResultFilter>(1000);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddConsul(builder.Configuration.GetSection("Consul"));

var app = builder.Build();

// Configure the HzTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
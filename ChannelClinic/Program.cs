using Application.Command.CreateStaff;
using Application.Command.SignIn;
using Application.Exceptions;
using Application.RequestResponsePipeline;
using Application.Utilities;
using ChannelClinic.Utilities;
using DBService;
using DBService.Seeding;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseKestrel(option => option.AddServerHeader = false);
//builder.WebHost.UseUrls("http://*:5000");

// Add services to the container.

#pragma warning disable CS0618 // Type or member is obsolete
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApplicationWebExceptionHandler));
    options.Filters.Add(typeof(CustomValidationAttribute));
})
.AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
.AddNewtonsoftJson(x => x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
.AddNewtonsoftJson(x => x.UseCamelCasing(true))
//.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateStaffValidation>())
;
#pragma warning restore CS0618 // Type or member is obsolete

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(typeof(CreateStaffValidation).Assembly);


builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// services.AddLogging()

// Catch all fluent validator errors
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

// Add mediator
builder.Services.AddMediatR(typeof(SignInCommand).GetTypeInfo().Assembly);

// Check for JWT authentication where neccessary
builder.Services.AddApplicationJwtAuthentication();

// Adds a default in-memory implementation of IDistributedCache
builder.Services.AddMemoryCache();

// Add services
builder.Services.AddDBRepositoryServices();


// Allow cors 
builder.Services.AddCors(options =>
    options.AddPolicy("MyPolicy", builder =>
    {
        builder
            // .WithOrigins("http://localhost:4200")
            .SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    }));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();

builder.Services.ConfigureDefaultDataAccessDatabaseConnections(EnvironmentFunctions.CONNECTION_STRING);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseCors("MyPolicy");

app.EnsureDefaultDataAccessDatabaseAndMigrationsExtension();

await app.SeedDefualtDataContextDatabase();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();

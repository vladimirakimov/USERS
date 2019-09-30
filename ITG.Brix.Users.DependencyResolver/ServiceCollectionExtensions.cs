using AutoMapper;
using ITG.Brix.Users.API.Context.Services;
using ITG.Brix.Users.API.Context.Services.Arrangements;
using ITG.Brix.Users.API.Context.Services.Arrangements.Impl;
using ITG.Brix.Users.API.Context.Services.Impl;
using ITG.Brix.Users.API.Context.Services.Requests;
using ITG.Brix.Users.API.Context.Services.Requests.Impl;
using ITG.Brix.Users.API.Context.Services.Requests.Mappers;
using ITG.Brix.Users.API.Context.Services.Requests.Mappers.Impl;
using ITG.Brix.Users.API.Context.Services.Requests.Validators;
using ITG.Brix.Users.API.Context.Services.Requests.Validators.Components;
using ITG.Brix.Users.API.Context.Services.Requests.Validators.Components.Impl;
using ITG.Brix.Users.API.Context.Services.Requests.Validators.Impl;
using ITG.Brix.Users.API.Context.Services.Responses;
using ITG.Brix.Users.API.Context.Services.Responses.Impl;
using ITG.Brix.Users.API.Context.Services.Responses.Mappers;
using ITG.Brix.Users.API.Context.Services.Responses.Mappers.Impl;
using ITG.Brix.Users.Application.MappingProfiles;
using ITG.Brix.Users.Application.Services;
using ITG.Brix.Users.Application.Services.Impl;
using ITG.Brix.Users.Domain;
using ITG.Brix.Users.Infrastructure.ClassMaps;
using ITG.Brix.Users.Infrastructure.Configurations;
using ITG.Brix.Users.Infrastructure.Configurations.Impl;
using ITG.Brix.Users.Infrastructure.Providers;
using ITG.Brix.Users.Infrastructure.Providers.Impl;
using ITG.Brix.Users.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ITG.Brix.Users.DependencyResolver
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AutoMapper(this IServiceCollection services)
        {
            var assembly = typeof(DomainProfile).GetTypeInfo().Assembly;

            services.AddAutoMapper(assembly);

            return services;
        }

        public static IServiceCollection Persistence(this IServiceCollection services, string connectionString)
        {
            ClassMapsRegistrator.RegisterMaps();

            services.AddScoped<IPersistenceContext, PersistenceContext>();
            services.AddScoped<IPersistenceConfiguration>(x => new PersistenceConfiguration(connectionString));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserFinder, UserFinder>();
            services.AddScoped<IPublishIntegrationEventsService, PublishIntegrationEventsService>();

            return services;
        }

        public static IServiceCollection Providers(this IServiceCollection services)
        {
            services.AddScoped<IIdentifierProvider, IdentifierProvider>();
            services.AddScoped<IJsonProvider, JsonProvider>();
            services.AddScoped<IVersionProvider, VersionProvider>();
            services.AddScoped<IPasswordProvider, PasswordProvider>();
            services.AddScoped<IOdataProvider, OdataProvider>();

            return services;
        }

        public static IServiceCollection ApiServices(this IServiceCollection services)
        {
            services.AddScoped<IRequestComponentValidator, RequestComponentValidator>();
            services.AddScoped<IRequestValidator, GetRequestValidator>();
            services.AddScoped<IRequestValidator, ListRequestValidator>();
            services.AddScoped<IRequestValidator, CreateRequestValidator>();
            services.AddScoped<IRequestValidator, UpdateRequestValidator>();
            services.AddScoped<IRequestValidator, DeleteRequestValidator>();
            services.AddScoped<IRequestValidator, LoginRequestValidator>();



            services.AddScoped<IApiRequest, ApiRequest>();
            services.AddScoped<ICqsMapper, CqsMapper>();

            services.AddScoped<IErrorMapper, ErrorMapper>();
            services.AddScoped<IHttpStatusCodeMapper, HttpStatusCodeMapper>();
            services.AddScoped<IApiResponse, ApiResponse>();


            services.AddScoped<IValidationArrangement, ValidationArrangement>();
            services.AddScoped<IOperationArrangement, OperationArrangement>();
            services.AddScoped<IApiResult, ApiResult>();

            return services;
        }

    }
}

using Autofac;
using HRMS_V2.API.Application.CommandHandler;
using MediatR;
using System.Reflection;

namespace HRMS_V2.API.Ioc
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(AuthenticateCommandHandler).GetTypeInfo().Assembly)
               .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(AddUserCommandHandler).GetTypeInfo().Assembly)
              .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(AddRoleCommandHandler).GetTypeInfo().Assembly)
              .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(AddUserToRoleCommandHandler).GetTypeInfo().Assembly)
             .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(AddRoleToClaimCommandHandler).GetTypeInfo().Assembly)
           .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(AddHolidayCommandHandler).GetTypeInfo().Assembly)
               .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(UpdateHolidayCommandHandler).GetTypeInfo().Assembly)
              .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(DeleteHolidayByIdCommandHandler).GetTypeInfo().Assembly)
              .AsClosedTypesOf(typeof(IRequestHandler<,>));
        }
    }
}

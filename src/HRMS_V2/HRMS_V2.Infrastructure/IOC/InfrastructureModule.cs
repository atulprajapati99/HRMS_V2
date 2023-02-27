using System.Reflection;
using Autofac;
using HRMS_V2.Core.Interfaces;
using HRMS_V2.Core.Repositories.Base;
using HRMS_V2.Infrastructure.Behaviors;
using HRMS_V2.Infrastructure.Logging;
using HRMS_V2.Infrastructure.Repository.Base;
using MediatR;

namespace HRMS_V2.Infrastructure.IOC;

public class InfrastructureModule: Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
        {
            builder.Register<Func<Type, object>>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => componentContext.Resolve(t);
            });

            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(RepositoryBase<,>)).As(typeof(IRepositoryBase<,>)).InstancePerDependency();
           
            builder.RegisterGeneric(typeof(LoggerAdapter<>)).As(typeof(IAppLogger<>)).InstancePerDependency();

            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();
            
            builder.RegisterGeneric(typeof(TransactionBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
        }
}
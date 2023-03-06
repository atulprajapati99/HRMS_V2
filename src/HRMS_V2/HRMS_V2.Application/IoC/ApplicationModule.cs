using Autofac;
using HRMS_V2.Application.Interfaces;
using HRMS_V2.Application.Services;

namespace HRMS_V2.Application.IOC
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TokenService>().As<ITokenService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<HolidayService>().As<IHolidayService>().InstancePerLifetimeScope();
        }
    }
}

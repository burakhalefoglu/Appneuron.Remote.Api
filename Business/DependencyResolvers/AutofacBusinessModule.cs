using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using DataAccess.Abstract;
using DataAccess.Concrete.Cassandra;
using Module = Autofac.Module;

namespace Business.DependencyResolvers;

public class AutofacBusinessModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<CassLogRepository>().As<ILogRepository>().SingleInstance();
        builder.RegisterType<CassAdvStrategyRepository>().As<IAdvStrategyRepository>().SingleInstance();
        builder.RegisterType<CassInterstitialAdModelRepository>().As<IInterstielAdModelRepository>().SingleInstance();
        builder.RegisterType<CassRemoteOfferModelRepository>().As<IRemoteOfferModelRepository>().SingleInstance();
        builder.RegisterType<CassRemoteOfferProductModelRepository>().As<IRemoteOfferProductModelRepository>()
            .SingleInstance();

        var assembly = Assembly.GetExecutingAssembly();

        builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
            .EnableInterfaceInterceptors(new ProxyGenerationOptions
            {
                Selector = new AspectInterceptorSelector()
            }).SingleInstance().InstancePerDependency();
    }
}
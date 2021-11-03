using Autofac;
using Business.Constants;
using Business.DependencyResolvers;
using Business.Fakes.DArch;
using Business.MessageBrokers.Kafka;
using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Microsoft;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.ElasticSearch;
using Core.Utilities.IoC;
using Core.Utilities.MessageBrokers.RabbitMq;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Concrete.MongoDb;
using DataAccess.Concrete.MongoDb.Collections;
using DataAccess.Concrete.MongoDb.Context;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;

namespace Business
{
    public partial class BusinessStartup
    {
        protected readonly IHostEnvironment HostEnvironment;

        public BusinessStartup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            HostEnvironment = hostEnvironment;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <remarks>
        /// It is common to all configurations and must be called. Aspnet core does not call this method because there are other methods.
        /// </remarks>
        /// <param name="services"></param>

        public virtual void ConfigureServices(IServiceCollection services)
        {
            Func<IServiceProvider, ClaimsPrincipal> getPrincipal = (sp) =>

                            sp.GetService<IHttpContextAccessor>().HttpContext?.User ?? new ClaimsPrincipal(new ClaimsIdentity(Messages.Unknown));

            services.AddScoped<IPrincipal>(getPrincipal);
            services.AddMemoryCache();

            services.AddDependencyResolvers(Configuration, new ICoreModule[]
            {
                    new CoreModule()
            });

            services.AddSingleton<ConfigurationManager>();

            services.AddTransient<IElasticSearch, ElasticSearchManager>();

            services.AddTransient<IMessageBrokerHelper, MqQueueHelper>();
            services.AddTransient<IMessageConsumer, MqConsumerHelper>();
            services.AddSingleton<ICacheManager, MemoryCacheManager>();

            services.AddAutoMapper(typeof(ConfigurationManager));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(typeof(BusinessStartup).GetTypeInfo().Assembly);

            ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, expression) =>
            {
                return memberInfo.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.GetName();
            };
        }

        /// <summary>
        /// This method gets called by the Development
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            ConfigureServices(services);  
            services.AddTransient<IRemoteOfferEventModelRepository>(x=> new RemoteOfferEventModelRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.RemoteOfferEventModels));
            services.AddTransient<IInterstitialAdEventModelRepository>(x=> new InterstitialAdEventModelRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.InterstitialAdEventModels));
            services.AddTransient<IInterstielAdHistoryModelRepository>(x=> new InterstielAdHistoryModelRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.InterstielAdHistoryModels));
            services.AddTransient<IRemoteOfferHistoryModelRepository>(x=> new RemoteOfferHistoryModelRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.RemoteOfferHistoryModels));
            services.AddTransient<IRemoteOfferModelRepository>(x=> new RemoteOfferModelRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.RemoteOfferModels));
            services.AddTransient<IInterstielAdModelRepository>(x=> new InterstielAdModelRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.InterstielAdModels));
            services.AddTransient<IKafkaMessageBroker, KafkaMessageBroker>();

            services.AddDbContext<ProjectDbContext, DArchInMemory>(ServiceLifetime.Transient);
            services.AddSingleton<MongoDbContextBase, MongoDbContext>();
        }

        /// <summary>
        /// This method gets called by the Staging
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureStagingServices(IServiceCollection services)
        {
            ConfigureServices(services);           
            services.AddTransient<IRemoteOfferEventModelRepository>(x=> new RemoteOfferEventModelRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.RemoteOfferEventModels));
            services.AddTransient<IInterstitialAdEventModelRepository>(x=> new InterstitialAdEventModelRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.InterstitialAdEventModels));
            services.AddTransient<IInterstielAdHistoryModelRepository>(x=> new InterstielAdHistoryModelRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.InterstielAdHistoryModels));
            services.AddTransient<IRemoteOfferHistoryModelRepository>(x=> new RemoteOfferHistoryModelRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.RemoteOfferHistoryModels));
            services.AddTransient<IRemoteOfferModelRepository>(x=> new RemoteOfferModelRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.RemoteOfferModels));
            services.AddTransient<IInterstielAdModelRepository>(x=> new InterstielAdModelRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.InterstielAdModels));
            services.AddTransient<IKafkaMessageBroker, KafkaMessageBroker>();

            services.AddDbContext<ProjectDbContext>();

            services.AddSingleton<MongoDbContextBase, MongoDbContext>();
        }

        /// <summary>
        /// This method gets called by the Production
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureProductionServices(IServiceCollection services)
        {
            ConfigureServices(services);
            services.AddTransient<IRemoteOfferEventModelRepository>(x=> new RemoteOfferEventModelRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.RemoteOfferEventModels));
            services.AddTransient<IInterstitialAdEventModelRepository>(x=> new InterstitialAdEventModelRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.InterstitialAdEventModels));
            services.AddTransient<IInterstielAdHistoryModelRepository>(x=> new InterstielAdHistoryModelRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.InterstielAdHistoryModels));
            services.AddTransient<IRemoteOfferHistoryModelRepository>(x=> new RemoteOfferHistoryModelRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.RemoteOfferHistoryModels));
            services.AddTransient<IRemoteOfferModelRepository>(x=> new RemoteOfferModelRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.RemoteOfferModels));
            services.AddTransient<IInterstielAdModelRepository>(x=> new InterstielAdModelRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.InterstielAdModels));
            services.AddTransient<IKafkaMessageBroker, KafkaMessageBroker>();

            services.AddDbContext<ProjectDbContext>();

            services.AddSingleton<MongoDbContextBase, MongoDbContext>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacBusinessModule(new ConfigurationManager(Configuration, HostEnvironment)));
        }
    }
}

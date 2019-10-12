using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using Super.Core;
using 中间件跟服务;
using 中间件跟服务.Helper;

namespace 服务创建demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //  builder.RegisterType().As<IHostingEnvironment>().InstancePerLifetimeScope();
           // builder.RegisterType<IHostingEnvironment>();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.addtest();//自己写的服务
                 // 或者将Controller加入到Services中，这样写上面的代码就可以省略了
            services.AddControllersWithViews().AddControllersAsServices();
            //使用Autofac替换自带IOC
            //var builder = InitAutofac();
            //builder.Populate(services);
            //var container = builder.Build();

            //AutofacHelper.Container = container;

            //  return new AutofacServiceProvider(container);
        }
       
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
            PluginsManagement.RegistAtStart();
        }

        private ContainerBuilder InitAutofac()
        {
            var builder = new ContainerBuilder();

            var baseType = typeof(Dependency);
            // var baseTypeCircle = typeof(ICircleDependency);

            //Coldairarrow相关程序集
            //var assemblys = Assembly.GetEntryAssembly().GetReferencedAssemblies()
            //    .Select(Assembly.Load)
            //    .Cast<Assembly>()
            //    .Where(x => x.FullName.Contains("Coldairarrow")).ToList();

            //自动注入IDependency接口,支持AOP,生命周期为InstancePerDependency
            //builder.RegisterAssemblyTypes(assemblys.ToArray())
            //    .Where(x => baseType.IsAssignableFrom(x) && x != baseType)
            //    .AsImplementedInterfaces()
            //    .PropertiesAutowired()
            //    .InstancePerDependency()
            //    .EnableInterfaceInterceptors()
            //    .InterceptedBy(typeof(Interceptor));

            //自动注入ICircleDependency接口,循环依赖注入,不支持AOP,生命周期为InstancePerLifetimeScope
            //builder.RegisterAssemblyTypes(assemblys.ToArray())
            //    .Where(x => baseTypeCircle.IsAssignableFrom(x) && x != baseTypeCircle)
            //    .AsImplementedInterfaces()
            //    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
            //    .InstancePerLifetimeScope();

            //注册Controller
            builder.RegisterAssemblyTypes(typeof(Startup).GetTypeInfo().Assembly)
                .Where(t => typeof(Controller).IsAssignableFrom(t) && t.Name.EndsWith("Controller", StringComparison.Ordinal))
                .PropertiesAutowired();

            //AOP
            //   builder.RegisterType<Interceptor>();

            //请求结束自动释放
            //builder.RegisterType<DisposableContainer>()
            //    .As<IDisposableContainer>()
            //    .InstancePerLifetimeScope();

            return builder;
        }
    }
}

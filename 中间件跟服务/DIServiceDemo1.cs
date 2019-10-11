using Microsoft.Extensions.DependencyInjection;
using System;

namespace 中间件跟服务
{
    public static class DIServiceDemo1
    {

        public static IServiceCollection addtest(this IServiceCollection serviceCollection)
        {

            serviceCollection.AddScoped<iservicedemo,servicedemo>();//自定义方法
            return serviceCollection;

        }

        public class servicedemo:iservicedemo
        {
            public string name { get; set; }
            public string num { get; set;  }

            public string getname()
            {
                return "添加服务成功咯";
            }
        }

        public interface iservicedemo
        {
            string getname();

        }

        


    }
}

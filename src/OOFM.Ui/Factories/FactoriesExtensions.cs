using Microsoft.Extensions.DependencyInjection;
using OOFM.Ui.ViewModels.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOFM.Ui.Factories
{
    internal static class FactoriesExtensions
    {
        public static void AddAbstractFactory<TService, TImpl>(this IServiceCollection services)
            where TImpl : class, TService
            where TService : class
        {
            services.AddTransient<TService, TImpl>();
            services.AddSingleton<Func<TService>>(s => () => s.GetRequiredService<TService>());
            services.AddSingleton<IAbstractFactory<TService>, AbstractFactory<TService>>();
        }

        public static void AddAbstractFactory<TImpl>(this IServiceCollection services)
            where TImpl : class
        {
            services.AddTransient<TImpl>();
            services.AddSingleton<Func<TImpl>>(s => () => s.GetRequiredService<TImpl>());
            services.AddSingleton<IAbstractFactory<TImpl>, AbstractFactory<TImpl>>();
        }

        public static void AddStationItemFactory(this IServiceCollection services)
        {
            services.AddAbstractFactory<StationItemViewModel>();
            services.AddSingleton<IStationItemFactory, StationItemFactory>();
        }
    }
}

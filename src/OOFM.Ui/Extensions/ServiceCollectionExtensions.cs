﻿using Microsoft.Extensions.DependencyInjection;
using OOFM.Ui.Attributes;
using OOFM.Ui.Navigation;
using System.Reflection;
using WREdit.Base.Extensions;

namespace OOFM.Ui.Extensions;
internal static class ServiceCollectionExtensions
{
    public static void AddPages(this IServiceCollection services, Assembly? assembly = null)
    {
        assembly = assembly ?? Assembly.GetExecutingAssembly();

        foreach (var type in assembly.GetTypes())
        {
            if (type.IsAssignableTo(typeof(INavigationPage)) && !type.IsInterface)
            {
                string pageKey = type.Name;

                if (type.TryGetCustomAttribute<PageKeyAttribute>(out var attr))
                {
                    pageKey = attr.PageKey;
                }

                services.AddKeyedSingleton(typeof(INavigationPage), pageKey, type);
            }
        }
    }
}

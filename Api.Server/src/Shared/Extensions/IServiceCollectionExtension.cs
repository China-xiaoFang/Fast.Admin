using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Fast.Shared;

/// <summary>
/// <see cref="IServiceCollection"/> 拓展类
/// </summary>
public static class IServiceCollectionExtension
{
    /// <summary>
    /// 添加托管服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddHostedService(this IServiceCollection services)
    {
        var IHostedServiceType = typeof(IHostedService);

        var hostedServiceTypes = MAppContext.EffectiveTypes.Where(wh => IHostedServiceType.IsAssignableFrom(wh))
            .Select(sl => new
            {
                Type = sl,
                Order = sl.GetCustomAttribute<OrderAttribute>()
                            ?.Order
                        ?? 0
            })
            .OrderBy(ob => ob.Order)
            .Select(sl => sl.Type)
            .ToList();

        var addHostedService = typeof(ServiceCollectionHostedServiceExtensions).GetMethods()
            .Where(wh => wh.Name == nameof(ServiceCollectionHostedServiceExtensions.AddHostedService))
            .Where(wh => wh.IsGenericMethodDefinition)
            .First(wh => wh.GetParameters()
                             .Length
                         == 1);

        foreach (var hostedServiceType in hostedServiceTypes)
        {
            addHostedService.MakeGenericMethod(hostedServiceType)
                .Invoke(null, [services]);
        }

        return services;
    }
}
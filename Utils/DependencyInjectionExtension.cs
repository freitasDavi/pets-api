using Pets.Interfaces;
using Pets.Services;

namespace Pets.Utils;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddServices(services);
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IUsersService, UsersService>();
    }
}
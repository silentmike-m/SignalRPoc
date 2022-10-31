namespace Server.SignalR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Server.SignalR.Hubs;

internal static class DependencyInjection
{
    public static void AddPocSignalR(this IServiceCollection services)
    {
        services.AddSignalR();

        services.AddScoped<UserHub>();
    }

    public static void UsePocSignalR(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHub<UserHub>(UserHub.PATTERN);
    }
}

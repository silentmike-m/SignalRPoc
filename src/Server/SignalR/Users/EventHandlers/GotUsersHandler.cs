namespace Server.SignalR.Users.EventHandlers;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Server.SignalR.Hubs;
using Server.Users.Events;

internal sealed class GotUsersHandler : INotificationHandler<GotUsers>
{
    private readonly UserHub hub;
    private readonly IHubContext<UserHub> hubContext;
    private readonly ILogger<GotUsersHandler> logger;

    public GotUsersHandler(UserHub hub, IHubContext<UserHub> hubContext, ILogger<GotUsersHandler> logger) =>
        (this.hub, this.hubContext, this.logger) = (hub, hubContext, logger);

    public async Task Handle(GotUsers notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Send got users message");

        await this.hub.SendUsersAsync(notification.CompanyId, notification.Users, cancellationToken);

        //await this.hubContext.Clients
        //    .Group(groupId)
        //    .SendAsync("GotUsers", notification.Users, cancellationToken);
    }
}

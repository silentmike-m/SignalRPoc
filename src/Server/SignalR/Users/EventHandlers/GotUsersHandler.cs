namespace Server.SignalR.Users.EventHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;
    using Server.SignalR.Hubs;
    using Server.Users.Events;

    internal sealed class GotUsersHandler : INotificationHandler<GotUsers>
    {
        private readonly IHubContext<UserHub> hubContext;
        private readonly ILogger<GotUsersHandler> logger;

        public GotUsersHandler(IHubContext<UserHub> hubContext, ILogger<GotUsersHandler> logger) =>
            (this.hubContext, this.logger) = (hubContext, logger);

        public async Task Handle(GotUsers notification, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Send got users message");
            //.User(notification.UserId.ToString())
            await this.hubContext.Clients.User(notification.UserId.ToString()).SendAsync("GotUsers", notification.UserId.ToString(), notification.Users, cancellationToken);
        }
    }
}
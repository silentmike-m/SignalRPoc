namespace Server.SignalR.Hubs
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using Server.Entities;
    using SignalRSwaggerGen.Attributes;

    [SignalRHub("/userHub")]
    public sealed class UserHub : PocHub
    {
        public static string PATTERN = "/userHub";

        private readonly IHubContext<UserHub> hubContext;

        public UserHub(IHubContext<UserHub> hubContext)
            => this.hubContext = hubContext;

        [SignalRMethod("GotUsers")]
        [return: NotNull, SignalRReturn(typeof(IReadOnlyList<User>))]
        public async Task SendUsersAsync([SignalRParam(null, typeof(string))] string companyId, [SignalRHidden] IReadOnlyList<User> users, [SignalRHidden] CancellationToken cancellationToken = default)
        {
            await this.hubContext.Clients
                .Group(companyId)
                .SendAsync("GotUsers", users, cancellationToken);
        }
    }
}

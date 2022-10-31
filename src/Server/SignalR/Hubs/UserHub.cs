﻿namespace Server.SignalR.Hubs;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Server.Entities;
using SignalRSwaggerGen.Attributes;

[SignalRHub("/userHub")]
public sealed class UserHub : PocHub<UserHub>
{
    public static string PATTERN = "/userHub";

    private readonly string GOT_USERS_MESSAGE = "GotUsers";

    public UserHub(IHubContext<UserHub> context)
        : base(context)
    {
    }

    [SignalRMethod("GotUsers")]
    [return: NotNull, SignalRReturn(typeof(IReadOnlyList<User>))]
    public async Task SendUsersAsync(string companyId, [SignalRHidden] IReadOnlyList<User> users, [SignalRHidden] CancellationToken cancellationToken = default)
    {
        await this.context.Clients
            .Group(companyId)
            .SendAsync(this.GOT_USERS_MESSAGE, users, cancellationToken);
    }
}

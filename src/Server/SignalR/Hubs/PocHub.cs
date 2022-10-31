namespace Server.SignalR.Hubs;

using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

public abstract class PocHub<THub> : Hub<THub>
    where THub : Hub
{
    protected readonly IHubContext<THub> context;

    protected PocHub(IHubContext<THub> context)
        => this.context = context;

    [SignalRHidden]
    public override Task OnConnectedAsync()
    {
        var companyId = this.Context.User?.FindFirst(ClaimNames.CompanyId);

        if (companyId is not null)
        {
            this.Groups.AddToGroupAsync(this.Context.ConnectionId, companyId.Value)
                .GetAwaiter()
                .GetResult();
        }

        return base.OnConnectedAsync();
    }
}

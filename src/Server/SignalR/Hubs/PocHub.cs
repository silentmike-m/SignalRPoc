namespace Server.SignalR.Hubs;

using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

public abstract class PocHub : Hub
{
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

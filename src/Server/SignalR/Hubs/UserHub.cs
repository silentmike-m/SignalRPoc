namespace Server.SignalR.Hubs
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    public sealed class UserHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            var groupIdentifier = this.Context.User?.FindFirst("GroupId");

            if (groupIdentifier is not null)
            {
                this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupIdentifier.Value)
                    .GetAwaiter()
                    .GetResult();
            }

            return base.OnConnectedAsync();
        }
    }
}
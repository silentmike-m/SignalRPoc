namespace Server.Users.QueryHandlers;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Server.Users.Events;
using Server.Users.Queries;

internal sealed class GetUsersHandler : IRequestHandler<GetUsers>
{
    private readonly ApiContext context;
    private readonly ILogger<GetUsersHandler> logger;
    private readonly IMediator mediator;

    public GetUsersHandler(ApiContext context, ILogger<GetUsersHandler> logger, IMediator mediator) =>
        (this.context, this.logger, this.mediator) = (context, logger, mediator);

    public Task<Unit> Handle(GetUsers request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Receive get users request");

        var users = this.context.Users.ToList();

        var notification = new GotUsers
        {
            CompanyId = request.CompanyId,
            UserId = request.UserId,
            Users = users,
        };

        this.mediator.Publish(notification, cancellationToken);

        return Task.FromResult(Unit.Value);
    }
}

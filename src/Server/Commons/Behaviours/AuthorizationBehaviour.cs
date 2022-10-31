namespace Server.Commons.Behaviours;

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

internal sealed class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ICurrentRequestService currentUserService;

    public AuthorizationBehaviour(ICurrentRequestService currentUserService)
    {
        this.currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (request is IAuthId authId)
        {
            var (companyId, userId) = currentUserService.CurrentUser;

            if (authId.UserId == Guid.Empty)
            {
                authId.UserId = userId;
            }

            if (string.IsNullOrEmpty(authId.CompanyId))
            {
                authId.CompanyId = companyId;
            }
        }

        return await next();
    }
}

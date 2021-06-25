namespace Server.Commons.Behaviours
{
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
            IAuthId authId = request as IAuthId;

            if (authId != null && authId.UserId == Guid.Empty)
            {
                authId.UserId = currentUserService.UserId;
            }

            return await next();
        }
    }
}
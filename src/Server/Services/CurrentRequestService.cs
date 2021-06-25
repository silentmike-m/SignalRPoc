namespace Server.Services
{
    using System;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Http;
    using Server.Commons;

    public class CurrentRequestService : ICurrentRequestService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentRequestService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var nameIdentifier = this.httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(nameIdentifier))
                {
                    return Guid.Empty;
                }

                return new Guid(nameIdentifier);
            }
        }
    }
}
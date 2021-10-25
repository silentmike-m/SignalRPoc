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

        public (string groupId, Guid userId) CurrentUser
        {
            get
            {
                var nameIdentifier = this.httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                var groupIdentifier = this.httpContextAccessor.HttpContext?.User?.FindFirstValue("GroupId")
                    ?? string.Empty;

                var userId = string.IsNullOrEmpty(nameIdentifier)
                                  ? Guid.Empty
                                  : new Guid(nameIdentifier);

                return (groupIdentifier, userId);
            }
        }
    }
}
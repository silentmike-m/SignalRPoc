namespace Server.Commons
{
    using System;

    public interface ICurrentRequestService
    {
        Guid UserId { get; }
    }
}
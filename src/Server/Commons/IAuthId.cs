namespace Server.Commons
{
    using System;

    public interface IAuthId
    {
        Guid UserId { get; set; }
    }
}
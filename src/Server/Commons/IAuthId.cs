namespace Server.Commons
{
    using System;

    public interface IAuthId
    {
        string GroupId { get; set; }
        Guid UserId { get; set; }
    }
}
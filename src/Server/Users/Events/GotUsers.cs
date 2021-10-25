namespace Server.Users.Events
{
    using System;
    using System.Collections.Generic;
    using MediatR;
    using Server.Commons;
    using Server.Entities;

    public sealed record GotUsers : INotification, IAuthId
    {
        public IReadOnlyList<User> Users { get; init; } = new List<User>().AsReadOnly();
        public string GroupId { get; set; } = default;
        public Guid UserId { get; set; } = default;
    }
}
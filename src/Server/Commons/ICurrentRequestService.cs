namespace Server.Commons;

using System;

public interface ICurrentRequestService
{
    (string groupId, Guid userId) CurrentUser { get; }
}

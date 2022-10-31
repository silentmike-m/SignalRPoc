namespace Server.Commons;

using System;

public interface IAuthId
{
    string CompanyId { get; set; }
    Guid UserId { get; set; }
}

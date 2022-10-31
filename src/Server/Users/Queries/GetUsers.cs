namespace Server.Users.Queries;

using System;
using System.Text.Json.Serialization;
using MediatR;
using Server.Commons;

internal sealed record GetUsers : IRequest, IAuthId
{
    [JsonIgnore] public string CompanyId { get; set; } = default;
    [JsonIgnore] public Guid UserId { get; set; } = default;
}

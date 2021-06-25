namespace Server.Users.Queries
{
    using System;
    using System.Text.Json.Serialization;
    using MediatR;
    using Server.Commons;

    internal sealed record GetUsers : IRequest, IAuthId
    {
        [JsonPropertyName("userName")] public string UserName { get; init; } = string.Empty;
        [JsonIgnore] public Guid UserId { get; set; } = default;
    }
}
namespace Server.Users.Queries
{
    using System.Text.Json.Serialization;
    using MediatR;

    public sealed record GetToken : IRequest<string>
    {
        [JsonPropertyName("userName")] public string UserName { get; init; } = string.Empty;
        [JsonPropertyName("password")] public string Password { get; init; } = string.Empty;
    }
}
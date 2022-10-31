namespace Server.Users.Queries;

using System.Text.Json.Serialization;
using MediatR;

public sealed record GetToken : IRequest<string>
{
    [JsonPropertyName("company_id")] public string CompanyId { get; init; } = default;
    [JsonPropertyName("user_name")] public string UserName { get; init; } = string.Empty;
    [JsonPropertyName("password")] public string Password { get; init; } = string.Empty;
}

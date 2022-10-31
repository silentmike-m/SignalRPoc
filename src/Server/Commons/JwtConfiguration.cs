namespace Server.Commons;

internal sealed record JwtConfiguration
{
    public string Audience { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string SecurityKey { get; init; } = string.Empty;
}

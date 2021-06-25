namespace Client.Entities
{
    using System;

    internal sealed record User
    {
        public Guid Id { get; init; } = default;
        public string UserName { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}

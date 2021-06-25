namespace Server.Entities
{
    using System;

    public sealed class User
    {
        public Guid Id { get; set; } = default;

        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
namespace Server;

using Microsoft.EntityFrameworkCore;
using Server.Entities;

internal sealed class ApiContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public ApiContext(DbContextOptions<ApiContext> options) : base(options)
    { }
}
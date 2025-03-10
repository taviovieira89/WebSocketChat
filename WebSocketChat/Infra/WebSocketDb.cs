using Microsoft.EntityFrameworkCore;
public class WebSocketDb : DbContext
{
    public DbSet<User> Users { get; set; }
    public WebSocketDb(DbContextOptions<WebSocketDb> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
using Flunt.Notifications;
using Microsoft.EntityFrameworkCore;
using MoneyPro2.API.Data.Mappings;
using MoneyPro2.Domain.Entities;
using MoneyPro2.Domain.ValueObjects;

namespace MoneyPro2.API.Data;

public class MoneyPro2DataContext : DbContext
{
    public MoneyPro2DataContext() { }

    public MoneyPro2DataContext(DbContextOptions<MoneyPro2DataContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserLogin> UserLogins { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Ignorar ValueObjects
        modelBuilder.Ignore<Email>();
        modelBuilder.Ignore<ChangePassword>();
        modelBuilder.Ignore<CPF>();
        modelBuilder.Ignore<Login>();
        modelBuilder.Ignore<Notification>();

        // UserLogin - User
        modelBuilder
            .Entity<User>()
            .HasMany(e => e.UserLogins)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .IsRequired(false)
            .HasConstraintName("FK_UserLogin_User_UserId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.ApplyConfiguration(new UserMap());
        modelBuilder.ApplyConfiguration(new UserLoginMap());
    }
}

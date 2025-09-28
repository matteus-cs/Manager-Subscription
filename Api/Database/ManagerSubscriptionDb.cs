using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Database;

public class ManagerSubscriptionDb(DbContextOptions options) : DbContext(options)
{
    public DbSet<Address> Addresses  { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Plan> Plans { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Installment> Installments { get; set; }
    public DbSet<Admin> Admins { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Plan>()
            .Property(e => e.MonthlyPrice)
            .HasColumnType("decimal(6,2)");

        modelBuilder.Entity<Installment>()
            .Property(e => e.Amount)
            .HasColumnType("decimal(6,2)");
        modelBuilder.Entity<Installment>()
            .Property(e => e.Status)
            .HasConversion<string>();
    }
}
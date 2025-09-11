using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Database;

public class ManagerSubscriptionDb(DbContextOptions options) : DbContext(options)
{
    public DbSet<Address> Addresses  { get; set; }
    public DbSet<Customer> Customers { get; set; }
}
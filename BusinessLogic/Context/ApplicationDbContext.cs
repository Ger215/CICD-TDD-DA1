using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.Context;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<SpendingGoal> SpendingGoals { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<GeneralAccount> GeneralAccounts { get; set; }
    public DbSet<CreditCardAccount> CreditCardAccounts { get; set; }
    public DbSet<ExchangeRate> ExchangeRates { get; set; }



    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        if (!Database.IsInMemory())
        {
            Database.Migrate();
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>()
            .HasOne(transaction => transaction.AccountAssociated)
            .WithMany()
            .HasForeignKey(transaction => transaction.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(transaction => transaction.CategoryAssociated)
            .WithMany(category => category.Transactions)
            .HasForeignKey(transaction => transaction.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

            
    }
}

using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context;

public interface IApplicationDbContextFactory
{
    ApplicationDbContext CreateDbContext();
}

public class InMemoryAppContextFactory : IApplicationDbContextFactory
{
    public ApplicationDbContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseInMemoryDatabase("TestDB");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
using DBRepository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DBRepository.Factories
{
    public class AdaruDBContextFactory : IRepositoryContextFactory
    {
        public AdaruDBContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AdaruDBContext>();
            optionsBuilder.UseNpgsql(connectionString);
            return new AdaruDBContext(optionsBuilder.Options);
        }
    }
}

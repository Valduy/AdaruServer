namespace DBRepository.Interfaces
{
    public interface IRepositoryContextFactory
    {
        AdaruDBContext CreateDbContext(string connectionString);
    }
}

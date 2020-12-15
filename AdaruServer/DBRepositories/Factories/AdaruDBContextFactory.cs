using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.DBRepositories.Interfaces;
using AdaruServer.Models;
using Microsoft.EntityFrameworkCore;

namespace AdaruServer.DBRepositories.Factories
{
    public class AdaruDBContextFactory : IRepositoryContextFactory
    {
        public AdaruDBContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AdaruDBContext>();
            optionsBuilder.UseNpgsql();
            return new AdaruDBContext(optionsBuilder.Options);
        }
    }
}

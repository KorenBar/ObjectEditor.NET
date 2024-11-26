using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public abstract class BaseDbContext : DbContext
    {
        private readonly string _connectionString;
        public DbProvider DbProvider { get; }

        public BaseDbContext(DbProvider dbProvider, string connectionString)
        {
            DbProvider = dbProvider;
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .SetProvider(DbProvider, _connectionString)
                .UseSnakeCaseNamingConvention()
                .UseLazyLoadingProxies();
        }
    }
}

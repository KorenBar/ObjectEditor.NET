using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    internal static class DbContextExtensions
    {
        public static DbContextOptionsBuilder SetProvider(this DbContextOptionsBuilder optionsBuilder, DbProvider dbProvider, string connectionString)
        {
            if (optionsBuilder == null)
                throw new ArgumentNullException(nameof(optionsBuilder));
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            return dbProvider switch
            {
                DbProvider.SQLite => optionsBuilder.UseSqlite(connectionString),
                // TODO: Implement other providers below
                //DbProvider.SQLServer => optionsBuilder.UseSqlServer(connectionString),
                //DbProvider.PostgreSQL => optionsBuilder.UseNpgsql(connectionString),
                //DbProvider.MySQL => optionsBuilder.UseMySql(connectionString),
                DbProvider.None => throw new InvalidOperationException("Database provider not set!"),
                _ => throw new NotSupportedException($"Database provider '{dbProvider}' is not supported!")
            };
        }
    }
}

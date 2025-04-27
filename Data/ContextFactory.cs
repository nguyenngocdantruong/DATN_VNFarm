using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace VNFarm.Data
{
    public class ContextFactory : IDesignTimeDbContextFactory<VNFarmContext>
    {
        public VNFarmContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<VNFarmContext>()
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("VNFarm.Infrastructure"));

            return new VNFarmContext(builder.Options);
        }
    }
} 
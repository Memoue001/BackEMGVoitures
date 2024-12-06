using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EMGVoitures.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Configurez les options de DbContext pour la migration
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Utiliser la configuration du fichier appsettings.json pour récupérer la chaîne de connexion
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Assurez-vous que le chemin de base est correct
                .AddJsonFile("appsettings.json")
                .Build();

            // Configurez DbContext pour utiliser SQL Server
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);

            // Retourner l'instance de ApplicationDbContext avec les options
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}

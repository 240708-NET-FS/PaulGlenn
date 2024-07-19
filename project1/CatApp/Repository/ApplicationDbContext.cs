using Microsoft.EntityFrameworkCore; 
using Microsoft.Extensions.Configuration; 

namespace CatApp.Entities;


public class ApplicationDbContext : DbContext 
{

   public ApplicationDbContext () { }
   public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base (options)
   {

   }

    public DbSet<Cat> Cats {get;set; }

    public DbSet<User> Users {get;set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured) {
            IConfiguration configuration = new ConfigurationBuilder()
                                                .SetBasePath(Directory.GetCurrentDirectory())
                                                .AddJsonFile("appsettings.json")
                                                .Build() ; 
            var connectionString = configuration.GetConnectionString("DefaultConnection"); 
            optionsBuilder.UseSqlServer(connectionString); 
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cat>()
            .HasOne(c=>c.User)
            .WithMany(u=>u.Cats)
            .HasForeignKey(c=>c.UserID);
    }


}
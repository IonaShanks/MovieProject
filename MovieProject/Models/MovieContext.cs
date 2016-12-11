using System.Data.Entity;

namespace MovieModel
{
    public class MovieContext : DbContext
    {
        public MovieContext() : base("Movie Project")
        {
            Database.SetInitializer<MovieContext>(new CreateDatabaseIfNotExists<MovieContext>());
        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
    }

    
}

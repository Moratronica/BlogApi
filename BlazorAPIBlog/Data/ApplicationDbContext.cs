using BlazorAPIBlog.Modelos;
using Microsoft.EntityFrameworkCore;

namespace BlazorAPIBlog.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        // Agregar los modelos  (serán tablas en la base de datos)
        public DbSet<Post> Post { get; set; } 
        public DbSet<Usuario> Usuario { get; set; }
    }
}

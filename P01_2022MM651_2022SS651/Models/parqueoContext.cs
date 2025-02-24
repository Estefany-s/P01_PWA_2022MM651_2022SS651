using Microsoft.EntityFrameworkCore;

namespace P01_2022MM651_2022SS651.Models
{
    public class parqueoContext : DbContext
    {
        public parqueoContext(DbContextOptions<parqueoContext> options) : base(options)
        {
        }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<DetalleSucursal> DetalleSucursal { get; set; }
        public DbSet<Reserva> Reserva { get; set; }
        public DbSet<Sucursal> Sucursal { get; set; }

    }
}

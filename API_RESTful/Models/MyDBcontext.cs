using Microsoft.EntityFrameworkCore;

namespace API_RESTful.Models
{
    public class MyDBcontext : DbContext
    {
        // CONSTRUCTOR:
        public MyDBcontext(DbContextOptions<MyDBcontext> options)
            : base(options)
        {

        }


        // TABLAS A MAPEAR EN LA DB:
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Producto> Productos { get; set; }

    }
}

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sistema_Veterinaria.Models
{
    public partial class veterinariaContext : DbContext
    {
        public veterinariaContext()
        {
        }

        public veterinariaContext(DbContextOptions<veterinariaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categorias> Categorias { get; set; }
        public virtual DbSet<Citas> Citas { get; set; }
        public virtual DbSet<Clientes> Clientes { get; set; }
        public virtual DbSet<Compras> Compras { get; set; }
        public virtual DbSet<Configuracion> Configuracion { get; set; }
        public virtual DbSet<DetalleCompras> DetalleCompras { get; set; }
        public virtual DbSet<DetalleVentas> DetalleVentas { get; set; }
        public virtual DbSet<Mascotas> Mascotas { get; set; }
        public virtual DbSet<Productos> Productos { get; set; }
        public virtual DbSet<Proveedores> Proveedores { get; set; }
        public virtual DbSet<Registro> Registro { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }
        public virtual DbSet<Ventas> Ventas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=localhost;database=veterinaria;user=root;password=2351043820", x => x.ServerVersion("8.0.21-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categorias>(entity =>
            {
                entity.HasKey(e => e.IdCategorias)
                    .HasName("PRIMARY");

                entity.ToTable("categorias");

                entity.Property(e => e.IdCategorias).HasColumnName("idCategorias");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Citas>(entity =>
            {
                entity.HasKey(e => e.IdCitas)
                    .HasName("PRIMARY");

                entity.ToTable("citas");

                entity.HasIndex(e => e.RCliente)
                    .HasName("FK_R_Cliente_idx");

                entity.HasIndex(e => e.RMascota)
                    .HasName("FK_R2_Mascotas_idx");

                entity.Property(e => e.IdCitas).HasColumnName("idCitas");

                entity.Property(e => e.FechaHora).HasColumnType("datetime");

                entity.Property(e => e.Notas)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.RCliente).HasColumnName("R_Cliente");

                entity.Property(e => e.RMascota).HasColumnName("R_Mascota");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.RClienteNavigation)
                    .WithMany(p => p.Citas)
                    .HasForeignKey(d => d.RCliente)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_R_ClienteCita");

                entity.HasOne(d => d.RMascotaNavigation)
                    .WithMany(p => p.Citas)
                    .HasForeignKey(d => d.RMascota)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_R2_Mascotas");
            });

            modelBuilder.Entity<Clientes>(entity =>
            {
                entity.HasKey(e => e.IdClientes)
                    .HasName("PRIMARY");

                entity.ToTable("clientes");

                entity.Property(e => e.IdClientes).HasColumnName("idClientes");

                entity.Property(e => e.Correo)
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Telefono)
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Compras>(entity =>
            {
                entity.HasKey(e => e.IdCompras)
                    .HasName("PRIMARY");

                entity.ToTable("compras");

                entity.HasIndex(e => e.RProveedor)
                    .HasName("FK_R_Proveedor_idx");

                entity.HasIndex(e => e.RUsuario)
                    .HasName("FK_R_UsuarioCompras_idx");

                entity.Property(e => e.IdCompras).HasColumnName("idCompras");

                entity.Property(e => e.FechaHora).HasColumnType("datetime");

                entity.Property(e => e.RProveedor).HasColumnName("R_Proveedor");

                entity.Property(e => e.RUsuario).HasColumnName("R_Usuario");

                entity.HasOne(d => d.RProveedorNavigation)
                    .WithMany(p => p.Compras)
                    .HasForeignKey(d => d.RProveedor)
                    .HasConstraintName("FK_R_ProveedorCompras");

                entity.HasOne(d => d.RUsuarioNavigation)
                    .WithMany(p => p.Compras)
                    .HasForeignKey(d => d.RUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_R_UsuarioCompras");
            });

            modelBuilder.Entity<Configuracion>(entity =>
            {
                entity.HasKey(e => e.Clave)
                    .HasName("PRIMARY");

                entity.ToTable("configuracion");

                entity.Property(e => e.CantidadInventario).HasColumnName("Cantidad_Inventario");

                entity.Property(e => e.DiasCaducidad).HasColumnName("Dias_Caducidad");
            });

            modelBuilder.Entity<DetalleCompras>(entity =>
            {
                entity.HasKey(e => e.IdDetalleCompra)
                    .HasName("PRIMARY");

                entity.ToTable("detalle_compras");

                entity.HasIndex(e => e.RCompra)
                    .HasName("FK_R_Compra");

                entity.HasIndex(e => e.RProducto)
                    .HasName("FK_R_Producto_idx");

                entity.Property(e => e.IdDetalleCompra).HasColumnName("id_DetalleCompra");

                entity.Property(e => e.RCompra).HasColumnName("R_Compra");

                entity.Property(e => e.RProducto)
                    .IsRequired()
                    .HasColumnName("R_Producto")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.RCompraNavigation)
                    .WithMany(p => p.DetalleCompras)
                    .HasForeignKey(d => d.RCompra)
                    .HasConstraintName("FK_R_Compra");

                entity.HasOne(d => d.RProductoNavigation)
                    .WithMany(p => p.DetalleCompras)
                    .HasForeignKey(d => d.RProducto)
                    .HasConstraintName("FK_R_ProductoCompras");
            });

            modelBuilder.Entity<DetalleVentas>(entity =>
            {
                entity.HasKey(e => e.IdDetalleVentas)
                    .HasName("PRIMARY");

                entity.ToTable("detalle_ventas");

                entity.HasIndex(e => e.RProducto)
                    .HasName("FK_R_Producto_idx");

                entity.HasIndex(e => e.RVenta)
                    .HasName("FK_R_Venta_idx");

                entity.Property(e => e.IdDetalleVentas).HasColumnName("id_DetalleVentas");

                entity.Property(e => e.RProducto)
                    .IsRequired()
                    .HasColumnName("R_Producto")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.RVenta).HasColumnName("R_Venta");

                entity.HasOne(d => d.RProductoNavigation)
                    .WithMany(p => p.DetalleVentas)
                    .HasForeignKey(d => d.RProducto)
                    .HasConstraintName("FK_R_ProductoVentas");

                entity.HasOne(d => d.RVentaNavigation)
                    .WithMany(p => p.DetalleVentas)
                    .HasForeignKey(d => d.RVenta)
                    .HasConstraintName("FK_R_Venta");
            });

            modelBuilder.Entity<Mascotas>(entity =>
            {
                entity.HasKey(e => e.IdMascotas)
                    .HasName("PRIMARY");

                entity.ToTable("mascotas");

                entity.HasIndex(e => e.RCliente)
                    .HasName("FK_R_Cliente_idx");

                entity.Property(e => e.IdMascotas).HasColumnName("idMascotas");

                entity.Property(e => e.Descripcion)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Edad).HasColumnType("date");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.RCliente).HasColumnName("R_Cliente");

                entity.Property(e => e.Raza)
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Tipo)
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.RClienteNavigation)
                    .WithMany(p => p.Mascotas)
                    .HasForeignKey(d => d.RCliente)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_R_Cliente");
            });

            modelBuilder.Entity<Productos>(entity =>
            {
                entity.HasKey(e => e.IdProductos)
                    .HasName("PRIMARY");

                entity.ToTable("productos");

                entity.HasIndex(e => e.RCategoria)
                    .HasName("FK_R_Categoria_idx");

                entity.HasIndex(e => e.RProveedor)
                    .HasName("FK_R_Proveedor_idx");

                entity.Property(e => e.IdProductos)
                    .HasColumnName("idProductos")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Caducidad).HasColumnType("date");

                entity.Property(e => e.Descripcion)
                    .HasColumnType("varchar(60)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.PrecioCompra).HasColumnName("Precio_Compra");

                entity.Property(e => e.PrecioVenta).HasColumnName("Precio_Venta");

                entity.Property(e => e.RCategoria).HasColumnName("R_Categoria");

                entity.Property(e => e.RProveedor).HasColumnName("R_Proveedor");

                entity.HasOne(d => d.RCategoriaNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.RCategoria)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_R_Categoria");

                entity.HasOne(d => d.RProveedorNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.RProveedor)
                    .HasConstraintName("FK_R_ProveedorProve");
            });

            modelBuilder.Entity<Proveedores>(entity =>
            {
                entity.HasKey(e => e.IdProveedores)
                    .HasName("PRIMARY");

                entity.ToTable("proveedores");

                entity.Property(e => e.IdProveedores).HasColumnName("idProveedores");

                entity.Property(e => e.Correo)
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ProveedorNombre)
                    .IsRequired()
                    .HasColumnName("Proveedor_Nombre")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Telefono)
                    .IsRequired()
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Registro>(entity =>
            {
                entity.HasKey(e => e.IdAcciones)
                    .HasName("PRIMARY");

                entity.ToTable("registro");

                entity.HasIndex(e => e.RUsuario)
                    .HasName("FK_UsuarioRegistro_idx");

                entity.Property(e => e.IdAcciones).HasColumnName("idAcciones");

                entity.Property(e => e.Detalles)
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FechaHora)
                    .HasColumnName("Fecha_Hora")
                    .HasColumnType("datetime");

                entity.Property(e => e.RUsuario).HasColumnName("R_Usuario");

                entity.HasOne(d => d.RUsuarioNavigation)
                    .WithMany(p => p.Registro)
                    .HasForeignKey(d => d.RUsuario)
                    .HasConstraintName("FK_UsuarioRegistro");
            });

            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                    .HasName("PRIMARY");

                entity.ToTable("usuarios");

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Token)
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Ventas>(entity =>
            {
                entity.HasKey(e => e.IdVentas)
                    .HasName("PRIMARY");

                entity.ToTable("ventas");

                entity.HasIndex(e => e.RCliente)
                    .HasName("FK_R_Cliente_idx");

                entity.HasIndex(e => e.RUsuario)
                    .HasName("FK_R_UsuarioVentas_idx");

                entity.Property(e => e.IdVentas).HasColumnName("idVentas");

                entity.Property(e => e.FechaHora).HasColumnType("datetime");

                entity.Property(e => e.RCliente).HasColumnName("R_Cliente");

                entity.Property(e => e.RUsuario).HasColumnName("R_Usuario");

                entity.HasOne(d => d.RClienteNavigation)
                    .WithMany(p => p.Ventas)
                    .HasForeignKey(d => d.RCliente)
                    .HasConstraintName("FK_R_ClienteVentas");

                entity.HasOne(d => d.RUsuarioNavigation)
                    .WithMany(p => p.Ventas)
                    .HasForeignKey(d => d.RUsuario)
                    .HasConstraintName("FK_R_UsuarioVentas");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

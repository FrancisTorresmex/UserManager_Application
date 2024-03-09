using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace userManagerAplication.Models.Data;

public partial class UserManagerAplicationContext : DbContext
{
    public UserManagerAplicationContext()
    {
    }

    public UserManagerAplicationContext(DbContextOptions<UserManagerAplicationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccessScreen> AccessScreens { get; set; }

    public virtual DbSet<Screen> Screens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersRole> UsersRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlServer("Server=LAPTOP-TF5M9SUU;Database=UserManagerAplication;Trusted_Connection=True;TrustServerCertificate=True;");
    } 
 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccessScreen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AccessSc__3214EC0715B77227");

            entity.ToTable("AccessScreen");

            entity.HasOne(d => d.IdScreenNavigation).WithMany(p => p.AccessScreens)
                .HasForeignKey(d => d.IdScreen)
                .HasConstraintName("FK__AccessScr__IdScr__2B3F6F97");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.AccessScreens)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK__AccessScr__IdUse__2A4B4B5E");
        });

        modelBuilder.Entity<Screen>(entity =>
        {
            entity.HasKey(e => e.IdScreen).HasName("PK__Screens__E3DFB755BEE484EC");

            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.Name).IsUnicode(false);
            entity.Property(e => e.Url).IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__Users__B7C92638DC21B88C");

            entity.Property(e => e.DateAdmision).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.InactiveDate).HasColumnType("datetime");
            entity.Property(e => e.LastName).IsUnicode(false);
            entity.Property(e => e.Name).IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .IsUnicode(false);

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .HasConstraintName("FK__Users__IdRole__267ABA7A");
        });

        modelBuilder.Entity<UsersRole>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PK__UsersRol__B436905444642EF5");

            entity.ToTable("UsersRole");

            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

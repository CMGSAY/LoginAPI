using System;
using System.Collections.Generic;
using LoginAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginAPI.Data;

public partial class LoginDbContext : DbContext
{
    public LoginDbContext()
    {
    }

    public LoginDbContext(DbContextOptions<LoginDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Invitation> Invitations { get; set; }

    public virtual DbSet<Jwk> Jwks { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<ProjectConfig> ProjectConfigs { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioRol> UsuarioRols { get; set; }

    public virtual DbSet<Verification> Verifications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=ep-red-bar-aydblizd-pooler.c-5.us-east-2.aws.neon.tech; Database=neondb; Username=neondb_owner; Password=npg_gxfdBNWpYc36; SSL Mode=VerifyFull; Channel Binding=Require;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pg_session_jwt");

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("account_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.User).WithMany(p => p.Accounts).HasConstraintName("account_userId_fkey");
        });

        modelBuilder.Entity<Invitation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("invitation_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Inviter).WithMany(p => p.Invitations).HasConstraintName("invitation_inviterId_fkey");

            entity.HasOne(d => d.Organization).WithMany(p => p.Invitations).HasConstraintName("invitation_organizationId_fkey");
        });

        modelBuilder.Entity<Jwk>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("jwks_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("member_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(d => d.Organization).WithMany(p => p.Members).HasConstraintName("member_organizationId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Members).HasConstraintName("member_userId_fkey");
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("organization_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        });

        modelBuilder.Entity<ProjectConfig>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("project_config_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("rol_pkey");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("session_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.User).WithMany(p => p.Sessions).HasConstraintName("session_userId_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("usuario_pkey");

            entity.Property(e => e.Estado).HasDefaultValueSql("'activo'::character varying");
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<UsuarioRol>(entity =>
        {
            entity.HasKey(e => e.IdUsuarioRol).HasName("usuario_rol_pkey");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.UsuarioRols)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuario_rol_id_rol_fkey");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioRols)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuario_rol_id_usuario_fkey");
        });

        modelBuilder.Entity<Verification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("verification_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LoginAPI.Models;

[Table("usuario")]
[Index("CorreoInstitucional", Name = "usuario_correo_institucional_key", IsUnique = true)]
public partial class Usuario
{
    [Key]
    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("correo_institucional")]
    [StringLength(150)]
    public string CorreoInstitucional { get; set; } = null!;

    [Column("contrasena_hash")]
    [StringLength(255)]
    public string ContrasenaHash { get; set; } = null!;

    [Column("estado")]
    [StringLength(20)]
    public string? Estado { get; set; }

    [Column("fecha_creacion", TypeName = "timestamp without time zone")]
    public DateTime? FechaCreacion { get; set; }

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<UsuarioRol> UsuarioRols { get; set; } = new List<UsuarioRol>();
}

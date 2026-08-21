using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LoginAPI.Models;

[Table("usuario_rol")]
[Index("IdUsuario", "IdRol", Name = "usuario_rol_id_usuario_id_rol_key", IsUnique = true)]
public partial class UsuarioRol
{
    [Key]
    [Column("id_usuario_rol")]
    public int IdUsuarioRol { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("id_rol")]
    public int IdRol { get; set; }

    [ForeignKey("IdRol")]
    [InverseProperty("UsuarioRols")]
    public virtual Rol IdRolNavigation { get; set; } = null!;

    [ForeignKey("IdUsuario")]
    [InverseProperty("UsuarioRols")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}

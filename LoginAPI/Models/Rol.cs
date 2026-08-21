using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LoginAPI.Models;

[Table("rol")]
public partial class Rol
{
    [Key]
    [Column("id_rol")]
    public int IdRol { get; set; }

    [Column("nombre_rol")]
    [StringLength(50)]
    public string NombreRol { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [InverseProperty("IdRolNavigation")]
    public virtual ICollection<UsuarioRol> UsuarioRols { get; set; } = new List<UsuarioRol>();
}

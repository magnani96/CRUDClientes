using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CRUDPractico.Models.Entities
{
    public class Cliente
    {
        [Key]
        public long Cuil { get; set; }
        public string RazonSocial { get; set; }
        public int Telefono { get; set; }
        public string? Direccion { get; set; }
        public bool Activo { get; set; }
    }
}

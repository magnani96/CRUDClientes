using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CRUDPractico.Models
{
    public class AgregarClienteViewModel
    {
        public long Cuil { get; set; }
        public int Telefono { get; set; }
        public string? Direccion { get; set; }
        public bool Activo { get; set; }
    }
}

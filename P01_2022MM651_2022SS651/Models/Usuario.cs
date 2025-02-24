using System.ComponentModel.DataAnnotations;

namespace P01_2022MM651_2022SS651.Models
{
    public class Usuario
    {
        [Key]
        public int id_usuario {  get; set; }
        public string? nombre { get; set; }
        public string? correo { get; set; }
        public string? telefono { get; set; }
        public string? contrasena { get; set; }
        public string? rol { get; set; }
    }
}

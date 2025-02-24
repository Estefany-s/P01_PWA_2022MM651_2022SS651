using System.ComponentModel.DataAnnotations;

namespace P01_2022MM651_2022SS651.Models
{
    public class Sucursal
    {
        [Key]
        public int id_sucursal {  get; set; }
        public string? nombreSucursal { get; set; }
        public string? direccion { get; set; }
        public string? telefono { get; set; }
        public string? administrador { get; set; }
        public int cantEspacios { get; set; }
    }
}

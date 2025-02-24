using System.ComponentModel.DataAnnotations;

namespace P01_2022MM651_2022SS651.Models
{
    public class DetalleSucursal
    {
        [Key]
        public int id_detalleSucursal {  get; set; }
        public int numero { get; set; }
        public string? ubicacion { get; set; }
        public decimal costoHora { get; set; }
        public string? estado {  get; set; }
        public int id_sucursal {  get; set; }
    }
}

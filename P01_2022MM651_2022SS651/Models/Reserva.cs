using System.ComponentModel.DataAnnotations;

namespace P01_2022MM651_2022SS651.Models
{
    public class Reserva
    {
        [Key]
        public int id_reserva {  get; set; }
        public DateTime fechaHora { get; set; }
        public int cantHoras { get; set; }
        public int id_usuario { get; set; }
        public int id_sucursal { get; set; }

    }
}

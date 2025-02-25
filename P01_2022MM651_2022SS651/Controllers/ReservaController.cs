using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_2022MM651_2022SS651.Models;

namespace P01_2022MM651_2022SS651.Controllers
{
    public class ReservaController : Controller
    {
        private readonly parqueoContext _parqueoContexto;

        public ReservaController(parqueoContext parqueoContexto)
        {
            _parqueoContexto = parqueoContexto;
        }

        // crear reserva de parqueo solo usuario auntenticado puede, osea un empleado 
        [HttpPost]
        [Route("CrearReserva")]
        public IActionResult CrearReserva([FromBody] Reserva nuevaReserva)
        {

            // Ver si hay sucursales
            var sucursal = (from s in _parqueoContexto.Sucursal
                            where s.id_sucursal == nuevaReserva.id_sucursal
                            select s).FirstOrDefault();

            if (sucursal == null)
            {
                return NotFound(new { mensaje = "Sucursal no encontrada" });
            }

            // Ver si hay usuarios o no 
            var usuario = (from u in _parqueoContexto.Usuario
                           where u.id_usuario == nuevaReserva.id_usuario
                           select u).FirstOrDefault();
            if (usuario == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }

            // Validar que el usuario tenga el rol de empleado
            if (usuario.rol != "empleado")
            {
                return Unauthorized(new { mensaje = "Solo empleados pueden realizar reservas" });
            }


            _parqueoContexto.Reserva.Add(nuevaReserva);
            _parqueoContexto.SaveChanges();

            return Ok(new { mensaje = "Reserva creada exitosamente", datos = nuevaReserva });
        }

        // Mostrar una lista de reservas activas del usuario.
        [HttpGet]
        [Route("reservasActivasPorUsuario/{id_usuario}")]
        public IActionResult ObtenerReservasActivasPorUsuario(int id_usuario)
        {
            var listadoReservasActivas = (from r in _parqueoContexto.Reserva
                                          join u in _parqueoContexto.Usuario on r.id_usuario equals u.id_usuario
                                          join s in _parqueoContexto.Sucursal on r.id_sucursal equals s.id_sucursal
                                          where r.id_usuario == id_usuario && r.estado == "activo"
                                          select new
                                          {
                                              r.fechaHora,
                                              r.cantHoras,
                                              u.nombre,
                                              s.nombreSucursal,
                                              r.estado
                                          }).ToList();

            if (listadoReservasActivas.Count == 0)
            {
                return NotFound(new { mensaje = "El usuario no tiene reservas activas." });
            }

            return Ok(listadoReservasActivas);
        }

        //Cancelar una reserva
        [HttpPut]
        [Route("/cancelarReserva/{id}")]
        public IActionResult cancelarReserva(int id, [FromBody] Reserva _cancelarReserva)
        {
            Reserva? reservaActual = (from r in _parqueoContexto.Reserva where r.id_reserva == id select r).FirstOrDefault();
            if (reservaActual == null)
            {
                return NotFound();
            }

            reservaActual.estado = _cancelarReserva.estado;

            if(reservaActual.estado != "inactiva")
            {
                return Unauthorized(new { mensaje = "Solo se pueden cancelar las reservas" });
            }

            _parqueoContexto.Entry(reservaActual).State = EntityState.Modified;
            _parqueoContexto.SaveChanges();

            return Ok(new { mensaje = "Reserva cancelada exitosamente", _cancelarReserva });
        }

        //Mostrar una lista de los espacios reservados por día de todas las sucursales.
        [HttpGet]
        [Route("ReservasXDia")]
        public IActionResult ObtenerReservasXDia()
        {
            var reservas = (from r in _parqueoContexto.Reserva
                            join s in _parqueoContexto.Sucursal on r.id_sucursal equals s.id_sucursal
                            group r by new { Fecha = r.fechaHora.Date, s.id_sucursal, s.nombreSucursal } into colReserva
                            select new
                            {
                                NombreSucursal = colReserva.Key.nombreSucursal,
                                EspaciosReservados = colReserva.Count(),
                                Fecha = colReserva.Key.Fecha
                            }).OrderBy(r => r.Fecha).ToList();

            if (reservas.Count == 0)
            {
                return NotFound();
            }

            return Ok(reservas);
        }

        //Mostrar una lista de los espacios reservados entre dos fechas dadas de una sucursal especifica.
        [HttpGet]
        [Route("ReservasEntreFechas")]
        public IActionResult ObtenerReservasEntreFechas(DateTime fechaInicio, DateTime fechaFin, int idSucursal)
        {
            var reservas = (from r in _parqueoContexto.Reserva
                            join s in _parqueoContexto.Sucursal on r.id_sucursal equals s.id_sucursal
                            where r.fechaHora >= fechaInicio && r.fechaHora <= fechaFin
                                  && r.id_sucursal == idSucursal
                            select new
                            {
                                FechaHora = r.fechaHora,
                                CantidadHoras = r.cantHoras,
                                NombreSucursal = s.nombreSucursal
                            })
                            .OrderBy(r => r.FechaHora)
                            .ToList();

            if (reservas.Count == 0)
            {
                return NotFound(new { mensaje = "No hay reservas en el rango de fechas especificado para esta sucursal." });
            }

            return Ok(reservas);
        }


    }
}

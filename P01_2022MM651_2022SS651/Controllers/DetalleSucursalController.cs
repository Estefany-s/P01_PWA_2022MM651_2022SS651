using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_2022MM651_2022SS651.Models;

namespace P01_2022MM651_2022SS651.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleSucursalController : Controller
    {
        private readonly parqueoContext _parqueoContexto;

        public DetalleSucursalController(parqueoContext parqueoContexto)
        {
            _parqueoContexto = parqueoContexto;
        }

        // Registrar nuevos espacios de parqueo con número, ubicación, costo por hora y estado
        // (disponible/ocupado) por sucursal.
        [HttpPost]
        [Route("registrarNuevosEspacios/{id_sucursal}")]
        public IActionResult RegistrarNuevosEspacios([FromBody] DetalleSucursal _detalleSucursal)
        {
            try
            {
                _parqueoContexto.DetalleSucursal.Add(_detalleSucursal);
                _parqueoContexto.SaveChanges();
                return Ok(_detalleSucursal);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Error de base de datos: {ex.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error general: {ex.Message}");
            }
        }

        // Mostrar una lista de todos los espacios de parqueo disponibles.
        [HttpGet]
        [Route("parqueosDisponibles")]
        public IActionResult obtenerParqueosDisponibles()
        {

            var listadoDetalleSucursal = (from d in _parqueoContexto.DetalleSucursal
                                          join Sucursal s in _parqueoContexto.Sucursal
                                            on d.id_sucursal equals s.id_sucursal
                                          where d.estado == "Disponible"
                                          select new
                                          {
                                              d.numero,
                                              d.ubicacion,
                                              d.costoHora,
                                              d.estado,
                                              s.nombreSucursal
                                          }).ToList();

            if (listadoDetalleSucursal.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoDetalleSucursal);
        }

        // Obtener la información de los espacios 
        [HttpGet]
        [Route("obtenerDetalleSucursales")]
        public IActionResult obtenerDetalleSucursal()
        {

            var listadoDetalleSucursal = (from d in _parqueoContexto.DetalleSucursal
                                          join Sucursal s in _parqueoContexto.Sucursal
                                            on d.id_sucursal equals s.id_sucursal
                                          select new
                                          {
                                              d.numero,
                                              d.ubicacion,
                                              d.costoHora,
                                              d.estado,
                                              s.nombreSucursal
                                          }).ToList();

            if (listadoDetalleSucursal.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoDetalleSucursal);
        }


    }
}

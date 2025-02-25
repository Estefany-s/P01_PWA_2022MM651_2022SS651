using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_2022MM651_2022SS651.Models;

namespace P01_2022MM651_2022SS651.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SucursalController : Controller
    {
        private readonly parqueoContext _parqueoContexto;

        public SucursalController(parqueoContext parqueoContexto)
        {
            _parqueoContexto = parqueoContexto;
        }

        //CRUD de sucursales
        [HttpGet]
        [Route("obtenerSucursales")]
        public IActionResult obtenerSucursal()
        {
            List<Sucursal> listadoSucursal = (from s in _parqueoContexto.Sucursal select s).ToList();

            if (listadoSucursal.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoSucursal);
        }

        [HttpPost]
        [Route("agregarSucursal")]
        public IActionResult AgregarSucursal([FromBody] Sucursal _sucursal)
        {
            try
            {
                _parqueoContexto.Sucursal.Add(_sucursal);
                _parqueoContexto.SaveChanges();
                return Ok(_sucursal);
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

        // En este Actualizar correspondiente al CRUD de Sucursales no se actualiza la cantidad de espacios
        // ya que eso se trabaja más abajo en los otros endpoints.
        [HttpPut]
        [Route("actualizarSucursal/{id_sucursal}")]
        public IActionResult actualizarSucursal(int id, [FromBody] Sucursal actualizarSucursal)
        {
            Sucursal? sucursalActual = (from s in _parqueoContexto.Sucursal where s.id_sucursal == id select s).FirstOrDefault();
            if (sucursalActual == null)
            {
                return NotFound();
            }

            sucursalActual.nombreSucursal = actualizarSucursal.nombreSucursal;
            sucursalActual.direccion = actualizarSucursal.direccion;
            sucursalActual.telefono = actualizarSucursal.telefono;
            sucursalActual.administrador = actualizarSucursal.administrador;

            _parqueoContexto.Entry(sucursalActual).State = EntityState.Modified;
            _parqueoContexto.SaveChanges();

            return Ok(actualizarSucursal);
        }

        [HttpDelete]
        [Route("eliminarSucursal/{id_sucursal}")]
        public IActionResult EliminarSucursal(int id)
        {
            Sucursal? sucursal = (from s in _parqueoContexto.Sucursal where s.id_sucursal == id select s).FirstOrDefault();

            if (sucursal == null) { return NotFound(); }

            _parqueoContexto.Sucursal.Attach(sucursal);
            _parqueoContexto.Sucursal.Remove(sucursal);
            _parqueoContexto.SaveChanges();

            return Ok(sucursal);
        }

        // Actualizar información de un espacio de parqueo.
        [HttpPut]
        [Route("actualizarEspacioDeParqueo/{id_sucursal}")]
        public IActionResult ActualizarEspacioDeParqueo(int id, [FromBody] Sucursal actualizarSucursal)
        {
            Sucursal? sucursalActual = (from s in _parqueoContexto.Sucursal where s.id_sucursal == id select s).FirstOrDefault();
            if (sucursalActual == null)
            {
                return NotFound();
            }

            sucursalActual.cantEspacios = actualizarSucursal.cantEspacios;

            _parqueoContexto.Entry(sucursalActual).State = EntityState.Modified;
            _parqueoContexto.SaveChanges();

            return Ok(actualizarSucursal);
        }

        // Eliminar un espacio de parqueo.
        // Aqui como se trata de eliminar se valida que la actualización del dato sea un número menor que la cantidad que ya tenía
        [HttpPut]
        [Route("eliminarEspacioDeParqueo/{id_sucursal}")]
        public IActionResult EliminarEspacioDeParqueo(int id, [FromBody] Sucursal actualizarSucursal)
        {
            Sucursal? sucursalActual = _parqueoContexto.Sucursal.FirstOrDefault(s => s.id_sucursal == id);

            if (sucursalActual == null)
            {
                return NotFound();
            }

            // Validar que la nueva cantidad de espacios sea menor que la actual
            if (actualizarSucursal.cantEspacios >= sucursalActual.cantEspacios)
            {
                return BadRequest("El nuevo valor de espacios debe ser menor que el actual.");
            }

            sucursalActual.cantEspacios = actualizarSucursal.cantEspacios;

            _parqueoContexto.Entry(sucursalActual).State = EntityState.Modified;
            _parqueoContexto.SaveChanges();

            return Ok(sucursalActual);
        }



    }
}

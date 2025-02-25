using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_2022MM651_2022SS651.Models;

namespace P01_2022MM651_2022SS651.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly parqueoContext _parqueoContexto;

        public UsuarioController(parqueoContext parqueoContexto)
        {
            _parqueoContexto = parqueoContexto;
        }

        //Create user
        [HttpPost]
        [Route("/AgregarUsuario")]
        public IActionResult AgregarUsuario([FromBody] Usuario _usuario)
        {
            try
            {
                _parqueoContexto.Usuario.Add(_usuario);
                _parqueoContexto.SaveChanges();
                return Ok(_usuario);
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

        //Login user, se uso el correo para que no hayan dos usuaros iguales
        [HttpGet]
        [Route("/InicioSesion")]
        public IActionResult Login(string _correo, string _contra)
        {
            var login = (from u in _parqueoContexto.Usuario
                         where u.correo == _correo && u.contrasena == _contra
                         select new
                         {
                             u.nombre,
                             u.correo,
                             u.telefono,
                             u.rol
                         }).ToList();

            if (login == null || login.Count == 0)
            {
                return NotFound(new { mensaje = "Credenciales invalidas" });
            }

            return Ok(new { mensaje = "Credenciales validas", datos = login });
        }

        //CRUD de usuario
        //See user
        [HttpGet]
        [Route("/ObtenerUsuarios")]
        public IActionResult obtenerUsuario()
        {
            List<Usuario> listadoUsuarios = (from u in _parqueoContexto.Usuario select u).ToList();

            if (listadoUsuarios.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoUsuarios);
        }

        //Actualizar user
        [HttpPut]
        [Route("/actualizarUsuario/{id}")]
        public IActionResult actualizarUsuario(int id, [FromBody] Usuario actualizarUsuario)
        {
            Usuario? usuarioActual = (from u in _parqueoContexto.Usuario where u.id_usuario == id select u).FirstOrDefault();
            if (usuarioActual == null)
            {
                return NotFound();
            }

            usuarioActual.nombre = actualizarUsuario.nombre;
            usuarioActual.correo = actualizarUsuario.correo;
            usuarioActual.telefono = actualizarUsuario.telefono;
            usuarioActual.contrasena = actualizarUsuario.contrasena;
            usuarioActual.rol = actualizarUsuario.rol;

            _parqueoContexto.Entry(usuarioActual).State = EntityState.Modified;
            _parqueoContexto.SaveChanges();

            return Ok(actualizarUsuario);
        }

        //Delete user
        [HttpDelete]
        [Route("/eliminarUsuario/{id}")]
        public IActionResult EliminarUsuario(int id)
        {
            Usuario? usuario = (from u in _parqueoContexto.Usuario where u.id_usuario == id select u).FirstOrDefault();

            if (usuario == null) { return NotFound(); }

            _parqueoContexto.Usuario.Attach(usuario);
            _parqueoContexto.Usuario.Remove(usuario);
            _parqueoContexto.SaveChanges();

            return Ok(usuario);
        }
    }
}

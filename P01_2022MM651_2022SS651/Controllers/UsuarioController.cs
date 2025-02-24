using Microsoft.AspNetCore.Mvc;
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
    }
}

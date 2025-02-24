using Microsoft.AspNetCore.Mvc;
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
    }
}

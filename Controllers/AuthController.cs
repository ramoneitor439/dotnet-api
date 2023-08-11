using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Context;
using SocialNetwork.Dto;
using SocialNetwork.Security;

namespace SocialNetwork.Controllers
{
    [ApiController]
    [Route("api/v1/login")]
    public class AuthController : ControllerBase
    {
        /*
         Aqui es donde vas a usar el token por asi decirlo, es el Endpoint de Login. 
         Un endpoint por si no lo sabes en solamente un punto de salida del api, o sea
         un url terminado por ejemplo en ('api/v1/login') que va a dar una cierta respuesta
         */
        private DataBaseContext _context;
        private IConfiguration _configuration; /* 
                                                * Esto que esta aqui es para poder trabajar
                                                * con appsettings.json Esto en produccion no lo 
                                                * hagas a menos que el proyecto lo requiera,
                                                * en appsettings.json no se guardan datos ni del 
                                                * token ni de la base de datos etc.
                                                * */

        public AuthController(DataBaseContext context, IConfiguration configuration) 
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserAuthDto credentials)
        {
            /* Cuando recibes los datos de username y password buscas al usuario por el username */
            var user = await _context.Users.Where(u => u.UserName == credentials.UserName).FirstOrDefaultAsync();
            if(user is null)
                return NotFound();
            // Si aparece y la password esta bien (Cabe destacar que la password debe estar hasheada)
            if (!user.CheckPassword(credentials.Password))
                return BadRequest();

            // Entonces usas este metodo para generar un token y lo devuelves
            var jwtHelper = new JWTTools(_configuration, _context);
            string token = jwtHelper.GenerateToken(user);
            return Ok(token);
        }
    }
}

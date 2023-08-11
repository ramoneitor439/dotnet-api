using Microsoft.IdentityModel.Tokens;
using SocialNetwork.Context;
using SocialNetwork.Dto;
using SocialNetwork.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialNetwork.Security
{
    public class JWTTools 
    {
        private IConfiguration _configuration;
        private readonly DataBaseContext _context;

        public JWTTools(IConfiguration configuration, DataBaseContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        /*
         Aqui vas a generar el token para la autorizacion, este token es super importante
         pues es el que te va a garantizar que la gente no se meta donde no debe y es 
         practicamente la base de la seguridad de tu API. Ahora te voy a explicar paso a paso
         */

        public string GenerateToken(User user/* Para hacer el token vas a tomar al usuario que se esta logueando */)
        {
            if (user != null)
            {
                // Aqui saco el nombre del rol del usuario ya que solo tengo el Id que le hace referencia
                var role = _context.Roles.Where(r => r.Id == user.RoleId).FirstOrDefault();

                /* 
                  Esta linea que esta medio rara es para sacar las propiedades que hacen falta para
                  generar el token, cabe destacar que el lugar donde las guarde no es seguro
                  y en una aplicacion real hay que meterlas en una variable de entorno o algo asi
                */
                var jwt = _configuration.GetSection("Jwt").Get<Jwt>();


                /*
                 Aqui mas abajo voy a estar sacando los "claims", que son propiedades que 
                 quiero guardar en el token, como el id del usuario, su rol etc.
                 Esto va a servir para despues usando el token saber que usuario es el que
                 esta accediendo al sistema y manejar los filtros y permisos.
                 */
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, "Subject"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    // Esos primeros tres son valores por defecto que se guardan en el token,
                    // eso se planifica con mas calma en un proyecto real
                    new Claim("Id", user.Id.ToString()),
                    new Claim("UserName", user.UserName),
                    new Claim("Email", user.Email),
                    new Claim("Role", (role is null) ? "None" : role.Name)
                    /* 
                     Si te fijas aqui mas abajo si meti algunas propiedades del usuario 
                     para despues hacer verificaciones
                     */
                };

                /*
                 Aqui estoy sacando la clave de serializacion del token, esto es para 
                 que el token sea unico y no puedan falsificarlo, es importante que la
                 Key del token permanezca en un lugar seguro *OJO*
                 */
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));

                /*
                 Esta propiedad se saca para lo mismo que el key, para hacer que el token no
                 se pueda falsificar
                 */
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                /*
                 Ya aqui es donde se crea el token, que va a ser un objeto con un grupo de propiedades
                 que luego va a ser escrito es forma de string como vas a ver mas abajo
                 */
                var token = new JwtSecurityToken(
                                    jwt.Issuer,
                                    jwt.Audience,
                                    claims,
                                    expires: DateTime.Now.AddMinutes(20),
                                    signingCredentials: signIn
                                    );

                /* Ya al final se convierte el token a string y se devuelve */
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            return "Invalid User";
        }

    }
}

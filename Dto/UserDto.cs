using SocialNetwork.Models;
using System.Text.Json.Serialization;

namespace SocialNetwork.Dto
{
    /*
     No se si sabes que es un DTO, asi que lo explico igual. Un DTO es un Data Transfer Object
     esta cosita se encarga de mantener una estructura que es la que le vas a enviar al frontend
     (Por ejemplo, reemplazas tus foraneas de Id por una instancia del objeto para que haya claridad
     en los datos). 
     */
    public class UserDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("username")]
        public string UserName { get; set; } = null!;

        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;

        [JsonPropertyName("role")]
        public string Role { get; set; } = null!;

        [JsonPropertyName("posts")]
        public IEnumerable<PostSimpleDto>? Posts { get; set; }
    }

    public class UserSimpleDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("username")]
        public string UserName { get; set; } = null!;

        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;
    }

    /*
     Puedes tener varios DTO en dependencia de lo que quieras hacer con ellos.
     Por ejemplo, para la autenticacion solo necesito el usuario y la password, asi que cree un DTO
     con esos dos atributos para no cargar por gusto el body de la request que va a llegar desde el frontend
     */

    public class UserAuthDto
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; } = null!;

        [JsonPropertyName("password")]
        public string Password { get; set; } = null!;
    }

}

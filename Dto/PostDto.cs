using Microsoft.EntityFrameworkCore;
using SocialNetwork.Models;
using System.Text.Json.Serialization;

namespace SocialNetwork.Dto
{
    /*
     No se si sabes que es un DTO, asi que lo explico igual. Un DTO es un Data Transfer Object
     esta cosita se encarga de mantener una estructura que es la que le vas a enviar al frontend
     (Por ejemplo, reemplazas tus foraneas de Id por una instancia del objeto para que haya claridad
     en los datos)
     */
    public class PostDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = null!;

        [JsonPropertyName("content")]
        public string Content { get; set; } = null!;

        [JsonPropertyName("user")]
        public UserSimpleDto User { get; set; } = null!;

    }

    public class PostSimpleDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = null!;

        [JsonPropertyName("content")]
        public string Content { get; set; } = null!;
    }
}

namespace SocialNetwork.Security
{
    /* 
     Esto no es muy importante, es solo un modelo que hice para guardar
     la estructura de lo que va a necesitar el token, poca cosa
     */
    public class Jwt
    {
        public string Key { get; set; } = null!;

        public string Issuer { get; set; } = null!;

        public string Audience { get; set; } = null!;

        public string Subject { get; set; } = null!;
    }
}

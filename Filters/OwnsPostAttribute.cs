using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialNetwork.Context;
using SocialNetwork.Identity;

namespace SocialNetwork.Filters
{
    /*
     Esto que hice aqui es un filtro, se usa como un decorador sobre una funcion o una clase
     y lo que hace es que ejecuta un cierto metodo antes de la ejecucion de una funcion 
     determinada.
     */
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class OwnsPostAttribute : Attribute, IAuthorizationFilter
    {
        /*
         Aqui voy a verificar si el usuario que esta realizando el request es dueño del post
         que quiere manipular, este filtro solo lo voy a usar para PostsController
         */

        // Sacas tu context de EntityFramework para buscar el Post
        private readonly DataBaseContext _context = new DataBaseContext();

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Aqui se recoge el {id} que esta en el url de la request y se mete en idValue
            if (context.RouteData.Values.TryGetValue("id", out var idValue))
            {
                // Revisas si no es nulo y coincide con el formato que debe tener un id
                if (idValue != null && Guid.TryParse(idValue.ToString(), out Guid post_id))
                {
                    /*
                     Luego de tener el {id} y que sea valido vas a buscar el Post que tenga ese id
                     para luego verificar si el usuario dueño de ese Post es el mismo que el del token
                     */
                    var post = _context.Posts.Find(post_id);
                    if (post != null)
                    {
                        // Si no coincide como ves aqui... 403
                        if (!context.HttpContext.User.HasClaim(IdentityData.IdClaim, post.UserId.ToString()))
                        {
                            context.Result = new UnauthorizedResult();
                        }
                    }
                    else
                    {
                        context.Result = new UnauthorizedResult();
                    }
                }
                else
                {
                    context.Result = new UnauthorizedResult();
                }
            }
        }
    }
}

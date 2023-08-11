using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Identity;

namespace SocialNetwork.Filters
{
    /*
     Esto que hice aqui es un filtro, se usa como un decorador sobre una funcion o una clase
     y lo que hace es que ejecuta un cierto metodo antes de la ejecucion de una funcion 
     determinada.
     */
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class OwnsAccount : Attribute, IAuthorizationFilter
    {
        /*
         Aqui voy a verificar si el usuario que esta realizando el request es dueño de la cuenta
         que quiere manipular, este filtro solo lo voy a usar para UserController
         */
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Aqui se recoge el {id} que esta en el url de la request y se mete en idValue
            if (context.RouteData.Values.TryGetValue("id", out var idValue))
            {
                // Revisas si no es nulo y coincide con el formato que debe tener un id
                if (idValue != null && Guid.TryParse(idValue.ToString(), out Guid user_id))
                {
                    /*
                     Aqui es donde ves si el {user_id} del claim coincide con el {id}
                     del URL, eso quiere decir que el usuario es dueño de la cuenta y todo esta bien
                     */
                    if (!context.HttpContext.User.HasClaim(IdentityData.IdClaim, user_id.ToString()))
                    {
                            // Sino devuelves 403
                            context.Result = new UnauthorizedResult();
                    }
                }
                else
                {
                    // Sino devuelves 403
                    context.Result = new UnauthorizedResult();
                }
            }
        }
    }
}

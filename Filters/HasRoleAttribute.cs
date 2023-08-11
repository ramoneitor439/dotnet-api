using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialNetwork.Identity;

namespace SocialNetwork.Filters
{
    /*
     Esto que hice aqui es un filtro, se usa como un decorador sobre una funcion o una clase
     y lo que hace es que ejecuta un cierto metodo antes de la ejecucion de una funcion 
     determinada.
     */
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class HasRoleAttribute : Attribute, IAuthorizationFilter
    {
        /*
         Aqui voy a definir un filtro que verifique si el usuario que esta realizando una determinada
         peticion tiene un rol especifico. Si te das cuenta lo que voy a hacer es usar una variable
         para indicar que rol quiero en la funcion.
         */
        private readonly string _claimValue; // Aqui va a ir el nombre del rol

        public HasRoleAttribute(string claimValue)
        {
            _claimValue = claimValue;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            /*
             Esta funcion se va a ejecutar antes de todo, y va a verificar si en el token que 
             mando el usuario hay un claim que se llama "role": _claimValue <-- Aqui va el rol que desees
             */
            if (!context.HttpContext.User.HasClaim(IdentityData.RoleClaim, _claimValue))
            {
                //Si no se cumple esa condicion se cancela la request y devuelve un 403
                context.Result = new ForbidResult();
            }
        }
    }
}

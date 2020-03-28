using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace WebAPI_IdentityPlatformProtection_Example.Helper.Security
{
    public class Verifications
    {
        public static void ValidateAppRole(string appRole)
        {
            //
            // The `role` claim tells you what permissions the client application has in the service.
            // In this case, we look for a `role` value of `access_as_application`.
            //
            Claim roleClaim = ClaimsPrincipal.Current.FindFirst(ConfigurationManager.AppSettings["RoleFieldNameType1"]);
            if (roleClaim == null)
            {
                //using old format
                roleClaim = ClaimsPrincipal.Current.FindFirst(ConfigurationManager.AppSettings["RoleFieldNameType2"]);
            }

            if (roleClaim == null || !roleClaim.Value.Split(' ').Contains(appRole))
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ReasonPhrase = $"The 'roles' claim does not contain '{appRole}' or was not found."
                });
            }
        }

        public static void EnsureCallerHasAppOnlyToken()
        {
            string oid = ClaimsPrincipal.Current.FindFirst("oid")?.Value;
            string sub = ClaimsPrincipal.Current.FindFirst("sub")?.Value;
            bool isAppOnlyToken = oid == sub;
            if (!isAppOnlyToken)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ReasonPhrase = $"Provided token can only be issued with app roles."
                });
            }
        }
    }
}
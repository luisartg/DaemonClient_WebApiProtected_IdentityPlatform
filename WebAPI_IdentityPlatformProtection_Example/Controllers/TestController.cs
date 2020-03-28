using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI_IdentityPlatformProtection_Example.Models;
using WebAPI_IdentityPlatformProtection_Example.Helper.Security;
using System.Security.Claims;
using System.Threading;
using System.Configuration;

namespace WebAPI_IdentityPlatformProtection_Example.Controllers
{
    [Authorize]
    public class TestController : ApiController
    {
        [Route("api/Test")]
        public GenericResponse Post([FromBody] TestRequest request)
        {
            Verifications.ValidateAppRole(ConfigurationManager.AppSettings["AppRoleName"]);
            Verifications.EnsureCallerHasAppOnlyToken();

            var response = new GenericResponse();


            response.message = $"Hi {request.name}! You were able to access to the function. Ooohh weeee!!";
            return response;
        }
    }
}

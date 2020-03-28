using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(WebAPI_IdentityPlatformProtection_Example.Startup))]

namespace WebAPI_IdentityPlatformProtection_Example
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Invoke the ConfigureAuth method, which will set up
            // the OWIN authentication pipeline using OAuth 2.0

            ConfigureAuth(app);
        }
    }
}
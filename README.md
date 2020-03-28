# Daemon Client with Protected Web API

A simple example of a daemon client that uses an access token provided by Azure Active Directory (Azure AD) to be able to consume a Web API that is protected and expects a bearer access token and assigned app roles. 

The app roles are defined by the Web API application register in Azure AD, and are assigned and consented by an Admin in the daemon client application register.

This example follows these sources:

Web API protected
https://docs.microsoft.com/en-us/azure/active-directory/develop/scenario-protected-web-api-overview

Daemon client with app roles
https://docs.microsoft.com/en-us/azure/active-directory/develop/scenario-daemon-overview

# How to run the example

Follow the sources instructions to do the following:

1. Create Client and Web API application registers
1. Add app roles to the manifest of the Web API app register.
1. Add these app roles to the client app register, and give consent.
1. Open the proyect and fill the required app settings in both app.config and web.config
1. Press F5 or run the solution (both projects are configured to start at the same time). The client will execute, request a token, call the web api and print the result of the called function. If everything is correctly configured, the web api will authorize the call, check that the access token contains the expected scopes, and return an object with a string message.

> Note: If you plan to apply these configuration to one of your proyects, be mindful of the required versions of Owin, Newtonsoft and IdentityModel packages, since diferent versions may lead to unexpected errors, other modern versions just don't have the required classes or interfaces. Apply first this given configuration, then experiment with the versions.

> Note: Be careful when changing nuget packages in the Web API project. Even if Nuget Package Manager updates or downgardes the packages, Web.Config file also must be updated to the correct version of the used assembly, otherwise you won't be able to run the API without getting an error regarding to not finding the correct assembly version.
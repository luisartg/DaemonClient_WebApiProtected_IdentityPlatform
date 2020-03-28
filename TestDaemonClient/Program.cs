using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TestDaemonClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string requestUrl = ConfigurationManager.AppSettings["RequestURL"];
            string ClientID = ConfigurationManager.AppSettings["ClientID"];
            string TenantID = ConfigurationManager.AppSettings["TenantID"];
            string Authority = $"https://login.microsoftonline.com/{TenantID}/v2.0";
            string ClientSecret = ConfigurationManager.AppSettings["ClientSecret"];
            string[] scopes = ConfigurationManager.AppSettings["Scopes"].ToString().Split(',');

            //Setting app with MSAL
            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(ClientID)
           .WithClientSecret(ClientSecret)
           .WithAuthority(new Uri(Authority))
           .Build();

            //requesting token
            AuthenticationResult result = null;
            try
            {
                Task<AuthenticationResult> authTask = app.AcquireTokenForClient(scopes)
                                 .ExecuteAsync();
                //authTask.Start();
                authTask.Wait();
                result = authTask.Result;
            }
            catch (MsalUiRequiredException ex)
            {
                // The application doesn't have sufficient permissions.
                // - Did you declare enough app permissions during app creation?
                // - Did the tenant admin grant permissions to the application?
            }
            catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
            {
                // Invalid scope. The scope has to be in the form "https://resourceurl/.default"
                // Mitigation: Change the scope to be as expected.
            }


            // Call Web API
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            string json = "{'name':'Mr. Testbuddy'}";
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            Task<HttpResponseMessage> postTask = httpClient.PostAsync(requestUrl, data);
            postTask.Wait();

            var webApiResponse = postTask.Result;

            Task<string> messageTask = webApiResponse.Content.ReadAsStringAsync();
            messageTask.Wait();

            Console.WriteLine(messageTask.Result);
            Console.ReadLine();
        }
    }
}

using MVC.Resources.Constants;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MVC
{
    public class AuthorizationHeaderHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            AuthenticationHeaderValue authorization = request.Headers.Authorization;
            string userName = null;
            string password = null;
            // Verification.   
            if (request.Headers.TryGetValues(ApiInfo.API_KEY_HEADER, out var apiKeyHeaderValues) &&
                !string.IsNullOrEmpty(authorization.Parameter))
            {
                var apiKeyHeaderValue = apiKeyHeaderValues.First();
                // Get the auth token   
                string authToken = authorization.Parameter;
                // Decode the token from BASE64   
                string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                // Extract username and password from decoded token   
                userName = decodedToken.Substring(0, decodedToken.IndexOf(":"));
                password = decodedToken.Substring(decodedToken.IndexOf(":") + 1);
                // Verification.   
                if (apiKeyHeaderValue.Equals(ApiInfo.API_KEY_VALUE) && userName.Equals(ApiInfo.USERNAME_VALUE) &&
                    password.Equals(ApiInfo.PASSWORD_VALUE))
                {
                    // Setting   
                    var identity = new GenericIdentity(userName);
                    SetPrincipal(new GenericPrincipal(identity, null));
                }
            }
            return base.SendAsync(request, cancellationToken).ContinueWith(task =>
            {
                var response = task.Result;
                response.Headers.Add("Suppress-Redirect", "True");
                return response;
            }, cancellationToken); ;
        }

        private static void SetPrincipal(IPrincipal principal)
        {
            // setting.   
            Thread.CurrentPrincipal = principal;
            // Verification.   
            if (HttpContext.Current != null)
            {
                // Setting.   
                HttpContext.Current.User = principal;
            }
        }
    }
}
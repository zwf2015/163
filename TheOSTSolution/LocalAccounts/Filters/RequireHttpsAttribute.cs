using System;

using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace LocalAccounts.Filters
{
    /// <summary>
    /// 自定义的Filter
    /// </summary>
    public class RequireHttpsAttribute:AuthorizationFilterAttribute
    {
        public int Port { get; set; }

        public RequireHttpsAttribute()
        {
            Port = 443;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var request = actionContext.Request;

            if (request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden) { ReasonPhrase = "HTTPS Required" };

                //var response = new HttpResponseMessage();

                //if (request.Method == HttpMethod.Get || request.Method == HttpMethod.Head)
                //{
                //    var uri = new UriBuilder(request.RequestUri);
                //    uri.Scheme = Uri.UriSchemeHttps;
                //    uri.Port = this.Port;

                //    response.StatusCode = HttpStatusCode.Found;
                //    response.Headers.Location = uri.Uri;
                //}
                //else
                //{
                //    response.StatusCode = HttpStatusCode.Forbidden;
                //}

                //actionContext.Response = response;
            }
            else
            {
                base.OnAuthorization(actionContext);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace EIS.API
{
    public class APIKeyHadler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Contains("Access-Control-Request-Headers"))
            {
                return base.SendAsync(request, cancellationToken);
            }
            //var apiKey = request.Headers.GetValues("my_Token").FirstOrDefault();
            //return base.SendAsync(request, cancellationToken);
            else if (request.Headers.Contains("my_Token"))
            {
                var apiKey = request.Headers.GetValues("my_Token").FirstOrDefault();

                if (apiKey == "123456789")
                {
                    return base.SendAsync(request, cancellationToken);
                }
            }

            var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
            var taskObj = new TaskCompletionSource<HttpResponseMessage>();
            taskObj.SetResult(response);
            return taskObj.Task;

        }
    }
}
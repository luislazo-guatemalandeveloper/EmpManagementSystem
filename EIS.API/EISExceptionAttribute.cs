using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace EIS.API
{
    public class EISExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {

            var filePath = HttpContext.Current.Server.MapPath("~/Files/log.txt");
            var txt = DateTime.Now.ToString() + " : " + actionExecutedContext.Exception.Message + "\n";
            System.IO.File.AppendAllText(filePath, txt);



            if (actionExecutedContext.Exception.GetType() == typeof(SqlException))
            {
                actionExecutedContext.ActionContext.ModelState.AddModelError("", "Sql Server service is not available");
                actionExecutedContext.Response = actionExecutedContext.Request
                        .CreateErrorResponse(HttpStatusCode.BadGateway, actionExecutedContext.ActionContext.ModelState);


            }
            else if (actionExecutedContext.Exception.GetType() == typeof(System.Net.Mail.SmtpException))
            {
                actionExecutedContext.ActionContext.ModelState.AddModelError("", "Unable to send Email.");
                actionExecutedContext.Response = actionExecutedContext.Request
                        .CreateErrorResponse(HttpStatusCode.GatewayTimeout, actionExecutedContext.ActionContext.ModelState);

            }
            else
            {
                actionExecutedContext.Response =
                    actionExecutedContext.Request
                    .CreateErrorResponse(HttpStatusCode.InternalServerError, actionExecutedContext.Exception);

            }
        }
    }
}
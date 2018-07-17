using EIS.BLL;
using EIS.BOL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace EIS.API.Controllers
{
    [EnableCors("*", "*", "*")]
    public class LoginController : ApiController
    {
        EmployeeBs employeeObjBs;

        public LoginController()
        {
            employeeObjBs = new EmployeeBs();
        }
        [EISException]
        [ResponseType(typeof(Employee))]
        public IHttpActionResult Post(Employee emp)
        {

            //try
            //{
            if (employeeObjBs.GetByEmail(ref emp))
            {
                return Ok(emp);
            }
            else
            {
                foreach (var error in employeeObjBs.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                return BadRequest(ModelState);
            }
            //}
            //catch (Exception E)
            //{

            //    return InternalServerError(E);
            //}
        }

        [ResponseType(typeof(Employee))]
        [ActionName("RecoverPassword")]
        public IHttpActionResult Get(string empStr)
        {
            var emp = JsonConvert.DeserializeObject<Employee>(empStr);

            if (employeeObjBs.RecoverPasswordByEmail(ref emp))
            {
                return Ok(emp);
            }
            else
            {
                foreach (var error in employeeObjBs.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                return BadRequest(ModelState);
            }
        }
    }
}

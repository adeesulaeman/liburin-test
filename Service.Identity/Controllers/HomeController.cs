using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Service.Identity.Controllers
{
    [Route("/")]
    public class HomeController : ControllerBase
    {

        [HttpGet]
        public OkObjectResult Home()
        {
            return Ok("Identity Service");
        }        
    }
}

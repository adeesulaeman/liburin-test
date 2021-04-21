using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Gateway.CMS.Controllers
{
    [ApiController]
    public class Controller:ControllerBase
    {
        public Controller() { }

        protected ActionResult<T> ProcessResponse<T>(Response<T> response)
        {
            try
            {
                if (response.ResponseMessage.IsSuccessStatusCode)
                {
                    return Ok(response.GetContent());
                }
                else
                {
                    return StatusCode((int)response.ResponseMessage.StatusCode, response.StringContent);

                }
            }
            catch (Exception exception)
            {
                if (response.ResponseMessage.IsSuccessStatusCode)
                {
                    var o = (JObject)JsonConvert.DeserializeObject(response.StringContent);
                    return Ok(o);
                }
                else
                    return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }
        protected T GetData<T>(Response<T> response)
        {
            try
            {
                var data = response.GetContent();

                return data;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}

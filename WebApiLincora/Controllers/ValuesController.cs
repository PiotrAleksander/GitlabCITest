using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApiLincora.Helpers;
using StackExchange.Redis;

namespace WebApiLincora.Controllers
{
    public class ValuesController : Controller
    {
        public static IDatabase cache;

        public ValuesController()
        {
            cache = RedisConnectorHelper.Connection.GetDatabase();
            cache.StringSet("driverRouter", "sadasdqwe");
            cache.StringSet("driver", "1qaz!QAZ");
            cache.StringSet("542982025331250", "public");
            cache.StringSet("dispatcherClient", "dispatcherclient");
        }

        // POST: mqtt/auth
        [HttpPost]
        [Route("mqtt/auth")]
        public IActionResult PostMqttUser(string clientid, string username, string password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pass = cache.StringGet(clientid);

            if (pass == password)
            {
                return Ok();
            }

            return BadRequest();
        }

        // POST: mqtt/superuser
        [HttpPost]
        [Route("mqtt/superuser")]
        public IActionResult PostMqttSuperuser(string clientid, string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }

        // GET: mqtt/acl
        [HttpGet]
        [Route("mqtt/acl")]
        public IActionResult GetMqttAccess(int access, string clientid, string username, string ipaddr, string topic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

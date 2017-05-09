using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApiLincora.Helpers;
using StackExchange.Redis;

namespace WebApiLincora.Controllers
{
    public class ValuesController : Controller, IMqttAuthorizationStorage
    {
        private static IDatabase cache;
        private static ISubscriber publisher;
        private static TopicsMatchingHelper helper;
        private static string[] subscriptionTopics;
        private static string[] publishingTopics;

        public static TopicsMatchingHelper Helper { get => helper; set => helper = value; }
        public static ISubscriber Publisher { get => publisher; set => publisher = value; }
        public static IDatabase Cache { get => cache; set => cache = value; }
        public static string[] SubscriptionTopics { get => subscriptionTopics; set => subscriptionTopics = value; }
        public static string[] PublicationTopics { get => publishingTopics; set => publishingTopics = value; }
        
        public ValuesController()
        {
            Cache = RedisConnectorHelper.Cache;
            Publisher = RedisConnectorHelper.Connection.GetSubscriber();
            Helper = new TopicsMatchingHelper(RedisConnectorHelper.SubscriptionTopics,
                RedisConnectorHelper.PublicationTopics);
        }

        public IDatabase GetCache()
        {
            return RedisConnectorHelper.Cache;
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
            
            if (Cache.StringGet(username) == password)
            {
                return Ok();
            }

            return BadRequest();
        }

        //// POST: mqtt/superuser
        //[HttpPost]
        //[Route("mqtt/superuser")]
        //public IActionResult PostMqttSuperuser(string clientid, string username)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    return Ok();
        //}

        // GET: mqtt/acl
        [HttpGet]
        [Route("mqtt/acl")]
        public IActionResult GetMqttAccess(int access, string clientid, string username, string ipaddr, string topic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (Helper.IsTopicAllowed(topic, access))
            {
                return Ok();
            }

            return BadRequest();
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

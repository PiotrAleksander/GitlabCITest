using Microsoft.AspNetCore.Mvc;

namespace WebApiLincora.Controllers
{
    internal interface IMqttAuthorizationStorage
    {
        IActionResult PostMqttUser(string clientid, string username, string password);
        IActionResult GetMqttAccess(int access, string clientid, string username, string ipaddr, string topic);
    }
}
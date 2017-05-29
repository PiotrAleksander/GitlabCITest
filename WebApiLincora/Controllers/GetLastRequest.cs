using System;

namespace WebApiLincora.Controllers
{
    public class GetLastDataRequest
    {
        public long[] ContainersIds;
        public DateTime StartDate;
        public DateTime EndDate;
    }
}
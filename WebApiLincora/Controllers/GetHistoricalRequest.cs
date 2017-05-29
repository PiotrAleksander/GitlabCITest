using System;

namespace WebApiLincora.Controllers
{
    public class GetHistoricalRequest
    {
        public long[] ContainersIds;
        public DateTime StartDate;
        public DateTime EndDate;
    }
}
using System;
using System.Collections.Generic;

namespace WebApiLincora.Controllers
{
    public class GetDataRequest
    {
        private DateTime startDate;
        private DateTime endDate;
        private List<long> containerIds;

        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }
        public List<long> ContainersIds { get => containerIds; set => containerIds = value; }
    }
}
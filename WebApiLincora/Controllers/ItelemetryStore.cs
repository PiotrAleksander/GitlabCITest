using RiakClient.Commands.TS;
using System;
using System.Collections.Generic;
using WebApiLincora.DTOs;

namespace WebApiLincora.Controllers
{
    public interface ITelemetryStore
    {
        bool Save(string table, Row[] rows);
        Dictionary<DateTime, Dictionary<long, object>> GetDataReadings(GetDataRequest request);
        Dictionary<long, Dictionary<DateTime, object>> GetHistoricalData(GetHistoricalRequest request);
        Dictionary<long, TimestampValueDto> GetLastData(GetLastDataRequest request);
    }
}
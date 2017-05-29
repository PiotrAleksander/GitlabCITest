using RiakClient.Commands.TS;
using RiakClient.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApiLincora.DTOs;
using WebApiLincora.Helpers;

namespace WebApiLincora.Controllers
{
    public class RiakTelemetryStore : ITelemetryStore
    {
        static RiakConnectorHelper riakConnectorHelper = new RiakConnectorHelper("riak", "172.16.40.121");

        public bool Save(string table, Row[] rows)
        {
            Column[] columns = GetColumnNames();
            return riakConnectorHelper.AddRows(table, columns, rows);
        }

        private Column[] GetColumnNames()
        {
            return new Column[]
            {
                new Column("containerId", ColumnType.SInt64),
                new Column("value", ColumnType.Blob),
                new Column("time", ColumnType.Timestamp)
            };
        }

        public Dictionary<DateTime, Dictionary<long, object>> GetDataReadings(GetDataRequest request)
        {
            var telemetry = new List<ReadingsRow>();
            foreach (var containerId in request.ContainersIds)
            {
                var qfmt = "SELECT time, value FROM Telemetry2 WHERE time >= {0} AND time <= {1} AND containerId = {2}";
                var q = string.Format(
                    qfmt,
                    DateTimeUtil.ToUnixTimeMillis(request.StartDate),
                    DateTimeUtil.ToUnixTimeMillis(request.EndDate),
                    containerId);

                var rslt = riakConnectorHelper.Query("Telemetry2", q);
                foreach (var row in rslt.Value)
                {
                    var cells = row.Cells.ToList();
                    var temp = new ReadingsRow()
                    {
                        Timestamp = cells[0].ValueAsDateTime,
                        ContainerId = containerId,
                        Value = cells[1].Value
                    };
                    telemetry.Add(temp);
                }
            }

            var readings = telemetry
                .GroupBy(x => x.Timestamp)
                .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.ContainerId, y => y.Value));

            return readings;
        }

        public Dictionary<long, Dictionary<DateTime, object>> GetHistoricalData(GetHistoricalRequest request)
        {
            var telemetry = new List<ReadingsRow>();
            foreach (var containerId in request.ContainersIds)
            {
                var qfmt = "SELECT time, value FROM Telemetry2 WHERE time >= {0} and time <= {1} containerId = {2}";
                var q = string.Format(
                    qfmt,
                    DateTimeUtil.ToUnixTimeMillis(request.StartDate),
                    DateTimeUtil.ToUnixTimeMillis(request.EndDate),
                    containerId);

                var rslt = riakConnectorHelper.Query("Telemetry2", q);
                foreach (var row in rslt.Value)
                {
                    var cells = row.Cells.ToList();
                    var temp = new ReadingsRow()
                    {
                        Timestamp = cells[0].ValueAsDateTime,
                        ContainerId = containerId,
                        Value = cells[1].Value
                    };
                    telemetry.Add(temp);
                }
            }
            var readings = telemetry
                .GroupBy(x => x.ContainerId)
                .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.Timestamp, y => y.Value));

            return readings;
        }

        public DateTime? SelectMaxTimestamp(DateTime startDate, DateTime endDate, long containerId)
        {
            var qfmt = "SELECT MAX(time) FROM Telemetry2 WHERE time > {0} and time < {1} containerId = {2}";
            var q = string.Format(
                qfmt,
                DateTimeUtil.ToUnixTimeMillis(startDate),
                DateTimeUtil.ToUnixTimeMillis(endDate),
                containerId);
            var first = riakConnectorHelper.Query("Telemetry2", q).Value.FirstOrDefault();

            if (first != null)
            {
                return first.Cells.First().ValueAsDateTime;
            }

            return null;
        }

        public Dictionary<long, TimestampValueDto> GetLastData(GetLastDataRequest request)
        {
            var telemetry = new List<ReadingsRow>();
            foreach (var containerId in request.ContainersIds)
            {
                var maxTimestamp = SelectMaxTimestamp(request.StartDate, request.EndDate, containerId);
                if (maxTimestamp == null)
                {
                    continue;
                } 
                var qfmt = "SELECT time, value FROM Telemetry2 WHERE time = {0} and containerId = {1}";
                var q = string.Format(
                    qfmt,
                    DateTimeUtil.ToUnixTimeMillis(maxTimestamp.Value),
                    containerId);

                var rslt = riakConnectorHelper.Query("Telemetry2", q);
                foreach (var row in rslt.Value)
                {
                    var cells = row.Cells.ToList();
                    var temp = new ReadingsRow()
                    {
                        Timestamp = cells[0].ValueAsDateTime,
                        ContainerId = containerId,
                        Value = cells[1].Value
                    };
                    telemetry.Add(temp);
                }
            }
            var readings = telemetry
                .GroupBy(x => x.ContainerId)
                .ToDictionary(x => x.Key, y => new TimestampValueDto());

            return readings;
        }
    }
}

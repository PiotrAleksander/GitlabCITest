using M2Mqtt;
using M2Mqtt.Messages;
using Newtonsoft.Json;
using RiakClient.Commands.TS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebApiLincora.Helpers;

namespace WebApiLincora.Controllers
{
    public class RiakSaver
    {
        readonly MqttClient _client;
        static RiakConnectorHelper riakConnectorHelper = new RiakConnectorHelper("riak", "172.16.40.121");
        readonly string[] topic = { "dev/ac_emu:542982025331250/events/telemetry", "sensor/humidity" };
        readonly byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };
        readonly string[] conditionerParams = new string[] { "Temperature", "Humidity", "Port1Voltage", "Port2Voltage", "Timestamp"};
        readonly Dictionary<string, Column[]> _deviceParametersDictionary = new Dictionary<string, Column[]>() { { "airConditioner", new Column[] { new Column("temperature", ColumnType.Blob),
                                                                new Column("humidity", ColumnType.Blob),
                                                                new Column("Port1Voltage", ColumnType.Blob),
                                                                new Column("Port2Voltage", ColumnType.Blob),
                                                                new Column("Time", ColumnType.Timestamp)} } };

        public RiakSaver()
        {
            _client = new MqttClient("172.16.40.121");
            _client.Connect("saver");
            _client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            _client.Subscribe(topic, qosLevels);
        }

        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var tokens = Encoding.UTF8.GetString(e.Message).Split(';');
            var columns = new Column[]
            {
                new Column("containerId", ColumnType.SInt64),
                new Column("value", ColumnType.Blob),
                new Column("time", ColumnType.Timestamp)
            };
            var timestamp = new Cell(Convert.ToDateTime(tokens[tokens.Length - 1]));
            Row[] rows = new Row[tokens.Length - 1];
            for (var i = 0; i < (tokens.Length - 1); i++)
            {
                var containerId = i;
                var value = new Cell(Encoding.UTF8.GetBytes(tokens[i]));
                var cell = new Cell[]
                {
                    new Cell(containerId),
                    value,
                    timestamp
                };
                rows[i] = new Row(cell);
            }
            Save("Telemetry2", columns, rows);
        }

        public bool Save(string table, Column[] columns, Row[] rows)
        {
            return riakConnectorHelper.AddRows(table, columns, rows);
        }
    }
}

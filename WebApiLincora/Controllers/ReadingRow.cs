using System;

namespace WebApiLincora.Controllers
{
    internal class ReadingsRow
    {
        private DateTime timestamp;
        private long containerId;
        private object value;

        public DateTime Timestamp { get => timestamp; set => timestamp = value; }
        public long ContainerId { get => containerId; set => containerId = value; }
        public object Value { get => value; set => this.value = value; }
    }
}
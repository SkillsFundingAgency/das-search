using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using NLog.Config;

namespace NLog.Targets
{
    [Target("AzureEventHub")]
    public class AzureEventHubTarget : TargetWithLayout
    {
        private EventHubClient _eventHubClient = null;
        private MessagingFactory _messsagingFactory = null;

        [RequiredParameter]
        public string EventHubConnectionString { get; set; }

        [RequiredParameter]
        public string EventHubPath { get; set; }

        public string PartitionKey { get; set; }

        protected override void Write(LogEventInfo logEvent)
        {
            SendAsync(PartitionKey, logEvent);
        }

        private async Task<bool> SendAsync(string partitionKey, LogEventInfo logEvent)
        {
            if (this._messsagingFactory == null)
            {
                this._messsagingFactory = MessagingFactory.CreateFromConnectionString(EventHubConnectionString);
            }

            if (this._eventHubClient == null)
            {
                this._eventHubClient = this._messsagingFactory.CreateEventHubClient(EventHubPath);
            }

            string logMessage = this.Layout.Render(logEvent);

            var log = new ExpandoObject() as IDictionary<string, object>;

            log.Add("app_Name", "FindApprenticeshipTraining");

            foreach (var item in logEvent.Properties)
            {
                log.Add((string)item.Key, item.Value);
            }

            var log2 = JsonConvert.SerializeObject(log);

            using (var eventHubData = new EventData(Encoding.UTF8.GetBytes(log2)) { PartitionKey = partitionKey })
            {
                //foreach (var key in logEvent.Properties.Keys)
                //{
                //    eventHubData.Properties.Add(key.ToString(), logEvent.Properties[key]);
                //}

                await _eventHubClient.SendAsync(eventHubData);
                return true;
            }
        }
    }
}
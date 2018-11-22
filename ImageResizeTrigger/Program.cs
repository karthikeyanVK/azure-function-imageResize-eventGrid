using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizeTrigger
{
    class Program
    {
        static void Main(string[] args)
        {
            var topicHostName = ConfigurationManager.AppSettings.Get("topicHostName");
            ImageResizerMetaData metaData = new ImageResizerMetaData();
            metaData.ImagePath = "netconf.jpg";
            sendEventGridMessageWithEventGridClientAsync(topicHostName, "Test the Data", metaData).GetAwaiter().GetResult();

        }

        private static async Task sendEventGridMessageWithEventGridClientAsync(string topicHostName, string subject, object data)
        {
            var topicKey = ConfigurationManager.AppSettings.Get("topicKey");
            var credentials = new Microsoft.Azure.EventGrid.Models.TopicCredentials(topicKey);
            var client = new Microsoft.Azure.EventGrid.EventGridClient(credentials);
            var eventGridEvent = new Microsoft.Azure.EventGrid.Models.EventGridEvent
            {
                Subject = subject,
                EventType = "func-event",
                EventTime = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString(),
                Data = data,
                DataVersion = "1.0.0",
            };
            var events = new List<Microsoft.Azure.EventGrid.Models.EventGridEvent>
            {
                eventGridEvent
            };
            await client.PublishEventsWithHttpMessagesAsync(topicHostName, events);

        }
    }
}

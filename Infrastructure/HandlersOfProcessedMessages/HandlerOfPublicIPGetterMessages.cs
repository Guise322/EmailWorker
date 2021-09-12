using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages;
using MailKit;
using Microsoft.Extensions.Logging;

namespace EmailWorker.Infrastructure.HandlersOfProcessedMessages
{
    public class HandlerOfPublicIPGetterMessages : IHandlerOfPublicIPGetterMessages
    {
        private readonly string ipRequestAddress = "http://checkip.dyndns.org/";
        private readonly ILogger<HandlerOfPublicIPGetterMessages> _logger;
        public HandlerOfPublicIPGetterMessages(ILogger<HandlerOfPublicIPGetterMessages> logger)
        {
            _logger = logger;
        }
        public (string emailText, string emailSubject) HandleProcessedMessages(
            IList<UniqueId> messages)
        {
            if (messages == null)
            {
                return (null, null);
            }

            //TO DO: getting a response by HTTPClient (HTTPFactory)

            string address;
            WebRequest request = WebRequest.Create(ipRequestAddress);
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new(response.GetResponseStream()))
            {
                address = stream.ReadToEnd();
            }

            int first = address.IndexOf("Address: ") + 9;
            int last = address.LastIndexOf("</body>");
            address = address.Substring(first, last - first);
            
            _logger.LogInformation("Getting of the current succeeds.");

            return (address, "Ip By Email Project");
        }
    }
}
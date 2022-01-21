using Ardalis.GuardClauses;
using System.Collections.Generic;
using EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages;
using MailKit;
using MailKit.Net.Imap;
using Microsoft.Extensions.Logging;

namespace EmailWorker.Infrastructure.HandlersOfProcessedMessages
{
    public class HandlerOfAsSeenMarkerMessages : IHandlerOfAsSeenMarkerMessages
    {
        private readonly ILogger<HandlerOfAsSeenMarkerMessages> _logger;
        private IMailStore Client { get; }
        public HandlerOfAsSeenMarkerMessages(
            ILogger<HandlerOfAsSeenMarkerMessages> logger,
            IMailStore client) =>
            (_logger, Client) = (logger, client);
        public (string emailText, string emailSubject) HandleProcessedMessages(
            IList<UniqueId> messages)
        {
            Guard.Against.Null(messages, nameof(messages));

            foreach (var message in messages)
            {
                Client.Inbox.AddFlags(message, MessageFlags.Seen, true);
            }
            
            int maxNumberOfMessages = 1000;

            if (messages.Count < maxNumberOfMessages)
            {
                _logger.LogInformation("All messages is marked as seen.");

                return (messages.Count.ToString(), "The count of messages marked as seen");    
            }

            return (null, null);
        }
    }
}
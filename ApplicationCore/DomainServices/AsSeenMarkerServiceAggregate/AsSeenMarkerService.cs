using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.DomainServices.Shared;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages;
using EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxServiceAggregate;
using MailKit;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.AsSeenMarkerServiceAggregate
{
    public class AsSeenMarkerService : IAsSeenMarkerService
    {
        private readonly ILogger<AsSeenMarkerService> _logger;
        private readonly string myEmail  = "guise322@yandex.ru";
        private IReportSender ReportSender { get; set; }
        private IGetterOfUnseenMessages GetterOfUnseenMessages { get; set; }
        private IHandlerOfAsSeenMarkerMessages HandlerOfProcessedMessages { get; set; }
        private IClientConnector ClientConnector { get; set; }

        public AsSeenMarkerService(ILogger<AsSeenMarkerService> logger,
            IReportSender reportSender,
            IGetterOfUnseenMessages getterOfUnseenMessages,
            IHandlerOfAsSeenMarkerMessages handlerOfProcessedMessages,
            IClientConnector clientConnector) =>

            (_logger, ReportSender, GetterOfUnseenMessages, HandlerOfProcessedMessages,
                ClientConnector) = 
            (logger, reportSender, getterOfUnseenMessages, handlerOfProcessedMessages,
                clientConnector);

        public async Task ProcessEmailInbox(EmailCredentials emailCredentials)
        {
            //TO DO: add logging.

            IList<UniqueId> messages = 
                await MessagesFromEmailGetter.GetMessagesFromEmail(ClientConnector,
                    GetterOfUnseenMessages,
                    emailCredentials);

            IList<UniqueId> processedMessages = AnalyzerOfMessages.AnalyzeMessages(_logger, messages);

            (string emailText, string emailSubject) =
                HandlerOfProcessedMessages.HandleProcessedMessages(processedMessages);

            if(emailText != null)
            {
                MimeMessage message = ReportMessageBuilder.BuildReportMessage(emailCredentials,
                    myEmail,
                    emailSubject,
                    emailText);

                ReportSender.SendReportViaSmtp(message, emailCredentials);
            }
        }
    }
}
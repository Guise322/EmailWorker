using System.Collections.Generic;
using System.Threading.Tasks;
using MailKit;

namespace EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages
{
    public interface IHandlerOfProcessedMessage
    {
        (string emailText, string emailSubject) HandleProcessedMessages(IList<UniqueId> messages);
    }
}
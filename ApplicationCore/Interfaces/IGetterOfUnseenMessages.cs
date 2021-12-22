using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using MailKit;

namespace EmailWorker.ApplicationCore.Interfaces
{
    //TO DO: unite this interface with IGetterOfMessages
    public interface IGetterOfUnseenMessages
    {
        Task<IList<UniqueId>> GetUnseenMessagesAsync(EmailCredentials emailCredentials);
    }
}
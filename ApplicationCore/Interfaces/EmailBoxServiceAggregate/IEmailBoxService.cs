using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using MailKit;
using MimeKit;

namespace EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxServiceAggregate
{
    public interface IEmailBoxService
    {
        Task ProcessEmailInbox(EmailCredentials emailCredentials);
    }
}
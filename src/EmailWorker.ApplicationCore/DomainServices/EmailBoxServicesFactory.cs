using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using EmailWorker.ApplicationCore.DomainServices.AsSeenMarkerServiceAggregate;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Enums;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxServiceAggregate;

namespace EmailWorker.ApplicationCore.DomainServices;

public class EmailBoxServicesFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IEmailCredentialsGetter _emailCredentialsGetter;
    public EmailBoxServicesFactory(
        IServiceProvider serviceProvider,
        IEmailCredentialsGetter emailCredentialsGetter
    ) =>
        (_serviceProvider, _emailCredentialsGetter) = (serviceProvider, emailCredentialsGetter);
    public List<IEmailBoxService> CreateEmailBoxServices()
    {   
        List<EmailCredentials> emailCredentialsList = _emailCredentialsGetter.GetEmailCredentials();
        List<IEmailBoxService> emailBoxServices = new();
        
        foreach (var emailCredentials in emailCredentialsList)
        {
            IEmailBoxService service = null;
            switch (emailCredentials.DedicatedWork)
            {
                case DedicatedWorkType.MarkAsSeen:
                    service = _serviceProvider.GetRequiredService<IAsSeenMarkerService>();
                    break;
                case DedicatedWorkType.SearchRequest:
                    service = _serviceProvider.GetRequiredService<IPublicIPGetterService>();
                    break;
            }

            service.EmailCredentials = emailCredentials;
            emailBoxServices.Add(service);
        }
        return emailBoxServices;
    }
}
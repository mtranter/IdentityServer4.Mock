using System;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer4.Mock
{
    public interface IMockIdentityServerConfig
    {
           IMockIdentityServerConfig AddClients(params Client[] clients);

           IMockIdentityServerConfig AddIdentityResources(params IdentityResource[] identityResources);

           IMockIdentityServerConfig AddApiResources(params ApiResource[] apiResources);

           IMockIdentityServerConfig AddUsers(params TestUser[] users);

           IMockIdentityServerConfig Configure(Action<IApplicationBuilder> appCfg);

           IMockIdentityServerConfig Services(Action<IServiceCollection> servivceCfg);

           IMockIdentityServerConfig WithRequestLogging();

	       IMockIdentityServerConfig AddSigningCredential(SigningCredentials credentials);

    }
}

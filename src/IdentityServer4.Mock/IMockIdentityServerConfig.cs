using System;
using IdentityServer4.Models;
using IdentityServer4.Services.InMemory;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer4.Mock
{
    public interface IMockIdentityServerConfig
    {
           IMockIdentityServerConfig AddClients(params Client[] clients);

           IMockIdentityServerConfig AddScopes(params Scope[] scopes);

           IMockIdentityServerConfig AddUsers(params InMemoryUser[] users);

           IMockIdentityServerConfig Configure(Action<IApplicationBuilder> appCfg);

           IMockIdentityServerConfig Services(Action<IServiceCollection> servivceCfg);
    }
}
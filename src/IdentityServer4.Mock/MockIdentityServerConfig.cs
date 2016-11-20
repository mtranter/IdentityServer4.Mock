using System;
using IdentityServer4.Models;
using IdentityServer4.Services.InMemory;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer4.Mock
{
    public class MockIdentityServerConfig : IMockIdentityServerConfig
    {
        public IMockIdentityServerConfig AddClients(params Client[] clients)
        {
            Clients = clients;
            return this;
        }

        public IMockIdentityServerConfig AddScopes(params Scope[] scopes)
        {
            Scopes = scopes;
            return this;
        }

        public IMockIdentityServerConfig AddUsers(params InMemoryUser[] users)
        {
            Users = users;
            return this;
        }

        public IMockIdentityServerConfig Configure(Action<IApplicationBuilder> appCfg)
        {
            AppConfig = appCfg;
            return this;
        }

        public IMockIdentityServerConfig Services(Action<IServiceCollection> servivceCfg)
        {
            ServiceConfig = servivceCfg;
            return this;
        }

        internal Client[] Clients { get; private set; } = new Client[]{};
        internal Scope[] Scopes { get; private set; }= new Scope[]{};
        internal InMemoryUser[] Users { get; private set; }= new InMemoryUser[]{};

        internal Action<IApplicationBuilder> AppConfig { get; private set; } = _ => {};

        internal Action<IServiceCollection> ServiceConfig { get; private set; } = _ => {};
    }
}
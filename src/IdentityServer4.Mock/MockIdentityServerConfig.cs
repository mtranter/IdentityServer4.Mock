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

        public IMockIdentityServerConfig AddIdentityResources(params IdentityResource[] identityResources)
        {
            IdentityResources = identityResources;
            return this;
        }

        public IMockIdentityServerConfig AddApiResources(params ApiResource[] apiResources)
        {
            ApiResources = apiResources;
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

        public IMockIdentityServerConfig WithRequestLogging()
        {
            UseRequestLogging = true;
            return this;
        }

        internal bool UseRequestLogging {get; private set;} = false;

        internal Client[] Clients { get; private set; } = new Client[]{};
        internal ApiResource[] ApiResources { get; private set; }= new ApiResource[]{};

        internal IdentityResource[] IdentityResources { get; private set; }= new IdentityResource[]{};
        internal InMemoryUser[] Users { get; private set; }= new InMemoryUser[]{};

        internal Action<IApplicationBuilder> AppConfig { get; private set; } = _ => {};

        internal Action<IServiceCollection> ServiceConfig { get; private set; } = _ => {};
    }
}
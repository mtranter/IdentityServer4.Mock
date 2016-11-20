using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer4.Mock
{
    public class MockIdentityServer : IStartIdentityServer, IDisposable
    {
        private readonly TestServer _server;

        private MockIdentityServer(TestServer server)
        {
            _server = server;
        }

        MockIdentityServer IStartIdentityServer.Start()
        {
            return this;
        }

        public static IStartIdentityServer Configure(Action<IMockIdentityServerConfig> cfgFn)
        {
            var cfg = new MockIdentityServerConfig();
            cfgFn(cfg);
            var server = new TestServer(new WebHostBuilder().Configure(app => {
                app.UseIdentityServer();
                cfg.AppConfig(app);
            }).ConfigureServices(s => {
                var idServerCfg = s.AddIdentityServer();
                idServerCfg
                    .AddInMemoryClients(cfg.Clients)
                    .AddInMemoryScopes(cfg.Scopes)
                    .AddInMemoryUsers(cfg.Users.ToList())
                    .AddTemporarySigningCredential()
                    .AddDefaultSecretParsers()
                    .AddDefaultSecretValidators()
                    .AddDefaultEndpoints();
                cfg.ServiceConfig(s);
            }));

            return new MockIdentityServer(server);
        }

        public HttpClient CreateClient()
        {
            return _server.CreateClient();
        }

        public HttpMessageHandler CreateHandler()
        {
            return _server.CreateHandler();
        }

        public Uri BaseAddress
        {
            get
            {
                return _server.BaseAddress;
            }
        }

        public void Dispose()
        {
            _server.Dispose();
        }
    }
}
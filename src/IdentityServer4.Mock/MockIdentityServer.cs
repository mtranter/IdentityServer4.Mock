using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
                if(cfg.UseRequestLogging)
                {
                    app.ApplicationServices.GetRequiredService<ILoggerFactory>().AddConsole();
                }
            }).ConfigureServices(s => {
                var idServerCfg = s.AddIdentityServer();
                if(cfg.UseRequestLogging)
                {
                    s.AddLogging();
                }
                
                idServerCfg
                    .AddInMemoryClients(cfg.Clients)
                    .AddInMemoryIdentityResources(cfg.IdentityResources)
                    .AddInMemoryApiResources(cfg.ApiResources)
                    .AddInMemoryUsers(cfg.Users.ToList())
                    //.AddTemporarySigningCredential()
                    .AddDefaultSecretParsers()
                    .AddDefaultSecretValidators()
                    .AddDefaultEndpoints();
		if(cfg.SigningCredentials != null)
		{
		    idServerCfg.AddSigningCredential(cfg.SigningCredentials);
		}
		else
		{
		    idServerCfg.AddTemporarySigningCredential();
		}
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

using System;
using Xunit;
using IdentityServer4.Mock;
using IdentityServer4.Models;
using IdentityServer4.Endpoints;
using IdentityModel.Client;
using System.Net;

namespace IdentityServer4.Mock.Tests
{
    public class MockIdentityServerShould
    {
        MockIdentityServer _sut;

        public MockIdentityServerShould()
        {
            _sut = MockIdentityServer.Configure(c => {
                c.AddApiResources(new ApiResource(){ 
                    Name = "testscope",
                    Scopes = new[]{new Scope("testscope")},
                    ApiSecrets = new[]{new Secret("apisecret".Sha256())}
                    })
                .AddClients(new Client(){
                    ClientId = "testclient",
                    ClientSecrets = new[]{new Secret("clientsecret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = new[]{"testscope"}
                });
            }).Start();
        }

        [Fact]
        public async void SupplyADiscoveryEndpoint() 
        {
            var proxyHandler = _sut.CreateHandler();
            var discoClient = new DiscoveryClient(_sut.BaseAddress.ToString(), proxyHandler);
            var disco = await discoClient.GetAsync();

            Assert.NotNull(disco);
            Assert.Equal(HttpStatusCode.OK, disco.StatusCode);
        }

        [Fact]
        public async void ReturnATokenForAValidClient() 
        {
            var proxyHandler = _sut.CreateHandler();
            var discoClient = new DiscoveryClient(_sut.BaseAddress.ToString(), proxyHandler);
            var disco = await discoClient.GetAsync();

            var tokenClient = new TokenClient(disco.TokenEndpoint, "testclient","clientsecret", proxyHandler);
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("testscope");
            Assert.Equal(HttpStatusCode.OK, tokenResponse.HttpStatusCode);
        }
        
        [Fact]
        public async void AuthenticateAClient() 
        {
            var proxyHandler = _sut.CreateHandler();
            var discoClient = new DiscoveryClient(_sut.BaseAddress.ToString(), proxyHandler);
            var disco = await discoClient.GetAsync();

            var tokenClient = new TokenClient(disco.TokenEndpoint, "testclient","clientsecret", proxyHandler);
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("testscope");
            Assert.Equal(HttpStatusCode.OK, tokenResponse.HttpStatusCode);

            var introClient = new IntrospectionClient(disco.IntrospectionEndpoint, "testscope","apisecret", proxyHandler);
            var introResponse = await introClient.SendAsync(new IntrospectionRequest(){
                ClientId = "testclient",
                Token = tokenResponse.AccessToken
            });

            Assert.Equal(HttpStatusCode.OK, introResponse.HttpStatusCode);
        }
    }
}

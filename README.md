# IdentityServer4.Mock
Mock Identity Server for Integration testing

## Usage

####Creating a mock identity server

```cs
    var identityServer = MockIdentityServer.Configure(c => {
        c.AddApiResources(new ApiResource() {
                Name = "testscope",
                Scopes = new[] {new Scope("testscope")},
                ApiSecrets = new[] {new Secret("apisecret".Sha256())}
            })
            .AddClients(new Client() {
                ClientId = "testclient",
                ClientSecrets = new[] {new Secret("clientsecret".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = new[] {"testscope"}
            });
    }).Start();
```

####Authenticating a client

```cs
    var proxyHandler = identityServer.CreateHandler();
    var discoClient = new DiscoveryClient(identityServer.BaseAddress.ToString(), proxyHandler);
    var disco = await discoClient.GetAsync();

    var tokenClient = new TokenClient(disco.TokenEndpoint, "testclient","clientsecret", proxyHandler);
    var tokenResponse = await tokenClient.RequestClientCredentialsAsync("testscope");
```

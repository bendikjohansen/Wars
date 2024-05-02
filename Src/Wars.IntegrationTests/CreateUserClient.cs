using System.Net.Http.Headers;
using Bogus;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc.Testing;
using Wars.Users.Features;

namespace Wars.IntegrationTests;

internal static class UserClientFactory
{
    internal delegate HttpClient CreateClient(WebApplicationFactoryClientOptions? o = null);

    public static async Task<HttpClient> Create(CreateClient createClient)
    {
        var client = createClient();
        var user = User.CreateFake();

        await Register(client, user);
        var accessToken = await Login(client, user);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return client;
    }

    private static async Task Register(HttpClient client, User user)
    {
        var registerRequest = new Register.RequestBody(user.EmailAddress, user.Password);
        var registerResponse = await client.POSTAsync<Register.Endpoint, Register.RequestBody>(registerRequest);
        registerResponse.EnsureSuccessStatusCode();
    }

    private static async Task<string> Login(HttpClient client, User user)
    {
        var loginRequest = new Login.RequestBody(user.EmailAddress, user.Password);
        var loginResponse = await client.POSTAsync<Login.Endpoint, Login.RequestBody, Login.ResponseBody>(loginRequest);
        loginResponse.Response.EnsureSuccessStatusCode();

        return loginResponse.Result.AccessToken;
    }

    private record User(string EmailAddress, string Password)
    {
        public static User CreateFake()
        {
            var faker = new Faker();
            return new User(faker.Internet.Email(), faker.Internet.Password(prefix: "aA1!"));
        }
    }
}

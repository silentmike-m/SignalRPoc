namespace Client
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Client.Entities;
    using Microsoft.AspNetCore.SignalR.Client;

    internal sealed class SignalRClient
    {
        public async Task StartConnection(string userName, string password)
        {
            var accessToken = await GetToken(userName, password);

            var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/userHub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(accessToken);
                })
                .WithAutomaticReconnect()
                .Build();

            await connection.StartAsync();
            Console.WriteLine("CONNECTED");

            connection.On<string, List<User>>("GotUsers", (userId, users) =>
            {
                Console.WriteLine($"Got users for {userId}:");

                foreach (var user in users)
                {
                    Console.WriteLine(user);
                }
            });

            await GetUsers(accessToken);
            Console.WriteLine("Sent get users");
        }

        private async Task<string> GetToken(string userName, string password)
        {
            using var client = this.GetClient();
            var result = await client.PostAsJsonAsync("User/Login", new { userName, password });
            var accessToken = await result.Content.ReadAsStringAsync();
            Console.WriteLine($"Access token: {accessToken}");
            return accessToken;
        }

        private async Task GetUsers(string accessToken)
        {
            using var client = this.GetClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "User/GetUsers");
            request.Headers.Add("Accept", "application/vnd.github.v3+json");
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");
            request.Headers.Add("Authorization", "Bearer " + accessToken);
            await client.SendAsync(request);
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient { BaseAddress = new Uri("https://localhost:5001") };
            return client;
        }
    }
}
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
        public async Task StartConnection(string userName, string password, string groupId)
        {
            var accessToken = await this.GetToken(userName, password, groupId);


            await this.StartHubConnection(accessToken);

            if (userName == "User2")
            {
                await this.GetUsers(accessToken);
            }
        }

        private async Task StartHubConnection(string accessToken)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/userHub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(accessToken);
                })
                .WithAutomaticReconnect()
                .Build();


            await connection.StartAsync();
            Console.WriteLine("UserHub: CONNECTED");

            connection.On<List<User>>("GotUsers", (response) =>
            {
                Console.WriteLine("Users received:");

                foreach (var user in response)
                {
                    Console.WriteLine($"{user.Id} : ${user.UserName}");
                }
            });
        }

        private async Task<string> GetToken(string userName, string password, string groupId)
        {
            using var client = GetClient();
            var result = await client.PostAsJsonAsync("User/Login", new { user_name = userName, password = password, group_id = groupId });
            var accessToken = await result.Content.ReadAsStringAsync();
            Console.WriteLine($"Access token: {accessToken}");
            return accessToken;
        }

        private async Task GetUsers(string accessToken)
        {
            Console.WriteLine("Send get users request");

            using var client = GetClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "User/GetUsers");
            request.Headers.Add("Accept", "application/vnd.github.v3+json");
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");
            request.Headers.Add("Authorization", "Bearer " + accessToken);
            await client.SendAsync(request);
        }

        private static HttpClient GetClient()
        {
            var client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
            return client;
        }
    }
}
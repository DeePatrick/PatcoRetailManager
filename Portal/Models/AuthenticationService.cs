using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Portal.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class AuthenticationService :  IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthenticationService(HttpClient httpClient, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }



        public async Task<AuthenticatedUserModel> Login(AuthenticationUserModel userForAuthentication)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", userForAuthentication.Email),
                new KeyValuePair<string, string>("password", userForAuthentication.Password),
            });

            //var authResult = await _httpClient.PostAsync("/Token", data);
            var authResult = await _httpClient.PostAsync("https://localhost:5001/token", data);
            var authContent = await authResult.Content.ReadAsStringAsync();

            if (authResult.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<AuthenticatedUserModel>(authContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                await _localStorage.SetItemAsync(key: "authToken", data: result.Access_Token);

                ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Access_Token);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "bearer", parameter: result.Access_Token);

                return result;
            }
            else
            {
                throw new Exception(authResult.ReasonPhrase);
            }


        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync(key: "authToken");
            ((AuthStateProvider)_authStateProvider).NoitifyUserLogOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}



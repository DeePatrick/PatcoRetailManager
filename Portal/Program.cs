using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Portal.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using PRMDesktopUI.Library.Api;
using PRMDesktopUI.Library.Models;

namespace Portal
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            //Added custom authentication and Blazor local storage service
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

            builder.Services.AddSingleton<IAPIHelper, APIHelper>();
            builder.Services.AddSingleton<ILoggedInUserModel, LoggedInUserModel>();

            builder.Services.AddTransient<IProductEndpoint, ProductEndpoint>();
            builder.Services.AddTransient<IUserEndpoint, UserEndpoint>();
            builder.Services.AddTransient<ISaleEndpoint, SaleEndpoint>();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }
    }
}

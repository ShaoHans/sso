using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccessApi.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Access_ClientCredentials().Wait();
            Console.WriteLine("=============================");
            Access_ResourceOwnerPassword().Wait();
            Console.ReadKey();
        }

        static async Task Access_ClientCredentials()
        {
            var discoveryResponse = await DiscoveryClient.GetAsync("http://localhost:5000");
            if(discoveryResponse.IsError)
            {
                Console.WriteLine($"访问IdentityServer服务出错：{discoveryResponse.Error}");
                Console.ReadKey();
                return;
            }

            // 客户端模式
            // 使用IdentityServer服务端配置的客户端Id和Secret，通过TokenEndpoint地址向IdentityServer服务端获取访问user-api资源需要的AccessToken
            TokenClient tokenClient = new TokenClient(discoveryResponse.TokenEndpoint, "console_client", "console_secret");
            TokenResponse tokenResponse = await tokenClient.RequestClientCredentialsAsync("user-api");
            if(tokenResponse.IsError)
            {
                Console.WriteLine($"获取AccessToken出错：{tokenResponse.Error}");
                Console.ReadKey();
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            HttpClient httpClient = new HttpClient();
            // 使用获取到的AccessToken访问user-api资源
            httpClient.SetBearerToken(tokenResponse.AccessToken);
            var httpResonse = await httpClient.GetAsync("http://localhost:5002/api/user/claims");
            if(httpResonse.IsSuccessStatusCode)
            {
                var content = await httpResonse.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
            else
            {
                Console.WriteLine($"访问UserApi接口失败：{httpResonse.StatusCode}");
            }
        }

        static async Task Access_ResourceOwnerPassword()
        {
            var discoveryResponse = await DiscoveryClient.GetAsync("http://localhost:5000");
            if (discoveryResponse.IsError)
            {
                Console.WriteLine($"访问IdentityServer服务出错：{discoveryResponse.Error}");
                Console.ReadKey();
                return;
            }

            // 客户端模式
            // 使用IdentityServer服务端配置的客户端Id和Secret，通过TokenEndpoint地址向IdentityServer服务端获取访问user-api资源需要的AccessToken
            TokenClient tokenClient = new TokenClient(discoveryResponse.TokenEndpoint, "console_client", "console_secret");
            TokenResponse tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("张三", "123456", "user-api");
            if (tokenResponse.IsError)
            {
                Console.WriteLine($"获取AccessToken出错：{tokenResponse.Error}");
                Console.ReadKey();
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            HttpClient httpClient = new HttpClient();
            // 使用获取到的AccessToken访问user-api资源
            httpClient.SetBearerToken(tokenResponse.AccessToken);
            var httpResonse = await httpClient.GetAsync("http://localhost:5002/api/user/claims");
            if (httpResonse.IsSuccessStatusCode)
            {
                var content = await httpResonse.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
            else
            {
                Console.WriteLine($"访问UserApi接口失败：{httpResonse.StatusCode}");
            }
        }
    }
}

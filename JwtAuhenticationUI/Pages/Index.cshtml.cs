using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelObjects;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace JwtAuhenticationUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        HttpClient httpClient;
        TokenModel tm;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

            tm = new()
            {
                RefreshToken= "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJyZWZyZXNoVG9rZW4iLCJqdGkiOiJmNWVhYThjNC0zZjllLTQ4NzctYWRlNC02OTc3Nzk1NjYzZjMiLCJleHAiOjE3MDIzMzEyNTcsImlzcyI6IlJTSyIsImF1ZCI6IlJTSyJ9.WTe6GE3hLr3fyw-Eop2y7QsrVp4Tixp_-CIufRPO4jQ",
                Token= "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJSU0siLCJqdGkiOiIxNzFlYjBlYi0xZjQ5LTRhMmMtYjUxYi00NWQ4YzI5NThhM2YiLCJleHAiOjE3MDE5NzE0MzcsImlzcyI6IlJTSyIsImF1ZCI6IlJTSyJ9.wX5SPDlloGpL-OJZ-KOIqnjqsKGxibavDou0bDNxXE8",
                UserName="RSK"
            };

            httpClient = new HttpClient();

            var requestmessage = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri("https://localhost:44307/api/TokenGenerator/TestToken") };
                
            requestmessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhYmMiLCJqdGkiOiI1ODI0ZGYwNi0wNTIwLTQ3ODUtODVjNC01MDIxOTYwMGY2OTgiLCJleHAiOjE3MDE4OTIwODgsImlzcyI6IlJTSyIsImF1ZCI6IlJTSyJ9.IdvHQaf1lEdRh1XGOgIK5XpXvR8rgyWDx1LvO8C-h-Q");
            
            var response = httpClient.SendAsync(requestmessage).Result;
                    
            if (!response.IsSuccessStatusCode)
            {
                if(response.StatusCode == HttpStatusCode.Unauthorized && response.Headers.GetValues("WWW-Authenticate").FirstOrDefault()!.Contains("token expired"))
                {
                    refreshtoken();
                }

                var result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(result);
            }
            
            Console.WriteLine($"{response.Content.ReadAsStringAsync().Result}");
                
        }

        private void refreshtoken()
        {
            var requestmessage = new HttpRequestMessage { Method = HttpMethod.Post, RequestUri = new Uri("https://localhost:44307/api/TokenGenerator/GetRefreshToken"), 
                Content=new StringContent(JsonConvert.SerializeObject(tm), Encoding.UTF8, "application/json") };

            var response = httpClient.SendAsync(requestmessage).Result;

            if (!response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(result);
            }

            Console.WriteLine($"{response.Content.ReadAsStringAsync().Result}");


        }
    }
}
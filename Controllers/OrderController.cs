using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderItemWebApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;
using System.Reflection.Metadata.Ecma335;
using System.Net.Http;
using System.Text;


namespace OrderItemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {


        public static HttpClient client = new HttpClient();
        [HttpPost("{menuitemid}")]
        public Cart POST(int menuitemid)
        {
            string token = GetToken("http://52.143.250.249/api/Token");

            Cart orderItem = new Cart();
            orderItem.Id = 1;
            orderItem.userId = 1;
            orderItem.menuItemId = menuitemid;
            orderItem.menuItemName = getname(menuitemid, token).ToString();
            return orderItem;





        }
        static string GetToken(string url)
        {
            var user = new User { Name = "Namit", Password = "Namit123" };
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {


                var response = client.PostAsync(url, data).Result;
                string name = response.Content.ReadAsStringAsync().Result;
                dynamic details = JObject.Parse(name);
                return details.token;
            }
        }

        private string getname(int id, string token)
        {
            string name;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = new HttpResponseMessage();

                response = client.GetAsync("api/MenuItem/" + id).Result;


                if (response.IsSuccessStatusCode)
                {
                    string name1 = response.Content.ReadAsStringAsync().Result;
                    name = JsonConvert.DeserializeObject<string>(name1);
                }
                else
                    name = null;

            }
            return name;
        }
    }
}

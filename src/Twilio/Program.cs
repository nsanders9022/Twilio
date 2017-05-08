using System;
using RestSharp;
using RestSharp.Authenticators;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Twilio
{
    class Program
    {
        public class Message
        {
            public string To { get; set; }
            public string From { get; set; }
            public string Body { get; set; }
            public string Status { get; set; }
        }
        static void Main(string[] args)
        {
            //1
            var client = new RestClient("https://api.twilio.com/2010-04-01");
            //2
            var request = new RestRequest("Accounts/ACefcc4311d0baf9d2aebdc5dc6a1370fe/Messages.json", Method.GET);
            //3
            client.Authenticator = new HttpBasicAuthenticator("ACefcc4311d0baf9d2aebdc5dc6a1370fe", "172c31cfc19e08d976078f4c51e71310");
            //4
            var response = new RestResponse();

            //5
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            var messageList = JsonConvert.DeserializeObject<List<Message>>(jsonResponse["messages"].ToString());
            foreach(var message in messageList)
            {
                Console.WriteLine("To: {0}", message.To);
                Console.WriteLine("From: {0}", message.From);
                Console.WriteLine("Body: {0}", message.Body);
                Console.WriteLine("Status: {0}", message.Status);
            }


            Console.ReadLine();
        }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response =>
            {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    }
}
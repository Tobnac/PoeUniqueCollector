using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PoeUniqueCollector
{
    public class APIRequester
    {
        private string nextID = "";
        private List<Task<string>> openRequests = new List<Task<string>>(); // placeholder for multiple async requests
        private ResponseParser parser = new ResponseParser();

        public void Run()
        {
            var response = GetResponseAsync();

            while (true)
            {
                if (response.IsCompleted)
                {
                    break;
                }

                else
                {
                    Console.WriteLine(response.Status);
                }

                Thread.Sleep(500);
            }
            
            UpdateNextID(response.Result);
            this.parser.ParseResponse(response.Result);
        }

        private async Task<string> GetResponseAsync()
        {
            var webClient = new WebClient();
            var reqUrl = "http://www.pathofexile.com/api/public-stash-tabs";

            if (this.nextID != "") reqUrl += "?id=" + this.nextID;

            using (webClient)
            {
                return await webClient.DownloadStringTaskAsync(reqUrl);
            }
        }

        private void UpdateNextID(string s)
        {
            var nextID = s.Substring(19);
            nextID = nextID.Split('"')[0];
            this.nextID = nextID;
        }

    }
}

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
        public string NextID
        {
            get { return nextID; }
            set { nextID = value; this.isUpToDate = true; }
        }

        private string nextID = "";
        private bool isUpToDate;
        private List<Task<string>> openRequests = new List<Task<string>>(); // placeholder for multiple async requests
        private StashScanner scanner;

        public APIRequester()
        {
            this.scanner = new StashScanner(this);
        }

        public void Run()
        {
            var response = GetResponseAsync();            

            while (true)
            {
                if (response.IsCompleted)
                {
                    this.isUpToDate = false;
                    break;
                }

                else
                {
                    Console.WriteLine(response.Status);
                }

                Thread.Sleep(500);
            }
            
            this.scanner.ParseToObject(response.Result);
            this.scanner.ScanUniques();
            UpdateNextID(response.Result);
        }

        private async Task<string> GetResponseAsync()
        {
            var webClient = new WebClient();
            var reqUrl = this.BuildRequestURL();

            using (webClient)
            {
                return await webClient.DownloadStringTaskAsync(reqUrl);
            }
        }

        private string BuildRequestURL()
        {
            var url = "http://www.pathofexile.com/api/public-stash-tabs";

            if (this.NextID != "")
            {
                url += "?id=" + this.NextID;
            }

            return url;
        }

        private void UpdateNextID(string s)
        {
            if (this.isUpToDate) return;

            Console.WriteLine("Warning: Requester had to update nextID himself");

            var nextID = s.Substring(19);
            nextID = nextID.Split('"')[0];
            this.NextID = nextID;
        }

    }
}

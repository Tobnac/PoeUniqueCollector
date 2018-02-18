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
            get
            {
                return nextID;
            }

            set
            {
                nextID = value;
                this.isUpToDate = true;
                this.SaveNextIDToFile(value);
            }
        }

        private string nextID = "";
        private string nextIDFilePath = "..\\..\\NextChangeID.txt";
        private bool isUpToDate;
        private List<Task<string>> openRequests = new List<Task<string>>(); // placeholder for multiple async requests
        private StashScanner scanner;

        public APIRequester()
        {
            this.scanner = new StashScanner(this);
            this.SearchForNextIDInFile();
        }

        public void Run()
        {
            var response = GetResponseAsync();            

            while (true)
            {
                if (response.IsCompleted)
                {
                    Console.WriteLine("Response received!");
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

        private void SearchForNextIDInFile()
        {
            string content;

            try
            {
                content = System.IO.File.ReadAllText(this.nextIDFilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while reading NextID file");
                Console.WriteLine(e.Message);
                Console.ReadLine();
                return;
            }

            this.nextID = content;            
        }

        private void SaveNextIDToFile(string nextID)
        {
            string[] myContent = { nextID };
            System.IO.File.WriteAllLines(this.nextIDFilePath, myContent);
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

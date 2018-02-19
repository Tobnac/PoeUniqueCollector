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
        private bool isUpToDate = true;
        private Task<string> openRequest;
        private StashScanner scanner;

        public APIRequester()
        {
            this.scanner = new StashScanner(this);
            this.ReadNextIDFromFile();
        }

        public void Run()
        {
            this.SendRequest();
            this.AwaitResponse();
        }

        private void AwaitResponse()
        {
            while (true)
            {
                if (this.openRequest.IsCompleted)
                {
                    Console.WriteLine("Response received!");
                    this.isUpToDate = false;
                    break;
                }

                else
                {
                    Console.WriteLine(this.openRequest.Status);
                }

                Thread.Sleep(500);
            }

            this.scanner.ParseToObject(this.openRequest.Result);
            this.SendRequest();
            this.scanner.ScanUniques();
            this.AwaitResponse();
        }

        private void ReadNextIDFromFile()
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

        private void SendRequest()
        {
            if (!this.isUpToDate)
            {
                Console.WriteLine("Error: nextID is not up to date!");
                return;
            }

            var request = CreateRequestAsync();
            this.openRequest = request;
        }

        private async Task<string> CreateRequestAsync()
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

    }
}

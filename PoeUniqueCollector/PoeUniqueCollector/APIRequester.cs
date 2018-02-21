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

            set
            {
                if (value == nextID) Console.WriteLine("<-- INFORMATION: NextID did not change! -->");
                nextID = value;
                this.isUpToDate = true;
                this.SaveNextIDToFile(value);
            }
        }

        public int UselessRequestTolerance { get; set; }

        private string nextID = "";
        private string nextIDFilePath = "..\\..\\NextChangeID.txt";
        private bool isUpToDate = true;
        private Task<string> openRequest;
        private StashScanner scanner;
        private int uselessRequestCount = 0;
        

        public APIRequester(int uselessRequestTolerance)
        {
            this.scanner = new StashScanner(this);
            this.UselessRequestTolerance = uselessRequestTolerance;
            this.ReadNextIDFromFile();
        }

        public void Run()
        {
            this.SendRequest();
            this.AwaitResponse();
            this.ProcessResponse();
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

        private void AwaitResponse()
        {
            while (!this.openRequest.IsCompleted)
            {
                Console.WriteLine("(INFO) " + this.openRequest.Status);
                Thread.Sleep(1000);
            }

            Console.WriteLine("(INFO) Response received!");
            this.isUpToDate = false;
        }

        private void ProcessResponse()
        {
            this.scanner.ParseToObject(this.openRequest.Result);

            if (this.DoContinueRequesting())
            {
                this.SendRequest();
            }

            var oldCounts = this.scanner.ItemProcessors.Select(x => x.CollectionSize).ToArray();
            this.scanner.ScanAllItems();
            if (oldCounts.SequenceEqual(this.scanner.ItemProcessors.Select(x => x.CollectionSize).ToArray()))
            {
                this.uselessRequestCount++;
                Console.Write("Useless request. Counter increased to " + this.uselessRequestCount);
                if (this.UselessRequestTolerance <= 0) Console.Write("\n");
                else Console.WriteLine("/" + this.UselessRequestTolerance);                
            }
            else
            {
                this.uselessRequestCount = 0;
            }

            if (this.DoContinueRequesting())
            {
                this.AwaitResponse();
                this.ProcessResponse();
            }
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

        private bool DoContinueRequesting()
        {
            if (this.UselessRequestTolerance <= 0) return true;

            if (this.uselessRequestCount >= this.UselessRequestTolerance)
            {
                return false;
            }

            return true;
        }

    }
}

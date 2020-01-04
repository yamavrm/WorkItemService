using System;
using System.IO;
using System.Net;

namespace WorkItemsService
{
    public class WorkItemWebPage : IWorkItem
    {
        public event EventHandler<WorkItemStatusUpdatedEventArgs> WorkItemStatusUpdated;

        public WorkItemWebPage(string url)
        {
            Name = url;
            Duration = 0;
            Status = WorkItemStatus.NotStarted;
        }

        public string Name { get; set; }

        public int Duration { get; set; }

        public WorkItemStatus Status { get; set; }

        public void Execute()
        {
            this.Status = WorkItemStatus.Running;
            WorkItemStatusUpdated.Invoke(this, new WorkItemStatusUpdatedEventArgs(this.Status));

            WebPageDownload(this.Name);

            this.Status = WorkItemStatus.Completed;
            WorkItemStatusUpdated.Invoke(this, new WorkItemStatusUpdatedEventArgs(this.Status));
        }

        private void WebPageDownload(string url)
        {
            string content = string.Empty;
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                try
                {
                    WebRequest myWebRequest = WebRequest.Create(url);
                    myWebRequest.Timeout = 60000;

                    using (System.IO.Stream s = myWebRequest.GetResponse().GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                        {
                            content = sr.ReadToEnd();
                        }
                    }
                }
                catch (Exception ex)
                {
                    content = ex.Message;
                }
            }
            else
            {
                content = url + " - Invalid url";
            }

            if (!string.IsNullOrEmpty(content))
            {
                File.WriteAllText($"WebPage-{DateTime.Now.ToString("yyyyMMddHHmmss.fff")}.html", content);
            }
        }

        private void WebPageDownload1(string url)
        {
            string content = string.Empty;
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                using (var client = new WebClient())
                {
                    content = client.DownloadString(new Uri(url));
                }
            }
            else
            {
                content = url + " - Invalid url";
            }

            if (!string.IsNullOrEmpty(content))
            {
                File.WriteAllText($"WebPage-{DateTime.Now.ToString("yyyyMMddHHmmss.fff")}.html", content);
            }
        }
    }
}

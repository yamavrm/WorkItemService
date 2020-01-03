using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WorkItemsService
{
    public class AsyncRun : IAsyncRun
    {
        public void DoTask(IWorkItem workItem)
        {
            Task.Run(() => { workItem.Execute(); });
        }
    }
}

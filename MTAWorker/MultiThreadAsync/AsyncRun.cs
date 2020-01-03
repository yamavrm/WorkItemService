using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcessWorkItems
{
    public class AsyncRun : IAsyncRun
    {
        public void DoTask(IWorkItem workItem)
        {
            Task.Run(() => { workItem.Execute(); });
        }
    }
}

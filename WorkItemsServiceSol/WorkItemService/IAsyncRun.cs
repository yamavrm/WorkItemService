using System;
using System.Collections.Generic;
using System.Text;

namespace WorkItemsService
{
    public interface IAsyncRun
    {
        void DoTask(IWorkItem workItem);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WorkItemsService
{
    public interface IWorkItemManager
    {
        event EventHandler<StatusUpdatedEventArgs> StatusUpdated;

        int QueueCount { get; }

        bool IsServiceRunning { get; }

        IAsyncRun Service { set; }

        void AddWorkItem(IWorkItem workItem);
    }
}

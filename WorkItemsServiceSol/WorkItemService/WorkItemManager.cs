using System;
using System.Collections.Generic;

namespace WorkItemsService
{
    public class WorkItemManager : IWorkItemManager
    {
        private readonly Queue<IWorkItem> workItemQueue = new Queue<IWorkItem>();
        private readonly object lockQueue = new Object();
        private IAsyncRun asyncService;

        public event EventHandler QueueItemAdded;
        public event EventHandler<StatusUpdatedEventArgs> StatusUpdated;

        public WorkItemManager()
        {
            QueueItemAdded += WorkItemManager_QueueItemAdded;
        }

        private void WorkItemManager_QueueItemAdded(object sender, EventArgs e)
        {
            ExecuteWorkItem();
        }

        public void AddWorkItem(IWorkItem workItem)
        {
            lock (lockQueue)
            {
                workItemQueue.Enqueue(workItem);
            }
            StatusUpdated?.Invoke(this, new StatusUpdatedEventArgs($"{workItem.Name} - Added"));
            QueueItemAdded.Invoke(this, null);
        }

        public IAsyncRun Service
        {
            set
            {
                this.asyncService = value;
            }
        }

        public int QueueCount
        {
            get
            {
                return workItemQueue.Count;
            }
        }

        public bool IsServiceRunning { get; private set; } = false;

        private void ExecuteWorkItem()
        {
            if (!this.IsServiceRunning)
            {
                this.IsServiceRunning = true;
                IWorkItem workItem = null;
                lock (lockQueue)
                {
                    if (this.workItemQueue.Count > 0)
                    {
                        workItem = this.workItemQueue.Dequeue();
                    }
                }
                if (workItem != null)
                {
                    workItem.WorkItemStatusUpdated += WorkItem_WorkItemStatusUpdated;
                    asyncService.DoTask(workItem);
                    StatusUpdated?.Invoke(this, new StatusUpdatedEventArgs($"{workItem.Name} - Started"));
                }
                else
                {
                    this.IsServiceRunning = false;
                }
            }
        }

        private void WorkItem_WorkItemStatusUpdated(object sender, WorkItemStatusUpdatedEventArgs e)
        {
            this.IsServiceRunning = e.Status == WorkItemStatus.Running ? true : false;
            
            if (e.Status == WorkItemStatus.Completed)
            {
                IWorkItem workItem = (IWorkItem)sender;
                workItem.WorkItemStatusUpdated -= WorkItem_WorkItemStatusUpdated;
                StatusUpdated?.Invoke(this, new StatusUpdatedEventArgs($"{workItem.Name} - Completed"));
                StatusUpdated?.Invoke(this, new StatusUpdatedEventArgs($"Queue Count - {this.QueueCount}"));
                ExecuteWorkItem();
            }
        }
    }
}

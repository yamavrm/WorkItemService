using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WorkItemsService
{
    public class WorkItemManager : IWorkItemManager
    {
        private readonly Queue<IWorkItem> workItemQueue = new Queue<IWorkItem>();
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
            lock (this.workItemQueue)
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
            if (this.workItemQueue.Count > 0 && !this.IsServiceRunning)
            {
                lock(this.workItemQueue)
                {
                    var workItem = workItemQueue.Dequeue();
                    workItem.WorkItemStatusUpdated += WorkItem_WorkItemStatusUpdated;
                    //Task.Run(() => { workItem.Execute(); });
                    asyncService.DoTask(workItem);
                    StatusUpdated?.Invoke(this, new StatusUpdatedEventArgs($"{workItem.Name} - Started"));
                }
            }
        }

        private void WorkItem_WorkItemStatusUpdated(object sender, WorkItemStatusUpdatedEventArgs e)
        {
            if (e.Status == WorkItemStatus.Running)
            {
                this.IsServiceRunning = true;
            }
            else if (e.Status == WorkItemStatus.Completed)
            {
                var workItem = (IWorkItem)sender;
                workItem.WorkItemStatusUpdated -= WorkItem_WorkItemStatusUpdated;
                this.IsServiceRunning = false;
                StatusUpdated?.Invoke(this, new StatusUpdatedEventArgs($"{workItem.Name} - Completed"));
                StatusUpdated?.Invoke(this, new StatusUpdatedEventArgs($"Queue Count - {this.QueueCount}"));
                ExecuteWorkItem();
            }
        }
    }
}

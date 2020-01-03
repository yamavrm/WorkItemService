using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WorkItemsService
{
    public class WorkItem : IWorkItem
    {
        public event EventHandler<WorkItemStatusUpdatedEventArgs> WorkItemStatusUpdated;

        public WorkItem(string name, int duration)
        {
            Name = name;
            Duration = duration;
            Status = WorkItemStatus.NotStarted;
        }

        public string Name { get; set; }

        public int Duration { get; set; }

        public WorkItemStatus Status { get; set; }

        public void Execute()
        {
            this.Status = WorkItemStatus.Running;
            WorkItemStatusUpdated.Invoke(this, new WorkItemStatusUpdatedEventArgs(this.Status));

            for (int i = 0; i < Duration; i++)
            {
                Thread.Sleep(100);
            }

            this.Status = WorkItemStatus.Completed;
            WorkItemStatusUpdated.Invoke(this, new WorkItemStatusUpdatedEventArgs(this.Status));
        }
    }
}

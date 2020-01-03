using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessWorkItems
{
    public class WorkItemStatusUpdatedEventArgs
    {
        public WorkItemStatusUpdatedEventArgs(WorkItemStatus status)
        {
            this.Status = status;
        }

        public WorkItemStatus Status { get; private set; }
    }
}

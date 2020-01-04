using System;

namespace WorkItemsService
{
    public interface IWorkItem
    {
        event EventHandler<WorkItemStatusUpdatedEventArgs> WorkItemStatusUpdated;

        string Name { get; set; }

        int Duration { get; set; }

        WorkItemStatus Status { get; set; }

        void Execute();
    }
}

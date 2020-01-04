namespace WorkItemsService
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

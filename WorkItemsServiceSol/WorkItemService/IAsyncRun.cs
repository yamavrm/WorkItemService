namespace WorkItemsService
{
    public interface IAsyncRun
    {
        void DoTask(IWorkItem workItem);
    }
}

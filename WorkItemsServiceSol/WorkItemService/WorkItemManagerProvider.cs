using System;

namespace WorkItemsService
{
    public static class WorkItemManagerProvider
    {
        private static IWorkItemManager workerManager;
        private static readonly object lockOjbect = new Object();

        public static IWorkItemManager GetWorkItemManager()
        {
            lock (lockOjbect)
            {
                if (workerManager == null)
                {
                    workerManager = new WorkItemManager();
                    workerManager.Service = new AsyncRun();
                }
            }
            return workerManager;
        }
    }
}

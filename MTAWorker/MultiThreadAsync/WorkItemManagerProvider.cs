using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessWorkItems
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

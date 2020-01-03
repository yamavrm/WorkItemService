using System;

namespace WorkItemsService
{
    public class StatusUpdatedEventArgs : EventArgs
    {
        public StatusUpdatedEventArgs(string updateInfo)
        {
            this.UpdateInfo = updateInfo;
        }

        public string UpdateInfo { get; private set; }
    }
}

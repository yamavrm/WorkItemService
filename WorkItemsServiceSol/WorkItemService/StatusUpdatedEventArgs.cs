using System;

namespace WorkItemsService
{
    public class StatusUpdatedEventArgs : EventArgs
    {
        public StatusUpdatedEventArgs(string message)
        {
            this.Message = message;
        }

        public string Message { get; private set; }
    }
}

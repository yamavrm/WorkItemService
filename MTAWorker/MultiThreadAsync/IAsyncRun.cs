using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessWorkItems
{
    public interface IAsyncRun
    {
        void DoTask(IWorkItem workItem);
    }
}

using WorkItemsService;
using Prism.Commands;
using Prism.Mvvm;

namespace WorkItemsClient
{
    class ClientViewModel : BindableBase
    {
        private int duration;
        private string name;
        private string response;
        private IWorkItemManager workItemManager;

        public ClientViewModel()
        {
            AddCommand = new DelegateCommand(ExecuteAdd, CanExecuteAdd);

            InitService();
        }

        private void InitService()
        {
            this.workItemManager = WorkItemManagerProvider.GetWorkItemManager();
            this.workItemManager.StatusUpdated += WorkItemManager_StatusUpdated;
        }

        private void WorkItemManager_StatusUpdated(object sender, StatusUpdatedEventArgs e)
        {
            this.Response += e.UpdateInfo + "\n";
        }


        private bool CanExecuteAdd()
        {
            return (!string.IsNullOrEmpty(Name) && Duration > 0);
        }

        public DelegateCommand AddCommand { get; private set; }

        public string Name
        {
            get => this.name;
            set
            {
                SetProperty(ref this.name, value);
                AddCommand.RaiseCanExecuteChanged();
            }
        }

        public int Duration
        {
            get => this.duration;
            set
            {
                SetProperty(ref this.duration, value);
                AddCommand.RaiseCanExecuteChanged();
            }
        }

        private void ExecuteAdd()
        {
            if (Duration > 0)
            {
                workItemManager?.AddWorkItem(new WorkItem(Name, Duration));
            }
        }

        public string Response
        {
            get => this.response;
            set
            {
                SetProperty(ref this.response, value);
            }
        }
    }
}

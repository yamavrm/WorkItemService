using WorkItemsService;
using Prism.Commands;
using Prism.Mvvm;

namespace WorkItemsClient
{
    class ClientViewModel : BindableBase
    {
        private int duration;
        private string name;
        private string url;
        private string response;
        private IWorkItemManager workItemManager;

        public ClientViewModel()
        {
            AddCommand = new DelegateCommand(ExecuteAdd, CanExecuteAdd);
            UrlAddCommand = new DelegateCommand(ExecuteUrlAdd, CanExecuteUrlAdd);
            this.name = "A";
            this.duration = 10;
            this.url = "https://www.google.com";
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

        private bool CanExecuteUrlAdd()
        {
            return (!string.IsNullOrEmpty(Url));
        }

        public DelegateCommand AddCommand { get; private set; }

        public DelegateCommand UrlAddCommand { get; private set; }

        public string Name
        {
            get => this.name;
            set
            {
                SetProperty(ref this.name, value);
                AddCommand.RaiseCanExecuteChanged();
            }
        }

        public string Url
        {
            get => this.url;
            set
            {
                SetProperty(ref this.url, value);
                UrlAddCommand.RaiseCanExecuteChanged();
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

        private void ExecuteUrlAdd()
        {
            if (!string.IsNullOrWhiteSpace(Url))
            {
                workItemManager?.AddWorkItem(new WorkItemWebPage(Url));
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

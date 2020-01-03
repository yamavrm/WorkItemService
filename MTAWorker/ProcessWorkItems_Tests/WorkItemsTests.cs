using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProcessWorkItems;
using Shouldly;
using System.Threading;
using Moq;

namespace ProcessWorkItems_Tests
{
    [TestClass]
    public class WorkItemsTests
    {
        [TestMethod]
        public void WorkItemManager_GetSingleton_Test()
        {
            //Arrange
            IWorkItemManager workerManager1 = WorkItemManagerProvider.GetWorkItemManager();
            IWorkItemManager workerManager2 = WorkItemManagerProvider.GetWorkItemManager();

            //Assert
            Should.ReferenceEquals(workerManager1, workerManager2);
        }

        [TestMethod]
        public void WorkItemManager_WhenWorkItemAdded_ShoulBe_Running()
        {
            IWorkItemManager workerManager = WorkItemManagerProvider.GetWorkItemManager();

            //Arrange
            var moqWorkItem = new Mock<IWorkItem>();
            moqWorkItem.Setup(c => c.Name).Returns("A");
            moqWorkItem.Setup(c => c.Duration).Returns(100);

            Mock<IAsyncRun> moqAsyncService = new Mock<IAsyncRun>();
            moqAsyncService.Setup(c => c.DoTask(It.IsAny<IWorkItem>()))
                           .Callback<IWorkItem>((workItem) =>
                           {
                               moqWorkItem.Raise(x => x.WorkItemStatusUpdated += null,
                                    moqWorkItem.Object, 
                                    new WorkItemStatusUpdatedEventArgs(WorkItemStatus.Running));
                           });
            workerManager.Service = (IAsyncRun)moqAsyncService.Object;

            //moqWorkItem.Setup(c => c.Execute())
            //           .Callback(() =>
            //           {
            //               moqWorkItem.Raise(x => x.WorkItemStatusUpdated += null,
            //               moqWorkItem.Object,
            //               new WorkItemStatusUpdatedEventArgs(WorkItemStatus.Running));
            //           });

            //Act
            workerManager.AddWorkItem((IWorkItem)moqWorkItem.Object);
            var result = workerManager.IsServiceRunning;

            //Assert
            result.ShouldBeTrue();
        }

        [TestMethod]
        public void WorkItemManager_Start_Service_Test()
        {
            //Arrange
            IWorkItemManager workerManager1 = WorkItemManagerProvider.GetWorkItemManager();

            //Act
            //workerManager1.Start();
            //Thread.Sleep(10);

            var result = workerManager1.IsServiceRunning;

            //Assert
            result.ShouldBeTrue();
        }

    }
}

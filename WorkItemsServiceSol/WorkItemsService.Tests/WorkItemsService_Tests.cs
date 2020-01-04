using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkItemsService;
using Shouldly;
using Moq;

namespace ProcessWorkItems_Tests
{
    [TestClass]
    public class WorkItemsTests
    {
        private IWorkItemManager GetWorkItemManager()
        {
            return WorkItemManagerProvider.GetWorkItemManager();
        }

        [TestMethod]
        public void When_GetTwoWorkItemManagerObjects_Expect_ShouldRefSameObject()
        {
            //Arrange
            IWorkItemManager workerManager1 = WorkItemManagerProvider.GetWorkItemManager();
            IWorkItemManager workerManager2 = WorkItemManagerProvider.GetWorkItemManager();

            //Assert
            Should.ReferenceEquals(workerManager1, workerManager2);
        }

        [TestMethod]
        public void When_WorkItemAdded_Expect_ServiceShouldExecuteIt()
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
                                    new WorkItemStatusUpdatedEventArgs(WorkItemStatus.Completed));
                           });
            workerManager.Service = (IAsyncRun)moqAsyncService.Object;

            //Act
            workerManager.AddWorkItem((IWorkItem)moqWorkItem.Object);

            //Assert
            moqAsyncService.Verify(c => c.DoTask(It.IsAny<IWorkItem>()), Times.Once);
        }

        [TestMethod]
        public void When_WorkItemsAdded_Expect_ItShouldRunInTheOrder()
        {
            IWorkItemManager workerManager = WorkItemManagerProvider.GetWorkItemManager();

            //Arrange
            string workItemNames = "";
            var moqWorkItem1 = new Mock<IWorkItem>();
            moqWorkItem1.Setup(c => c.Name).Returns("A");
            moqWorkItem1.Setup(c => c.Duration).Returns(10);

            var moqWorkItem2 = new Mock<IWorkItem>();
            moqWorkItem2.Setup(c => c.Name).Returns("B");
            moqWorkItem2.Setup(c => c.Duration).Returns(10);

            Mock<IAsyncRun> moqAsyncService = new Mock<IAsyncRun>();
            moqAsyncService.Setup(c => c.DoTask(It.IsAny<IWorkItem>()))
                           .Callback<IWorkItem>((workItem) =>
                           {
                               if (workItem.Equals(moqWorkItem1.Object))
                               {
                                   workItemNames += workItem.Name.ToString();
                                   moqWorkItem1.Raise(x => x.WorkItemStatusUpdated += null,
                                        moqWorkItem1.Object,
                                        new WorkItemStatusUpdatedEventArgs(WorkItemStatus.Completed));
                               }

                               if (workItem.Equals(moqWorkItem2.Object))
                               {
                                   workItemNames += workItem.Name.ToString();
                                   moqWorkItem2.Raise(x => x.WorkItemStatusUpdated += null,
                                        moqWorkItem2.Object,
                                        new WorkItemStatusUpdatedEventArgs(WorkItemStatus.Completed));
                               }
                           });
            workerManager.Service = (IAsyncRun)moqAsyncService.Object;

            //Act
            workerManager.AddWorkItem((IWorkItem)moqWorkItem1.Object);
            workerManager.AddWorkItem((IWorkItem)moqWorkItem2.Object);

            //Assert
            Assert.AreEqual("AB", workItemNames);
        }
    }
}

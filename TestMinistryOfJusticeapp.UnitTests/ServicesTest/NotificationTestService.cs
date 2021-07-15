using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNet.Identity;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.IdentityEntities;
using ministryofjusticeDomain.Interfaces.Repository;
using ministryofjusticeWebUi.Models;
using ministryofjusticeWebUi.Services;
using Moq;

namespace TestMinistryOfJusticeapp.UnitTests.ServicesTest
{
    [TestFixture]
    public class NotificationTestService
    {
        private Mock<INotificationUnitOfWork> _notificationUnitOfMock;
        private NotificationService _reportService;
        private readonly Mock<IMappingEngine> mapper = new Mock<IMappingEngine>();
        private IEnumerable<Notification> allNotifications;
        [SetUp]
        public void SetUp()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Notification, NotificationViewModel>();
            });
            _notificationUnitOfMock = new Mock<INotificationUnitOfWork>();
            _reportService = new NotificationService(_notificationUnitOfMock.Object);
            allNotifications = new List<Notification>
            {
                new Notification{Id = 1, CaseId = 1, IsViewed = true, Message = "Do i care", ReceiverId = "receiverId"},
                new Notification{Id = 2, CaseId = 2, IsViewed = false, Message = "Do i care", ReceiverId = "receiver"},
                new Notification{Id = 3, CaseId = 3, IsViewed = true, Message = "Do i care", ReceiverId = "receive"},
                new Notification{Id = 4, CaseId = 1, IsViewed = false, Message = "Do i care", ReceiverId = "receiver"},
            };
        }
        [Test]
        [TestCase(1)]
        public void Get_Notification_Return_NotificationViewModel(int id)
        {
            _notificationUnitOfMock.Setup(x => x.NotificationRepo.GetNotification(It.IsAny<int>()))
                .Returns(allNotifications.SingleOrDefault(x => x.Id == id));
            var result = _reportService.GetNotification(id).GetType();
            Assert.IsNotNull(result);
            Assert.AreEqual(result, typeof(NotificationViewModel));
            _notificationUnitOfMock.Verify(x => x.NotificationRepo.GetNotification(It.IsAny<int>()));
        }

        [Test]
        public void Get_All_Notifications_Returns_4_Notifications()
        {
            var user = new ApplicationUser
            {
                Id = "receiver"
            };
            _notificationUnitOfMock.Setup(x => x.UserManagerRepo.GetCurrentUser()).Returns(user);
            _notificationUnitOfMock.Setup(x => x.NotificationRepo.GetAll())
                .Returns(allNotifications.Where(x => x.ReceiverId == user.Id));
            var result = _reportService.GetNotifications().Count();
            Assert.AreEqual(2, result);
            _notificationUnitOfMock.Verify(c => c.NotificationRepo.GetAll());
        }
    }
}

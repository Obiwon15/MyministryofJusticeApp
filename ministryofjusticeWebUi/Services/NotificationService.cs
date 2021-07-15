using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNet.Identity;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.Interfaces.Repository;
using ministryofjusticeWebUi.Interfaces;
using ministryofjusticeWebUi.Models;
using static System.Web.HttpContext;

namespace ministryofjusticeWebUi.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationUnitOfWork _notificationUnitOfWork;
        public NotificationService(INotificationUnitOfWork notificationUnitOfWork)
        {
            _notificationUnitOfWork = notificationUnitOfWork;
        }

        public IEnumerable<NotificationViewModel> GetNotifications()
        {
            var userId = _notificationUnitOfWork.UserManagerRepo.GetCurrentUser();
            var notifications = _notificationUnitOfWork.NotificationRepo.GetAll()
                .Where(n => n.ReceiverId == userId.Id);

            return Mapper.Map<IEnumerable<NotificationViewModel>>(notifications);
        }

        public NotificationViewModel GetNotification(int id)
        { 
            var notifyId = _notificationUnitOfWork.NotificationRepo.GetById(id);

            if(notifyId == null)
                throw new Exception();

            var notification = _notificationUnitOfWork.NotificationRepo.GetNotification(id);
            _notificationUnitOfWork.NotificationRepo.Save();
            return Mapper.Map<NotificationViewModel>(notification);
        }

        public void NotifyUser(int caseId, string userId, string message)
        {
            throw new NotImplementedException();
        }
    }
}
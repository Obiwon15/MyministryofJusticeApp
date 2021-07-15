using System.Collections.Generic;
using ministryofjusticeDomain.Entities;
using ministryofjusticeWebUi.Models;

namespace ministryofjusticeWebUi.Interfaces
{
    public interface INotificationService
    {
        IEnumerable<NotificationViewModel> GetNotifications();
        NotificationViewModel GetNotification(int id);
    }
}

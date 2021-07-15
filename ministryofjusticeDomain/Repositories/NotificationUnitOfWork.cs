using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ministryofjusticeDomain.Interfaces.Repository;

namespace ministryofjusticeDomain.Repositories
{
    public class NotificationUnitOfWork : INotificationUnitOfWork
    {
        public INotificationRepo NotificationRepo { get;  }
        public IUserManagerRepo UserManagerRepo { get; }

        public NotificationUnitOfWork
        (
            IUserManagerRepo userManagerRepo,
            INotificationRepo notificationRepo
        )
        {
            UserManagerRepo = userManagerRepo;
            NotificationRepo = notificationRepo;
        }
    }
}
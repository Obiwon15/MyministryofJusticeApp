using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ministryofjusticeDomain.Entities;

namespace ministryofjusticeDomain.Interfaces.Repository
{
    public interface INotificationRepo : IGenericRepo<Notification>
    {
        void CreateNotification(int caseId, string receiverId, string message);
        Notification GetNotification(int id);
        void AlertAttorneyGeneral(int caseId, string message);
    }
}

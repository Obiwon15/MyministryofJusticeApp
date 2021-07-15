using System.Linq;
using System.Threading.Tasks;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.IdentityEntities;
using ministryofjusticeDomain.Interfaces.Repository;

namespace ministryofjusticeDomain.Repositories
{
    public class NotificationRepo : GenericRepo<Notification>, INotificationRepo
    {
        private readonly ApplicationDbContext _context;
        public NotificationRepo(ApplicationDbContext context)
            :base(context)
        {
            _context = context;
        }
        public void CreateNotification(int caseId, string receiverId, string message)
        {
            var notification = new Notification()
            {
                CaseId = caseId,
                ReceiverId = receiverId,
                Message = message,
            };

            _context.Notifications.Add(notification);
            _context.SaveChanges();
        }

        public Notification GetNotification(int id)
        {
            var ntfn = _context.Notifications.Find(id);
            ntfn.IsViewed = true;
            return ntfn;
        }

        public void AlertAttorneyGeneral(int caseId, string message)
        {
            var agUser = _context.Users.FirstOrDefault(u => u.Email == "attorneygeneral@ministryofjustice.com");
            if (agUser != null)
            {
                var notification = new Notification()
                {
                    CaseId = caseId,
                    ReceiverId = agUser.Id,
                    Message = message
                };
                _context.Notifications.Add(notification);
                _context.SaveChanges();
            }
            
        }
    }
}

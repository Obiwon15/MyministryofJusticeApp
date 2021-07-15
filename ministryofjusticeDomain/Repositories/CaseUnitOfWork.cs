using ministryofjusticeDomain.IdentityEntities;
using ministryofjusticeDomain.Interfaces.Repository;
using ministryofjusticeDomain.Interfaces.Services;

namespace ministryofjusticeDomain.Repositories
{
    public class CaseUnitOfWork : ICaseUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IDepartmentRepo DepartmentRepo { get; set; }
        public IMailSender MailSender { get; set; }
        public ICaseRepo CaseRepo { get; set; }
        public ILawyerRepo LawyerRepo { get; set; }
        public ICommentRepo CommentRepo { get; set; }
        public IUserManagerRepo UserManagerRepo { get; }
        public INotificationRepo NotificationRepo { get; }
        public IDepartmentHeadRepo DepartmentHeadRepo { get; }

        public CaseUnitOfWork
        (
            IDepartmentRepo departmentRepo,
            IUserManagerRepo userManagerRepo,
            IMailSender mailSender,
            ILawyerRepo lawyerRepo,
            IDepartmentHeadRepo departmentHeadRepo,
            INotificationRepo notificationRepo,
            ICaseRepo caseRepo,
            ICommentRepo commentRepo,
            ApplicationDbContext context
        )
        {
            DepartmentHeadRepo = departmentHeadRepo;
            NotificationRepo = notificationRepo;
            UserManagerRepo = userManagerRepo;
            DepartmentRepo = departmentRepo;
            MailSender = mailSender;
            LawyerRepo = lawyerRepo;
            CaseRepo = caseRepo;
            CommentRepo = commentRepo;
            _context = context;
        }
    }
}
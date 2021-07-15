using ministryofjusticeDomain.Interfaces.Services;

namespace ministryofjusticeDomain.Interfaces.Repository
{
    public interface ICaseUnitOfWork
    {
        IMailSender MailSender { get; set; }
        ICaseRepo CaseRepo { get; set; }
        ILawyerRepo LawyerRepo { get; set; }
        ICommentRepo CommentRepo { get; set; }
        IDepartmentRepo DepartmentRepo { get; set; }
        IUserManagerRepo UserManagerRepo { get; }
        IDepartmentHeadRepo DepartmentHeadRepo { get; }
        INotificationRepo NotificationRepo { get; }
    }
}
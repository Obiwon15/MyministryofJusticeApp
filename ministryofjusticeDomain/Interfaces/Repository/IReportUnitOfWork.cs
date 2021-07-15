using ministryofjusticeDomain.Interfaces.Services;

namespace ministryofjusticeDomain.Interfaces.Repository
{
    public interface IReportUnitOfWork
    {
        IMailSender MailSender { get; }
        ICaseRepo CaseRepo { get; }
        ICaseFileRepo CaseFileRepo { get; }
    }
}
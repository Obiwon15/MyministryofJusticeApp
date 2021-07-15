using ministryofjusticeDomain.Interfaces.Repository;
using ministryofjusticeDomain.Interfaces.Services;

namespace ministryofjusticeDomain.Repositories
{
    public class ReportUnitOfWork : IReportUnitOfWork
    {
        public IMailSender MailSender { get; }
        public ICaseRepo CaseRepo { get; }
        public ICaseFileRepo CaseFileRepo { get; }

        public ReportUnitOfWork
        (
            ICaseRepo caseRepo,
            IMailSender mailSender,
            ICaseFileRepo caseFileRepo
        )
        {
            MailSender = mailSender;
            CaseRepo = caseRepo;
            CaseFileRepo = caseFileRepo;
        }
    }
}
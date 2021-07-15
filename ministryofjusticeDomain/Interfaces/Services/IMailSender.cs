using System.Threading.Tasks;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.Enum;

namespace ministryofjusticeDomain.Interfaces.Services
{
    public interface IMailSender
    {
        Task AccountCreatedAsync(string receiverEmail, string receiverName);
        Task NotifyAsync(string receiverEmail, string receiverName, AssignedTo assignedTo);
        Task SendCaseStatus(string receiverEmail, string receiverName, Case currentCase);
    }
}

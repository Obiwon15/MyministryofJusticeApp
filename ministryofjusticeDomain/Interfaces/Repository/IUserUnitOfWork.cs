using System;
using System.Threading.Tasks;
using ministryofjusticeDomain.Interfaces.Services;

namespace ministryofjusticeDomain.Interfaces.Repository
{
    public interface IUserUnitOfWork
    {
        IDepartmentRepo DepartmentRepo { get; }
        IUserManagerRepo UserManagerRepo { get; }
        IRoleService RoleService { get; }
        IProfileRepo ProfileRepo { get; }
        IMailSender MailSender { get; }
        void Complete();
        Task<int> SaveAsync();
    }
}
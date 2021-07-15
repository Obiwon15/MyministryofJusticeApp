using System;
using System.Threading.Tasks;
using ministryofjusticeDomain.IdentityEntities;
using ministryofjusticeDomain.Interfaces.Repository;
using ministryofjusticeDomain.Interfaces.Services;

namespace ministryofjusticeDomain.Repositories
{

    public class UserUnitOfWork : IUserUnitOfWork

    {
        private readonly ApplicationDbContext _context;

        public IDepartmentRepo DepartmentRepo { get; }
        public IUserManagerRepo UserManagerRepo { get; }
        public IRoleService RoleService { get; }
        public IProfileRepo ProfileRepo { get; set; }
        public IMailSender MailSender { get; set; }

        public UserUnitOfWork(IDepartmentRepo departmentRepo,
            IUserManagerRepo userManagerRepo,
            IRoleService roleService,
            IProfileRepo profileRepo,
            IMailSender mailSender,

            ApplicationDbContext context)
        {
            DepartmentRepo = departmentRepo;
            UserManagerRepo = userManagerRepo;
            RoleService = roleService;
            ProfileRepo = profileRepo;
            MailSender = mailSender;
            _context = context;
        }

        public void Complete()
        {
            _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
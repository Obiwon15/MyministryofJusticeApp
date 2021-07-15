using ministryofjusticeDomain.Interfaces.Repository;

namespace ministryofjusticeDomain.Repositories
{
    public class DepartmentUnitOfWork : IDepartmentUnitOfWork
    {

        public IDepartmentRepo DepartmentRepo { get; }
        public IUserManagerRepo UserManagerRepo { get; }

        public DepartmentUnitOfWork
        (
            IDepartmentRepo departmentRepo,
            IUserManagerRepo userManagerRepo
        )
        {

            DepartmentRepo = departmentRepo;
            UserManagerRepo = userManagerRepo;
        }
    }
}
namespace ministryofjusticeDomain.Interfaces.Repository
{
    public interface IDepartmentUnitOfWork
    {
        IDepartmentRepo DepartmentRepo { get; }
        IUserManagerRepo UserManagerRepo { get; }
    }
}
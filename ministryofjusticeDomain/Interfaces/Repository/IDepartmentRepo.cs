using System.Collections.Generic;
using ministryofjusticeDomain.Entities;

namespace ministryofjusticeDomain.Interfaces.Repository
{
    public interface IDepartmentRepo : IGenericRepo<Department>
    {
        IEnumerable<Department> GetDepartments();
        void AddUserToDepartment(string userId, int departmentId);
    }
}
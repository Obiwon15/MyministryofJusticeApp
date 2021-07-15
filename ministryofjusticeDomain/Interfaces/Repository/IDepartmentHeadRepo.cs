using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ministryofjusticeDomain.Entities;

namespace ministryofjusticeDomain.Interfaces.Repository
{
    public interface IDepartmentHeadRepo : IGenericRepo<DepartmentHead>
    {
        DepartmentHead GetDepartmentHead(int departmentId);
    }
}

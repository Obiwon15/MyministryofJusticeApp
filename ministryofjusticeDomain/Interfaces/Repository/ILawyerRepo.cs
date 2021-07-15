using System.Collections.Generic;
using ministryofjusticeDomain.Entities;

namespace ministryofjusticeDomain.Interfaces.Repository
{
    public interface ILawyerRepo : IGenericRepo<Lawyer>
    {
        IEnumerable<Lawyer> GetLawyersInDepartment(int departmentId);
        IEnumerable<Case> GetAllLawyerCase();
        Lawyer GetCurrentLawyer();
    }
}

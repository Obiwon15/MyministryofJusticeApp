using System.Collections.Generic;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.Enum;

namespace ministryofjusticeDomain.Interfaces.Repository
{
    public interface ICaseRepo : IGenericRepo<Case>
    {
        Case PostCase(Case report);
        IEnumerable<Case> PendingCases();
        void AssignToDept(int id);
        void RejectCase(int id);
        bool AssignCaseToLawyer(int lawyerId, int caseId);
        bool UnAssignCaseToLawyer(int lawyerId, int caseId);
        Case TrackCase(string caseID);
        IEnumerable<Case> Search(SearchCategory category, string value);
        Case GetCasesWithComment(int id);
        Case ChangeCaseStatus(int CaseID, Status status);
        IEnumerable<Case> CasesWithLaywers();
    }
    
}

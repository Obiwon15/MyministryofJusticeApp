using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.  Tasks;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.Enum;
using ministryofjusticeWebUi.Models;

namespace ministryofjusticeWebUi.Interfaces
{
    public interface ICaseService
    {
        IEnumerable<CaseDetailsViewModel> GetCases(QueryType queryBy, string queryParam = null);
        CaseDetailsViewModel GetCaseDetails(int id);
        AssignCaseViewModel GetCaseLawyers(int id);
        void AuditCase(int id, CaseAction action);
        bool AssignAction(int caseId, int lawyerId, CaseAction action);
        void NotifyClient(CaseDetailsViewModel model);
        void InsertComment(Comment comment);
        bool CaseStatusChange (int CaseID, Status status);
        IEnumerable<CaseDetailsViewModel> FilterLawyerCases(IEnumerable<CaseDetailsViewModel> allCases);
    }
}

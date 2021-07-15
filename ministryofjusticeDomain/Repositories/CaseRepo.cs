using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using ministryofjusticeDomain.Enum;
using ministryofjusticeDomain.Interfaces.Repository;

namespace ministryofjusticeDomain.Repositories
{
    public class CaseRepo : GenericRepo<Case>, ICaseRepo
    {
        private readonly ApplicationDbContext _context;

        public CaseRepo(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }
        private string CaseIDGeneration(int size)
        {
            Random random = new Random();
            string randomno = "abcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = randomno[random.Next(0, randomno.Length)];
                builder.Append(ch);
            }

            return "CASEMOJ" + builder.ToString();
        }

        public IEnumerable<Case> PendingCases()
        {
            return _context.Cases.Where(x => x.StatusID == Status.Pending);
        }

        public Case PostCase(Case report)
        {
            report.StatusID = Status.Pending;
            report.CaseID = CaseIDGeneration(6);
            report.SubmitDate = DateTime.Now;
            return report;
        }

        public void AssignToDept(int id)
        {
            var dCase = _context.Cases.Find(id);
            if (dCase != null) dCase.StatusID = Status.Accepted;
        }

        public bool AssignCaseToLawyer(int lawyerId, int caseId)
        {
            var lCase = _context.Cases.Find(caseId);
            var lawyer = _context.Lawyers.Find(lawyerId);


            try
            {
                if (lCase != null && lawyer != null)
                {
                    lCase.StatusID = Status.Assigned;
                    lCase.LawyerId = lawyerId;
                    lCase.Lawyer = lawyer;
                    _context.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                throw new Exception($"no case found {e}");
            }

            return false;
        }

        public bool UnAssignCaseToLawyer(int lawyerId, int caseId)
        {
            var lCase = _context.Cases.Find(caseId);
            var lawyer = _context.Lawyers.Find(lawyerId);

            try
            {
                if (lCase != null && lawyer != null)
                {
                    lCase.StatusID = Status.Accepted;
                    lCase.LawyerId = null;
                    _context.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                throw new Exception($"no case found {e}");
            }

            return false;
        }

        public void RejectCase(int id)
        {
            var dCase = _context.Cases.Find(id);
            if (dCase != null)
                dCase.StatusID = Status.Rejected;
        }
        public Case TrackCase(string caseID)
        {
            var trackedCase = _context.Cases.FirstOrDefault(x => x.CaseID.ToLower() == caseID.ToLower());
            return trackedCase;
        }
        public IEnumerable<Case> Search(SearchCategory category, string value)
        {
            switch (category)
            {
                case SearchCategory.CaseId:
                    return _context.Cases.Where(c => c.CaseID == value);
                case SearchCategory.ClientEmail:
                    return _context.Cases.Where(c => c.Email == value);
                default:
                    return new List<Case>();
            }
        }

        public Case GetCasesWithComment(int id)
        {
          return  _context.Cases.Include(c => c.Comments.Select(_ => _.User)).SingleOrDefault(c => c.Id == id );
        }

        public Case ChangeCaseStatus(int CaseID, Status status)
        {
            var dCase = _context.Cases.Find(CaseID);

            if (dCase != null)
            {
                dCase.StatusID = status;
                _context.Entry(dCase).State = EntityState.Modified;
                _context.SaveChanges();

            }
            return dCase;
        }
        public IEnumerable<Case> CasesWithLaywers()
        {
            return _context.Cases.Where(x => x.StatusID == Status.RequestClose).Include(_=>_.Lawyer.User);
        }

    }
}
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.Interfaces.Repository;
using ministryofjusticeWebUi.Interfaces;
using ministryofjusticeWebUi.Models;
using ministryofjusticeDomain.Enum;
using static System.Web.HttpContext;

namespace ministryofjusticeWebUi.Services
{
    public class CaseService : ICaseService
    {
        private readonly ICaseUnitOfWork _caseUnitOfWork;
        
        public CaseService(ICaseUnitOfWork caseUnitOfWork)
        {
            _caseUnitOfWork = caseUnitOfWork;
        }
        public CaseDetailsViewModel GetCaseDetails(int id)
        {
            var targetCase = _caseUnitOfWork.CaseRepo.GetById(id);
            if (targetCase == null)
                return null;
            var caseDetails = Mapper.Map<CaseDetailsViewModel>(targetCase);
            caseDetails.Department = _caseUnitOfWork.DepartmentRepo.GetById(caseDetails.DepartmentId);
            if (caseDetails.LawyerId != null)
            {
                caseDetails.Lawyer = _caseUnitOfWork.LawyerRepo.GetById(caseDetails.LawyerId.Value);
                caseDetails.Lawyer.User = _caseUnitOfWork.UserManagerRepo.GetUserDetails(caseDetails.Lawyer.ApplicationUserId);
            }
            caseDetails.Comment = new CommentViewModel
            {
                CaseId = id,
                ApplicationUserId = _caseUnitOfWork.UserManagerRepo.GetCurrentUser().Id
            };
            return caseDetails;
        }

        /// <summary>
        /// Carries out actions based on the query type provided
        /// </summary>
        /// <param name="queryType">Determines the kind of query to be ran on it</param>
        /// <param name="queryParam">The parameter that will be used to filter the results</param>
        /// <returns>An IEnumerable of cases based on the query type</returns>
        public IEnumerable<CaseDetailsViewModel> GetCases(QueryType queryType, string queryParam = null)
        {
            var cases = Mapper.Map<IEnumerable<CaseDetailsViewModel>>(_caseUnitOfWork.CaseRepo.GetAll());
            switch (queryType)
            {
                case QueryType.Status:
                    return GetCasesByStatus(cases, queryParam);

                case QueryType.Assigned:
                    return GetDepartmentCases(cases, queryParam);

                case QueryType.Search:
                    return SearchCases(queryParam);

                case QueryType.LawyerCase:
                    return GetLawyersCases();
                default:
                    return cases;
            }
        }

        /// <summary>
        /// Gets cases based on their status. Whether pending, assigned, accepted or rejected
        /// </summary>
        /// <param name="cases">All the cases from the database</param>
        /// <param name="status">Status of case</param>
        /// <returns></returns>
        /// chnage back to private
        public IEnumerable<CaseDetailsViewModel> GetCasesByStatus(IEnumerable<CaseDetailsViewModel> cases, string status)
        {
            switch (status)
            {
                case "Pending":
                    cases = cases.Where(c => c.StatusID == Status.Pending);
                    break;
                case "Accepted":
                    cases = cases.Where(c => c.StatusID == Status.Accepted || c.StatusID == Status.Assigned || c.StatusID == Status.Processing);
                    break;
                case "Rejected":
                    cases = cases.Where(c => c.StatusID == Status.Rejected);
                    break;
                case "Closed":
                    cases = cases.Where(c => c.StatusID == Status.Closed);
                    break;
                case "Request to Close":
                    cases = Mapper.Map<IEnumerable<CaseDetailsViewModel>>(_caseUnitOfWork.CaseRepo.CasesWithLaywers());
                    break;
            }

            return cases;
        }
        //return back to private
        public IEnumerable<CaseDetailsViewModel> GetDepartmentCases(IEnumerable<CaseDetailsViewModel> cases, string assigned)
        {
            var userId = _caseUnitOfWork.UserManagerRepo.GetCurrentUser();
            var departmentId = userId.DepartmentId; 
            var departmentCases = cases.Where(c => c.IsAccepted());

            if (departmentId != null)
            {
                departmentCases = departmentCases.Where(c => c.DepartmentId == departmentId);
            }


            switch (assigned)
            {
                case "Assigned":
                    return departmentCases.Where(c => c.LawyerId != null).OrderBy(c => c.Id);
                case "Unassigned":
                    return departmentCases.Where(c => c.LawyerId == null).OrderBy(c => c.Id);
            }

            return departmentCases;
        }

        /// <summary>
        /// Searches for matching cases according to the query param
        /// </summary>
        /// <param name="query">Ex: "category=CaseId&param=CaseMOJab123"</param>
        /// <returns></returns>
        public IEnumerable<CaseDetailsViewModel> SearchCases(string query)
        {
            var cases = new List<Case>();
            var queryList = query.Split('&');
            var category = queryList[0].Replace("category=", "");
            var param = queryList[1].Replace("param=", "");
            switch (category)
            {
                case "CaseId":
                    cases = _caseUnitOfWork.CaseRepo.Search(SearchCategory.CaseId, param).ToList();
                    break;
                case "ClientEmail":
                    cases = _caseUnitOfWork.CaseRepo.Search(SearchCategory.ClientEmail, param).ToList();
                    break;
            }

            // Filter results for Director of Department
            if (Current.User.IsInRole("Director of Department"))
            {
                return GetDepartmentCases(Mapper.Map<IEnumerable<CaseDetailsViewModel>>(cases), null);
            }

            //Filter results for Lawyer
            if (Current.User.IsInRole("Lawyer"))
            {
                cases = cases.FindAll(c=>c.LawyerId != null);
                var lawyer = _caseUnitOfWork.LawyerRepo.GetCurrentLawyer();
                cases = cases.FindAll(c => c.LawyerId == lawyer.Id);
            }

            return Mapper.Map<IEnumerable<CaseDetailsViewModel>>(cases);
        }
        
        //change back to private
        public IEnumerable<CaseDetailsViewModel> GetLawyersCases()  
        {
            var lawyerCases = Mapper.Map<IEnumerable<CaseDetailsViewModel>>(_caseUnitOfWork.LawyerRepo.GetAllLawyerCase());
            return lawyerCases;
        }

        public AssignCaseViewModel GetCaseLawyers(int id)
        {
            var assignCase = _caseUnitOfWork.CaseRepo.GetById(id);
            var assignCaseViewModel = new AssignCaseViewModel
            {
                CaseId = id,
                Case = assignCase,
                Lawyers = _caseUnitOfWork.LawyerRepo.GetLawyersInDepartment(assignCase.DepartmentId).ToList()
            };
            return assignCaseViewModel;
        }

        /// <summary>
        /// Assigns or retains lawyer to a case
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="lawyerId"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool AssignAction(int caseId, int lawyerId, CaseAction action)
        {
            var lawyer = _caseUnitOfWork.LawyerRepo.GetById(lawyerId);
            var user = _caseUnitOfWork.UserManagerRepo.GetUserDetails(lawyer.ApplicationUserId);
            var lawyerName = user.FullName;
            var lawyerEmail = user.Email;
            var lawyerUserId = lawyer.ApplicationUserId;

            switch (action)
            {
                case CaseAction.Assign:
                    _caseUnitOfWork.MailSender.NotifyAsync(lawyerEmail, lawyerName, AssignedTo.Lawyer);
                    _caseUnitOfWork.NotificationRepo.CreateNotification(caseId, lawyerUserId, 
                        "A new case has been assigned to you.");
                    return _caseUnitOfWork.CaseRepo.AssignCaseToLawyer(lawyerId, caseId);

                case CaseAction.UnAssign:
                    return _caseUnitOfWork.CaseRepo.UnAssignCaseToLawyer(lawyerId, caseId);
            }

            return false;
        }


        /// <summary>
        /// The handles the assignment of a case to a department and the deleting of a case
        /// </summary>
        /// <param name="id"></param>
        /// <param name="action"></param>
        public void AuditCase(int id, CaseAction action)
        {
            var targetCase = _caseUnitOfWork.CaseRepo.GetById(id);
            var departmentHead = _caseUnitOfWork.DepartmentHeadRepo.GetDepartmentHead(targetCase.DepartmentId);
            switch (action)
            {
                case CaseAction.Accept:
                    _caseUnitOfWork.CaseRepo.AssignToDept(id);
                    _caseUnitOfWork.NotificationRepo.CreateNotification(id, departmentHead.ApplicationUserId, 
                        $"A new Case {targetCase.CaseID} has been assigned to your department");
                    _caseUnitOfWork.CaseRepo.Save();
                    break;

                case CaseAction.Reject:
                    _caseUnitOfWork.CaseRepo.RejectCase(id);
                    _caseUnitOfWork.CaseRepo.Save();
                    break;
            }
        }

        public void NotifyClient(CaseDetailsViewModel model)
        {
            var clientCase = _caseUnitOfWork.CaseRepo.GetById(model.Id);
            _caseUnitOfWork.MailSender.SendCaseStatus(model.Email, model.Fullname, clientCase);
        }

        public void InsertComment(Comment comment)
        {

            _caseUnitOfWork.CommentRepo.Insert(comment);
            _caseUnitOfWork.CommentRepo.Save();

            _caseUnitOfWork.CommentRepo.Insert(comment);
            _caseUnitOfWork.CommentRepo.Save();

        }
        public bool CaseStatusChange(int CaseID, Status status)
        {
            if (status == Status.Processing)
            {
                var caseUpdate = _caseUnitOfWork.CaseRepo.ChangeCaseStatus(CaseID, status);
                _caseUnitOfWork.MailSender.SendCaseStatus(caseUpdate.Email, caseUpdate.Fullname, caseUpdate);
                return true;
            }
            if (status == Status.RequestClose)
            {
                _caseUnitOfWork.CaseRepo.ChangeCaseStatus(CaseID, status);
                return true;

            }
            if (status == Status.Closed)
            {
                _caseUnitOfWork.CaseRepo.ChangeCaseStatus(CaseID, status);
                _caseUnitOfWork.NotificationRepo.AlertAttorneyGeneral(CaseID, "Request to close case");
                _caseUnitOfWork.CaseRepo.Save();
                return true;

            }

            return false;
        }

        public IEnumerable<CaseDetailsViewModel> FilterLawyerCases(IEnumerable<CaseDetailsViewModel> allCases)
        {
            allCases = allCases.Where(c => c.LawyerId != null);
            var lawyer = _caseUnitOfWork.LawyerRepo.GetCurrentLawyer();
            return allCases.Where(c => c.LawyerId == lawyer.Id);
        }
    }
}
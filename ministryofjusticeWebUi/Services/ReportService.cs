using System;
using System.Web;
using AutoMapper;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.Enum;
using ministryofjusticeDomain.Interfaces.Repository;
using ministryofjusticeWebUi.Interfaces;
using ministryofjusticeWebUi.Models;

namespace ministryofjusticeWebUi.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportUnitOfWork _reportUnitOfWork;

        public ReportService(IReportUnitOfWork reportUnitOfWork)
        {
            _reportUnitOfWork = reportUnitOfWork;
        }
        public CaseDetailsViewModel FindCase(string caseId)
        {
            var clientCase = _reportUnitOfWork.CaseRepo.TrackCase(caseId);
            if (clientCase != null)
            {
                var vm = Mapper.Map<CaseDetailsViewModel>(clientCase);
                return vm;
            }
            
            return null;
            
        }

        public Case SubmitCase(CaseViewModel report, HttpPostedFileBase file, string path)
        {
			var reportDb = Mapper.Map<CaseViewModel, Case>(report);
            
            reportDb = _reportUnitOfWork.CaseRepo.PostCase(reportDb);
            _reportUnitOfWork.CaseRepo.Insert(reportDb);
            _reportUnitOfWork.CaseRepo.Save();

            if (file != null)
            {
                var fileUp = new File()
                {
                    FileCategory = FileCategory.Evidence,
                    CaseId = reportDb.Id,
                    Comments = "User upload",
                    Filename = "User"
                };
                var fileDb = _reportUnitOfWork.CaseFileRepo.UploadFile(fileUp, path, file);


                _reportUnitOfWork.CaseFileRepo.Insert(fileDb);
                _reportUnitOfWork.CaseRepo.Save();

                _reportUnitOfWork.CaseFileRepo.Insert(fileDb);
                _reportUnitOfWork.CaseFileRepo.Save();

            }

            try
            {
                _reportUnitOfWork.MailSender.SendCaseStatus(report.Email, reportDb.Fullname, reportDb);
            }

            catch
            {
                throw new Exception("Mail not send due to no network");
            }

            return reportDb;
        }
    }
}
using System;
using System.Web;
using AutoMapper;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.Interfaces.Repository;
using ministryofjusticeDomain.Interfaces.Services;
using ministryofjusticeWebUi.Interfaces;
using ministryofjusticeWebUi.Models;

namespace ministryofjusticeWebUi.Services
{
    public class CaseFileServices : ICaseFileServices
    {
        private readonly ICaseFileRepo _caseFileRepo;
        private readonly ICustomFileSystem _customFileSystem;

        public CaseFileServices(ICaseFileRepo caseFileRepo, ICustomFileSystem customFileSystem)
        {
            _customFileSystem = customFileSystem;
            _caseFileRepo = caseFileRepo;
        }

        public ManageCaseFilesViewModel ManageCaseFiles(int id)
        {
            var mcf = new ManageCaseFilesViewModel()
            {
                Files = _caseFileRepo.GetAllCaseFiles(id),
                CaseId = id
            };
            return mcf;
        }

        public bool UploadCaseFile(UploadFileViewModels uploadFile, string path, HttpPostedFileBase file)
        {
            try
            {
                var caseFile = Mapper.Map<UploadFileViewModels, File>(uploadFile);

                if (caseFile == null)
                    throw new Exception("no file return");

                var fileDb = _caseFileRepo.UploadFile(caseFile, path, file);

               _caseFileRepo.Insert(fileDb);
                _caseFileRepo.Save();
                return true;
            }
            catch (Exception e)
            { 
                throw new Exception($"Allowed file not uploaded {e}");
            }
        }

        public bool DeleteFile(int id)
        {
            try
            {
                var file = _caseFileRepo.GetById(id);

                if (file == null)
                    throw new Exception("no file matches the id");

                _customFileSystem.DeleteFile(file);
                _caseFileRepo.Delete(id);
                _caseFileRepo.Save();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception($"error no file path {e} to delete");
            }

        }

        public string GetFileType(string path)
        {
            if(string.IsNullOrWhiteSpace(path))
                throw new Exception("path no found");

            var fileExists = _customFileSystem.DoesFileExists(path);
            if (!fileExists)
                return "Not Found";

            if (path.Contains(".pdf")) return "pdf";
            if (path.Contains(".doc") || path.Contains("docx")) return "doc";
            if (path.Contains(".jpeg")) return "jpeg";
            if (path.Contains(".png")) return "png";
            if (path.Contains(".jpg")) return "jpg";
            return "Not Found";
        }
    }
}
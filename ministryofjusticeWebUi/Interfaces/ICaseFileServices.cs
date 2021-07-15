using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ministryofjusticeWebUi.Models;

namespace ministryofjusticeWebUi.Interfaces
{
	public interface ICaseFileServices
	{
		ManageCaseFilesViewModel ManageCaseFiles(int id);
		bool UploadCaseFile(UploadFileViewModels uploadFile, string path, HttpPostedFileBase file);
		bool DeleteFile(int id);
		string GetFileType(string path);
	}
}
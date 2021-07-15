using System.Collections.Generic;
using System.Web;
using ministryofjusticeDomain.Entities;

namespace ministryofjusticeDomain.Interfaces.Repository
{
	public interface ICaseFileRepo : IGenericRepo<File>
	{
		File UploadFile(File file, string path, HttpPostedFileBase postedFile);
		IEnumerable<File> GetAllCaseFiles(int caseId);
		string getFileType(string path);
	}
}

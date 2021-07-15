using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.Enum;
using ministryofjusticeDomain.IdentityEntities;
using ministryofjusticeDomain.Interfaces;
using ministryofjusticeDomain.Interfaces.Repository;

namespace ministryofjusticeDomain.Repositories
{
	public class CaseFileRepo : GenericRepo<File>, ICaseFileRepo
	{
		private  readonly ApplicationDbContext _context;
        public CaseFileRepo(ApplicationDbContext context)
		   : base(context)
		{
			_context = context;
		}

		/// <summary>
		/// This class is for file upload.
		/// Casefiles is the entity, path is the local storage of selected file.
		/// </summary>
		/// <param name="caseFiles"></param>
		/// <param name="path">path = Server.MapPath("~/CaseFiles/")</param>
		public File UploadFile(File file, string path, HttpPostedFileBase postedFile)
		{
			try
			{
				if (!System.IO.Directory.Exists(path))
				{
					System.IO.Directory.CreateDirectory(path);
				}

				string fileName = System.IO.Path.GetFileName(postedFile.FileName);
				path = path + fileName;
				postedFile.SaveAs(path);
				file.FilePath = path;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			return file;
		}

		/// <summary>
		/// Uses case ID to get all files associated with the case
		/// </summary>
		/// <param name="caseId"></param>
		/// <returns></returns>
		public IEnumerable<File> GetAllCaseFiles(int caseId)
		{
			return _context.Files.Where(c => c.CaseId == caseId).ToList();

		}

		/// <summary>
		/// This method uses the file path to return the extension.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public string getFileType(string path)
		{
			if (!System.IO.Directory.Exists(path) )
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

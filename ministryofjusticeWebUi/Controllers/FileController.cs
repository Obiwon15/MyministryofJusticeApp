using System.Web;
using System.Web.Mvc;
using ministryofjusticeWebUi.Interfaces;
using ministryofjusticeWebUi.Models;

namespace ministryofjusticeWebUi.Controllers
{
	[Authorize]

	public class FileController : Controller
	{
		private readonly ICaseFileServices _caseFileServices;
		public FileController()
		{

		}

		public FileController(ICaseFileServices caseFiles)
		{
			_caseFileServices = caseFiles;
		}

		public ActionResult ManageCaseFiles(int id)
		{
			var viewModel = _caseFileServices.ManageCaseFiles(id);
			return View(viewModel);
		}

		//[HttpGet]
		public ActionResult UploadCaseFile(int id)
		{
			var uploadFileView = new UploadFileViewModels()
			{
				CaseId = id
			};
			return View(uploadFileView);
		}

		[HttpPost]
		public ActionResult UploadCaseFile(UploadFileViewModels uploadFile, HttpPostedFileBase file)
		
		{
			if (ModelState.IsValid)
			{
				if (file != null)
				{
					var path = Server.MapPath("~/CaseFiles/");
					var result = _caseFileServices.UploadCaseFile(uploadFile, path, file);
					if (result)
					{
						TempData["Message"] = "File Successfully Uploaded";
						return RedirectToAction("ManageCaseFiles", new { id = uploadFile.CaseId });
					}
				}

				ModelState.Clear();
			}
			return View();
		}

		// GET: File
		public ActionResult DeleteFile(int id, int caseId)
		{
			var result = _caseFileServices.DeleteFile(id);
			return RedirectToAction("ManageCaseFiles", new { id = caseId });
		}
		 public ActionResult FileNotFound(string returnUrl)
		{
			//So that the user can be referred back to where they were when they click ViewFile
			if (string.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null)
				returnUrl = Server.UrlEncode(Request.UrlReferrer.PathAndQuery);

			if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
			{
				ViewBag.ReturnURL = returnUrl;
			}
			return View();
		}
		/// <summary>
		/// this loads a file in  a new tab
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		/// 
		public ActionResult Download(string path)
		{
			string type = _caseFileServices.GetFileType(path);
			if (type.Equals("Not Found"))
				return View("FileNotFound");
			if (type.Equals("pdf"))
			{
				byte[] FileBytes = System.IO.File.ReadAllBytes(path);
				return this.File(FileBytes, "application/pdf");
			}
			var file = this.File(path, type);
			return file;
		}

	}
}
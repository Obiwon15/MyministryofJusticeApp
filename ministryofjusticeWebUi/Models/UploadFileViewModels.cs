using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ministryofjusticeDomain.Enum;

namespace ministryofjusticeWebUi.Models
{
	public class UploadFileViewModels
	{
		[DisplayName("Case No: ")]
		public int CaseId { get; set; }

		[Required(ErrorMessage = "Please file name is required")]
		[DisplayName("Name of File")]
		public string Filename { get; set; }
		public FileCategory FileCategory { get; set; }
		[DisplayName("Observations(Comments)")]
		[DataType(DataType.MultilineText)]
		public string Comments { get; set; }
	}
}
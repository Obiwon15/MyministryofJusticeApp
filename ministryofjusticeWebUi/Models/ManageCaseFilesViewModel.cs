using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ministryofjusticeDomain.Entities;

namespace ministryofjusticeWebUi.Models
{
	public class ManageCaseFilesViewModel
	{
		public IEnumerable<File> Files { get; set; }
		public int CaseId { get; set; }

	}
}
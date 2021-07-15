using ministryofjusticeWebUi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ministryofjusticeDomain.Entities;

namespace ministryofjusticeWebUi.Interfaces
{
    public interface IReportService
    {
        Case SubmitCase(CaseViewModel report, HttpPostedFileBase file, string path);
        CaseDetailsViewModel FindCase(string caseId);
    }
}
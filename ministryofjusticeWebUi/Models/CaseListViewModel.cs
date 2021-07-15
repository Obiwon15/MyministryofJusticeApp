using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ministryofjusticeWebUi.Models
{
    public class CaseListViewModel
    {
        public IEnumerable<CaseDetailsViewModel> Cases { get; set; }
        public Pagination PageInfo { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace ministryofjusticeDomain.Enum
{
    public enum SearchCategory
    {
        [Display(Name="Case Id")]
        CaseId,
        [Display(Name="Client email")]
        ClientEmail
    }
}

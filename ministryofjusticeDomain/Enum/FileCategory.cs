using System.ComponentModel.DataAnnotations;

namespace ministryofjusticeDomain.Enum
{
    public enum FileCategory
    {
        [Display(Name = "Evidence")]
        Evidence,
        [Display(Name="Lawyer File")]
        CaseFile,
        [Display(Name="Police Report")]
        PoliceReport,
        [Display(Name="Knowledge Base")]
        KnowledgeBase
    }
}

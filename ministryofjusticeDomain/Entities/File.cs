using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ministryofjusticeDomain.Enum;

namespace ministryofjusticeDomain.Entities
{
    public class File
    {
        public int Id { get; set; }
        public int? CaseId { get; set; }
        public string Filename { get; set; }
        public string FilePath { get; set; }
        public FileCategory FileCategory { get; set; }
        public string Comments { get; set; }

        public static bool Exists(string path)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.Interfaces.Services;

namespace ministryofjusticeDomain.Services
{
    public class CustomFileSystem : ICustomFileSystem
    {
        public void DeleteFile(File file)
        {
            if(System.IO.File.Exists(file.FilePath))
                System.IO.File.Delete(file.FilePath); ;
        }

        public bool DoesFileExists(string path)
        {
            return System.IO.File.Exists(path);
        }

        public void DoesDirectoryExist(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }
    }
}

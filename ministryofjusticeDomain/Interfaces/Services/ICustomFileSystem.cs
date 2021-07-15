using ministryofjusticeDomain.Entities;

namespace ministryofjusticeDomain.Interfaces.Services
{
    public interface ICustomFileSystem
    {
        void DeleteFile(File file);
        bool DoesFileExists(string path);
        void DoesDirectoryExist(string path);
    }
}
using SQLite.Net.Interop;

namespace ServicesMVVM.Interfaces
{
    public interface IConfig
    {
        string DirectoryDB { get; }

        ISQLitePlatform Platform { get; }
    }
}

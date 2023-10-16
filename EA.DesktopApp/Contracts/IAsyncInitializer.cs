using System.Threading.Tasks;

namespace EA.DesktopApp.Contracts
{
    public interface IAsyncInitializer
    {
        Task InitializeAsync();
    }
}
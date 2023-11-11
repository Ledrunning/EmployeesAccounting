using EA.DesktopApp.Models;

namespace EA.DesktopApp.Contracts
{
    public interface ICredentialService
    {
        void SetCredentials(Credentials credentials);
    }
}
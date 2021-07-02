using CodeBase.Infrastructure.Services;

namespace CodeBase.UI.Services.Windows
{
    public interface IWindowsService : IService
    {
        void Open(WindowId windowId);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.Explorer;
using Dev2.Common.Interfaces.Infrastructure;
using Dev2.Common.Interfaces.Toolbox;

namespace Dev2.Common.Interfaces
{
    public interface IServer:IResource
    {
        Task<bool> Connect();
        List<IResource> Load();
        Task<IExplorerItem> LoadExplorer();
        IList<IServer> GetServerConnections();
        IList<IToolDescriptor> LoadTools();
        IExplorerRepository ExplorerRepository { get; }
        bool IsConnected();
        void ReloadTools();
        void Disconnect();
        void Edit();
        List<IWindowsGroupPermission> Permissions { get; } 

        event PermissionsChanged PermissionsChanged;
        event NetworkStateChanged NetworkStateChanged;

        IStudioUpdateManager UpdateRepository{get;}
    }

    public delegate void PermissionsChanged(PermissionsChangedArgs args);
    public delegate void NetworkStateChanged(INetworkStateChangedEventArgs args);
}
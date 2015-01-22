using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.Explorer;
using Dev2.Common.Interfaces.Toolbox;
using Dev2.Controller;
using Dev2.Network;
using Dev2.Runtime.ServiceModel.Data;
using Dev2.Threading;

namespace Warewolf.Studio.AntiCorruptionLayer
{
    public class Server : Resource,IServer
    {
        readonly ServerProxy _environmentConnection;
        readonly Guid _serverId;
        StudioServerProxy _proxyLayer;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Server(string uri,string userName,string password):this(uri,new NetworkCredential(userName,password))
        {
        }

        public Server(Uri uri)
            : this(uri.ToString(), CredentialCache.DefaultNetworkCredentials)
        {
        }
        
        public Server(string uri,ICredentials credentials)
        {
            _environmentConnection = new ServerProxy(uri,credentials,new AsyncWorker());
            _environmentConnection.Connect(Guid.NewGuid()); // todo: temp id to ge
            _serverId = Guid.NewGuid();
            _proxyLayer = new StudioServerProxy(new CommunicationControllerFactory(), _environmentConnection);

        }

        #region Implementation of IServer

        public async Task<bool> Connect()
        {
            return await _environmentConnection.ConnectAsync(_serverId);
        }

        public List<IResource> Load()
        {
            
            return null;
        }

        public async Task<IExplorerItem> LoadExplorer()
        {
            var result = await _proxyLayer.QueryManagerProxy.Load();
            return result;
        }

        public IList<IServer> GetServerConnections()
        {
            return null;
        }

        public IList<IToolDescriptor> LoadTools()
        {
            return null;
        }

        public IExplorerRepository ExplorerRepository
        {
            get
            {
                return _proxyLayer;
            }
        }

        public bool IsConnected()
        {
            return _environmentConnection.IsConnected;
        }

        public void ReloadTools()
        {
        }

        public void Disconnect()
        {
            _environmentConnection.Disconnect();
        }

        public void Edit()
        {
        }

        public event PermissionsChanged PermissionsChanged;

        #endregion
    }
}
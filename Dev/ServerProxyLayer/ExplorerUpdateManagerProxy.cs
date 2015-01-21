﻿using System;
using Dev2.Common;
using Dev2.Common.Interfaces.Explorer;
using Dev2.Common.Interfaces.Infrastructure;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Dev2.Common.Interfaces.Studio.Core;
using Dev2.Common.Interfaces.Studio.Core.Controller;

namespace Warewolf.Studio.ServerProxyLayer
{
    public class ExplorerUpdateManagerProxy:ProxyBase,IExplorerUpdateManager
    {
        #region Implementation of IExplorerUpdateManager

        


        public ExplorerUpdateManagerProxy(ICommunicationControllerFactory communicationControllerFactory, IEnvironmentConnection connection):base(communicationControllerFactory,connection)
        {

        }

        /// <summary>
        /// Add a folder to a warewolf server
        /// </summary>
        /// <param name="path">relative path</param>
        public void AddFolder(string path)
        {
            var controller = CommunicationControllerFactory.CreateController("AddFolderService");
            controller.AddPayloadArgument("path", path);
            controller.ExecuteCommand<IExplorerRepositoryResult>(Connection, GlobalConstants.ServerWorkspaceID);
        }

        /// <summary>
        /// delete a folder from a warewolf server
        /// </summary>
        /// <param name="path">the folder path</param>
        public void DeleteFolder(string path)
        {
            var controller = CommunicationControllerFactory.CreateController("DeleteFolderService");

            controller.AddPayloadArgument("path", path);
            controller.ExecuteCommand<IExplorerRepositoryResult>(Connection, GlobalConstants.ServerWorkspaceID);
        }

        /// <summary>
        /// delete a resource from a warewolf server
        /// </summary>
        /// <param name="id">resource id</param>
        public void DeleteResource(Guid id)
        {
            var controller = CommunicationControllerFactory.CreateController("DeleteItemService");
            controller.AddPayloadArgument("itemToDelete", id.ToString());
            controller.ExecuteCommand<IExplorerRepositoryResult>(Connection, GlobalConstants.ServerWorkspaceID);
        }

        /// <summary>
        /// Rename a resource
        /// </summary>
        /// <param name="id">the resource id</param>
        /// <param name="newName">the new name</param>
        public void Rename(Guid id, string newName)
        {
            var controller = CommunicationControllerFactory.CreateController("RenameItemService");
            controller.AddPayloadArgument("itemToRename", id.ToString());
            controller.AddPayloadArgument("newName", newName);
            controller.ExecuteCommand<IExplorerRepositoryResult>(Connection, GlobalConstants.ServerWorkspaceID);

        }

        /// <summary>
        /// Move a resource to another folder
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newPath"></param>
        public void MoveItem(Guid id, string newPath)
        {
            var controller = CommunicationControllerFactory.CreateController("MoveItemService");
            controller.AddPayloadArgument("itemToMove", id.ToString());
            controller.AddPayloadArgument("newPath", newPath);
            controller.ExecuteCommand<IExplorerRepositoryResult>(Connection, GlobalConstants.ServerWorkspaceID);
        }



        #endregion
    }
}

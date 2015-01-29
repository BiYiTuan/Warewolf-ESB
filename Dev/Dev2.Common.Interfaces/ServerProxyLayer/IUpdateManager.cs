﻿using System;
using System.Text;
using Dev2.Common.Interfaces.Data;

namespace Dev2.Common.Interfaces.ServerProxyLayer
{
    /// <summary>
    /// Updates resources on a warewolf servers. Order of execution is gaurenteed
    /// </summary>
    public interface IUpdateManager
    {
        /// <summary>
        /// Deploy a resource. order of execution is gauranteed
        /// </summary>
        /// <param name="resource"></param>
        void DeployItem(IResource resource);

        /// <summary>
        /// Save a resource to the server
        /// </summary>
        /// <param name="resource">resource to save</param>
        /// <param name="workspaceId">the workspace to save to</param>
        void SaveResource(StringBuilder resource, Guid workspaceId);


        /// <summary>
        /// Save a resource to the server
        /// </summary>
        /// <param name="resource">resource to save</param>
        /// <param name="workspaceId">the workspace to save to</param>
        void SaveResource(IResource resource, Guid workspaceId);

        /// <summary>
        /// Tests if a valid connection to a server can be made returns 'Success' on a successful connection
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        string TestConnection(IResource resource);

    }
}
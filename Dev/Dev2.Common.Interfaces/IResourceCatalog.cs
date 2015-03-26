
/*
*  Warewolf - The Easy Service Bus
*  Copyright 2014 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Dev2.Common.Interfaces.Core.DynamicServices;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.Infrastructure.SharedModels;

namespace Dev2.Common.Interfaces
{
    // PBI 953 - 2013.05.16 - TWR - Created
    public interface IResourceCatalog
    {
        Action<IResource> ResourceSaved { get; set; }
        Action<Guid, IList<ICompileMessageTO>> SendResourceMessages { get; set; }
        int WorkspaceCount { get; }

        int GetResourceCount(Guid workspaceId);

        void RemoveWorkspace(Guid workspaceId);

        IResource GetResource(Guid workspaceId, string resourceName, ResourceType resourceType = ResourceType.Unknown, string version = null);

        IResource GetResource(Guid workspaceId, Guid resourceId);

        /// <summary>
        /// Gets the contents of the resource with the given name.
        /// </summary>
        /// <param name="workspaceId">The workspace ID to be queried.</param>
        /// <param name="resourceId">The resource ID to be queried.</param>
        /// <returns>
        /// The resource's contents or <code>string.Empty</code> if not found.
        /// </returns>
        StringBuilder GetResourceContents(Guid workspaceId, Guid resourceId);

        /// <summary>
        /// Gets the resource's contents.
        /// </summary>
        /// <param name="resource">The resource to be queried.</param>
        /// <returns>
        /// The resource's contents or <code>string.Empty</code> if not found.
        /// </returns>
        StringBuilder GetResourceContents(IResource resource);

        /// <summary>
        /// Gets the contents of the resource with the given guids.
        /// </summary>
        /// <param name="workspaceId">The workspace ID to be queried.</param>
        /// <param name="guidCsv">The guids to be queried.</param>
        /// <param name="type">The type string: WorkflowService, Service, Source, ReservedService or *, to be queried.</param>
        /// <returns>The resource's contents or <code>string.Empty</code> if not found.</returns>
        /// <exception cref="System.ArgumentNullException">type</exception>
        StringBuilder GetPayload(Guid workspaceId, string guidCsv, string type);

        /// <summary>
        /// Gets the contents of the resources with the given source type.
        /// </summary>
        /// <param name="workspaceId">The workspace ID to be queried.</param>
        /// <param name="sourceType">The type of the source to be queried.</param>
        /// <returns>The resource's contents or <code>string.Empty</code> if not found.</returns>
        StringBuilder GetPayload(Guid workspaceId, enSourceType sourceType);

        /// <summary>
        /// Gets the contents of the resource with the given name and type (WorkflowService, Service, Source, ReservedService or *).
        /// </summary>
        /// <param name="workspaceId">The workspace ID to be queried.</param>
        /// <param name="resourceName">The name of the resource to be queried.</param>
        /// <param name="type">The type string: WorkflowService, Service, Source, ReservedService or *, to be queried.</param>
        /// <param name="userRoles">The user roles to be queried.</param>
        /// <param name="useContains"><code>true</code> if matching resource name's should contain the given <paramref name="resourceName"/>;
        /// <code>false</code> if resource name's must exactly match the given <paramref name="resourceName"/>.</param>
        /// <returns>The resource's contents or <code>string.Empty</code> if not found.</returns>
        /// <exception cref="System.Exception">ResourceName or Type is missing from the request</exception>
        StringBuilder GetPayload(Guid workspaceId, string resourceName, string type, string userRoles, bool useContains = true);

        /// <summary>
        /// Loads the workspace.
        /// </summary>
        /// <param name="workspaceId">The workspace unique identifier.</param>
        void LoadWorkspace(Guid workspaceId);

        /// <summary>
        /// Loads the workspace via builder.
        /// </summary>
        /// <param name="workspacePath">The workspace path.</param>
        /// <param name="folders">The folders.</param>
        /// <returns></returns>
        IList<IResource> LoadWorkspaceViaBuilder(string workspacePath, params string[] folders);

        bool CopyResource(Guid resourceId, Guid sourceworkspaceId, Guid targetworkspaceId, string userRoles = null);

        bool CopyResource(IResource resource, Guid targetworkspaceId, string userRoles = null);

        IResourceCatalogResult SaveResource(Guid workspaceId, StringBuilder resourceXml, string userRoles = null,string reason ="",string user ="");

        IResourceCatalogResult SaveResource(Guid workspaceId, IResource resource, string userRoles = null, string reason = "", string user = "");

        IResourceCatalogResult DeleteResource(Guid workspaceId, string resourceName, string type, string userRoles = null, bool deleteVersions = true);


        void SyncTo(string sourceWorkspacePath, string targetWorkspacePath, bool overwrite = true, bool delete = true, IList<string> filesToIgnore = null);

        List<TServiceType> GetDynamicObjects<TServiceType>(Guid workspaceId, string resourceName, bool useContains = false)
            where TServiceType : IDynamicServiceObject;

        List<IDynamicServiceObject> GetDynamicObjects(IResource resource);

        List<IDynamicServiceObject> GetDynamicObjects(Guid workspaceId);

        List<IDynamicServiceObject> GetDynamicObjects(IEnumerable<IResource> resources);

        List<Guid> GetDependants(Guid workspaceId, Guid? resourceId);

        List<IResourceForTree> GetDependentsAsResourceForTrees(Guid workspaceId, Guid resourceId);

        IList<IResource> GetResourceList(Guid workspaceId);

        IResourceCatalogResult RenameResource(Guid workspaceId, Guid? resourceId, string newName);

        IResourceCatalogResult RenameCategory(Guid workspaceId, string oldCategory, string newCategory);

        IResourceCatalogResult RenameCategory(Guid workspaceId, string oldCategory, string newCategory, List<IResource> resourcesToUpdate);

        T GetResource<T>(Guid workspaceId, Guid resourceId) where T : IResource, new();

        T GetResource<T>(Guid workspaceId, string resourceName) where T : IResource, new();
        IResourceCatalogResult DeleteResource(Guid workspaceId, Guid resourceId, string type, string userRoles = null, bool deleteVersions = true);
        IList<IResource> GetResourceList(Guid workspaceId, string resourceName, string type, string userRoles, bool useContains = true);
        IList<IResource> GetResourceList(Guid workspaceId, string guidCsv, string type);

        /// <summary>
        /// Gets the contents of the resources with the given source type.
        /// </summary>
        /// <param name="workspaceID">The workspace ID to be queried.</param>
        /// <param name="sourceType">The type of the source to be queried.</param>
        /// <returns>The resource's contents or <code>string.Empty</code> if not found.</returns>
        IEnumerable GetModels(Guid workspaceID, enSourceType sourceType);

        List<TServiceType> GetDynamicObjects<TServiceType>(Guid workspaceID, Guid resourceID)
            where TServiceType : IDynamicServiceObject;
    }
}

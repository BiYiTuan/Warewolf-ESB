
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
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Dev2.Common.Interfaces {
    /// <summary>
    /// Defines the requirements for a workspace.
    /// </summary>
    public interface IWorkspace : ISerializable, IEquatable<IWorkspace> {
        /// <summary>
        /// Gets or sets the unique ID.
        /// </summary>
        Guid ID {
            get;
        }


        /// <summary>
        /// Gets the items for this workspace.
        /// </summary>
        IList<IWorkspaceItem> Items {
            get;
        }

        /// <summary>
        /// Performs the <see cref="IWorkspaceItem.Action" /> on the specified workspace item.
        /// </summary>
        /// <param name="workspaceItem">The workspace item to be actioned.</param>
        /// <param name="isLocalSave">if set to <c>true</c> [is local save].</param>
        /// <param name="roles">The roles.</param>
        /// <exception cref="System.ArgumentNullException">workspaceItem</exception>
        void Update(IWorkspaceItem workspaceItem, bool isLocalSave, string roles = null);
    }
}

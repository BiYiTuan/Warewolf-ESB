
/*
*  Warewolf - The Easy Service Bus
*  Copyright 2014 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System.Collections.Generic;
using Dev2;
using Dev2.Common.Interfaces.Infrastructure.Providers.Errors;
using Dev2.Enums;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace
namespace Unlimited.Applications.BusinessDesignStudio.Activities
{
    // Place holder interface used to locate model items in WorkflowDesignerViewModel using a string which must be a guid
    public interface IDev2Activity
    {
        /// <summary>
        /// UniqueID is the InstanceID and MUST be a guid.
        /// </summary>
        string UniqueID { get; set; }
        IList<IActionableErrorInfo> PerformValidation();
        enFindMissingType GetFindMissingType();
        IDev2Activity Execute(IDSFDataObject data);
    }
}

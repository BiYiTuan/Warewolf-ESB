using System;
using System.Collections.Generic;
using Dev2.Common.Interfaces.Data.TO;

namespace Dev2.Common.Interfaces
{
    public interface IWorkflowApplicationFactory
    {
    }

    public interface IWorkflowApplicationFactory<T> : IWorkflowApplicationFactory where T:class
    {
        IErrorResultTO AllErrors { get; }

        /// <summary>
        /// Invokes the workflow.
        /// </summary>
        /// <param name="workflowActivity">The workflow activity.</param>
        /// <param name="dataTransferObject">The data transfer object.</param>
        /// <param name="executionExtensions">The execution extensions.</param>
        /// <param name="instanceId">The instance id.</param>
        /// <param name="workspace">The workspace.</param>
        /// <param name="bookmarkName">Name of the bookmark.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        IDSFDataObject InvokeWorkflow(IWarewolfActivity<T> workflowActivity, IDSFDataObject dataTransferObject, IList<object> executionExtensions, Guid instanceId, IWorkspace workspace, string bookmarkName, out IErrorResultTO errors);
    }
}
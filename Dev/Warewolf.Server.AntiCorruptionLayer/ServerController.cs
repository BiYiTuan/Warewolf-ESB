using System.Activities;
using Dev2.Common.Interfaces;

namespace Warewolf.Server.AntiCorruptionLayer
{
    public class ServerController : IServerController
    {
        public IWorkflowExecutionController<Activity> WorkflowExecutionController { get; private set; }
        public IResourceCatalogController ResourceCatalogController { get; private set; }

        public ServerController(IWorkflowExecutionController<Activity> workflowExecutionController,IResourceCatalogController resourceCatalogController)
        {
            WorkflowExecutionController = workflowExecutionController;
            ResourceCatalogController = resourceCatalogController;
        }

        public IResourceCatalog GetResourceCatalog()
        {
            return ResourceCatalogController.GetResourceCatalog();
        }

        public IWorkflowApplicationFactory<T> GetWorkflowApplicationFactory<T>() where T : class
        {
            return WorkflowExecutionController.GetWorkflowApplicationFactory() as IWorkflowApplicationFactory<T>;
        }
    }
}

using System.Activities;
using Dev2.Common.Interfaces;

namespace Warewolf.Server.AntiCorruptionLayer
{
    public class ServerController : IServerController
    {
        private readonly IWorkflowExecutionController<Activity> _workflowExecutionController;

        public ServerController(IWorkflowExecutionController<Activity> workflowExecutionController)
        {
            _workflowExecutionController = workflowExecutionController;
        }
        
        public IWorkflowApplicationFactory<T> GetWorkflowApplicationFactory<T>() where T : class
        {
            return _workflowExecutionController.GetWorkflowApplicationFactory() as IWorkflowApplicationFactory<T>;
        }
    }
}

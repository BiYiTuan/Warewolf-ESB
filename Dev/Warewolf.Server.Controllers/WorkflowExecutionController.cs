using System.Activities;
using Dev2.Common.Interfaces;
using Dev2.Runtime.ESB.WF;

namespace Warewolf.Server.Controllers
{
    public class WorkflowExecutionController : IWorkflowExecutionController<Activity>
    {
        public IWorkflowApplicationFactory<Activity> GetWorkflowApplicationFactory()
        {
            return new WorkflowApplicationFactory();
        }
    }
}
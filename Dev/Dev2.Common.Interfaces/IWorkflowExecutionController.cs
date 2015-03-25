namespace Dev2.Common.Interfaces
{
    public interface IWorkflowExecutionController<T> where T:class 
    {
        IWorkflowApplicationFactory<T> GetWorkflowApplicationFactory();
    }
}
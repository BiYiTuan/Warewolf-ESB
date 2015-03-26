namespace Dev2.Common.Interfaces
{
    public interface IServerController
    {
        IWorkflowApplicationFactory<T> GetWorkflowApplicationFactory<T>() where T:class ;
        IResourceCatalog GetResourceCatalog();
    }
}